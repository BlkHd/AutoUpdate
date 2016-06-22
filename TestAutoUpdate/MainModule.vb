Imports AppUpdater.AppUpdater

Public Class MainModule

    Public Shared Sub Main()

        Dim updater As New Updater("http://whereyouwant/AppUpdater/update.xml")
        updater.VerifyUpdates()

        Dim _FrmTestApp As New FrmTestApp

        FrmTestApp.ShowDialog()

    End Sub

End Class
