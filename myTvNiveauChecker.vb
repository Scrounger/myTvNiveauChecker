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
Imports myTvNiveauChecker.Language
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
        MyLog.Info("")
        MyLog.Info("----------------myTvNiveauChecker started------------------")
        Translator.TranslateSkin()
        GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
        GUIPropertyManager.SetProperty("#myTvNiveauChecker.CountDown", 0)

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
            mySettings.Load(True)
            CheckTvDatabaseTable()
            MyLog.Info("Version: " & Assembly.GetExecutingAssembly().GetName().Version.ToString)

            _deactivated = False
            _idCurrentProgram = 0

            TvEventTimer(True)
        End If
    End Sub

    Private _ChannelChangeCounter As Integer = 1
    Private Sub Event_TvChannelChange()
        If _ChannelChangeCounter = 1 Then
            'GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
            _ChannelChangeCounter = 2
        Else
            _ChannelChangeCounter = 1
            If g_Player.IsTV = True And g_Player.IsTVRecording = False Then
                Try
                    CheckTimer(False)
                Catch ex As Exception
                End Try
                getCurrentProgram()
            End If
        End If
    End Sub

    Private Sub Event_GuiAction(ByVal action As Global.MediaPortal.GUI.Library.Action)
        'myTvNiveauChecker aktivieren / deaktivieren, by user key press (key defined in setup)
        'nur aktiv in TvHomeServer & Fullscreen (window ids 1/ 602)

        If GUIWindowManager.ActiveWindow = 1 Or GUIWindowManager.ActiveWindow = 602 Then
            If g_Player.IsTV = True And g_Player.IsTVRecording = False Then

                'Window spezifische tasten abfangen um prop zu reseten (sonst wird das erst nach kanal wechsel ausgeführt)
                If GUIWindowManager.ActiveWindow = 1 Then
                    Select Case action.wID
                        Case Is = action.ActionType.ACTION_PAGE_UP
                            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
                        Case Is = action.ActionType.ACTION_PAGE_DOWN
                            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
                        Case Is = action.ActionType.ACTION_STOP
                            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
                            TvEventTimer(False)
                    End Select
                End If

                If GUIWindowManager.ActiveWindow = 602 Then
                    Select Case action.wID
                        Case Is = action.ActionType.ACTION_STOP
                            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
                            TvEventTimer(False)
                        Case Is = action.ActionType.ACTION_NEXT_CHANNEL
                            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
                        Case Is = action.ActionType.ACTION_PREV_CHANNEL
                            GUIPropertyManager.SetProperty("#myTvNiveauChecker.active", False)
                    End Select
                End If

                'myTvNiveauChecker actions
                Select Case action.wID
                    Case CType([Enum].Parse(GetType(Action.ActionType), mySettings.BtnDeactivate), Action.ActionType)
                        If _deactivated = False Then
                            _deactivated = True
                            MyLog.Info("myTvNiveauChecker deactivated")

                            Try
                                CheckTimer(False)
                            Catch ex As Exception
                            End Try

                            MP_Notify(Translation.deactivated)
                        Else
                            _deactivated = False
                            MyLog.Info("myTvNiveauChecker activated")
                            CheckTimer(True)
                            MP_Notify(Translation.activated)
                        End If
                    Case CType([Enum].Parse(GetType(Action.ActionType), mySettings.BtnAdd), Action.ActionType)
                        If Not _CountDownTimer.Enabled = True Then
                            Try
                                Dim _test As myTNC = myTNC.Retrieve(_CurrentProgram.Title)
                                MP_Notify(Translation.stilladded)
                            Catch ex As Exception
                                MsgBox("add to table?")

                            End Try
                        End If
                End Select

               
            End If
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

    Private Sub TvEventTimer(ByVal startNow As Boolean)
        'timer der prüft ob sich das Progran geändert hat, also wechsel des Program auf gleichem Sender        
        If startNow Then
            If _stateTimer Is Nothing Then
                MyLog.Info("TvEvent timer started...")
                getCurrentProgram()


                _stateTimer = New System.Timers.Timer()
                AddHandler _stateTimer.Elapsed, New ElapsedEventHandler(AddressOf programChanged)
                _stateTimer.Interval = 1000
                _stateTimer.AutoReset = True

                GC.KeepAlive(_stateTimer)
            End If
            _stateTimer.Start()
            _stateTimer.Enabled = True

        Else
            _idCurrentProgram = 0
            _CurrentProgram = Nothing

            Try
                CheckTimer(False)
            Catch ex As Exception
            End Try

            _stateTimer.Enabled = False
            _stateTimer.[Stop]()

            'Event Handler key press
            'RemoveHandler GUIWindowManager.OnNewAction, AddressOf Event_GuiAction
            _stateTimer = Nothing

            MyLog.Info("TvEvent timer stopped")
            MyLog.Info("-------------------------------------------------------")
        End If
    End Sub
    Private Sub programChanged()
        If g_Player.IsTV = True And g_Player.IsTVRecording = False Then
            If _idCurrentProgram > 0 And Now > _CurrentEndTime Then
                getCurrentProgram()
            End If
        Else
            'Falls Player nicht mehr TV, dann alle timer beenden
            TvEventTimer(False)
        End If
    End Sub

    Private Sub CheckTimer(ByVal startNow As Boolean)
        'CountDown Timer, wird nur gestartet wenn program blacklisted ist!
        If startNow And _deactivated = False Then

            MyLog.Debug("myTvNiveauChecker CheckTimer started!")

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

            MyLog.Debug("myTvNiveauChecker CheckTimer stopped!")
        End If
    End Sub

    Private Sub blacklisted()
        MyLog.Warn("{0}: blacklisted!", _CurrentProgram.Title)

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
            MP_Notify(mySettings.dlgMessage)
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
                MyLog.Warn(ex.Message)
            End Try
        Else
            'MSSQL
            Try
                Gentle.Framework.Broker.Execute("CREATE TABLE [mptvdb].[dbo].[myTNC] ( Name varchar(255) NOT NULL,  matchingRule varchar(45),  PRIMARY KEY (Name))")
                MyLog.Info("myTNC table created")
            Catch ex As Exception
                MyLog.Warn(ex.Message)
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
