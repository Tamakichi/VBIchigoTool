'
' IchigoJam�f�[�^�������݃c�[��
' �ŏI�C���� 2015/05/24 v1.03
'

Imports System.Threading
Public Class Ichigotool

    Dim datafpath As String     '�f�[�^�t�@�C���p�X
    Dim readDatafpath As String '�ۑ��p�f�[�^�t�@�C���p�X
    Dim addr As UInt32          '�f�[�^�������ݐ擪�A�h���X
    Dim flgActive As Boolean    '�]�����t���O(0:���]�� 1:�]����)
    Dim mThread As Thread       '�f�[�^�]���p�X���b�h
    Dim jobsts As Boolean       '�X���b�h�ғ�����
    Dim readdata As String
    Dim rdata(2048) As Byte

    'Delegate Sub AddDataDelegate(ByVal str As String)

    '�f�[�^�t�@�C���̎w��
    Private Sub BtnFopen_Click(sender As Object, e As EventArgs)
        OpenFileDialogData.ShowDialog()
        datafpath = OpenFileDialogData.FileName
    End Sub

    '���O���b�Z�[�W�̏�������
    Private Sub addLog(ByVal str As String)
        Tb_message.Text = Tb_message.Text + str + vbCrLf
    End Sub

    '�R���g���[������̐���
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


    '�f�[�^�������ݏ���
    '�@�o�b�N�O�����h�p�X���b�h����Ăяo�����
    Private Sub Upload()
        Dim data As Array
        Dim sdt(6) As Byte
        Dim wdt(1) As Byte
        Dim cnt As UInt32
        Dim flgend As Boolean
        Dim cksum As Integer

        flgend = False

        cksum = 0

        '�]������ԂɈڍs
        flgActive = True
        SerialPort1.NewLine = vbCrLf
        Me.BeginInvoke( _
        Sub()
            addLog("�o�b�N�O���E���h�ŒʐM�������J�n���܂���.")
            ctrlSetting()
        End Sub)


        '** �����ݗp�f�[�^�̃��[�h **
        Try
            data = My.Computer.FileSystem.ReadAllBytes(datafpath)
            'data = My.Computer.FileSystem.ReadAllText(datafpath)
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog(ex.Message)
                    addLog("�t�@�C���̓ǂݍ��݂Ɏ��s���܂���.�������I�����܂�.")
                End Sub)
            flgActive = False
            ctrlSetting()
            GoTo FOREND
        End Try
        Me.BeginInvoke( _
            Sub()
                addLog("�t�@�C����ǂݍ��݂܂���. �T�C�Y:" & data.Length)
            End Sub)

        Me.BeginInvoke( _
         Sub()
             Lbl_status.Text = "��: �ʐM�� [" & cnt & "/" & data.Length & "]"
             Lbl_status.Update()
         End Sub)

        'NEW �R�}���h�̑��M
        Thread.Sleep(400)
        SerialPort1.WriteLine("")
        Thread.Sleep(400)
        SerialPort1.WriteLine("NEW")
        Thread.Sleep(400)

        '** �ȍ~�f�[�^���M����
        ' �P�o�C�g���M�̓s�x�A��������M���Ă���
        For Each d As Byte In data

            '1�o�C�g���M
            wdt(0) = d
            SerialPort1.Write(wdt, 0, 1)
            If d = &HA Then
                Thread.Sleep(400)
            Else
                Thread.Sleep(20)
            End If

            '�i���\��
            cnt = cnt + 1
            If cnt Mod 100 = 0 Then
                Me.BeginInvoke( _
                Sub()
                    Lbl_status.Text = "��: �ʐM�� [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
            End If

            If jobsts = False Then
                Me.BeginInvoke( _
                Sub()
                    Lbl_status.Text = "��: ���~ [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                    addLog("�ʐM�𒆎~���܂���.")
                End Sub)
                Exit For
            End If
        Next

        '�ʐM�I��
        If jobsts = True Then
            Me.BeginInvoke( _
            Sub()
                addLog("�ʐM���������܂���.")
                ctrlSetting()
            End Sub)
            Me.BeginInvoke( _
            Sub()
                Lbl_status.Text = "��: ���� [" & cnt & "/" & data.Length & "]"
                Lbl_status.Update()
                MessageBox.Show(Me, "�ʐM���������܂���.", "�A�b�v���[�h�I��", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Sub)
        Else
            Me.BeginInvoke( _
            Sub()
                MessageBox.Show(Me, "�ʐM�𒆒f���܂���.", "�A�b�v���[�h�I��", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Sub)
        End If

FOREND:
        If SerialPort1.IsOpen = True Then
            Call SerialPort1.Close()
            Me.BeginInvoke( _
            Sub()
                addLog("�V���A���ڑ���ؒf���܂���.")
                ctrlSetting()
            End Sub)
        End If

        '��ʐM��ԂɈڍs
        flgActive = False
        Me.BeginInvoke( _
        Sub()
            addLog("�o�b�N�O���E���h�������I�����܂���.")
            ctrlSetting()
        End Sub)

    End Sub

    '�_�E�����[�h
    Sub Download()
        Dim b_data(2048) As Byte

        '���O�̃N���A
        Tb_message.Text = ""
        SerialPort1.NewLine = vbCrLf
        '�V���A���ڑ�
        Try
            SerialPort1.PortName = Cmb_comport.Text
            If SerialPort1.IsOpen = True Then
                MessageBox.Show(Me, "���ł�" & SerialPort1.PortName & "�͐ڑ�����Ă��܂��B", Me.Text, _
                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                addLog("���ł�" & SerialPort1.PortName & "�͐ڑ�����Ă��܂��B")
                Exit Sub
            End If
            Call SerialPort1.Open()
        Catch ex As Exception
            addLog(ex.Message)
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
        addLog("�V���A���ڑ����܂���.")

        'LIST �R�}���h�̑��M

        SerialPort1.WriteLine("")
        Thread.Sleep(50)
        SerialPort1.WriteLine("LIST")
        Thread.Sleep(1000)

        '�f�[�^�̓ǂݍ���(�o�C�i���œǂݍ���)
        Dim sz As Integer = 0
        Dim pos As Integer

        SerialPort1.ReadTimeout = 500
        For pos = 0 To 2028
            Try
                b_data(pos) = SerialPort1.ReadByte()
            Catch ex As TimeoutException
                sz = pos
                If sz = 0 Then
                    MessageBox.Show(Me, "�v���O�������擾���邱�Ƃ��o���܂���ł���." & vbCr & "�������I�����܂�.", Me.Text, _
                      MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("�v���O�������擾���邱�Ƃ��o���܂���ł���.")
                    GoTo EXITDOWNLOAD
                End If

                Exit For
            Catch ex As InvalidOperationException
                MessageBox.Show(Me, "�V���A���ʐM���ɃG���[���������܂���." & vbCr & "�������I�����܂�.", Me.Text, _
                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                addLog("�V���A���ʐM�G���[�ɂ�菈�����I�����܂���.")
                GoTo EXITDOWNLOAD
            End Try
        Next

        If b_data(sz - 3) = Asc("O") And b_data(sz - 2) = Asc("K") Then
            addLog("�v���O������ǂݍ��݂܂���.")
            sz = sz - 3
        Else
            MessageBox.Show(Me, "�v���O�������Ō�܂Ŏ擾���邱�Ƃ��o���܂���ł���." & vbCr & "�������I�����܂�.", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            addLog("�v���O�������Ō�܂Ŏ擾���邱�Ƃ��o���܂���ł���.")
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
            MessageBox.Show(Me, "�t�@�C���̃I�[�v���Ɏ��s���܂���." & vbCr & "�������I�����܂�.", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            addLog("�t�@�C���I�[�v���G���[�ɂ�菈�����I�����܂���.")
            GoTo EXITDOWNLOAD
        End Try

        '�f�[�^�̕ۑ�
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
            MessageBox.Show(Me, "�t�@�C���̏������݂Ɏ��s���܂���." & vbCr & "�������I�����܂�.", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            addLog("�t�@�C���������݃G���[�ɂ�菈�����I�����܂���.")
            fs.Close()
            fs.Dispose()
            GoTo EXITDOWNLOAD
        End Try
        fs.Close()
        fs.Dispose()

        addLog("�f�[�^��ۑ����܂���.")
        If MessageBox.Show(Me, "�f�[�^��ۑ����܂���." & vbCr & "�������ŊJ���܂���?", _
                        "�_�E�����[�h�I��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            System.Diagnostics.Process.Start("Notepad", readDatafpath)
        End If

EXITDOWNLOAD:
        '�V���A���ؒf
        If SerialPort1.IsOpen = True Then
            Call SerialPort1.Close()
            Me.BeginInvoke( _
            Sub()
                addLog("�V���A���ڑ���ؒf���܂���.")
                ctrlSetting()
            End Sub)
        End If


    End Sub

    'IchigoJam�ւ̃A�b�v���[�h
    Sub doUpload()
        If MessageBox.Show(Me, "�A�b�v���[�h���J�n���܂��B" & vbCr & "��낵���ł����H", _
                               "�A�b�v���[�h�̎��s", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> vbYes Then
            Exit Sub
        End If

        '���O�̃N���A
        Tb_message.Text = ""

        '�f�[�^�t�@�C���̃`�F�b�N
        If datafpath = "" Then
            MessageBox.Show(Me, "�f�[�^�t�@�C�����w�肳��Ă��܂���", Me.Text, _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '�V���A���ڑ�
        Try
            SerialPort1.PortName = Cmb_comport.Text
            If SerialPort1.IsOpen = True Then
                MessageBox.Show(Me, "���ł�" & SerialPort1.PortName & "�͐ڑ�����Ă��܂��B", Me.Text, _
                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                addLog("���ł�" & SerialPort1.PortName & "�͐ڑ�����Ă��܂��B")
                Exit Sub
            End If
            Call SerialPort1.Open()
        Catch ex As Exception
            addLog(ex.Message)
            MessageBox.Show(Me, ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
        addLog("�V���A���ڑ����܂���.")

        '�����ݗp�X���b�g�̎��s
        flgActive = True
        jobsts = True
        ctrlSetting()
        '�f�[�^�]���p�̃X���b�h�̍쐬
        mThread = New Thread(AddressOf job_thread) '�o�b�N�O���E���h�����̐ݒ�
        mThread.IsBackground = True '�o�b�N�O���E���h�E�X���b�h�ɂ���

        'System.Threading.Thread.Sleep(2000)
        mThread.Start() '�X���b�h�̊J�n
    End Sub

    '�f�[�^�������݃{�^��
    Private Sub BtnWriteData_Click(sender As Object, e As EventArgs) Handles BtnWriteData.Click
        datafpath = ""
        OpenFileDialogData.ShowDialog()
        datafpath = OpenFileDialogData.FileName
        If datafpath <> "" Then
            doUpload()
        End If
    End Sub

    '�f�[�^�]���p�̃X���b�h�{��
    Private Sub job_thread()
        Upload()
    End Sub

    '[�I��]�{�^��������������
    Private Sub Btn_end_Click(sender As Object, e As EventArgs) Handles Btn_end.Click
        Me.Close()
    End Sub

    '�v���O�����I�����̏���([�I��]�{�^���A[�~]�{�^����)
    Private Sub FormEPPROM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If flgActive = True Then
            If MessageBox.Show(Me, "�f�[�^�ʐM���ł����I�����܂����H", Me.Text, _
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    'Form���[�h���̏���������
    Private Sub FormEPPROM_Load(sender As Object, e As EventArgs) Handles Me.Load

        '�V���A���|�[�g�̈ꗗ���擾���R���{�{�b�N�X�ɕ\������
        Dim ports() As String = IO.Ports.SerialPort.GetPortNames()
        For Each str As String In ports
            Cmb_comport.Items.Add(str)
            If ports.Length > 0 Then
                Cmb_comport.SelectedIndex = 0
            End If
        Next

        '����{�^���̑��쐧��ݒ�
        flgActive = False
        ctrlSetting()
        Cmb_comport.DropDownStyle = ComboBoxStyle.DropDownList

    End Sub

    '[���~]�{�^���ɂ�钆�~����
    Private Sub btn_stop_Click(sender As Object, e As EventArgs) Handles btn_stop.Click
        If MessageBox.Show(Me, "�f�[�^�ʐM���I�����܂����H", Me.Text, _
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        '�X���b�h���I������

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

    '�t�@�C���̕ۑ�
    Private Sub BtnReadData_Click(sender As Object, e As EventArgs) Handles BtnReadData.Click
        readDatafpath = ""
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName <> "" Then
            If MessageBox.Show(Me, "�_�E�����[�h���J�n���܂��B" & vbCr & "��낵���ł����H", "�_�E�����[�h�̎��s", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> vbYes Then
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
