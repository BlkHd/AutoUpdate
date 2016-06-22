Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography.X509Certificates

Namespace AppUpdater

    Public Class TransferManager

        Public Delegate Sub TransferProgress(ByVal progress As Integer)

        Private transferProgressDelegate As TransferProgress
        Private abortTransfer As Boolean = False

        Public Sub New()

        End Sub

        Public Sub AddObserver(ByVal ev As Notification_EN)
            AddHandler ev.AbortUpdateEvent, AddressOf Notification_AbortUpdateEvent
        End Sub

        Private Sub Notification_AbortUpdateEvent()
            Common.VolatileWrite(Me.abortTransfer, True)
        End Sub

        Public Function downloadFile(ByVal url As String, <System.Runtime.InteropServices.Out()> ByRef s As Stream, ByVal path As String, ByVal del As TransferProgress) As Boolean

            System.Net.ServicePointManager.CertificatePolicy = New TrustAllCertificatePolicy()

            Me.transferProgressDelegate = del

            Dim buffer(4095) As Byte
            Dim fileStream As New FileStream(path, FileMode.Create, FileAccess.ReadWrite)

            Dim wr As WebRequest = WebRequest.Create(url)
            wr.Proxy = System.Net.GlobalProxySelection.Select

            Try

                Using response As WebResponse = wr.GetResponse()

                    Using responseStream As Stream = response.GetResponseStream()

                        Dim count As Integer = 0
                        Dim dataRead As Integer = 0

                        Do
                            count = responseStream.Read(buffer, 0, buffer.Length)
                            fileStream.Write(buffer, 0, count)

                            Dim progress As Single = (CSng(dataRead) / CSng(response.ContentLength)) * 100.0F
                            OnProgress(CInt(Fix(progress)))

                            dataRead += count
                        Loop While count <> 0 AndAlso Not Common.VolatileRead(abortTransfer)

                    End Using

                End Using

            Catch wex As WebException

                Logger.Instance.log("No Connection to update server")
                s = Nothing
                Return False

            End Try

            fileStream.Close()
            s = fileStream
            Return True

        End Function

        Protected Sub OnProgress(ByVal progress As Integer)

            If transferProgressDelegate IsNot Nothing Then
                transferProgressDelegate(progress)
            End If

        End Sub

    End Class

    Public Class TrustAllCertificatePolicy

        Implements ICertificatePolicy

        Public Sub New()
        End Sub

        Public Function CheckValidationResult(ByVal srvPoint As System.Net.ServicePoint, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal request As System.Net.WebRequest, ByVal certificateProblem As Integer) As Boolean Implements System.Net.ICertificatePolicy.CheckValidationResult
            Return True
        End Function
    End Class

End Namespace
