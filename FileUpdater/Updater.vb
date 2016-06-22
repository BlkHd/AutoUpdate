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
Imports System.Threading
Imports System.Runtime.InteropServices

Namespace FileUpdater

    Public Class Updater

        Private Const UPDATE_FOLDER_NAME As String = "AppUpdate"
        Private Const BACKUP_FOLDER_NAME As String = "AppBackup"

        Private ReadOnly NameUpdateApp As String
        Private ReadOnly appPath As String

        Public Sub New()

            Me.NameUpdateApp = getFilenameFromPath(Assembly.GetExecutingAssembly.GetName.CodeBase)

            If File.Exists(Assembly.GetExecutingAssembly.GetName.CodeBase) Then
                appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly.GetName.CodeBase)
            Else
                appPath = String.Empty
            End If

        End Sub

        Public Sub PerformUpdate()

            Dim updateDir As String = appPath & "\" & UPDATE_FOLDER_NAME
            Dim backupDir As String = appPath & "\" & BACKUP_FOLDER_NAME

            If Directory.Exists(backupDir) Then
                Directory.Delete(backupDir, True)
            End If

            If Not Directory.Exists(updateDir) Then
                Return
            End If

            MobileConfiguration.Settings("UpdatingFiles") = 1
            MobileConfiguration.Save()

            Directory.CreateDirectory(backupDir)

            For Each filepath As String In Directory.GetFiles(updateDir)

                Dim _FileName As String = getFilenameFromPath(filepath)

                If Not String.Compare(_FileName, NameUpdateApp, True) = 0 Then

                    Dim originalFile As String = appPath & "\" & _FileName

                    If File.Exists(originalFile) Then

                        Dim backupFilepath As String = backupDir & "\" & getFilenameFromPath(filepath)

                        File.Move(originalFile, backupFilepath)
                        File.Move(filepath, originalFile)

                    Else
                        Me.removeFile(filepath)
                    End If

                End If

            Next filepath

            MobileConfiguration.Settings("UpdatingFiles") = 0
            MobileConfiguration.Save()

            If Directory.Exists(updateDir) Then
                Directory.Delete(updateDir, True)
            End If

            If Directory.Exists(backupDir) Then
                Directory.Delete(backupDir, True)
            End If

        End Sub

        Private Function getFilenameFromPath(ByVal path As String) As String

            Dim delim() As Char = {"\"c}
            Dim a() As String = path.Split(delim)
            Return a(a.Length - 1)

        End Function

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
