Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Diagnostics
Imports System.IO
Imports System.Reflection

Namespace AppUpdater

    Public NotInheritable Class Logger

        Private Shared instance_Renamed As Logger
        Private Shared syncRoot As New Object()
        Private Shared strWr As System.IO.StreamWriter

        Private Sub New()

            Dim fullAppName As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase

            Dim path As String = System.IO.Path.GetDirectoryName(fullAppName)
            Dim logFile As String = System.IO.Path.Combine(path, "update-log.txt")

            strWr = New System.IO.StreamWriter(logFile, True, System.Text.Encoding.UTF8)
            Dim tr2 As New TextWriterTraceListener(strWr)

            Debug.Listeners.Add(tr2)
            Debug.AutoFlush = True

            Me.log("----------------------------------------------------------------------------")

        End Sub

        Protected Overrides Sub Finalize()

            strWr.Dispose()

        End Sub

        Public Shared ReadOnly Property Instance() As Logger

            Get

                If instance_Renamed Is Nothing Then
                    SyncLock syncRoot
                        If instance_Renamed Is Nothing Then
                            instance_Renamed = New Logger()
                        End If
                    End SyncLock
                End If

                Return instance_Renamed

            End Get

        End Property

        Public Sub log(ByVal message As String)

            SyncLock syncRoot
                Debug.WriteLine("[" & DateTime.Now.ToString() & "]: " & message)
            End SyncLock

        End Sub

    End Class

End Namespace

