Imports System.Reflection
Imports System.IO
Imports FileUpdater.FileUpdater

Namespace FileUpdater

    Module Main

        Private Structure NameCommandLineStuct
            Dim Name As String
            Dim Value As String
        End Structure

        Private CommandLineArgs As New List(Of NameCommandLineStuct)

        Function ParseCommandLine(ByRef Args() As String) As Boolean

            If Args.Count = 0 Then
                Return False
            End If

            For Each MaiorArg In Args

                Dim Params As String() = Split(MaiorArg, "/")

                For Each arg As String In Params

                    If Not String.IsNullOrEmpty(arg) Then

                        If arg.Contains("=") Then

                            Dim tmp As NameCommandLineStuct

                            Dim idx As Integer = arg.IndexOf("=")

                            If idx < arg.Length - 1 Then

                                tmp.Name = arg.Substring(0, idx).Trim()
                                tmp.Value = arg.Substring(idx + 1).Trim()
                                CommandLineArgs.Add(tmp)

                            End If

                        End If

                    End If

                Next

            Next

            Return CommandLineArgs.Count > 0

        End Function

        Sub Main(ByVal Args() As String)

            Dim CallingApp As String = String.Empty

            'File Update process started

            If ParseCommandLine(Args) = False Then
                'No arguments passed to update program
                Return
            Else

                For Each Arg In CommandLineArgs

                    If String.Compare(Arg.Name, "app", True) = 0 Then
                        CallingApp = Arg.Value
                    End If

                Next

            End If

            If CallingApp.Length = 0 Then
                'No arguments passed about the application to update
                Return
            End If

            Dim FullCallingApp As String = Assembly.GetExecutingAssembly.GetName.CodeBase

            Dim _AppPath As String = Path.GetDirectoryName(FullCallingApp)
            FullCallingApp = _AppPath & "\" & CallingApp

            If Not System.IO.File.Exists(FullCallingApp) Then
                'Calling Application to update doesn't exist
                Return
            End If

            MobileConfiguration.Settings("Updating") = "1"
            MobileConfiguration.Save()

            Dim _Updater As New Updater()

            _Updater.PerformUpdate()

            MobileConfiguration.Settings("Updating") = "0"
            MobileConfiguration.Save()

            System.Diagnostics.Process.Start(FullCallingApp, "")
            Application.Exit()

        End Sub

    End Module

End Namespace

