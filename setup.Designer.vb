<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class setup
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvTNC = New System.Windows.Forms.DataGridView()
        Me.C_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Match = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TB_Name = New System.Windows.Forms.TextBox()
        Me.CB_matchRule = New System.Windows.Forms.ComboBox()
        Me.BT_Add = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TB_Message = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Check_ShowDlg = New System.Windows.Forms.CheckBox()
        Me.CB_Action = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Num_Timer = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BT_save = New System.Windows.Forms.Button()
        Me.Check_Debug = New System.Windows.Forms.CheckBox()
        Me.CB_ActionAdd = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        CType(Me.dgvTNC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.Num_Timer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvTNC
        '
        Me.dgvTNC.AllowUserToAddRows = False
        Me.dgvTNC.AllowUserToDeleteRows = False
        Me.dgvTNC.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvTNC.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dgvTNC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTNC.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.C_Name, Me.C_Match})
        Me.dgvTNC.Cursor = System.Windows.Forms.Cursors.Hand
        Me.dgvTNC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTNC.Location = New System.Drawing.Point(3, 16)
        Me.dgvTNC.Name = "dgvTNC"
        Me.dgvTNC.RowHeadersVisible = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.dgvTNC.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvTNC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTNC.Size = New System.Drawing.Size(329, 171)
        Me.dgvTNC.TabIndex = 2
        '
        'C_Name
        '
        Me.C_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.C_Name.HeaderText = "Name"
        Me.C_Name.Name = "C_Name"
        Me.C_Name.ReadOnly = True
        Me.C_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'C_Match
        '
        Me.C_Match.HeaderText = "Match"
        Me.C_Match.Name = "C_Match"
        Me.C_Match.Width = 80
        '
        'TB_Name
        '
        Me.TB_Name.Location = New System.Drawing.Point(75, 5)
        Me.TB_Name.Name = "TB_Name"
        Me.TB_Name.Size = New System.Drawing.Size(247, 20)
        Me.TB_Name.TabIndex = 3
        '
        'CB_matchRule
        '
        Me.CB_matchRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CB_matchRule.FormattingEnabled = True
        Me.CB_matchRule.Location = New System.Drawing.Point(75, 33)
        Me.CB_matchRule.Name = "CB_matchRule"
        Me.CB_matchRule.Size = New System.Drawing.Size(151, 21)
        Me.CB_matchRule.TabIndex = 29
        '
        'BT_Add
        '
        Me.BT_Add.Location = New System.Drawing.Point(241, 31)
        Me.BT_Add.Name = "BT_Add"
        Me.BT_Add.Size = New System.Drawing.Size(81, 23)
        Me.BT_Add.TabIndex = 30
        Me.BT_Add.Text = "add"
        Me.BT_Add.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "match type:"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.myTvNiveauChecker.My.Resources.Resources.Config
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(70, 61)
        Me.PictureBox1.TabIndex = 33
        Me.PictureBox1.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(88, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(259, 25)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "my TvNiveauChecker"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "dialog message:"
        '
        'TB_Message
        '
        Me.TB_Message.Location = New System.Drawing.Point(121, 13)
        Me.TB_Message.Name = "TB_Message"
        Me.TB_Message.Size = New System.Drawing.Size(204, 20)
        Me.TB_Message.TabIndex = 36
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.dgvTNC)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 79)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(335, 250)
        Me.GroupBox1.TabIndex = 37
        Me.GroupBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.BT_Add)
        Me.Panel1.Controls.Add(Me.TB_Name)
        Me.Panel1.Controls.Add(Me.CB_matchRule)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(3, 187)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(329, 60)
        Me.Panel1.TabIndex = 38
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox2.Controls.Add(Me.CB_ActionAdd)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Check_ShowDlg)
        Me.GroupBox2.Controls.Add(Me.CB_Action)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Num_Timer)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.TB_Message)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 335)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(335, 121)
        Me.GroupBox2.TabIndex = 38
        Me.GroupBox2.TabStop = False
        '
        'Check_ShowDlg
        '
        Me.Check_ShowDlg.AutoSize = True
        Me.Check_ShowDlg.Location = New System.Drawing.Point(213, 42)
        Me.Check_ShowDlg.Name = "Check_ShowDlg"
        Me.Check_ShowDlg.Size = New System.Drawing.Size(86, 17)
        Me.Check_ShowDlg.TabIndex = 41
        Me.Check_ShowDlg.Text = "Show Dialog"
        Me.Check_ShowDlg.UseVisualStyleBackColor = True
        '
        'CB_Action
        '
        Me.CB_Action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CB_Action.FormattingEnabled = True
        Me.CB_Action.Location = New System.Drawing.Point(121, 67)
        Me.CB_Action.Name = "CB_Action"
        Me.CB_Action.Size = New System.Drawing.Size(204, 21)
        Me.CB_Action.TabIndex = 40
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 70)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(109, 13)
        Me.Label7.TabIndex = 40
        Me.Label7.Text = "activate / deactivate:"
        '
        'Num_Timer
        '
        Me.Num_Timer.Location = New System.Drawing.Point(121, 41)
        Me.Num_Timer.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Num_Timer.Name = "Num_Timer"
        Me.Num_Timer.Size = New System.Drawing.Size(51, 20)
        Me.Num_Timer.TabIndex = 38
        Me.Num_Timer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.Num_Timer.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 37
        Me.Label5.Text = "delay timer:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(172, 43)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(12, 13)
        Me.Label6.TabIndex = 39
        Me.Label6.Text = "s"
        '
        'BT_save
        '
        Me.BT_save.Location = New System.Drawing.Point(266, 492)
        Me.BT_save.Name = "BT_save"
        Me.BT_save.Size = New System.Drawing.Size(81, 23)
        Me.BT_save.TabIndex = 39
        Me.BT_save.Text = "save"
        Me.BT_save.UseVisualStyleBackColor = True
        '
        'Check_Debug
        '
        Me.Check_Debug.AutoSize = True
        Me.Check_Debug.Location = New System.Drawing.Point(15, 496)
        Me.Check_Debug.Name = "Check_Debug"
        Me.Check_Debug.Size = New System.Drawing.Size(56, 17)
        Me.Check_Debug.TabIndex = 40
        Me.Check_Debug.Text = "debug"
        Me.Check_Debug.UseVisualStyleBackColor = True
        '
        'CB_ActionAdd
        '
        Me.CB_ActionAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CB_ActionAdd.FormattingEnabled = True
        Me.CB_ActionAdd.Location = New System.Drawing.Point(121, 94)
        Me.CB_ActionAdd.Name = "CB_ActionAdd"
        Me.CB_ActionAdd.Size = New System.Drawing.Size(204, 21)
        Me.CB_ActionAdd.TabIndex = 43
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 97)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(81, 13)
        Me.Label8.TabIndex = 42
        Me.Label8.Text = "add to blacklist:"
        '
        'setup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(357, 527)
        Me.Controls.Add(Me.Check_Debug)
        Me.Controls.Add(Me.BT_save)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "setup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "myTvNiveauChecker configuration"
        CType(Me.dgvTNC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.Num_Timer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvTNC As System.Windows.Forms.DataGridView
    Friend WithEvents TB_Name As System.Windows.Forms.TextBox
    Friend WithEvents CB_matchRule As System.Windows.Forms.ComboBox
    Friend WithEvents BT_Add As System.Windows.Forms.Button
    Friend WithEvents C_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Match As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TB_Message As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Num_Timer As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BT_save As System.Windows.Forms.Button
    Friend WithEvents CB_Action As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Check_Debug As System.Windows.Forms.CheckBox
    Friend WithEvents Check_ShowDlg As System.Windows.Forms.CheckBox
    Friend WithEvents CB_ActionAdd As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
