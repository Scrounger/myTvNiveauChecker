Imports System.Windows.Forms
Imports System.Timers


Imports MediaPortal.Configuration
Imports MediaPortal.Dialogs
Imports MediaPortal.Player
Imports MediaPortal.GUI.Library
Imports TvControl
Imports TvDatabase

Imports myTvNiveauChecker.MediaPortal.TvDatabase
Imports myTvNiveauChecker.Settings
Imports System.Reflection

<PluginIcons("myTvNiveauChecker.Config.png", "myTvNiveauChecker.Config_disable.png")> _
Public Class myTvNiveauChecker

    Implements IPlugin
    Implements ISetupForm

#Region "Members"

    Private _idCurrentProgram As Integer = 0
    Private _CurrentEndTime As Date
    Private _CurrentProgram As Program
    Private _deactivated As Boolean = False

    Private _stateTimer As System.Timers.Timer
    Private _CheckTimer As System.Timers.Timer
    Private _CountDownTimer As System.Timers.Timer

    Private _CountDown As Integer

    Public Enum Match
        exact = 0
        contains = 1
    End Enum
#End Region


#Region "iSetupFormImplementation"

    Public Function Author() As String Implements ISetupForm.Author
        Return "Scrounger"
    End Function

    Public Function CanEnable() As Boolean Implements ISetupForm.CanEnable
        Return True
    End Function

    Public Function DefaultEnabled() As Boolean Implements ISetupForm.DefaultEnabled
        Return True
    End Function

    Public Function Description() As String Implements ISetupForm.Description
        Return "Describtion"
    End Function

    Public Function GetHome(ByRef strButtonText As String, ByRef strButtonImage As String, ByRef strButtonImageFocus As String, ByRef strPictureImage As String) As Boolean Implements ISetupForm.GetHome
        strButtonText = "myTvNiveauChecker"

        strButtonImage = String.Empty

        strButtonImageFocus = String.Empty

        strPictureImage = "Config.png"

    End Function

    Public Function GetWindowId() As Integer Implements ISetupForm.GetWindowId
        Return 15498
    End Function

    Public Function HasSetup() As Boolean Implements ISetupForm.HasSetup
        Return True
    End Function

    Public Function PluginName() As String Implements ISetupForm.PluginName
        Return "myTvNiveauChecker"
    End Function

    Public Sub ShowPlugin() Implements ISetupForm.ShowPlugin
        Dim setup As New setup
        setup.Show()
    End Sub

    Public Sub Start() Implements IPlugin.Start
        MyLog.BackupLogFiles()
        MyLog.Info("myTvNiveauChecker started")

        'Event Handler (channel change, tv started)
        AddHandler g_Player.TVChannelChanged, New g_Player.TVChannelChangeHandler(AddressOf Event_TvChannelChange)
        AddHandler g_Player.PlayBackStarted, New g_Player.StartedHandler(AddressOf Event_TvStarted)


        'Event Handler key press
        AddHandler GUIWindowManager.OnNewAction, AddressOf Event_GuiAction

        'AddHandler GUIWindowManager.OnNewAction, New GUIWindowManager.PostRenderActionHandler(AddressOf Event_Action)
        MyLog.Info("handler for g_player tv events registered")


    End Sub

    Public Sub [Stop]() Implements IPlugin.Stop
        MyLog.Info("myTvNiveauChecker stopped")
    End Sub
#End Region


  

#Region "Gui Events"
    Sub Event_TvStarted(ByVal type As g_Player.MediaType, ByVal filename As String)
        If g_Player.IsTV = True And g_Player.IsTVRecording = False Then
            MyLog.Info("")
            MyLog.Info("----------------myTvNiveauChecker started------------------")
            MyLog.Info("Version: " & Assembly.GetExecutingAssembly().GetName().Version.ToString)
            mySettings.Load(True)
            CheckTvDatabaseTable()

            _idCurrentProgram = 0
            getCurrentProgram()
            StartStopTimer(True)
        End If
    End Sub
    Private Sub Event_TvChannelChange()
        If g_Player.IsTV = True And g_Player.IsTVRecording = False Then
            Try
                CheckTimer(False)
            Catch ex As Exception
            End Try
            getCurrentProgram()
        End If
    End Sub

    Private Sub Event_GuiAction(ByVal action As Global.MediaPortal.GUI.Library.Action)
        'myTvNiveauChecker aktivieren / deaktivieren, by user key press (key defined in setup)

        If g_Player.IsTV = True And g_Player.IsTVRecording = False Then
            Select Case action.wID
                Case CType([Enum].Parse(GetType(Action.ActionType), mySettings.BtnDeactivate), Action.ActionType)
                    If _deactivated = False Then
                        _deactivated = True
                        MyLog.Info("myTvNiveauChecker deactivated")

                        Try
                            CheckTimer(False)
                        Catch ex As Exception
                        End Try

                        MP_Notify("deactivated")
                    Else
                        _deactivated = False
                        MyLog.Info("myTvNiveauChecker activated")
                        CheckTimer(True)
                        MP_Notify("activated")
                    End If
            End Select
        End If
    End Sub
#End Region



    Private Sub getCurrentProgram()
        Dim _userList As New List(Of IUser)

        Dim _cards = TvDatabase.Card.ListAll()
        For Each _card In _cards
            _userList.AddRange(RemoteControl.Instance.GetUsersForCard(_card.IdCard))
        Next

        If Not _userList.Count = 0 Then
            For Each user In _userList
                If user.Name.ToLower = My.Computer.Name.ToLower Then

                    'Program laden
                    Dim _channel = Channel.Retrieve(user.IdChannel)
                    Dim _program = _channel.GetProgramAt(Now)

                    If Not _program.IdProgram = _idCurrentProgram Then
                        _idCurrentProgram = _program.IdProgram
                        _CurrentEndTime = _program.EndTime

                        MyLog.Info("current program: {0} ({1})", _program.Title, _channel.DisplayName)
                        _CurrentProgram = _program

                        CheckTimer(True)

                    End If
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub StartStopTimer(ByVal startNow As Boolean)
        'timer der prüft ob sich das Progran geändert hat, also wechsel des Program auf gleichem Sender
        If startNow Then
            If _stateTimer Is Nothing Then
                MyLog.Debug("TvEvent timer started...")
                _stateTimer = New System.Timers.Timer()
                AddHandler _stateTimer.Elapsed, New ElapsedEventHandler(AddressOf programChanged)
                _stateTimer.Interval = 1000
                _stateTimer.AutoReset = True

                GC.KeepAlive(_stateTimer)
            End If
            _stateTimer.Start()
            _stateTimer.Enabled = True

        Else
            _stateTimer.Enabled = False
            _stateTimer.[Stop]()
            _idCurrentProgram = 0
            _CurrentProgram = Nothing
            MyLog.Debug("TvEvent timer stopped")
        End If
    End Sub
    Private Sub programChanged()
        If g_Player.IsTV = True And g_Player.IsTVRecording = False Then
            If _idCurrentProgram > 0 And Now > _CurrentEndTime Then
                getCurrentProgram()
            End If
        Else
            'Falls Player nicht mehr TV, dann timer beenden
            StartStopTimer(False)
        End If
    End Sub

    Private Sub CheckTimer(ByVal startNow As Boolean)
        'CountDown Timer, wird nur gestartet wenn program blacklisted ist!
        If startNow And _deactivated = False Then

            'wenn in myTNC gefunden, dann Countdown starten
            Dim _myTncList As List(Of myTNC) = myTNC.ListAll
            If _myTncList.FindAll(Function(x) InStr(_CurrentProgram.Title, x.name) > 0).Count > 0 Then

                Dim _match As myTNC = _myTncList.Find(Function(x) InStr(_CurrentProgram.Title, x.name) > 0)

                Select Case _match.matchRule
                    Case Is = myTvNiveauChecker.Match.contains.ToString
                        blacklisted()
                    Case Is = myTvNiveauChecker.Match.exact.ToString
                        If _CurrentProgram.Title = _match.name Then
                            blacklisted()
                        Else
                            MyLog.Info("{0}: clear (rule: {1})", _CurrentProgram.Title, _match.matchRule)
                            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
                        End If
                End Select
            Else
                MyLog.Info("{0}: clear ;)", _CurrentProgram.Title)
                GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
            End If
        Else
            _CheckTimer.Enabled = False
            _CheckTimer.[Stop]()

            _CountDownTimer.Enabled = False
            _CountDownTimer.[Stop]()

            _CountDown = 0
            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
            MyLog.Debug("myTvNiveauChecker Countdown stopped!")
        End If
    End Sub

    Private Sub blacklisted()
        MyLog.Info("{0}: blacklisted!", _CurrentProgram.Title)

        'skin properties ausgeben
        GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", True)

        _CountDown = mySettings.delay - 2
        If _CountDownTimer Is Nothing Then
            _CountDownTimer = New System.Timers.Timer()
            AddHandler _CountDownTimer.Elapsed, New ElapsedEventHandler(AddressOf countDownPropertiy)
            _CountDownTimer.Interval = 1000
            _CountDownTimer.AutoReset = True

            GC.KeepAlive(_CountDownTimer)
        End If

        If _CheckTimer Is Nothing Then
            _CheckTimer = New System.Timers.Timer()
            AddHandler _CheckTimer.Elapsed, New ElapsedEventHandler(AddressOf StopPlayer)
            _CheckTimer.Interval = mySettings.delay * 1000
            _CheckTimer.AutoReset = True

            GC.KeepAlive(_CheckTimer)
        End If

        MyLog.Debug("myTvNiveauChecker Countdown started ...")
        _CheckTimer.Start()
        _CheckTimer.Enabled = True

        _CountDownTimer.Start()
        _CountDownTimer.Enabled = True
    End Sub

    Private Sub countDownPropertiy()
        GUIPropertyManager.SetProperty("#myTvNiveauChecker.CountDown", _CountDown)
        _CountDown = _CountDown - 1
    End Sub

    Private Sub StopPlayer()

        CheckTimer(False)

        If mySettings.dlgMessageShow = True Then
            MsgBox(mySettings.dlgMessage)
        End If

        'Action Stop an MP schicken
        Dim action__1 As New Action(Action.ActionType.ACTION_STOP, 0, 0)
        GUIGraphicsContext.OnAction(action__1)
        GUIGraphicsContext.OnAction(action__1)
        MyLog.Info("Tv stopped!")

    End Sub

    Public Shared Sub CheckTvDatabaseTable()
        If Gentle.Framework.Broker.ProviderName = "MySQL" Then
            'MySQL
            Try
                Gentle.Framework.Broker.Execute("CREATE TABLE mptvdb.myTNC ( `Name` varchar(255) NOT NULL,  `matchRule` varchar(45),  PRIMARY KEY (`Name`))")
                MyLog.Info("myTNC table created")
            Catch ex As Exception
            End Try
        Else
            'MSSQL
            Try
                Gentle.Framework.Broker.Execute("CREATE TABLE [mptvdb].[dbo].[moveriesmapping] ( Name varchar(255) NOT NULL,  matchingRule varchar(45),  PRIMARY KEY (Name))")
                MyLog.Info("myTNC table created")
            Catch ex As Exception
            End Try
        End If
    End Sub

    Friend Shared Sub MP_Notify(ByVal message As String)
        Try
            Dim dlgContext As GUIDialogNotify = CType(GUIWindowManager.GetWindow(CType(GUIWindow.Window.WINDOW_DIALOG_NOTIFY, Integer)), GUIDialogNotify)
            dlgContext.Reset()

            dlgContext.SetHeading("myTvNiveauChecker")
            dlgContext.SetImage(Config.GetFile(Config.Dir.Skin, GUIGraphicsContext.SkinName & "\Media\" & "icon_myTvNiveauChecker.png"))


            dlgContext.SetText(message)
            dlgContext.TimeOut = 5

            dlgContext.DoModal(GUIWindowManager.ActiveWindow)
            dlgContext.Dispose()
            dlgContext.AllocResources()
        Catch ex As Exception
            MyLog.Error("[Helper]: [Notify]: exception err: {0} stack: {1}", ex.Message, ex.StackTrace)
        End Try
    End Sub

End Class
