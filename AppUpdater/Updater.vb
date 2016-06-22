Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Xml
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Ionic.Zip
Imports System.Threading
Imports System.Runtime.InteropServices

Namespace AppUpdater

    Public Class Updater

        Public Delegate Sub UpdateDone()
        Public Delegate Sub UpdateError()

        Public Event UpdateDoneEvent As UpdateDone
        Public Event UpdateErrorEvent As UpdateError

        Private Delegate Sub performUpdateDelegate()

        Private Const UPDATE_PROGRAMM As String = "FileUpdater.exe"
        Private Const UPDATE_FOLDER_NAME As String = "AppUpdate"
        Private Const BACKUP_FOLDER_NAME As String = "AppBackup"
        Private Const XML_FILENAME As String = "AppUpdate.xml"

        Private ReadOnly NameUpdateApp As String

        Private ReadOnly URL As String
        Private ReadOnly updateFilePath As String
        Private ReadOnly appPath As String
        Private ReadOnly callingAssembly As System.Reflection.Assembly
        Private ReadOnly callingAppFileName As String

        Private newVersion As Version
        Private zipFileURL As String
        Private notificationForm As Notification_EN
        Private abortUpdate As Boolean = False
        Private message As String

        Public Enum UpdateResult As Byte
            NoUpdateAvailable = 0
            UpdatingFileInProgress = 1
            UpdateCancelled = 2
            FileReadyToUpdate = 3
            FileDownloadError = 4
        End Enum

        Public Function VerifyUpdates() As Updater.UpdateResult

            If MobileConfiguration.Settings("DisableUpdates") = 0 Then

                If MobileConfiguration.Settings("Updating") = 1 Then
                    VerifyUpdates = Updater.UpdateResult.UpdatingFileInProgress
                    Application.Exit()
                ElseIf CheckNewVersion() Then

                    Dim _UpdateResult As Updater.UpdateResult = ShowDialogNewVersion()

                    VerifyUpdates = _UpdateResult

                    Select Case _UpdateResult

                        Case Updater.UpdateResult.UpdatingFileInProgress
                            Application.Exit()
                        Case Updater.UpdateResult.FileReadyToUpdate

                            Dim _UpdateApp As String = appPath & "\" & UPDATE_PROGRAMM

                            If File.Exists(_UpdateApp) Then
                                System.Diagnostics.Process.Start(_UpdateApp, "/App=" & Me.callingAppFileName)
                                Application.Exit()

                            End If

                    End Select

                Else
                    VerifyUpdates = Updater.UpdateResult.NoUpdateAvailable
                End If

            Else
                VerifyUpdates = Updater.UpdateResult.NoUpdateAvailable
            End If

        End Function

        Public Sub New(ByVal url As String)

            callingAssembly = System.Reflection.Assembly.GetCallingAssembly()
            callingAppFileName = getFilenameFromPath(callingAssembly.GetName.CodeBase)

            Dim FullCallingApp As String = callingAssembly.GetName.CodeBase

            If Not System.IO.File.Exists(FullCallingApp) Then
                Logger.Instance.log("Application to update doesn't exist")
                Return
            End If

            Dim _XmlUpdateLink As String = MobileConfiguration.Settings("XmlUpdateLink")

            If url Is Nothing OrElse url.Length = 0 Then
                Logger.Instance.log("The link to the update file is missing")
                Return
            End If

            Me.URL = url
            Me.NameUpdateApp = getFilenameFromPath(Assembly.GetExecutingAssembly.GetName.CodeBase)

            Debug.Assert(url IsNot Nothing)

            appPath = Assembly.GetExecutingAssembly.GetName.CodeBase

            If File.Exists(appPath) Then

                Dim fullAppName As String = callingAssembly.GetName().CodeBase

                appPath = Path.GetDirectoryName(fullAppName)

                updateFilePath = Path.Combine(appPath, XML_FILENAME)

            Else
                appPath = String.Empty
            End If

            Me.assertPreviousUpdate()

        End Sub

        Private Sub assertPreviousUpdate()

            Dim updateDir As String = appPath & "\" & UPDATE_FOLDER_NAME

            If MobileConfiguration.Settings("UpdatingFiles") = 1 Then

                Dim backupDir As String = appPath & "\" & BACKUP_FOLDER_NAME

                If Not Directory.Exists(backupDir) Then Return

                If Directory.Exists(updateDir) Then
                    Directory.Delete(updateDir, True)
                End If

                If File.Exists(updateFilePath) Then
                    Me.removeFile(updateFilePath)
                End If

                If Directory.Exists(backupDir) Then

                    For Each f As String In Directory.GetFiles(backupDir)
                        File.Move(f, appPath & "\" & getFilenameFromPath(f))
                    Next f

                    Directory.Delete(backupDir, True)

                End If

                MobileConfiguration.Settings("UpdatingFiles") = 0
                MobileConfiguration.Save()

            End If

            If Directory.Exists(updateDir) Then
                Directory.Delete(updateDir, True)
            End If

        End Sub

        Private Function CheckNewVersion() As Boolean

            Dim _result As Boolean = False

            Dim s As Stream = Nothing

            Dim tm As New TransferManager()

            Try

                If tm.downloadFile(URL, s, updateFilePath, Nothing) Then
                    _result = Me.IsNewVersion(s)
                    Me.removeFile(updateFilePath)
                Else
                    Logger.Instance.log("The update xml file is missing")
                End If

            Catch ex As Exception
                Logger.Instance.log(String.Format("Unexpected error: {0}", ex.Message))
            End Try

            Return _result

        End Function

        Private Function GetFileVersionInfo(ByVal filename As String) As Version
            Dim _AssemblyFile As Assembly = Assembly.LoadFrom(filename)
            Return _AssemblyFile.GetName.Version
        End Function

        Protected Function IsNewVersion(ByVal file As Stream) As Boolean

            Dim xDoc As New XmlDocument()

            Try
                xDoc.Load(updateFilePath)
            Catch ex As Exception
                Logger.Instance.log("The update xml file is wrong or corrupted")
                Return False
            End Try

            Dim modules As XmlNodeList = xDoc.GetElementsByTagName("download")
            Dim versions As XmlNodeList = xDoc.GetElementsByTagName("version")

            Dim xmlAppName As String = modules(0).Attributes("name").Value

            If String.Compare(callingAppFileName, xmlAppName, True) <> 0 Then
                Logger.Instance.log(String.Format("The update is not for {0} but for {1}", callingAppFileName, xmlAppName))
                Return False
            End If

            Dim xmlAppVersion As New Version(Integer.Parse(versions(0).Attributes("maj").Value), Integer.Parse(versions(0).Attributes("min").Value), Integer.Parse(versions(0).Attributes("bld").Value), Integer.Parse(versions(0).Attributes("rev").Value))

            Try

                If Not callingAssembly.GetName.Version.CompareTo(xmlAppVersion) < 0 Then
                    Logger.Instance.log("No update is available")
                    Return False
                End If

            Catch ex As Exception

                Logger.Instance.log(String.Format("Warning: Error reading current {0} version info, the update will continue", callingAppFileName))
                Return False

            End Try

            Me.newVersion = xmlAppVersion

            Dim messages As XmlNodeList = xDoc.GetElementsByTagName("message")
            Dim links As XmlNodeList = xDoc.GetElementsByTagName("link")

            Dim link As String = links(0).InnerText

            Me.message = messages(0).InnerText.Replace(vbLf, vbCr & vbLf)

            Me.zipFileURL = link

            Return True

        End Function


        Private Function ShowDialogNewVersion() As UpdateResult

            notificationForm = New Notification_EN(callingAppFileName, message, newVersion.ToString(), callingAssembly, Me)

            AddHandler notificationForm.AbortUpdateEvent, AddressOf notification_AbortUpdateEvent
            AddHandler notificationForm.StartUpdateEvent, AddressOf startUpdate

            Logger.Instance.log(String.Format("Version: {0}", newVersion.ToString))
            Logger.Instance.log(String.Format("Message: {0}", message))
            Logger.Instance.log(String.Format("Link: {0}", Me.zipFileURL))

            Dim _DialogResult As Notification_EN.CustomDialogResults = notificationForm.ShowDialog()

            Select Case _DialogResult

                Case Is = Notification_EN.CustomDialogResults.UpdateCancelled

                    Logger.Instance.log("Update cancelled")
                    notificationForm.Dispose()
                    Return UpdateResult.UpdateCancelled

                Case Is = Notification_EN.CustomDialogResults.FileDownloadError

                    Logger.Instance.log("Error Downloading Zip File")
                    notificationForm.Dispose()
                    Return UpdateResult.FileDownloadError

                Case Is = Notification_EN.CustomDialogResults.FileReadyToUpdate

                    Logger.Instance.log("Update finished")
                    notificationForm.Dispose()
                    Return UpdateResult.FileReadyToUpdate

            End Select

        End Function

        Private Sub notification_AbortUpdateEvent()
            Common.VolatileWrite(Me.abortUpdate, True)
        End Sub

        Private Sub startUpdate()
            ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf performUpdate))
        End Sub

        Private Sub performUpdate(ByVal obj As Object)

            Dim url As String = CStr(zipFileURL)

            Dim appPath As String = Path.GetDirectoryName(callingAssembly.GetName.CodeBase)

            Dim updateDir As String = appPath & "\" & UPDATE_FOLDER_NAME

            Dim updateFilename As String = getFilename(url)

            Dim updateFileZip As String = updateDir & "\" & updateFilename

            Directory.CreateDirectory(updateDir)

            Dim tm As New TransferManager()

            tm.AddObserver(notificationForm)

            Dim s As Stream = Nothing

            If Not tm.downloadFile(url, s, updateFileZip, notificationForm.trans) Then

                Logger.Instance.log("The update zip file is missing")

                If s IsNot Nothing Then
                    s.Close()
                End If

                OnUpdateError()

            Else

                If s IsNot Nothing Then
                    s.Close()
                End If

                If (Not Common.VolatileRead(abortUpdate)) Then

                    Using zip1 As ZipFile = ZipFile.Read(updateFileZip)

                        For Each e As ZipEntry In zip1
                            e.Extract(updateDir, ExtractExistingFileAction.OverwriteSilently)
                        Next e

                    End Using

                    File.Delete(updateFileZip)

                    OnUpdateDone()

                End If

            End If

        End Sub

        Private Function getFilename(ByVal url As String) As String

            Dim delim() As Char = {"/"c}
            Dim a() As String = url.Split(delim)
            Return a(a.Length - 1)

        End Function

        Private Function getFilenameFromPath(ByVal path As String) As String

            Dim delim() As Char = {"\"c}
            Dim a() As String = path.Split(delim)
            Return a(a.Length - 1)

        End Function

        Protected Sub OnUpdateDone()
            RaiseEvent UpdateDoneEvent()
        End Sub

        Protected Sub OnUpdateError()
            RaiseEvent UpdateErrorEvent()
        End Sub

        Protected Function removeFile(ByVal FileName As String) As Boolean

            Dim Ret As Boolean = False

            Try
                If File.Exists(FileName) = False Then
                    Return True
                End If
                File.Delete(FileName)
                Ret = True
            Catch e1 As Exception
                Throw
            End Try

            Return Ret

        End Function

    End Class

End Namespace
