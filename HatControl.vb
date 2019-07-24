Option Explicit On
Imports System.Data.OleDb
Module HatControl
    'we get the hat IDs and names from the database
    Public Sub hatsimulator(ByVal exepath As String)
        Dim buffer As String
        Dim count As Integer
        count = 0
        Do
            buffer = ReadIni(exepath & "SkylanderDB.ini", "skyhats", count, "")
            If buffer <> "" Then
                Form1.ComboBox1.Items.Insert(count, buffer)
            Else
                Exit Do
            End If
            count = count + 1
        Loop

    End Sub

    'we get which hat is written in the figure
    Public Function gethat(ByVal hatid1 As Byte, ByVal hatid2 As Byte, ByVal hatid3 As Byte, ByVal hatid4 As Byte) As Integer
        If hatid1 <> 0 Then
            gethat = Int(hatid1)
        ElseIf hatid2 <> 0 Then
            gethat = Int(hatid2)
        ElseIf hatid3 <> 0 Then
            gethat = Int(hatid3)
        ElseIf hatid4 <> 0 Then
            gethat = Int(hatid4) + 255
        Else
            Return 0
        End If
    End Function
End Module
