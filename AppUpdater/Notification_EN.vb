Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Reflection
Imports System.IO
Imports System.Threading

Namespace AppUpdater

    Partial Public Class Notification_EN

        Inherits Form

        Public Delegate Sub StartUpdate()
        Public Delegate Sub FormLoaded()
        Public Delegate Sub StopUpdate()
        Public Delegate Sub AbortUpdate()

        Public Event NotificationFormReady As FormLoaded
        Public Event StartUpdateEvent As StartUpdate
        Public Event StopUpdateEvent As StopUpdate
        Public Event AbortUpdateEvent As AbortUpdate

        Private ReadOnly callingAssembly As System.Reflection.Assembly

        Public trans As TransferManager.TransferProgress

        Private updateStarted As Boolean = False

        Public Enum CustomDialogResults As Byte
            UpdateCancelled = 2
            FileReadyToUpdate = 3
            FileDownloadError = 4
        End Enum


        Private mDialogResult As CustomDialogResults

        Public Overloads Property DialogResult() As CustomDialogResults
            Get
                Return mDialogResult
            End Get
            Set(ByVal value As CustomDialogResults)
                mDialogResult = value
            End Set
        End Property


        Private mErrorDownloadingFile As Boolean

        Public ReadOnly Property ErrorDownloadingFile() As Boolean
            Get
                Return mErrorDownloadingFile
            End Get
        End Property

        Public Shadows Function ShowDialog() As CustomDialogResults
            MyBase.ShowDialog()
            Return DialogResult
        End Function


        Public Sub New(ByVal name As String, ByVal message As String, ByVal version As String, ByVal callingAssem As System.Reflection.Assembly, ByVal updater As Updater)

            InitializeComponent()
            Me.appname_label.Text = name
            Me.version_label.Text = version
            Me.message_textbox.Text = message
            Me.callingAssembly = callingAssem
            trans = New TransferManager.TransferProgress(AddressOf UpdateProgressBar)

            AddHandler updater.UpdateDoneEvent, AddressOf updater_UpdateDoneEvent
            AddHandler updater.UpdateErrorEvent, AddressOf updater_UpdateErrorEvent

            RaiseEvent NotificationFormReady()

        End Sub

        Private Sub updater_UpdateDoneEvent()

            If Me.InvokeRequired Then
                Me.BeginInvoke(New Updater.UpdateDone(AddressOf updater_UpdateDoneEvent), Nothing)
                Return
            End If

            btnAnnulla.Visible = False
            btnInstalla.Visible = False
            progressBar1.Visible = False
            btnResult.Text = "Restart"
            btnResult.Visible = True
            btnAnnulla.Dock = DockStyle.None
            btnInstalla.Dock = DockStyle.None
            progressBar1.Dock = DockStyle.None
            btnResult.Dock = DockStyle.Top

            mErrorDownloadingFile = False

        End Sub

        Private Sub updater_UpdateErrorEvent()

            If Me.InvokeRequired Then
                Me.BeginInvoke(New Updater.UpdateError(AddressOf updater_UpdateErrorEvent), Nothing)
                Return
            End If

            btnAnnulla.Visible = False
            btnInstalla.Visible = False
            progressBar1.Visible = False
            btnResult.Text = "Download Error - Restart"
            btnResult.Visible = True
            btnAnnulla.Dock = DockStyle.None
            btnInstalla.Dock = DockStyle.None
            progressBar1.Dock = DockStyle.None
            btnResult.Dock = DockStyle.Top

            mErrorDownloadingFile = True

        End Sub

        Private Sub btnAnnulla_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAnnulla.Click

            If Common.VolatileRead(updateStarted) Then
                RaiseEvent AbortUpdateEvent()
            End If

            Me.DialogResult = CustomDialogResults.UpdateCancelled
            Me.Close()

        End Sub

        Public Sub UpdateProgressBar(ByVal status As Integer)

            If Me.InvokeRequired Then
                If (Not Me.IsDisposed) Then
                    Me.BeginInvoke(New TransferManager.TransferProgress(AddressOf UpdateProgressBar), status)
                End If
                Return
            End If

            progressBar1.Value = CInt(Fix(status))

        End Sub

        Private Sub btnInstalla_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInstalla.Click

            If StartUpdateEventEvent IsNot Nothing Then

                Dim button As Button = CType(sender, Button)

                button.Visible = False

                button.Dock = DockStyle.None
                progressBar1.Dock = DockStyle.Top

                progressBar1.Visible = True

                RaiseEvent StartUpdateEvent()

                Common.VolatileWrite(updateStarted, True)

            End If

        End Sub

        Private Sub btnResult_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResult.Click

            If Common.VolatileRead(updateStarted) Then

                If ErrorDownloadingFile Then
                    Me.DialogResult = CustomDialogResults.FileDownloadError
                Else
                    Me.DialogResult = CustomDialogResults.FileReadyToUpdate
                End If

                Me.Close()

            End If

        End Sub

    End Class

End Namespace