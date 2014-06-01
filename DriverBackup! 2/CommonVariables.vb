
Imports DriverBackup__2.LanguageManager

Module CommonVariables

    'Contiene variabili condivise dall'applicazione

    Private langStrs As New Dictionary(Of String, String)
    Private lang As String

    Public Enum HelpGuideSection
        index
        Backup
        Restore
        CommandLine
        PathFormat
        Builder
        Remove
        Info
    End Enum

    Public Function OpenHelpGuide(ByVal section As HelpGuideSection) As Boolean
        Try
            'Tenta di aprire la sezione guida specificata
            'Carica il percorso principale
            Dim pt As String = Path.Combine(My.Application.Info.DirectoryPath, lang)

            If section <> HelpGuideSection.index Then
                pt = Path.Combine(pt, "index_file")
            End If
            'Il nome del file in particolare
            pt = Path.Combine(pt, section.ToString & ".htm")

            Return Utils.OpenShellFile(frmMain.Handle, pt)

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function CheckDonate() As Boolean
        Dim regKey As RegistryKey = Nothing
        Dim value As Object

        Try

            regKey = Registry.LocalMachine.CreateSubKey(My.Settings.RegistryKey)
            value = regKey.GetValue("Donate")

            If value Is Nothing Then
                'Il messaggio non è stato ancora visualizzato
                regKey.SetValue("Donate", True)
                Return True
            Else
                Return CBool(value)
            End If

            regKey.Close()
            regKey = Nothing
        Catch ex As Exception
        Finally
            If regKey IsNot Nothing Then regKey.Close()
        End Try
    End Function


    Public Function GetLanguageFiles() As Dictionary(Of String, String)
        Dim lst As New Dictionary(Of String, String)

        Try

            Dim langMn As LanguageFileReader

            Dim d As New DirectoryInfo(My.Application.Info.DirectoryPath)

            For Each f As FileInfo In d.GetFiles("*.xml")
                langMn = LanguageFileReader.LoadLanguageFile(f.FullName)
                If langMn Is Nothing OrElse langMn.IsValid = False Then Continue For 'File non valido
                If Not lst.ContainsKey(f.Name) Then lst.Add(f.Name, langMn.Author)
            Next

            Return lst
        Catch ex As Exception
            Return lst
        End Try

    End Function

    Public Function GenerateLanguageFile(ByVal languageFile As String, Optional ByVal updateMode As Boolean = False) As Boolean
        Try
            Dim fw As New LanguageManager.LanguageFileWriter(languageFile)
            
            Dim memberFilter As String = ""
            Dim propertyFilter As String = "^Text$"

            With My.Forms
                fw.AddObject(.frmBackup, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
                fw.AddObject(.frmCmdBuilder, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
                fw.AddObject(.frmMain, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
                'fw.AddObject(.frmRemove, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
                fw.AddObject(.frmRestore, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
                fw.AddObject(.frmDonate, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
                fw.AddObject(.frmOffline, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
                fw.AddObject(.frmHelpDevelop, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, memberFilter, propertyFilter, False)
            End With

            If updateMode Then
                'Aggiornamento del file lingua
                Dim dbgList As Dictionary(Of String, String) = GetDebugLangStrs()
                For Each k As KeyValuePair(Of String, String) In dbgList
                    If Not langStrs.ContainsKey(k.Key) Then langStrs.Add(k.Key, k.Value)
                Next

            End If
            fw.AddCustomArr("CommonVariables", "langStrs", langStrs)

            fw.WriteToFile(languageFile)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetLangStr(ByVal name As String) As String

        name = name.Replace(":", "_")

        If langStrs.ContainsKey(name) = False Then Return [String].Empty

        Dim result As String = langStrs(name).Replace("\n", ControlChars.NewLine)
        result = result.Replace("\t", ControlChars.Tab)

        Return result
    End Function


    Private Function GetDebugLangStrs() As Dictionary(Of String, String)
        Dim lst As New Dictionary(Of String, String)

        With lst
            .Add("GENERIC", "Unknown error")
            .Add("YES", "Yes")
            .Add("NO", "No")
            .Add("ERROR_FLD", "Selected directory does not exist. Select a valid directory")
            .Add("ERROR_BERR", "An error occurred during program's execution. Restart DriverBackup!")
            .Add("ERROR_FileWrite", "Cannot save file.")
            .Add("ERROR_FileOpen", "Can't open file. File could be damaged.")
            .Add("ERROR_RegistryAccess", "DriverBackup! can't open system registry. Check for administrative privileges.")
            .Add("ERROR_Admin", "DriverBackup! requires administrative privileges. Restart program with required privileges or contact system administrator.")
            .Add("ERROR_BadSystem", "DriverBackup! can't run under this operating system. Program will be closed.")
            .Add("BRE_Generic", "Unknown error occurred during operation.")
            .Add("BRE_InvalidDevice", "Information about devices are damaged or incorrect.")
            .Add("BRE_UnformattablePath", "Invalid path format.")
            .Add("BRE_NoDevices", "No devices selected.")
            .Add("BRE_FileOverwiting", "Cannot overwrite file")
            .Add("BRE_LackOfSpace", "Destination disk is full.")
            .Add("BRE_CantReadWriteBkInfo", "Cannot read\write backup file.")
            .Add("BRE_FileIOError", "File access denied. Check for administrative privileges..")
            .Add("BRE_OpCanceled", "Operation canceled from user.")
            .Add("BRE_OemInfExist", "Driver is already installed on this computer.")
            .Add("BRE_OemInfAlreadyUsed", "Driver is already used by one or more devices and it is unremoveable.")
            .Add("BRE_MissingInfFile", "Cannot locate installation file.")
            .Add("BRE_CantCopyDriver", "Can't copy driver's files in restoration path.")
            .Add("DCmp_None", "None")
            .Add("DCmp_Partial", "Partial")
            .Add("DCmp_Full", "Full")
            .Add("LOG_Framework", "Microsoft Framework .NET version: ")
            .Add("LOG_Memory", "Available memory: {0} of {1} Kbytes.")
            .Add("LOG_Device", "\n{0}\n\tClass: {1}\n\tProvider:  {2}\n\tVersion: {3}\n\tRelease date: {4}\n\tInf file:  {5}\n\tTotal files:  {6}\n\n")
            .Add("LOG_DeviceOK", "Device processed successfully!")
            .Add("LOG_DeviceError", "An error occurred during device processing.")
            .Add("FRMMAIN_NOPCIDTB", "Database file pci.ids not found. Cannot retrieve pci devices info.")
            .Add("FRMMAIN_NODEVICES", "No devices found. Check for administrative privileges or change listing options.")
            .Add("FRMMAIN_COMPFULL", "Driver is compatible for Backup and Restore.")
            .Add("FRMMAIN_COMPPARTIAL", "Driver should be used on current Windows version only.")
            .Add("FRMMAIN_RCOMPPARTIAL", "Driver should not be compatible with this operating system.")
            .Add("FRMMAIN_COMPNONE", "Driver will not be restoreable.")
            .Add("FRMMAIN_RCOMPNONE", "Driver is not restoreable.")
            .Add("FRMMAIN_FILES", "All files found.")
            .Add("FRMMAIN_NOFILES", "Some files missing.")
            .Add("FRMMAIN_NORESTDEVICES", "Open a Backup file or change listing options.")
            .Add("FRMMAIN_DEVFOUND", "Listed devices: {0} of {1}")
            .Add("FRMMAIN_DIFFERENTSYSTEMS", "Drivers' original system is different from current one.They should be incompatible with this system.")
            .Add("FRMMAIN_TREENODEDEV", "{0}   ({1} Devices)")
            .Add("FRMMAIN_DRIVERREQUIRED", "Selected driver is required by an hardware device connected to computer but not installed.")
            .Add("FRMMAIN_DRIVERUPDATE", "Driver is already installed with an older version.Proceed with Restore to update driver.")
            .Add("FRMMAIN_DRIVERNOTREQUIRED", "Driver is required by any hardware device but it could be restored.")
            .Add("FRMBACK_DEVFOUND", "Selected devices: {0}   Size: {1} Mbytes")
            .Add("FRMBACK_BEGINBACKUP", "Backup started. {0} selected devices.")
            .Add("FRMBACK_ENDBACKUP", "Backup completed. Backuped devices: {0} of {1}.")
            .Add("FRMBACK_ENDBACKUPERR", "Backup canceled because an unknown error occurred.")
            .Add("FRMBACK_ENDDEVICE", "Device completed. Copied files: {0} of {1}")
            .Add("FRMBACK_FILECOPIED", "File copy: {0}")
            .Add("FRMBACK_LOGSAVED", "Log file saved!")
            .Add("FRMBACK_BACKUPTIME", "Elapsed time: {0} sec.")
            .Add("FRMRESTORE_BEGINRESTORE", "Restoration started. {0} selected devices.")
            .Add("FRMRESTORE_ENDRESTORE", "Restoration completed.")
            .Add("FRMRESTORE_PNPRESCAN", "Plug n Play scanning completed successfully.")
            .Add("FRMRESTORE_PNPRESCANFAILED", "Can't start scanning for new Plug n Play devices.")
            .Add("FRMRESTORE_ENDDEVICE", "Device completed.")
            .Add("FRMRESTORE_OEMINF", "Oem installation file: {0}")
            .Add("FRMREMOVE_USERFORCE", "Driver is already used by one or more devices. Remove it?")
            .Add("FRMREMOVE_REMOVED", "{0} drivers removed of {1}")
            .Add("FRMBUILDER_BADSETTINGS", "Some options or information are incorrect. Can't generate a valid command line.")
            .Add("FRMREMOVE_BETAVERSION", "Warning: Remove function isn't fully tested and it is a BETA version. Use it at you own risk.")
            .Add("CONSOLE_BADCOMMAND", "Command line syntax error. Check for all parameters.")
            .Add("CONSOLE_BADPARAMETER", "Syntax error in: {0}")
            .Add("CONSOLE_USAGE", "Read command line Help guide for more information")
            .Add("CONSOLE_DIRECTORY", "Specified directory is not available.")
            .Add("CONSOLE_FILE", "Can't open backup file.")
            .Add("CONSOLE_INFOCOLLECT", "Collecting information....")
            .Add("CONSOLE_WELCOME", "DriverBackup! 2.0 by Giuseppe Greco  2007-2008\n\nReleased with GPL license\n\nCommand line mode\n\n")
            .Add("CONSOLE_OPEND", "Completed.")
            .Add("CONSOLE_REGISTRYUPDATE", "System registry configuration completed successfully.")
            .Add("CONSOLE_MISSINGINFO", "Some options or information are missing.")
            .Add("ERROR_LANGUAGE", "Can't read language file.\nImpossibile caricare le risorse di linguaggio.")
            .Add("BRE_ForceUpdate", "Driver for {0} is already installed. Force installation of backuped driver?")
            .Add("BRE_CantForceUpdating", "Current driver is more suitable for device than backup one.Update is aborted.")
            .Add("LOG_OperationStarted", "Process started: {0}")
            .Add("LOG_OperationEnded", "Process ended: {0}")
            .Add("FRMRESTORE_FORCEUPDATE", "Driver for device {0} is already installed on system. Try to force installation of backuped driver ?. WARNING:Backuped driver will be installed though its version is older.")
            .Add("FRMOFFLINE_GENERIC", "Can't initialize offline backup.")
            .Add("FRMOFFLINE_PRIVILEGE", "Program can't load offline system registry settings.Administrative privileges are required.")
            .Add("FRMOFFLINE_PATH", "Selected path don't contains a valid Windows installation.")
            .Add("CONSOLE_CANTCREATEDIR", "Can't create specified directory.")
            .Add("CONSOLE_RESTORECONFIRM", "Restore driver (y/n)?")
        End With
        Return lst

    End Function


    Private Function GetDebugLangStrsIta() As Dictionary(Of String, String)
        'Lista DEBUG di messaggi testo
        Dim lst As New Dictionary(Of String, String)

        With lst
            .Add("GENERIC", "Unknown error")
            .Add("YES", "Yes")
            .Add("NO", "No")
            .Add("ERROR_FLD", "Selected directory does not exist. Select a valid directory.")
            .Add("ERROR_BERR", "An error occurred during program's execution. Restart DriverBackup!")
            .Add("ERROR_FileWrite", "Cannot save file.")
            .Add("ERROR_FileOpen", "Can't open file. File could be damaged.")
            .Add("ERROR_RegistryAccess", "Il programma non può accedere al registro di sistema. Controllare di avere i privilegi di amministratore.")
            .Add("ERROR_LANGUAGE", "Can't read language file.\nImpossibile caricare le risorse di linguaggio.")
            .Add("ERROR_Admin", "DriverBackup! non può funzionare senza i privilegi di amministratore. Riavviare il programma con i privilegi richiesti o contattare l'amministratore di sistema.")
            .Add("ERROR_BadSystem", "DriverBackup! non supporta questo sistema operativo.Il programma sarà chiuso.")

            .Add("BRE_Generic", "Errore sconosciuto durante l'esecuzione dell'operazione.")
            .Add("BRE_InvalidDevice", "Le informazioni sul dispositivo corrente risultano mancanti o danneggiate.")
            .Add("BRE_UnformattablePath", "Il formato del percorso non è valido.")
            .Add("BRE_NoDevices", "Nessun device è stato selezionato.")
            .Add("BRE_FileOverwiting", "Impossibile sovrascrivere il file")
            .Add("BRE_LackOfSpace", "Il disco di destinazione non ha memoria sufficiente.")
            .Add("BRE_CantReadWriteBkInfo", "Impossibile elaborare le informazioni del backup.")
            .Add("BRE_FileIOError", "Errore generico di accesso al file. Assicurarsi di avere i privilegi di amministratore.")
            .Add("BRE_OpCanceled", "Operazione annullata dall'utente.")
            .Add("BRE_OemInfExist", "Il drivers risulta già installato sul computer.") 'Il drivers è già installato sul computer
            .Add("BRE_OemInfAlreadyUsed", "Impossibile eliminare il drivers poichè è attualmente in uso da uno o più dispositivi.") 'Il drivers è correntemente in uso e non può essere disinstallato.
            .Add("BRE_MissingInfFile", "Impossibile trovare il file di installazione.") 'Il file di installazione è mancante
            .Add("BRE_ForceUpdate", "Il driver per {0} è già installato. Forzare l'installazione del driver di backup?")
            .Add("BRE_CantForceUpdating", "Il driver della periferica è più appropriato di quello di backup.Impossibile aggiornare.")
            .Add("DCmp_None", "NESSUNA")
            .Add("DCmp_Partial", "PARZIALE")
            .Add("DCmp_Full", "COMPLETA")

            .Add("LOG_Framework", "Versione Microsoft Framework .NET: ")
            .Add("LOG_Memory", "Memoria disponibile: {0} su {1} Kbytes.")
            .Add("LOG_Device", "\n{0}\n\tClasse: {1}\n\tProduttore:  {2}\n\tVersione: {3}\n\tData rilascio: {4}\n\tFile installazione:  {5}\n\tFile totali:  {6}\n\n")
            .Add("LOG_DeviceOK", "Device processato con successo!")
            .Add("LOG_DeviceError", "Si sono verificati errori nell'elaborazione del device.")
            .Add("LOG_OperationStarted", "Processo iniziato: {0}")
            .Add("LOG_OperationEnded", "Processo terminato: {0}")

            .Add("FRMMAIN_NODEVICES", "Nessun device trovato. Assicurarsi di avere i privilegi di amministratore o cambiare i criteri di visualizzazione.")
            .Add("FRMMAIN_COMPFULL", "Il driver è compatibile con il backup e il ripristino.")
            .Add("FRMMAIN_COMPPARTIAL", "Il driver potrebbe essere utilizzabile solamente in questa versione del sistema operativo.")
            .Add("FRMMAIN_RCOMPPARTIAL", "Il driver potrebbe non essere compatibile con questo sistema operativo.")
            .Add("FRMMAIN_COMPNONE", "Il driver non potrà essere ripristinato.")
            .Add("FRMMAIN_RCOMPNONE", "Il driver non può essere installato.")
            .Add("FRMMAIN_NOPCIDTB", "Impossibile aprire il file database pci.ids. Controllare che il file sia presente nel percorso dell'applicazione.")
            .Add("FRMMAIN_FILES", "Tutti i files sono presenti.")
            .Add("FRMMAIN_NOFILES", "Alcuni files sono mancanti.")
            .Add("FRMMAIN_NORESTDEVICES", "Aprire un file di backup per visualizzare i devices salvati o cambiare i criteri di visualizzazione.")
            .Add("FRMMAIN_DEVFOUND", "Dispositivi visualizzati: {0} su {1}")
            .Add("FRMMAIN_DIFFERENTSYSTEMS", "Il sistema operativo in cui è stato creato il backup non corrisponde con quello locale. I drivers potrebbero essere inutilizzabili.")
            .Add("FRMMAIN_TREENODEDEV", "{0}   ({1} Dispositivi)")
            'Nuovo set di stringhe testo
            .Add("FRMMAIN_DRIVERREQUIRED", "Il driver è richiesto da una periferica collegata al computer e non ancora installata.")
            .Add("FRMMAIN_DRIVERUPDATE", "Il driver è già installato nel computer ma la versione è meno recente. Proseguire con il ripristino per aggiornare il driver.")
            .Add("FRMMAIN_DRIVERNOTREQUIRED", "Il driver non è al momento richiesto da nessuna periferica collegata. Il ripristino è però sempre possibile.")
            '###############################
            .Add("FRMBACK_DEVFOUND", "Dispositivi selezionati: {0}   Dimensioni: {1} Mbytes")
            .Add("FRMBACK_BEGINBACKUP", "Backup iniziato. {0} devices selezionati.")
            .Add("FRMBACK_BACKUPTIME", "Tempo impiegato: {0} sec.")
            .Add("FRMBACK_ENDBACKUP", "Backup terminato con successo. {0} devices processati su {1}.")
            .Add("FRMBACK_ENDBACKUPERR", "Backup terminato a causa di un errore grave.")
            .Add("FRMBACK_ENDDEVICE", "Device processato. Files copiati {0} su {1}")
            .Add("FRMBACK_FILECOPIED", "Copia del file: {0}")
            .Add("FRMBACK_LOGSAVED", "File log salvato con successo.")

            .Add("FRMRESTORE_BEGINRESTORE", "Ripristino dei drivers iniziato. {0} devices selezionati.")
            .Add("FRMRESTORE_ENDRESTORE", "Ripristino completato.")
            .Add("FRMRESTORE_PNPRESCAN", "Aggiornamento configurazione dispositivi Plug & Play effettuato con successo.")
            .Add("FRMRESTORE_PNPRESCANFAILED", "Impossibile aggiornare automaticamente i dispositivi PnP, procedere manualmente.")
            .Add("FRMRESTORE_ENDDEVICE", "Device processato.")
            .Add("FRMRESTORE_OEMINF", "Nuovo file di installazione: {0}")
            .Add("FRMRESTORE_FORCEUPDATE", "Il driver per il dispositivo {0} è già installato sul sistema. Provare a forzare l'installazione del driver di backup ?. NB: Il driver di backup verrà installato anche se la sua versione è MENO RECENTE.")

            '.Add("FRMREMOVE_USERFORCE", "Il driver è attualmente utilizzato da uno o più dispositivi. Provare a forzare la rimozione?")
            '.Add("FRMREMOVE_REMOVED", "Drivers rimossi {0} su {1}")
            '.Add("FRMREMOVE_BETAVERSION", "Attenzione: La funzionalità di rimozione è ancora in fase BETA e potrebbe non produrre gli effetti desiderati. L'autore declina ogni responsabilità per eventuali danni arrecati al pc.")

            .Add("FRMBUILDER_BADSETTINGS", "Alcune impostazioni non sono corrette. Impossibile generare una riga di comando valida.")

            .Add("FRMOFFLINE_GENERIC", "Impossibile inizializzare il backup offline.")
            .Add("FRMOFFLINE_PRIVILEGE", "Il programma non può caricare le informazioni di configurazione del sistema offline sul registro locale.Sono richiesti i privilegi di amministratore.")
            .Add("FRMOFFLINE_PATH", "Il percorso selezionato non contiene un'installazione Windows valida.")

            .Add("CONSOLE_BADCOMMAND", "Errore di sintassi nella riga di comando. Assicurarsi di aver inserito tutti i parametri.")
            .Add("CONSOLE_BADPARAMETER", "Errore di sintassi nel parametro: {0}")
            .Add("CONSOLE_USAGE", "Non disponibile")
            .Add("CONSOLE_DIRECTORY", "La directory specificata non è disponibile o non esiste.")
            .Add("CONSOLE_FILE", "Impossibile aprire il file di backup specificato.")
            .Add("CONSOLE_INFOCOLLECT", "Raccolta delle informazioni necessarie in corso....")
            .Add("CONSOLE_WELCOME", "DriverBackup! 2.0 by Giuseppe Greco  2007-2008\n\nDistribuito con licenza GPL\n\nModalità riga di comando\n\n")
            .Add("CONSOLE_OPEND", "Operazione conclusa.")
            .Add("CONSOLE_REGISTRYUPDATE", "Configurazione del registro di sistema effettuata.")
            .Add("CONSOLE_MISSINGINFO", "Alcune informazioni necessarie non sono state inserite o non sono valide.")
            .Add("CONSOLE_CANTCREATEDIR", "Impossibile creare la directory specificata.")
            .Add("CONSOLE_RESTORECONFIRM", "Continuare con il ripristino del driver (y/n)?")
            '.Add("CONSOLE_OFFLINEPC", "Nome computer offline trovato: {0}")

        End With

        Return lst
    End Function

    Private Sub DebugInitializeLanguage()
        'Carica la lingua DEBUG (Italiano)
        langStrs.Clear()
        lang = "Italiano"
        langStrs = GetDebugLangStrs()
    End Sub

    Private Sub LoadStdLanguage()
        Try
            Dim langFile As String = ""
            Dim langReader As LanguageFileReader = Nothing

            langFile = Path.Combine(My.Application.Info.DirectoryPath, "English.xml")
            'Carica il file
            langReader = LanguageFileReader.LoadLanguageFile(langFile)

            If langReader Is Nothing Then
                'File linguaggio non trovato carica le risorse in italiano
                DebugInitializeLanguage()
                MsgBox(GetLangStr("ERROR:LANGUAGE"), MsgBoxStyle.Exclamation)
            End If

            If LoadLanguageOnForms(langReader) = False Then
                DebugInitializeLanguage()
                MsgBox(GetLangStr("ERROR:LANGUAGE"), MsgBoxStyle.Exclamation)
            Else
                lang = "English"
            End If

        Catch ex As Exception
            DebugInitializeLanguage()
            MsgBox(GetLangStr("ERROR:LANGUAGE"), MsgBoxStyle.Exclamation)
        End Try

    End Sub

    Private Function LoadLanguageOnForms(ByVal langReader As LanguageFileReader) As Boolean
        Try

            With langReader
                .LoadLanguageOnForm(frmBackup, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, "", "^Text$", False)
                .LoadLanguageOnForm(frmCmdBuilder, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, "", "^Text$", False)
                .LoadLanguageOnForm(frmMain, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, "", "^Text$", False)
                .LoadLanguageOnForm(frmOffline, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, "", "^Text$", False)
                .LoadLanguageOnForm(frmRestore, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, "", "^Text$", False)
                .LoadLanguageOnForm(frmDonate, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, "", "^Text$", False)
                .LoadLanguageOnForm(frmHelpDevelop, LanguageManager.StdBindingFlags, New Type() {GetType(Control), GetType(ToolStripItem)}, "", "^Text$", False)

                Dim objCont As ObjectContainer = .ReadContainer("CommonVariables")
                Dim tempArr As New Dictionary(Of String, String)
                If objCont IsNot Nothing Then
                    langStrs = objCont.StringArrays("langStrs")
                    If langStrs Is Nothing OrElse langStrs.Count <= 0 Then Return False
                Else
                    Return False
                End If

            End With
            Return True
        Catch ex As Exception
            Return False
        Finally
            frmMain.Text = "DriverBackup! " & My.Application.Info.Version.ToString
        End Try

    End Function


    Public Sub ChangeLanguage(ByVal file As String)
        Dim regKey As RegistryKey = Nothing

        Try

            regKey = Registry.LocalMachine.CreateSubKey(My.Settings.RegistryKey)
            regKey.SetValue("LanguageFile", file)
            regKey.Close()
            regKey = Nothing

            InitializeLanguage()
        Catch ex As Exception
        Finally
            If regKey IsNot Nothing Then regKey.Close()
        End Try


    End Sub

    Public Sub InitializeLanguage()
        'Inizializza le risorse linguaggio
        Dim langFile As String = ""
        Dim langReader As LanguageFileReader = Nothing
        Dim regKey As RegistryKey = Nothing
        Dim regValue As String = ""

        Try

            '#If DEBUG Then
            'Inizializzazione provvisoria della lingua
            '           DebugInitializeLanguage()
            '          Return
            '#End If

            'Carica la lingua inglese per impedire che file di linguaggio con
            'campi mancanti lascino inalterata ta text di alcuni controlli

            LoadStdLanguage()


            regKey = Registry.LocalMachine.CreateSubKey(My.Settings.RegistryKey)

            If regKey Is Nothing Then
                'Impossibile accedere al registro, carica la lingua di default "English.xml"
            Else
                'L'utente ha scelto precedentemente il file
                regValue = regKey.GetValue("LanguageFile", "")

                If [String].IsNullOrEmpty(regValue) Then
                    'Crea il valore di default
                    regKey.SetValue("LanguageFile", "English.xml")
                    Return
                End If

                langFile = Path.Combine(My.Application.Info.DirectoryPath, regValue)
                'Carica i form
                langReader = LanguageFileReader.LoadLanguageFile(langFile)

                If langReader Is Nothing Then
                    Exit Sub
                End If

                lang = langReader.LanguageName
                If LoadLanguageOnForms(langReader) = False Then
                    DebugInitializeLanguage()
                    MsgBox(GetLangStr("ERROR_LANGUAGE"), MsgBoxStyle.Exclamation)
                End If
            End If

        Catch ex As Exception
            DebugInitializeLanguage()
            MsgBox(GetLangStr("ERROR_LANGUAGE"), MsgBoxStyle.Exclamation)
        Finally
            'Scrive la versione del programma su frmMain.Text
            frmMain.Text = "DriverBackup! " & My.Application.Info.Version.ToString
            If regKey IsNot Nothing Then regKey.Close()
        End Try

    End Sub





End Module
