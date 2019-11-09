'
' IchigoJamデータ書き込みツール
' 最終修正日 2015/05/24 v1.03
'

Imports System.Threading
Public Class Ichigotool

    Dim datafpath As String     'データファイルパス
    Dim readDatafpath As String '保存用データファイルパス
    Dim addr As UInt32          'データ書き込み先頭アドレス
    Dim flgActive As Boolean    '転送中フラグ(0:未転送 1:転送中)
    Dim mThread As Thread       'データ転送用スレッド
    Dim jobsts As Boolean       'スレッド稼働許可
    Dim readdata As String
    Dim rdata(2048) As Byte

    'Delegate Sub AddDataDelegate(ByVal str As String)

    'データファイルの指定
    Private Sub BtnFopen_Click(sender As Object, e As EventArgs)
        OpenFileDialogData.ShowDialog()
        datafpath = OpenFileDialogData.FileName
    End Sub

    'ログメッセージの書き込み
    Private Sub addLog(ByVal str As String)
        Tb_message.Text = Tb_message.Text + str + vbCrLf
    End Sub

    'コントロール操作の制限
    Private Sub ctrlSetting()
        If flgActive = True Then
            Cmb_comport.Enabled = False
            BtnWriteData.Enabled = False
            BtnReadData.Enabled = False
            btn_stop.Enabled = True
        Else
            Cmb_comport.Enabled = True
            BtnReadData.Enabled = True
            BtnWriteData.Enabled = True
            btn_stop.Enabled = False
        End If
    End Sub


    'データ書き込み処理
    '　バックグランド用スレッドから呼び出される
    Private Sub Upload()
        Dim data As Array
        Dim sdt(6) As Byte
        Dim wdt(1) As Byte
        Dim cnt As UInt32
        Dim flgend As Boolean
        Dim cksum As Integer

        flgend = False

        cksum = 0

        '転送中状態に移行
        flgActive = True
        SerialPort1.NewLine = vbCrLf
        Me.BeginInvoke( _
        Sub()
            addLog("バックグラウンドで通信処理を開始しました.")
            ctrlSetting()
        End Sub)


        '** 書込み用データのロード **
        Try
            data = My.Computer.FileSystem.ReadAllBytes(datafpath)
            'data = My.Computer.FileSystem.ReadAllText(datafpath)
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog(ex.Message)
                    addLog("ファイルの読み込みに失敗しました.処理を終了します.")
                End Sub)
            flgActive = False
            ctrlSetting()
            GoTo FOREND
        End Try
        Me.BeginInvoke( _
            Sub()
                addLog("ファイルを読み込みました. サイズ:" & data.Length)
            End Sub)

        Me.BeginInvoke( _
         Sub()
             Lbl_status.Text = "状況: 通信中 [" & cnt & "/" & data.Length & "]"
             Lbl_status.Update()
         End Sub)

        'NEW コマンドの送信
        Thread.Sleep(400)
        SerialPort1.WriteLine("")
        Thread.Sleep(400)
        SerialPort1.WriteLine("NEW")
        Thread.Sleep(400)

        '** 以降データ送信処理
        ' １バイト送信の都度、応答を受信している
        For Each d As Byte In data

            '1バイト送信
            wdt(0) = d
            SerialPort1.Write(wdt, 0, 1)
            If d = &HA Then
                Thread.Sleep(400)
            Else
                Thread.Sleep(20)
            End If

            '進捗表示
            cnt = cnt + 1
            If cnt Mod 100 = 0 Then
                Me.BeginInvoke( _
                Sub()
                    Lbl_status.Text = "状況: 通信中 [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
            End If

            If jobsts = False Then
                Me.BeginInvoke( _
                Sub()
                    Lbl_status.Text = "状況: 中止 [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                    addLog("通信を中止しました.")
                End Sub)
                Exit For
            End If
        Next

        '通信終了
        If jobsts = True Then
            Me.BeginInvoke( _
            Sub()
                addLog("通信を完了しました.")
                ctrlSetting()
            End Sub)
            Me.BeginInvoke( _
            Sub()
                Lbl_status.Text = "状況: 完了 [" & cnt & "/" & data.Length & "]"
                Lbl_status.Update()
                MessageBox.Show(Me, "通信を完了しました.", "アップロード終了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Sub)
        Else
            Me.BeginInvoke( _
            Sub()
                MessageBox.Show(Me, "通信を中断しました.", "アップロード終了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Sub)
        End If

FOREND:
        If SerialPort1.IsOpen = True Then
            Call SerialPort1.Close()
            Me.BeginInvoke( _
            Sub()
                addLog("シリアル接続を切断しました.")
                ctrlSetting()
            End Sub)
        End If

        '非通信状態に移行
        flgActive = False
        Me.BeginInvoke( _
        Sub()
            addLog("バックグラウンド処理を終了しました.")
            ctrlSetting()
        End Sub)

    End Sub

    'ダウンロード
    Sub Download()
        Dim b_data(2048) As Byte

        'ログのクリア
        Tb_message.Text = ""
        SerialPort1.NewLine = vbCrLf
        'シリアル接続
        Try
            SerialPort1.PortName = Cmb_comport.Text
            If SerialPort1.IsOpen = True Then
                MessageBox.Show(Me, "すでに" & SerialPort1.PortName & "は接続されています。", Me.Text, _
                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                addLog("すでに" & SerialPort1.PortName & "は接続されています。")
                Exit Sub
            End If
            Call SerialPort1.Open()
        Catch ex As Exception
            addLog(ex.Message)
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
        addLog("シリアル接続しました.")

        'LIST コマンドの送信

        SerialPort1.WriteLine("")
        Thread.Sleep(50)
        SerialPort1.WriteLine("LIST")
        Thread.Sleep(1000)

        'データの読み込み(バイナリで読み込む)
        Dim sz As Integer = 0
        Dim pos As Integer

        SerialPort1.ReadTimeout = 500
        For pos = 0 To 2028
            Try
                b_data(pos) = SerialPort1.ReadByte()
            Catch ex As TimeoutException
                sz = pos
                If sz = 0 Then
                    MessageBox.Show(Me, "プログラムを取得することが出来ませんでした." & vbCr & "処理を終了します.", Me.Text, _
                      MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("プログラムを取得することが出来ませんでした.")
                    GoTo EXITDOWNLOAD
                End If

                Exit For
            Catch ex As InvalidOperationException
                MessageBox.Show(Me, "シリアル通信中にエラーが発生しました." & vbCr & "処理を終了します.", Me.Text, _
                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                addLog("シリアル通信エラーにより処理を終了しました.")
                GoTo EXITDOWNLOAD
            End Try
        Next

        If b_data(sz - 3) = Asc("O") And b_data(sz - 2) = Asc("K") Then
            addLog("プログラムを読み込みました.")
            sz = sz - 3
        Else
            MessageBox.Show(Me, "プログラムを最後まで取得することが出来ませんでした." & vbCr & "処理を終了します.", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            addLog("プログラムを最後まで取得することが出来ませんでした.")
            GoTo EXITDOWNLOAD
        End If

        Dim rc As Integer = 0
        Dim fi As System.IO.FileInfo
        Dim fs As System.IO.FileStream
        Dim j As Integer
        Dim b As Byte
        Dim cr() As Byte = {&HD, &HA}

        Try
            fi = New IO.FileInfo(readDatafpath)
            fs = fi.Open(IO.FileMode.Create)
        Catch ex As Exception
            MessageBox.Show(Me, "ファイルのオープンに失敗しました." & vbCr & "処理を終了します.", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            addLog("ファイルオープンエラーにより処理を終了しました.")
            GoTo EXITDOWNLOAD
        End Try

        'データの保存
        Try
            For j = 0 To sz - 1
                b = b_data(j)
                If b = &HA Then
                    fs.Write(cr, 0, 2)
                Else
                    fs.WriteByte(b)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(Me, "ファイルの書き込みに失敗しました." & vbCr & "処理を終了します.", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            addLog("ファイル書き込みエラーにより処理を終了しました.")
            fs.Close()
            fs.Dispose()
            GoTo EXITDOWNLOAD
        End Try
        fs.Close()
        fs.Dispose()

        addLog("データを保存しました.")
        If MessageBox.Show(Me, "データを保存しました." & vbCr & "メモ帳で開きますか?", _
                        "ダウンロード終了", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            System.Diagnostics.Process.Start("Notepad", readDatafpath)
        End If

EXITDOWNLOAD:
        'シリアル切断
        If SerialPort1.IsOpen = True Then
            Call SerialPort1.Close()
            Me.BeginInvoke( _
            Sub()
                addLog("シリアル接続を切断しました.")
                ctrlSetting()
            End Sub)
        End If


    End Sub

    'IchigoJamへのアップロード
    Sub doUpload()
        If MessageBox.Show(Me, "アップロードを開始します。" & vbCr & "よろしいですか？", _
                               "アップロードの実行", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> vbYes Then
            Exit Sub
        End If

        'ログのクリア
        Tb_message.Text = ""

        'データファイルのチェック
        If datafpath = "" Then
            MessageBox.Show(Me, "データファイルが指定されていません", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'シリアル接続
        Try
            SerialPort1.PortName = Cmb_comport.Text
            If SerialPort1.IsOpen = True Then
                MessageBox.Show(Me, "すでに" & SerialPort1.PortName & "は接続されています。", Me.Text, _
                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                addLog("すでに" & SerialPort1.PortName & "は接続されています。")
                Exit Sub
            End If
            Call SerialPort1.Open()
        Catch ex As Exception
            addLog(ex.Message)
            MessageBox.Show(Me, ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
        addLog("シリアル接続しました.")

        '書込み用スレットの実行
        flgActive = True
        jobsts = True
        ctrlSetting()
        'データ転送用のスレッドの作成
        mThread = New Thread(AddressOf job_thread) 'バックグラウンド処理の設定
        mThread.IsBackground = True 'バックグラウンド・スレッドにする

        'System.Threading.Thread.Sleep(2000)
        mThread.Start() 'スレッドの開始
    End Sub

    'データ書き込みボタン
    Private Sub BtnWriteData_Click(sender As Object, e As EventArgs) Handles BtnWriteData.Click
        datafpath = ""
        OpenFileDialogData.ShowDialog()
        datafpath = OpenFileDialogData.FileName
        If datafpath <> "" Then
            doUpload()
        End If
    End Sub

    'データ転送用のスレッド本体
    Private Sub job_thread()
        Upload()
    End Sub

    '[終了]ボタンを押した処理
    Private Sub Btn_end_Click(sender As Object, e As EventArgs) Handles Btn_end.Click
        Me.Close()
    End Sub

    'プログラム終了時の処理([終了]ボタン、[×]ボタン時)
    Private Sub FormEPPROM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If flgActive = True Then
            If MessageBox.Show(Me, "データ通信中ですが終了しますが？", Me.Text, _
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    'Formロード時の初期化処理
    Private Sub FormEPPROM_Load(sender As Object, e As EventArgs) Handles Me.Load

        'シリアルポートの一覧を取得しコンボボックスに表示する
        Dim ports() As String = IO.Ports.SerialPort.GetPortNames()
        For Each str As String In ports
            Cmb_comport.Items.Add(str)
            If ports.Length > 0 Then
                Cmb_comport.SelectedIndex = 0
            End If
        Next

        '操作ボタンの操作制約設定
        flgActive = False
        ctrlSetting()
        Cmb_comport.DropDownStyle = ComboBoxStyle.DropDownList

    End Sub

    '[中止]ボタンによる中止処理
    Private Sub btn_stop_Click(sender As Object, e As EventArgs) Handles btn_stop.Click
        If MessageBox.Show(Me, "データ通信を終了しますか？", Me.Text, _
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        'スレッドを終了する

        jobsts = False
        'mThread.Join()
        'mThread = Nothing
    End Sub

    Private Sub FormEPPROM_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim s() As String = e.Data.GetData("FileDrop", False)
        datafpath = s(0)
        doUpload()
    End Sub

    Private Sub FormEPPROM_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    'ファイルの保存
    Private Sub BtnReadData_Click(sender As Object, e As EventArgs) Handles BtnReadData.Click
        readDatafpath = ""
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName <> "" Then
            If MessageBox.Show(Me, "ダウンロードを開始します。" & vbCr & "よろしいですか？", "ダウンロードの実行", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> vbYes Then
                Exit Sub
            End If

            readDatafpath = SaveFileDialog1.FileName
            flgActive = True
            ctrlSetting()
            Download()
            flgActive = False
            ctrlSetting()
        End If
    End Sub
End Class
