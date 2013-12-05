Imports TvDatabase

Namespace Settings
    Public Class mySettings

        Private Shared _layer As New TvBusinessLayer

#Region "Properties"
        Private Shared m_dlgMessage As String = "Niveau ?!"
        Public Shared Property dlgMessage As String
            Get
                Return m_dlgMessage
            End Get
            Set(ByVal value As String)
                m_dlgMessage = value
            End Set
        End Property

        Private Shared m_dlgMessageShow As Boolean = True
        Public Shared Property dlgMessageShow As Boolean
            Get
                Return m_dlgMessageShow
            End Get
            Set(ByVal value As Boolean)
                m_dlgMessageShow = value
            End Set
        End Property

        Private Shared m_delay As Integer = 10
        Public Shared Property delay As Integer
            Get
                Return m_delay
            End Get
            Set(ByVal value As Integer)
                m_delay = value
            End Set
        End Property

        Private Shared m_BtnDeactivate As String = "ACTION_REMOTE_BLUE_BUTTON"
        Public Shared Property BtnDeactivate As String
            Get
                Return m_BtnDeactivate
            End Get
            Set(ByVal value As String)
                m_BtnDeactivate = value
            End Set
        End Property

        Private Shared m_BtnAdd As String = "ACTION_REMOTE_RED_BUTTON"
        Public Shared Property BtnAdd As String
            Get
                Return m_BtnAdd
            End Get
            Set(ByVal value As String)
                m_BtnAdd = value
            End Set
        End Property

        Private Shared m_debug As Boolean = False
        Public Shared Property debug As Boolean
            Get
                Return m_debug
            End Get
            Set(ByVal value As Boolean)
                m_debug = value
            End Set
        End Property

#End Region

#Region "Functions"
        Public Shared Sub Load(Optional ByVal log As Boolean = True)
            If log = True Then
                MyLog.Info("")
                MyLog.Info("-------------Load Settings---------------------")
            End If
            Dim _localPrefix As String = "myTvNiveauChecker."

            Dim clstype As Type = GetType(mySettings)
            Dim clsProperties As System.Reflection.PropertyInfo() = clstype.GetProperties()

            Dim _maxlenghtLog As Integer = clsProperties.Max(Function(x) x.Name.Length)

            For Each prop As System.Reflection.PropertyInfo In clsProperties.OrderBy(Function(x) x.Name)

                Dim _value As String = _layer.GetSetting(_localPrefix & prop.Name, prop.GetValue(clstype, Nothing).ToString).Value
                Select Case prop.PropertyType.Name
                    Case GetType(String).Name
                        prop.SetValue(clstype, CType(_value, String), Nothing)
                    Case GetType(Integer).Name
                        prop.SetValue(clstype, CType(_value, Integer), Nothing)
                    Case GetType(Boolean).Name
                        prop.SetValue(clstype, CType(_value, Boolean), Nothing)
                    Case GetType(Date).Name

                        prop.SetValue(clstype, CType(_value, Date), Nothing)
                End Select
                If log = True Then
                    MyLog.Debug("mySettings: {0}: {1}", _localPrefix & prop.Name, Space((_maxlenghtLog + 5) - prop.Name.Length) & prop.GetValue(clstype, Nothing).ToString)
                End If
            Next
            If log = True Then
                MyLog.Debug("-------------------------------------------------------")
            End If
        End Sub
        Public Shared Sub Save()

            Dim _localPrefix As String = "myTvNiveauChecker."

            Dim clstype As Type = GetType(mySettings)

            Dim clsProperties As System.Reflection.PropertyInfo() = clstype.GetProperties()
            For Each prop As System.Reflection.PropertyInfo In clsProperties

                Select Case prop.Name
                    Case Is = "pluginEnabledServer"
                        Exit Select
                    Case Else
                        Dim _setting As Setting = _layer.GetSetting(_localPrefix & prop.Name, prop.GetValue(clstype, Nothing).ToString)
                        _setting.Value = prop.GetValue(clstype, Nothing).ToString
                        _setting.Persist()
                End Select
            Next
        End Sub
#End Region

    End Class
End Namespace