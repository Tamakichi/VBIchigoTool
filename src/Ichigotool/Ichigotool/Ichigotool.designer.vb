<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Ichigotool
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.OpenFileDialogData = New System.Windows.Forms.OpenFileDialog()
        Me.BtnWriteData = New System.Windows.Forms.Button()
        Me.Tb_message = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Btn_end = New System.Windows.Forms.Button()
        Me.btn_stop = New System.Windows.Forms.Button()
        Me.Lbl_status = New System.Windows.Forms.Label()
        Me.Cmb_comport = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnReadData = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 115200
        Me.SerialPort1.NewLine = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "                "
        Me.SerialPort1.ReadTimeout = 5000
        Me.SerialPort1.ReceivedBytesThreshold = 2000
        Me.SerialPort1.WriteTimeout = 2000
        '
        'OpenFileDialogData
        '
        Me.OpenFileDialogData.Filter = "プログラムファイル (*.txt;*.bas)|*.txt;*.bas|All files (*.*)|*.*"
        Me.OpenFileDialogData.FilterIndex = 2
        '
        'BtnWriteData
        '
        Me.BtnWriteData.Location = New System.Drawing.Point(3, 114)
        Me.BtnWriteData.Name = "BtnWriteData"
        Me.BtnWriteData.Size = New System.Drawing.Size(75, 66)
        Me.BtnWriteData.TabIndex = 2
        Me.BtnWriteData.Text = "アップロード"
        Me.BtnWriteData.UseVisualStyleBackColor = True
        '
        'Tb_message
        '
        Me.Tb_message.BackColor = System.Drawing.SystemColors.Control
        Me.Tb_message.Enabled = False
        Me.Tb_message.Font = New System.Drawing.Font("MS UI Gothic", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Tb_message.Location = New System.Drawing.Point(8, 44)
        Me.Tb_message.Multiline = True
        Me.Tb_message.Name = "Tb_message"
        Me.Tb_message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Tb_message.Size = New System.Drawing.Size(307, 64)
        Me.Tb_message.TabIndex = 9
        Me.Tb_message.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "進行状況"
        '
        'Btn_end
        '
        Me.Btn_end.Location = New System.Drawing.Point(246, 114)
        Me.Btn_end.Name = "Btn_end"
        Me.Btn_end.Size = New System.Drawing.Size(71, 66)
        Me.Btn_end.TabIndex = 1
        Me.Btn_end.Text = "終了"
        Me.Btn_end.UseVisualStyleBackColor = True
        '
        'btn_stop
        '
        Me.btn_stop.Location = New System.Drawing.Point(165, 114)
        Me.btn_stop.Name = "btn_stop"
        Me.btn_stop.Size = New System.Drawing.Size(75, 66)
        Me.btn_stop.TabIndex = 3
        Me.btn_stop.Text = "中止"
        Me.btn_stop.UseVisualStyleBackColor = True
        '
        'Lbl_status
        '
        Me.Lbl_status.AutoSize = True
        Me.Lbl_status.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Lbl_status.Location = New System.Drawing.Point(163, 12)
        Me.Lbl_status.Name = "Lbl_status"
        Me.Lbl_status.Size = New System.Drawing.Size(86, 13)
        Me.Lbl_status.TabIndex = 18
        Me.Lbl_status.Text = "状況: 未接続"
        '
        'Cmb_comport
        '
        Me.Cmb_comport.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cmb_comport.FormattingEnabled = True
        Me.Cmb_comport.Location = New System.Drawing.Point(92, 8)
        Me.Cmb_comport.Name = "Cmb_comport"
        Me.Cmb_comport.Size = New System.Drawing.Size(64, 21)
        Me.Cmb_comport.Sorted = True
        Me.Cmb_comport.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 11)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(85, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "シリアルポート"
        '
        'BtnReadData
        '
        Me.BtnReadData.Location = New System.Drawing.Point(84, 114)
        Me.BtnReadData.Name = "BtnReadData"
        Me.BtnReadData.Size = New System.Drawing.Size(75, 66)
        Me.BtnReadData.TabIndex = 21
        Me.BtnReadData.Text = "ダウンロード"
        Me.BtnReadData.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "プログラムファイル (*.txt;*.bas)|*.txt;*.bas|All files (*.*)|*.*"
        Me.SaveFileDialog1.FilterIndex = 2
        '
        'Ichigotool
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(330, 187)
        Me.Controls.Add(Me.BtnReadData)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Cmb_comport)
        Me.Controls.Add(Me.Lbl_status)
        Me.Controls.Add(Me.btn_stop)
        Me.Controls.Add(Me.Btn_end)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Tb_message)
        Me.Controls.Add(Me.BtnWriteData)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Ichigotool"
        Me.Text = "Ichigoツール 1.03"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents OpenFileDialogData As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BtnWriteData As System.Windows.Forms.Button
    Friend WithEvents Tb_message As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Btn_end As System.Windows.Forms.Button
    Friend WithEvents btn_stop As System.Windows.Forms.Button
    Friend WithEvents Lbl_status As System.Windows.Forms.Label
    Friend WithEvents Cmb_comport As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents BtnReadData As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog

End Class
