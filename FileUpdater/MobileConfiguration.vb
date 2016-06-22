Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.Reflection
Imports System.Threading
Imports System.Configuration
Imports System.Xml

Public Class MobileConfiguration

    Public Shared Settings As System.Collections.Specialized.NameValueCollection

    Shared Sub New()

        Dim configFile As String = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly.GetName.CodeBase), "app.config")

        If Not File.Exists(configFile) Then
            CreateEmptyConfigFile(configFile)
            'Throw New FileNotFoundException(String.Format("Application configuration file '{0}' not found.", configFile))
        End If

        Dim xmlDocument As New XmlDocument

        Dim fileInfo As New FileInfo(configFile)
        Dim attributes As FileAttributes = fileInfo.Attributes

        If (attributes And FileAttributes.[ReadOnly]) = FileAttributes.[ReadOnly] Then
            fileInfo.Attributes = fileInfo.Attributes And Not FileAttributes.[ReadOnly]
        End If

OpenConfigFile:

        Try
            xmlDocument.Load(configFile)
        Catch ex As Exception

            File.Delete(configFile)
            CreateEmptyConfigFile(configFile)
            GoTo OpenConfigFile

        End Try

        Dim nodeList As XmlNodeList = xmlDocument.GetElementsByTagName("appSettings")

        Settings = New System.Collections.Specialized.NameValueCollection

        Dim node As XmlNode

        For Each node In nodeList

            Dim key As XmlNode

            For Each key In node.ChildNodes
                MobileConfiguration.Settings.Add(key.Attributes.ItemOf("key").Value, key.Attributes.ItemOf("value").Value)
            Next

        Next

    End Sub


    Private Shared Sub CreateEmptyConfigFile(ByVal configFile As String)

        Dim doc As XDocument = New XDocument(New XDeclaration("1.0", "utf-8", String.Empty), New XElement("configuration", New XElement("appSettings")))

        doc.Save(configFile)

    End Sub

    Shared Sub Save()

        Dim configFile As String = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly.GetName.CodeBase), "app.config")

        If Not File.Exists(configFile) Then
            Throw New FileNotFoundException(String.Format("Application configuration file '{0}' not found.", configFile))
        End If

        Dim xmlDocument As New XmlDocument

        xmlDocument.Load(configFile)

        Dim nodeList As XmlNodeList = xmlDocument.GetElementsByTagName("appSettings")

        For i As Integer = 0 To Settings.Count - 1

            Dim sKey As String = Settings.GetKey(i)
            Dim sValue As String = Settings.Get(i)
            Dim bFound As Boolean = False
            Dim node As XmlNode

            For Each node In nodeList

                Dim key As XmlNode

                For Each key In node.ChildNodes

                    If key.Attributes.ItemOf("key").Value = sKey Then

                        key.Attributes.ItemOf("value").Value = sValue
                        bFound = True
                        Exit For

                    End If

                Next

                If bFound Then Exit For

            Next

            If Not bFound Then

                Dim elem As XmlElement

                Dim attr As XmlAttribute

                'main node

                elem = xmlDocument.CreateElement("add")
                attr = xmlDocument.CreateAttribute("key")
                attr.Value = sKey
                elem.Attributes.Append(attr)
                attr = xmlDocument.CreateAttribute("value")
                attr.Value = sValue
                elem.Attributes.Append(attr)

                xmlDocument.SelectSingleNode("//appSettings").AppendChild(elem)

            End If

        Next

        xmlDocument.Save(configFile)

    End Sub


End Class
