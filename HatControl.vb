Option Explicit On
Imports System.Data.OleDb
Module HatControl
    'we get the hat IDs and names from the database
    Public Sub hatsimulator(ByVal SQLrst As OleDbDataReader, ByVal SQLcnt As OleDbConnection, ByVal SQLcmd As OleDbCommand)
        SQLcmd = New OleDbCommand("SELECT * FROM skyhats", SQLcnt)
        SQLrst = SQLcmd.ExecuteReader
        While SQLrst.Read
            Form1.ComboBox1.Items.Insert(SQLrst("ID"), SQLrst("hatname"))
        End While
        SQLrst.Close()
    End Sub

    'we get which hat is written in the figure
    Public Function gethat(ByVal hatid1 As Byte, ByVal hatid2 As Byte, ByVal hatid3 As Byte, ByVal hatid4 As Byte) As Integer
        If hatid1 <> 0 Then
            gethat = Int(hatid1)
            Exit Function
        End If
        If hatid2 <> 0 Then
            gethat = Int(hatid2)
            Exit Function
        End If
        If hatid3 <> 0 Then
            gethat = Int(hatid3)
            Exit Function
        End If
        If hatid4 <> 0 Then
            gethat = Int(hatid4) + 255
            Exit Function
        End If
        Return 0
    End Function
End Module
