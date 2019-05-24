Imports System.Windows.Forms

Public Class Dialog1

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Sub mapData(ByVal skylanderData As Byte())
        Label14.Text = Strings.Right("00000000" & Hex(skylanderData(0)) & Hex(skylanderData(1)) & Hex(skylanderData(2)) & Hex(skylanderData(3)), 8)
        Label15.Text = Strings.Right("0000" & Hex(skylanderData(17)) & Hex(skylanderData(16)), 4) & " (" & SwapEndianness(skylanderData(16), skylanderData(17)) & ")"
        Label16.Text = "A:" & Strings.Right("00" & Hex(skylanderData(137)), 2) & "  B:" & Strings.Right("00" & Hex(skylanderData(585)), 2)
        Label17.Text = "A:" & Strings.Right("00" & Hex(skylanderData(274)), 2) & "  B:" & Strings.Right("00" & Hex(skylanderData(722)), 2)
        Label18.Text = Strings.Right("00" & Hex(Form1.ComboBox1.SelectedIndex), 2) & " (" & Form1.ComboBox1.SelectedIndex & ")"

        Label20.Text = Strings.Right("00" & Hex(skylanderData(142)), 2) & Strings.Right("00" & Hex(skylanderData(143)), 2) & "    " & Strings.Right("00" & Hex(skylanderData(590)), 2) & Strings.Right("00" & Hex(skylanderData(591)), 2)
        Label22.Text = Strings.Right("00" & Hex(skylanderData(140)), 2) & Strings.Right("00" & Hex(skylanderData(141)), 2) & "    " & Strings.Right("00" & Hex(skylanderData(588)), 2) & Strings.Right("00" & Hex(skylanderData(589)), 2)
        Label24.Text = Strings.Right("00" & Hex(skylanderData(138)), 2) & Strings.Right("00" & Hex(skylanderData(139)), 2) & "    " & Strings.Right("00" & Hex(skylanderData(586)), 2) & Strings.Right("00" & Hex(skylanderData(587)), 2)
        Label26.Text = Strings.Right("00" & Hex(skylanderData(272)), 2) & Strings.Right("00" & Hex(skylanderData(273)), 2) & "    " & Strings.Right("00" & Hex(skylanderData(720)), 2) & Strings.Right("00" & Hex(skylanderData(721)), 2)

        Label19.Text = checksumCalc(1, 1, skylanderData) & "    " & checksumCalc(1, 2, skylanderData)
        Label21.Text = checksumCalc(2, 1, skylanderData) & "    " & checksumCalc(2, 2, skylanderData)
        Label23.Text = checksumCalc(3, 1, skylanderData) & "    " & checksumCalc(3, 2, skylanderData)
        Label25.Text = checksumCalc(4, 1, skylanderData) & "    " & checksumCalc(4, 2, skylanderData)



        If Label20.Text = Label19.Text And Label21.Text = Label22.Text And Label23.Text = Label24.Text And Label25.Text = Label26.Text Then
            Label39.ForeColor = Color.Green
            Label39.Text = "Checksums OK"
        Else
            Label39.ForeColor = Color.Red
            Label39.Text = "Checksums Error"
        End If
    End Sub

End Class
