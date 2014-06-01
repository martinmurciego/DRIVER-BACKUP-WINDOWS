Namespace CommandLineManager


    Public Class CommandLineBuilder
        Dim regxFilter As String = "(?<NAME>\w+)\s*\=\s*\x22(?<VALUE>.+?)\x22"
        'Generali
        Public Filter As DeviceFilter
        Public Mode As Integer = -1
        Public Logging As Boolean = True
        Public LogFileName As String
        'Informazioni BACKUP
        Public BackupPath As String = ""
        Public BackupPathFormat As String = ""
        Public BackupDevFormat As String = ""
        Public BackupFileName As String = ""
        Public BackupDescription As String = ""
        Public BackupDateFormat As String = ""
        Public SystemDirectory As String = ""
        Public UseOfflineComputerName As Boolean = False
        Public OverwriteFile As Boolean = False
        Public GenerateAutorun As Boolean = False
        'Informazioni RESTORE
        Public RestoreFileName As String = ""
        'Public RestorePath As String = ""
        Public UpdateOEMInf As Boolean = False
        Public EnabledPnPRescan As Boolean = False
        Public DisableInteraction As Boolean = False


        Public Sub New()
            Filter = New DeviceFilter(False, DeviceFilter.DeviceFilterProviders.Prov_All, -1, Nothing) 'Impostazioni di filtro standard
        End Sub

        Public Function Read(ByVal cmdArgs As String) As Boolean
            'Carica le impostazioni da cmdArgs
            'Riconosce i comandi inviati sulla riga di comando
            Dim parsed As Integer = 0
            Dim value As String

            Try
                For Each arg As Match In Regex.Matches(cmdArgs, Me.regxFilter)
                    value = arg.Groups("VALUE").Value 'Valore della proprietà corrente
                    If value Is Nothing Then value = ""

                    Select Case arg.Groups("NAME").Value
                        Case Is = "MODE"
                            'Imposta la modalità
                            Select Case value
                                Case Is = "BACKUP"
                                    Me.Mode = 0
                                Case Is = "RESTORE"
                                    Me.Mode = 1
                                Case Else
                                    Me.Mode = -1
                            End Select
                        Case Is = "SYSPATH"
                            Me.SystemDirectory = value
                        Case Is = "BKDESC"
                            Me.BackupDescription = value
                        Case Is = "BKPATH"
                            Me.BackupPath = value
                        Case Is = "BKFILE"
                            Me.RestoreFileName = value
                            Me.BackupFileName = value
                        Case Is = "BKPATHFMT"
                            Me.BackupPathFormat = value
                        Case Is = "BKDEVFMT"
                            Me.BackupDevFormat = value
                        Case Is = "BKDATEFMT"
                            Me.BackupDateFormat = value
                        Case Is = "OPT" 'OPZIONALE
                            'Estrae dalla stringa le opzioni da abilitare
                            'Produttore
                            If value.Contains("A") Then
                                Me.Filter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_All
                            End If

                            If value.Contains("M") Then
                                Me.Filter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_Oem
                            End If

                            If value.Contains("H") Then
                                Me.Filter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_Others
                            End If

                            If value.Contains("D") Then
                                Me.Filter.ProviderType = -1
                            End If
                            'Firma digitale
                            If value.Contains("S") Then
                                Me.Filter.MustSigned = True
                            Else
                                Me.Filter.MustSigned = False
                            End If
                            'Portabilità
                            If value.Contains("P") Then
                                Me.Filter.Portability = DevicePortability.DCmp_Full
                            Else
                                Me.Filter.Portability = -1
                            End If

                            If value.Contains("R") Then
                                Me.GenerateAutorun = True
                            Else
                                Me.GenerateAutorun = False
                            End If

                            If value.Contains("U") Then
                                Me.UpdateOEMInf = True
                            Else
                                Me.UpdateOEMInf = False
                            End If

                            If value.Contains("L") Then
                                Me.Logging = True
                            Else
                                Me.Logging = False
                            End If

                            If value.Contains("W") Then
                                Me.OverwriteFile = True
                            Else
                                Me.OverwriteFile = False
                            End If

                            If value.Contains("N") Then
                                Me.EnabledPnPRescan = True
                            Else
                                Me.EnabledPnPRescan = False
                            End If

                            If value.Contains("V") Then
                                Me.DisableInteraction = True
                            Else
                                Me.DisableInteraction = False
                            End If

                            If value.Contains("O") Then
                                Me.UseOfflineComputerName = True
                            Else
                                Me.UseOfflineComputerName = False
                            End If
                        Case Is = "LOG"
                            Me.LogFileName = value
                    End Select
                    parsed += 1
                Next

                If parsed = 0 Then Return False

                Return True

            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function Build() As String
            'Costruisce dalle impostazioni correnti una riga di comando valida
            Try
                Dim sBuild As New StringBuilder

                'Inserisce la modalità di funzionamento
                Select Case Me.Mode
                    Case Is = 0
                        sBuild.Append(" MODE=""BACKUP""")
                        'Percorso
                        sBuild.Append(" BKPATH=""" & Me.BackupPath & """")
                        'Descrizione
                        sBuild.Append(" BKDESC=""" & Me.BackupDescription & """")
                        'File
                        sBuild.Append(" BKFILE=""" & Me.BackupFileName & """")
                        'Formato percorso
                        sBuild.Append(" BKPATHFTM=""" & Me.BackupPathFormat & """")
                        'Formato device
                        sBuild.Append(" BKDEVFMT=""" & Me.BackupDevFormat & """")
                        'Formato data
                        sBuild.Append(" BKDATEFMT=""" & Me.BackupDateFormat & """")
                        'Modalità offline
                        If Not [String].IsNullOrEmpty(Me.SystemDirectory) Then
                            sBuild.Append(" SYSPATH=""" & Me.SystemDirectory & """")
                        End If
                    Case Is = 1
                        sBuild.Append(" MODE=""RESTORE""")
                        'File
                        sBuild.Append(" BKFILE=""" & Me.RestoreFileName & """")
                        'PErcorso
                        'sBuild.Append(" RSPATH=""" & Me.RestorePath & """ ")
                End Select

                'Inserisce le opzioni
                sBuild.Append(" OPT=""")
                Select Case Me.Filter.ProviderType
                    Case Is = DeviceFilter.DeviceFilterProviders.Prov_All
                        sBuild.Append("A")
                    Case Is = DeviceFilter.DeviceFilterProviders.Prov_Oem
                        sBuild.Append("M")
                    Case Is = DeviceFilter.DeviceFilterProviders.Prov_Others
                        sBuild.Append("H")
                    Case Is = -1
                        sBuild.Append("D")
                End Select

                If Me.Filter.MustSigned Then sBuild.Append("S")
                If Me.Filter.Portability = DevicePortability.DCmp_Full Then sBuild.Append("P")
                If Me.GenerateAutorun Then sBuild.Append("R")
                If Me.UpdateOEMInf Then sBuild.Append("U")
                If Me.Logging Then sBuild.Append("L")
                If Me.OverwriteFile Then sBuild.Append("W")
                If Me.EnabledPnPRescan Then sBuild.Append("N")
                If Me.DisableInteraction Then sBuild.Append("V")
                If Me.UseOfflineComputerName Then sBuild.Append("O")
                'Termina la stringa opzioni
                sBuild.Append(""" ")

                'FILE log
                If Not [String].IsNullOrEmpty(Me.LogFileName) Then
                    sBuild.Append(" LOG=""" & Me.LogFileName & """")
                End If

                Return sBuild.ToString

            Catch ex As Exception
                Return [String].Empty
            End Try
        End Function

    End Class

    Public Class CommandLine
        'Gestisce la riga di comando
        Dim conHandle As Integer
        Dim args As String

        'Oggetti principale
        Dim WithEvents devBackup As DeviceBackup
        Dim WithEvents devRestore As DeviceRestore

        'Impostazioni
        Dim regxFilter As String = "(?<NAME>\w+)\s*\=\s*\x22(?<VALUE>.+?)\x22"
        Dim cReader As CommandLineBuilder
        Dim currList As DeviceCollection
        Dim totalDevices As Integer
        Dim logFile As New TextFormatters.TXTFormatter
        Dim verboseMode As Boolean

        Public Sub New(ByVal commandArgs As String)
            Me.cReader = New CommandLineBuilder
            Me.args = commandArgs
        End Sub


        Private Function Validate() As Boolean
            'Corregge la riga di comando o eventualmente scatena un errore
            With Me.cReader
                Select Case .Mode
                    Case Is = 0
                        'Modo Backup
                        If [String].IsNullOrEmpty(.BackupPath) Then
                            Console.WriteLine(GetLangStr("CONSOLE_DIRECTORY"))
                            Return False
                        End If

                        'Crea la directory principale se non esiste
                        If Not Directory.Exists(.BackupPath) Then
                            Try
                                Directory.CreateDirectory(.BackupPath)
                            Catch ex As Exception
                                Console.WriteLine(GetLangStr("CONSOLE_CANTCREATEDIR"))
                                Return False
                            End Try
                        End If
                        'Corregge gli eventuali parametri opzionali
                        If [String].IsNullOrEmpty(.BackupPathFormat) Then .BackupPathFormat = My.Settings.StdBackupPathFormat
                        If [String].IsNullOrEmpty(.BackupDevFormat) Then .BackupDevFormat = My.Settings.StdDevicePathFormat
                        If [String].IsNullOrEmpty(.BackupFileName) Then .BackupFileName = My.Settings.StdBackupInfoFile
                        If [String].IsNullOrEmpty(.BackupDateFormat) Then .BackupDateFormat = My.Settings.DateTimePattern
                        'Imposta un file log di default se richiesto
                        If .Logging = True And [String].IsNullOrEmpty(.LogFileName) Then
                            .LogFileName = Path.Combine(.BackupPath, My.Settings.StdLogFileName)
                        End If

                    Case Is = 1
                        'Modo Restore
                        'Mancato inserimento del file BKI
                        If [String].IsNullOrEmpty(.RestoreFileName) Then
                            Console.WriteLine(GetLangStr("CONSOLE:FILE"))
                            Return False
                        End If

                        'Imposta un file log di default se richiesto
                        If .Logging = True And [String].IsNullOrEmpty(.LogFileName) Then
                            .LogFileName = Path.Combine(My.Application.Info.DirectoryPath, My.Settings.StdLogFileName)
                        End If

                    Case Else
                        'Errore nell'inserimento della modalità
                        Console.WriteLine(GetLangStr("CONSOLE:BADCOMMAND"))
                        Return False
                End Select
            End With
            Return True

        End Function

        Public Function Execute() As Boolean
            Dim offLineObj As DeviceBackupOffline = Nothing

            Try
                Dim tempList As DeviceCollection

                If cReader.Read(Me.args) = False Then
                    Return False
                End If

                'Crea la console
                Utils.AllocConsole()

                'Visualizza il messaggio di benvenuto
                Console.WriteLine("DriverBackup! " & My.Application.Info.Version.ToString & " by Giuseppe Greco 2009-2011")
                Console.WriteLine("Free driver management software. GPL License")

                Console.WriteLine()

                If Validate() = False Then
                    Console.WriteLine(GetLangStr("CONSOLE:MISSINGINFO"))
                    Return True
                End If

                With cReader
                    Select Case .Mode
                        Case Is = 0
                            'Imposta la configurazione per il backup
                            Console.WriteLine(GetLangStr("CONSOLE:INFOCOLLECT"))

                            logFile = New TextFormatters.TXTFormatter

                            If Not [String].IsNullOrEmpty(.SystemDirectory) Then
                                'Configura il modo offline
                                offLineObj = DeviceBackupOffline.Create(.SystemDirectory)
                                If offLineObj Is Nothing Then
                                    logFile.AddMsgError(GetLangStr("FRMOFFLINE_GENERIC"), False)
                                    Console.WriteLine(GetLangStr("FRMOFFLINE_GENERIC"))
                                    Return False
                                End If

                                If offLineObj.HasPathError Then
                                    logFile.AddMsgError(GetLangStr("FRMOFFLINE_PATH"), False)
                                    Console.WriteLine(GetLangStr("FRMOFFLINE_PATH"))
                                    Return False
                                End If

                                If offLineObj.HasPrivilegeError Then
                                    logFile.AddMsgError(GetLangStr("FRMOFFLINE_PRIVILEGE"), False)
                                    Console.WriteLine(GetLangStr("FRMOFFLINE_PRIVILEGE"))
                                    Return False
                                End If
                               
                                offLineObj.UseOfflinePCName = .UseOfflineComputerName
                            End If


                            tempList = DeviceCollection.Create(Nothing)

                            If tempList Is Nothing Then Throw New ArgumentNullException 'Errore imprevisto

                            If tempList.Count <= 0 Then 'Impossibile accedere al registro
                                Console.WriteLine(GetLangStr("ERROR:RegistryAccess"))
                                Return True
                            End If

                            If .Filter.ProviderType = -1 Then .Filter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_Others
                            tempList.SetDevicesProperties(.Filter, "Selected", GetType(Boolean), True, False)
                            currList = DeviceCollection.Create(tempList, "Selected", True)

                            If currList Is Nothing Then Throw New ArgumentNullException 'Errore imprevisto

                            If currList.Count <= 0 Then 'Nessun device selezionato
                                Console.WriteLine(GetLangStr(GetLangStr("FRMMAIN:NODEVICES")))
                                Return True
                            End If
                            totalDevices = currList.Count
                            'Reinizializza il file log

                            devBackup = New DeviceBackup(currList, .BackupPath, My.Settings.DateTimePattern)
                            devBackup.FileManager = New BRStdFileManager(.BackupPath)
                            devBackup.BackupPathFormat = .BackupPathFormat
                            devBackup.BackupDateFormat = .BackupDateFormat
                            devBackup.DevicePathFormat = .BackupDevFormat
                            devBackup.BackupInfoFile = .BackupFileName
                            devBackup.CanOverwrite = .OverwriteFile
                            devBackup.Description = .BackupDescription
                            Console.WriteLine("Ok.")
                            'Procede con il backup effettivo
                            devBackup.Backup()
                            'Genera se richiesto l'autorun
                            If .GenerateAutorun Then
                                Utils.GenerateAutorun(.BackupPath, [String].Format(My.Settings.StdRestoreCmdLine, devBackup.BackupInfoFile, My.Settings.StdRestorePath), Path.GetDirectoryName(Application.ExecutablePath), CommonVariables.GetLanguageFiles)
                            End If

                        Case Is = 1
                            'Prepara la configurazione del ripristino
                            Console.WriteLine(GetLangStr("CONSOLE:INFOCOLLECT"))
                            devRestore = DeviceRestore.Create(.RestoreFileName)

                            If devRestore Is Nothing Then
                                Console.WriteLine(GetLangStr("CONSOLE:FILE"))
                                Return False
                            End If

                            logFile = New TextFormatters.TXTFormatter

                            If .Filter.ProviderType <> -1 Then
                                'Applica un filtro solo se l'utente sceglie una specifica categoria
                                totalDevices = devRestore.DeviceList.SetDevicesProperties(.Filter, "Selected", GetType(Boolean), True, False)
                            End If
                            devRestore.UpdateDeviceInfo = .UpdateOEMInf
                            Console.WriteLine("Ok.")
                            'Effettua il ripristino dei devices selezionati
                            devRestore.RestoreDevices()
                    End Select
                End With

                Console.WriteLine(GetLangStr("CONSOLE:OPEND"))
                If Not cReader.DisableInteraction Then
                    Console.ReadLine()
                End If

                Return True
            Catch ex As Exception
                Console.WriteLine(GetLangStr("ERROR:BERR"))
                Return True
            Finally
                'Rilascia le risorse utilizzate
                If offLineObj IsNot Nothing Then offLineObj.Dispose()
                If logFile IsNot Nothing And Not [String].IsNullOrEmpty(cReader.LogFileName) Then
                    logFile.Write(cReader.LogFileName)
                    logFile.Dispose()
                End If
            End Try
        End Function


        Private Sub devBackup_BackupBeginDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs) Handles devBackup.BackupBeginDevice
            logFile.AddDevice(e.Source)
            'Notifica sulla console
            Console.WriteLine(e.Source.Description)
        End Sub

        Private Sub devBackup_BackupDeviceError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs) Handles devBackup.BackupDeviceError
            Dim errNotify As Boolean = False

            If e.Code = BackupRestoreErrorCodes.BRE_FileOverwiting Then
                'Aggiunge il nome del file che ha causato l'errore
                logFile.AddMsgError(ControlChars.Tab & GetLangStr(e.Code) & ": " & e.Data("Filename"), True)
                errNotify = True
            End If

            If e.Code = BackupRestoreErrorCodes.BRE_FileIOError Then
                'Aggiunge maggiori informazioni
                logFile.AddMsgError(ControlChars.Tab & e.Data("Msg"), True)
                errNotify = True
            End If

            If Not errNotify Then logFile.AddError(e.Code, True)
        End Sub

        Private Sub devBackup_BackupEndDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs) Handles devBackup.BackupEndDevice
            logFile.EndDevice(e.HasErrors)
            'Notifica sulla console
            If e.HasErrors Then
                Console.WriteLine(GetLangStr("LOG_DeviceError"))
            Else
                Console.WriteLine(GetLangStr("LOG_DeviceOK"))
            End If
        End Sub

        Private Sub devBackup_BackupEnded(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs) Handles devBackup.BackupEnded
            logFile.EndOperation([String].Format(GetLangStr("FRMBACK:ENDBACKUP"), e.TotalDevices, totalDevices))
            'Notifica sulla console
            Console.WriteLine([String].Format(GetLangStr("FRMBACK:ENDBACKUP"), e.TotalDevices, totalDevices))
        End Sub

        Private Sub devBackup_BackupError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs) Handles devBackup.BackupError
            logFile.AddError(e.Code, False)
        End Sub

        Private Sub devBackup_BackupFile(ByVal sender As Object, ByVal e As DeviceBackupRestore.FileEventArgs) Handles devBackup.BackupFile
            logFile.AddFile(e.FileName)
        End Sub

        Private Sub devBackup_BackupStarted(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs) Handles devBackup.BackupStarted
            logFile.BeginOperation([String].Format(GetLangStr("FRMBACK:BEGINBACKUP"), e.TotalDevices))
            'Notifica sulla console
            Console.WriteLine([String].Format(GetLangStr("FRMBACK:BEGINBACKUP"), e.TotalDevices))
        End Sub


        Private Sub devRestore_RestoreBegin(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs) Handles devRestore.RestoreBegin
            logFile.BeginOperation([String].Format(GetLangStr("FRMRESTORE:BEGINRESTORE"), e.TotalDevices))
            Console.WriteLine([String].Format(GetLangStr("FRMRESTORE:BEGINRESTORE"), e.TotalDevices))

        End Sub

        Private Sub devRestore_RestoreBeginDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs) Handles devRestore.RestoreBeginDevice

            logFile.AddDevice(e.Source)
            Console.WriteLine(e.Source.Description)

            If cReader.DisableInteraction Then
                e.Cancel = False
            Else
                Console.Write(ControlChars.Tab & GetLangStr("CONSOLE_RESTORECONFIRM"))
                If Console.ReadKey.Key = ConsoleKey.Y Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If
                Console.WriteLine()
            End If
        End Sub

        Private Sub devRestore_RestoreDeviceError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs) Handles devRestore.RestoreDeviceError
            If e.Code = BackupRestoreErrorCodes.BRE_ForceUpdate And Not cReader.DisableInteraction Then
                'Gestisce la forzatura
                Dim msg As String = ControlChars.Tab & [String].Format(GetLangStr(e.Code.ToString), DirectCast(e.Data("Device"), Device).Description) & " (y/n)"
                Console.Write(msg)
                If Console.ReadKey.Key = ConsoleKey.Y Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If
                Console.WriteLine()
                Return
            End If

            If e.Code = BackupRestoreErrorCodes.BRE_Generic Then
                'Aggiunge maggiori informazioni
                logFile.AddMsgError(ControlChars.Tab & e.Data("Msg"), True)
                Return
            End If

            Console.WriteLine(ControlChars.Tab & GetLangStr(e.Code.ToString))

            logFile.AddError(e.Code, True)
        End Sub

        Private Sub devRestore_RestoreEnd(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs) Handles devRestore.RestoreEnd
            logFile.EndOperation([String].Format(GetLangStr("FRMRESTORE:ENDRESTORE"), e.TotalDevices, totalDevices))
            Console.WriteLine([String].Format(GetLangStr("FRMRESTORE:ENDRESTORE"), e.TotalDevices, totalDevices))

            If Me.cReader.EnabledPnPRescan Then
                'Aggiorna la configurazione PnP
                If DeviceRestore.PnPConfigUpdate = True Then
                    'Configurazione aggiornata
                    Console.WriteLine(GetLangStr("FRMRESTORE:PNPRESCAN"))
                Else
                    'Impossibile aggiornare la configurazione
                    Console.WriteLine(GetLangStr("FRMRESTORE:PNPRESCANFAILED"))
                End If
            End If
        End Sub

        Private Sub devRestore_RestoreEndDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs) Handles devRestore.RestoreEndDevice
            If e.Data.ContainsKey("OEMINF") AndAlso Not [String].IsNullOrEmpty(e.Data.Item("OEMINF")) Then
                logFile.AddMsg([String].Format(ControlChars.Tab & GetLangStr("FRMRESTORE:OEMINF"), e.Data("OEMINF")), True)
            End If

            logFile.EndDevice(e.HasErrors)
            'Notifica sulla console
            If e.HasErrors Then
                Console.WriteLine(ControlChars.Tab & GetLangStr("LOG_DeviceError"))
            Else
                Console.WriteLine(ControlChars.Tab & GetLangStr("LOG_DeviceOK"))
            End If
        End Sub

        Private Sub devRestore_RestoreError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs) Handles devRestore.RestoreError
            logFile.AddMsgError(e.Data.Item("Msg"), False)
        End Sub
    End Class

End Namespace