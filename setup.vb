Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.CompilerServices

Imports MediaPortal.GUI.Library
Imports MediaPortal.Dialogs
Imports MediaPortal.Profile
Imports MediaPortal.Configuration
Imports MediaPortal.Utils
Imports MediaPortal.Util
Imports TvDatabase
Imports MediaPortal.Player

Imports Gentle.Common

Imports TvEngine.PowerScheduler.Interfaces

Imports MediaPortal.Database
Imports SQLite.NET

Imports System.Threading

Imports Action = MediaPortal.GUI.Library.Action
Imports Gentle.Framework
Imports System.Drawing
Imports MediaPortal.Playlists
Imports TvControl
Imports myTvNiveauChecker.myTvNiveauChecker
Imports myTvNiveauChecker.MediaPortal.TvDatabase
Imports myTvNiveauChecker.Settings



Public Class setup

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim bla As New List(Of notAllowed)

        'For i = 0 To 10
        '    Dim tmp As New notAllowed("test" & i, myTvNiveauChecker.myTvNiveauChecker.Match.exact)
        '    bla.Add(tmp)
        'Next


        'MsgBox(bla.Count)

    End Sub

    Private Sub setup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyLog.Info(String.Empty)
        MyLog.Info(String.Empty)
        MyLog.Info("Configuration loaded...")

        mySettings.Load(True)

        myTvNiveauChecker.CheckTvDatabaseTable()

        Dim _DeleteButton As New DataGridViewButtonColumn
        With _DeleteButton
            .DefaultCellStyle.Padding = New Padding(2, 6, 2, 6)
            .HeaderText = ""
            .Text = "Delete"
            .Name = "Delete"
            '.FlatStyle = FlatStyle.Flat
            .UseColumnTextForButtonValue = True
            '_editbutton.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Width = 60
        End With

        Me.dgvTNC.Columns.Add(_DeleteButton)

        For Each _match In [Enum].GetValues(GetType(myTvNiveauChecker.Match))
            CB_matchRule.Items.Add(_match.ToString)

            If _match = myTvNiveauChecker.Match.exact Then
                CB_matchRule.Text = _match.ToString
            End If
        Next

        TB_Message.Text = mySettings.dlgMessage
        Num_Timer.Value = mySettings.delay

        FillDGV()


        CB_Action.DataSource = System.Enum.GetValues(GetType(Action.ActionType))
        CB_Action.Text = mySettings.BtnDeactivate

        CB_ActionAdd.DataSource = System.Enum.GetValues(GetType(Action.ActionType))
        CB_ActionAdd.Text = mySettings.BtnAdd

        Check_Debug.Checked = mySettings.debug

        Check_ShowDlg.Checked = mySettings.dlgMessageShow
        TB_Message.Enabled = mySettings.dlgMessageShow

    End Sub

    Private Sub FillDGV()
        dgvTNC.Rows.Clear()

        Try
            Dim _myTNCs As List(Of myTNC) = myTNC.ListAll

            For i = 0 To _myTNCs.Count - 1
                Me.dgvTNC.Rows.Add(_myTNCs(i).name, _myTNCs(i).matchRule)
                Me.dgvTNC.Item(C_Name.Name, i).Style.Font = New Font(Me.dgvTNC.Font, FontStyle.Bold)
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub dgvTNC_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTNC.CellClick
        Try
            Select Case (e.ColumnIndex)
                Case Is = 2

                    Dim antwort As MsgBoxResult
                    antwort = MsgBox(Me.dgvTNC(C_Name.Name, e.RowIndex).Value() & " delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2)

                    If antwort = MsgBoxResult.Yes Then
                        Dim _tnc As myTNC = myTNC.Retrieve(Me.dgvTNC(C_Name.Name, e.RowIndex).Value())
                        _tnc.Remove()
                        FillDGV()
                    End If

            End Select
        Catch ex As Exception
            MyLog.Error("[dgvTNC_CellClick]: exception err:" & ex.Message & " stack:" & ex.StackTrace)
        End Try
    End Sub

    Private Sub BT_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Add.Click
        If Not String.IsNullOrEmpty(TB_Name.Text) Then
            Dim _tnc As New myTNC(TB_Name.Text)
            _tnc.matchRule = CB_matchRule.Text
            _tnc.Persist()

            FillDGV()
        End If
    End Sub

    Private Sub BT_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_save.Click
        mySettings.dlgMessage = TB_Message.Text
        mySettings.delay = Num_Timer.Value
        mySettings.BtnDeactivate = CB_Action.Text
        mySettings.BtnAdd = CB_ActionAdd.Text
        mySettings.debug = Check_Debug.Checked
        mySettings.dlgMessageShow = Check_ShowDlg.Checked

        mySettings.Save()
        Me.Close()

    End Sub

    Private Sub Check_ShowDlg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Check_ShowDlg.CheckedChanged
        mySettings.dlgMessageShow = Check_ShowDlg.Checked
        TB_Message.Enabled = mySettings.dlgMessageShow
    End Sub

End Class