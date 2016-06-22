Imports Microsoft.VisualBasic
Imports System
Namespace AppUpdater
    Partial Public Class Notification_IT
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing
        Private mainMenu1 As System.Windows.Forms.MainMenu

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.mainMenu1 = New System.Windows.Forms.MainMenu
            Me.label1 = New System.Windows.Forms.Label
            Me.label2 = New System.Windows.Forms.Label
            Me.appname_label = New System.Windows.Forms.Label
            Me.label4 = New System.Windows.Forms.Label
            Me.progressBar1 = New System.Windows.Forms.ProgressBar
            Me.btnAnnulla = New System.Windows.Forms.Button
            Me.label5 = New System.Windows.Forms.Label
            Me.version_label = New System.Windows.Forms.Label
            Me.panel1 = New System.Windows.Forms.Panel
            Me.message_textbox = New System.Windows.Forms.TextBox
            Me.panel2 = New System.Windows.Forms.Panel
            Me.btnInstalla = New System.Windows.Forms.Button
            Me.btnResult = New System.Windows.Forms.Button
            Me.panel1.SuspendLayout()
            Me.panel2.SuspendLayout()
            Me.SuspendLayout()
            '
            'label1
            '
            Me.label1.Location = New System.Drawing.Point(3, 0)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(186, 19)
            Me.label1.Text = "Una nuova versione di:"
            '
            'label2
            '
            Me.label2.Dock = System.Windows.Forms.DockStyle.Top
            Me.label2.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Bold)
            Me.label2.Location = New System.Drawing.Point(0, 0)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(238, 32)
            Me.label2.Text = "Agg. Disponibile"
            Me.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'appname_label
            '
            Me.appname_label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.appname_label.Location = New System.Drawing.Point(3, 19)
            Me.appname_label.Name = "appname_label"
            Me.appname_label.Size = New System.Drawing.Size(233, 20)
            Me.appname_label.Text = "Application Name"
            '
            'label4
            '
            Me.label4.Location = New System.Drawing.Point(3, 59)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(233, 36)
            Me.label4.Text = "è disponibile. Fai clic su ""Installa"" per installare o ""Annulla"" per saltarlo" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            '
            'progressBar1
            '
            Me.progressBar1.Location = New System.Drawing.Point(0, 0)
            Me.progressBar1.Name = "progressBar1"
            Me.progressBar1.Size = New System.Drawing.Size(240, 33)
            Me.progressBar1.Visible = False
            '
            'btnAnnulla
            '
            Me.btnAnnulla.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.btnAnnulla.Location = New System.Drawing.Point(0, 34)
            Me.btnAnnulla.Name = "btnAnnulla"
            Me.btnAnnulla.Size = New System.Drawing.Size(238, 33)
            Me.btnAnnulla.TabIndex = 5
            Me.btnAnnulla.Text = "Annulla"
            '
            'label5
            '
            Me.label5.Location = New System.Drawing.Point(3, 39)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(68, 20)
            Me.label5.Text = "Versione:"
            '
            'version_label
            '
            Me.version_label.Location = New System.Drawing.Point(69, 39)
            Me.version_label.Name = "version_label"
            Me.version_label.Size = New System.Drawing.Size(168, 20)
            Me.version_label.Text = "1.0.0.0"
            '
            'panel1
            '
            Me.panel1.Controls.Add(Me.message_textbox)
            Me.panel1.Controls.Add(Me.label1)
            Me.panel1.Controls.Add(Me.version_label)
            Me.panel1.Controls.Add(Me.appname_label)
            Me.panel1.Controls.Add(Me.label4)
            Me.panel1.Controls.Add(Me.label5)
            Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panel1.Location = New System.Drawing.Point(0, 32)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(238, 238)
            '
            'message_textbox
            '
            Me.message_textbox.Location = New System.Drawing.Point(4, 99)
            Me.message_textbox.Multiline = True
            Me.message_textbox.Name = "message_textbox"
            Me.message_textbox.ReadOnly = True
            Me.message_textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.message_textbox.Size = New System.Drawing.Size(232, 64)
            Me.message_textbox.TabIndex = 11
            '
            'panel2
            '
            Me.panel2.Controls.Add(Me.btnAnnulla)
            Me.panel2.Controls.Add(Me.btnInstalla)
            Me.panel2.Controls.Add(Me.progressBar1)
            Me.panel2.Controls.Add(Me.btnResult)
            Me.panel2.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.panel2.Location = New System.Drawing.Point(0, 203)
            Me.panel2.Name = "panel2"
            Me.panel2.Size = New System.Drawing.Size(238, 67)
            '
            'btnInstalla
            '
            Me.btnInstalla.Dock = System.Windows.Forms.DockStyle.Top
            Me.btnInstalla.Location = New System.Drawing.Point(0, 0)
            Me.btnInstalla.Name = "btnInstalla"
            Me.btnInstalla.Size = New System.Drawing.Size(238, 33)
            Me.btnInstalla.TabIndex = 6
            Me.btnInstalla.Text = "Installa"
            '
            'btnResult
            '
            Me.btnResult.Location = New System.Drawing.Point(0, 0)
            Me.btnResult.Name = "btnResult"
            Me.btnResult.Size = New System.Drawing.Size(240, 67)
            Me.btnResult.TabIndex = 8
            Me.btnResult.Text = "Riavvia"
            Me.btnResult.Visible = False
            '
            'Notification_IT
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.AutoScroll = True
            Me.ClientSize = New System.Drawing.Size(238, 270)
            Me.ControlBox = False
            Me.Controls.Add(Me.panel2)
            Me.Controls.Add(Me.panel1)
            Me.Controls.Add(Me.label2)
            Me.Menu = Me.mainMenu1
            Me.MinimizeBox = False
            Me.Name = "Notification_IT"
            Me.Text = "Aggiornamento"
            Me.TopMost = True
            Me.panel1.ResumeLayout(False)
            Me.panel2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private label1 As System.Windows.Forms.Label
        Private label2 As System.Windows.Forms.Label
        Private appname_label As System.Windows.Forms.Label
        Private label4 As System.Windows.Forms.Label
        Private progressBar1 As System.Windows.Forms.ProgressBar
        Private WithEvents btnAnnulla As System.Windows.Forms.Button
        Private label5 As System.Windows.Forms.Label
        Private version_label As System.Windows.Forms.Label
        Private panel1 As System.Windows.Forms.Panel
        Private panel2 As System.Windows.Forms.Panel
        Private message_textbox As System.Windows.Forms.TextBox
        Private WithEvents btnInstalla As System.Windows.Forms.Button
        Private WithEvents btnResult As System.Windows.Forms.Button

    End Class
End Namespace