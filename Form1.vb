Option Explicit On
Imports System.Data.OleDb
Imports Microsoft.Win32.SafeHandles
Imports SkylanderEditor.DeviceManagement

Public Class Form1
    Dim skylanderBytes(1023) As Byte
    Dim outRepoBytes(32) As Byte
    Dim inRepoBytes(32) As Byte
    Dim skylanderID As Integer
    Dim skylanderGold As Long
    Dim skylanderEXP As Long
    Dim skylanderHat As Integer
    Dim skylanderSkill As Integer
    Dim skylanderSkillbin As String
    Dim skylanderFilepath As String
    Dim exepath As String
    Dim indexVal As Integer
    Dim indexValB As Integer
    Dim moneyA, expA, expB, expC, hatA, hatB, hatC, hatD, skill As Integer
    Dim X
    Dim autom As Boolean
    Dim portalHandle As SafeFileHandle


    Dim SQLcnt As OleDbConnection
    Dim SQLrst As OleDbDataReader
    Dim SQLcmd As OleDbCommand



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'initialize file dialogs, database connection, hat combobox and CRC calculator table
        OpenFileDialog1.FileName = ""
        SaveFileDialog1.FileName = ""
        SQLcnt = New OleDbConnection

        exepath = Application.StartupPath & "\"

        SQLcnt.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & exepath & "SkylanderDB.accdb;Persist Security Info=False"
        SQLcnt.Open()

        hatsimulator(SQLrst, SQLcnt, SQLcmd)
        ComboBox1.SelectedIndex = 0
        CRCcalculator.inittable()
        lockControls()
        lockPortalControls()
        autom = False
    End Sub

    Private Sub OpenDecryptedSkylanderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenDecryptedSkylanderToolStripMenuItem.Click
        'reads data from a decrypted skylander file
        OpenFileDialog1.Filter = "Decrypted Skylanders (*.skd)|*.skd|All Files|*.*"
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileName = "" Then
            Exit Sub
        End If
        skylanderFilepath = OpenFileDialog1.FileName
        OpenFileDialog1.FileName = ""
        skylanderBytes = System.IO.File.ReadAllBytes(skylanderFilepath)
        If checkBlankSkylander(skylanderBytes) Then
            readskylandData(skylanderBytes, True)
            ToolStripStatusLabel1.Text = "This Skylander is blank/new"
        Else
            readskylandData(skylanderBytes, False)
        End If

    End Sub

    Private Sub OpenEncryptedSkylanderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenEncryptedSkylanderToolStripMenuItem.Click
        'reads data from an encrypted skylander file
        OpenFileDialog1.Filter = "Encrypted Skylanders (*.ske)|*.ske|All Files|*.*"
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileName = "" Then
            Exit Sub
        End If
        skylanderFilepath = OpenFileDialog1.FileName
        OpenFileDialog1.FileName = ""
        skylanderBytes = System.IO.File.ReadAllBytes(skylanderFilepath)

        If checkBlankSkylander(skylanderBytes) Then
            readskylandData(skylanderBytes, True)
            ToolStripStatusLabel1.Text = "This Skylander is blank/new"
        Else
            skylanderBytes = decryptSkylander(skylanderBytes)
            readskylandData(skylanderBytes, False)
        End If
        
    End Sub

    Private Sub SaveAsDecryptedSkylanderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsDecryptedSkylanderToolStripMenuItem.Click
        'save data to a decrypted skylander file
        SaveFileDialog1.Filter = "Decrypted Skylanders (*.skd)|*.skd|All Files|*.*"
        SaveFileDialog1.DefaultExt = "skd"
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName = "" Then
            Exit Sub
        End If
        skylanderBytes = writeskylandData(False, skylanderBytes)
        System.IO.File.WriteAllBytes(SaveFileDialog1.FileName, skylanderBytes)
        ToolStripStatusLabel1.Text = "Save Completed to " & SaveFileDialog1.FileName
        SaveFileDialog1.FileName = ""
    End Sub

    Private Sub SaveAsEncryptedSkylanderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsEncryptedSkylanderToolStripMenuItem.Click
        'save data to an encrypted skylander file
        SaveFileDialog1.Filter = "Encrypted Skylanders (*.ske)|*.ske|All Files|*.*"
        SaveFileDialog1.DefaultExt = "ske"
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName = "" Then
            Exit Sub
        End If
        skylanderBytes = writeskylandData(True, skylanderBytes)
        System.IO.File.WriteAllBytes(SaveFileDialog1.FileName, skylanderBytes)

        skylanderBytes = decryptSkylander(skylanderBytes)
        ToolStripStatusLabel1.Text = "Save Completed to " & SaveFileDialog1.FileName
        SaveFileDialog1.FileName = ""
    End Sub

    Private Sub ReadSkylanderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadSkylanderToolStripMenuItem.Click
        'reads skylander data from the portal
        Dim timeout As Integer
        Dim readBlock As Integer

        'reset portal
        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        'set to "read first skylander" mode
        outRepoBytes(1) = &H51
        outRepoBytes(2) = &H20
        readBlock = 0
        Do
            'send report and flush hid queue
            outRepoBytes(3) = readBlock
            outputReport(portalHandle, outRepoBytes)
            flushHid(portalHandle)
            timeout = 0
            Do
                'read the reply from the portal, the portal replies between 1 and 2 reports later
                inputReport(portalHandle, inRepoBytes)
                timeout = timeout + 1
            Loop Until inRepoBytes(1) <> &H53 Or timeout = 4

            If timeout <> 4 Then
                'if we didn't time out we copy the the bytes into the array
                Array.Copy(inRepoBytes, 4, skylanderBytes, readBlock * 16, 16)
                readBlock = readBlock + 1
            End If

        Loop While readBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        If checkBlankSkylander(skylanderBytes) Then
            readskylandData(skylanderBytes, True)
            ToolStripStatusLabel1.Text = "This Skylander is blank/new"
        Else
            skylanderBytes = decryptSkylander(skylanderBytes)
            readskylandData(skylanderBytes, False)
        End If
    End Sub

    Private Sub WriteSkylanderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WriteSkylanderToolStripMenuItem.Click
        'write data to skylander in portal
        Dim writeBlock As Integer

        'first we encrypt the data
        skylanderBytes = writeskylandData(True, skylanderBytes)

        'reset portal
        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        'set to "write first skylander" mode
        outRepoBytes(1) = &H57
        outRepoBytes(2) = &H20
        writeBlock = 0
        Do
            'we avoid writing to the first 4 blocks and access control blocks
            If ((writeBlock - 3) Mod 4 = 0) Or writeBlock < 5 Then

                writeBlock = writeBlock + 1

            Else

                outRepoBytes(3) = writeBlock
                'we get the bytes from the data array and put out the report, we need to wait a bit before sending another write report too

                Array.Copy(skylanderBytes, writeBlock * 16, outRepoBytes, 4, 16)
                outputReport(portalHandle, outRepoBytes)
                System.Threading.Thread.Sleep(100)
                writeBlock = writeBlock + 1

            End If
        Loop While writeBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        skylanderBytes = decryptSkylander(skylanderBytes)
        ToolStripStatusLabel1.Text = "Save Completed to portal"
    End Sub

    Private Sub WriteSwapperOtherHalfToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WriteSwapperOtherHalfToolStripMenuItem.Click

        'same as write to portal, but writes to the second position skylander (usually the Top half of a swapper)
        Dim writeBlock As Integer

        skylanderBytes = writeskylandData(True, skylanderBytes)

        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        outRepoBytes(1) = &H57
        outRepoBytes(2) = &H21
        writeBlock = 0
        Do

            If ((writeBlock - 3) Mod 4 = 0) Or writeBlock < 5 Then

                writeBlock = writeBlock + 1

            Else

                outRepoBytes(3) = writeBlock

                Array.Copy(skylanderBytes, writeBlock * 16, outRepoBytes, 4, 16)
                outputReport(portalHandle, outRepoBytes)
                System.Threading.Thread.Sleep(100)
                writeBlock = writeBlock + 1

            End If
        Loop While writeBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        skylanderBytes = decryptSkylander(skylanderBytes)
        ToolStripStatusLabel1.Text = "Save Completed to portal"
    End Sub

    Private Sub ReadSwapperOtherHalfToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadSwapperOtherHalfToolStripMenuItem.Click

        'same as read from portal, but reads the second position skylander (usually the Top half of a swapper)
        Dim timeout As Integer
        Dim readBlock As Integer


        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        outRepoBytes(1) = &H51
        outRepoBytes(2) = &H21
        readBlock = 0
        Do
            outRepoBytes(3) = readBlock
            outputReport(portalHandle, outRepoBytes)
            flushHid(portalHandle)
            timeout = 0
            Do
                inputReport(portalHandle, inRepoBytes)
                timeout = timeout + 1
            Loop Until inRepoBytes(1) <> &H53 Or timeout = 4

            If timeout <> 4 Then
                Array.Copy(inRepoBytes, 4, skylanderBytes, readBlock * 16, 16)
                readBlock = readBlock + 1
            End If

        Loop While readBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        If checkBlankSkylander(skylanderBytes) Then
            readskylandData(skylanderBytes, True)
            ToolStripStatusLabel1.Text = "This Skylander is blank/new"
        Else
            skylanderBytes = decryptSkylander(skylanderBytes)
            readskylandData(skylanderBytes, False)
        End If
    End Sub

    Private Sub ResetFigureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetFigureToolStripMenuItem.Click
        'writes 0s to a skylander, brand new figures come like this

        Dim writeBlock As Integer

        skylanderBytes = cleanSkylander(skylanderBytes)

        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        outRepoBytes(1) = &H57
        outRepoBytes(2) = &H20
        writeBlock = 0
        Do
            'along avoiding the first 4 blocks and access control blocks we also avoid blocks 0x22 and 0x3E because
            'here is where imaginator checksums are written and we don't want to 0 them
            If ((writeBlock - 3) Mod 4 = 0) Or writeBlock < 5 Or writeBlock = &H22 Or writeBlock = &H3E Then

                writeBlock = writeBlock + 1

            Else

                outRepoBytes(3) = writeBlock

                Array.Copy(skylanderBytes, writeBlock * 16, outRepoBytes, 4, 16)
                outputReport(portalHandle, outRepoBytes)
                System.Threading.Thread.Sleep(100)
                writeBlock = writeBlock + 1

            End If
        Loop While writeBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        ToolStripStatusLabel1.Text = "Reset Complete"
    End Sub

    Private Sub ResetSwapperOtherHalfToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetSwapperOtherHalfToolStripMenuItem.Click
        'writes 0s to the top half of a swapper skylander, brand new figures come like this
        Dim writeBlock As Integer

        skylanderBytes = cleanSkylander(skylanderBytes)

        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        outRepoBytes(1) = &H57
        outRepoBytes(2) = &H21
        writeBlock = 0
        Do

            If ((writeBlock - 3) Mod 4 = 0) Or writeBlock < 5 Or writeBlock = &H22 Or writeBlock = &H3E Then

                writeBlock = writeBlock + 1

            Else

                outRepoBytes(3) = writeBlock

                Array.Copy(skylanderBytes, writeBlock * 16, outRepoBytes, 4, 16)
                outputReport(portalHandle, outRepoBytes)
                System.Threading.Thread.Sleep(100)
                writeBlock = writeBlock + 1

            End If
        Loop While writeBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        ToolStripStatusLabel1.Text = "Reset Complete"
    End Sub

    Private Sub RawDumpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RawDumpToolStripMenuItem.Click
        'this function reads the skylander in the portal and saves it to a file without mapping it
        'useful for making 0'd backups of skylanders

        SaveFileDialog1.Filter = "Encrypted Skylanders (*.ske)|*.ske|All Files|*.*"
        SaveFileDialog1.DefaultExt = "ske"
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName = "" Then
            Exit Sub
        End If

        Dim timeout As Integer
        Dim readBlock As Integer


        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        outRepoBytes(1) = &H51
        outRepoBytes(2) = &H20
        readBlock = 0
        Do
            outRepoBytes(3) = readBlock
            outputReport(portalHandle, outRepoBytes)
            flushHid(portalHandle)
            timeout = 0
            Do
                inputReport(portalHandle, inRepoBytes)
                timeout = timeout + 1
            Loop Until inRepoBytes(1) <> &H53 Or timeout = 4

            If timeout <> 4 Then
                Array.Copy(inRepoBytes, 4, skylanderBytes, readBlock * 16, 16)
                readBlock = readBlock + 1
            End If

        Loop While readBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        System.IO.File.WriteAllBytes(SaveFileDialog1.FileName, skylanderBytes)
        ToolStripStatusLabel1.Text = "Save Completed to " & SaveFileDialog1.FileName
        SaveFileDialog1.FileName = ""
    End Sub

    Private Sub PortalToDecryptedFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PortalToDecryptedFileToolStripMenuItem.Click
        'this function reads the skylander in the portal and saves it to a file without mapping it
        'useful for making 0'd backups of skylanders

        SaveFileDialog1.Filter = "Decrypted Skylanders (*.skd)|*.skd|All Files|*.*"
        SaveFileDialog1.DefaultExt = "skd"
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName = "" Then
            Exit Sub
        End If

        Dim timeout As Integer
        Dim readBlock As Integer


        outRepoBytes(1) = &H52
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(50)

        outRepoBytes(1) = &H41
        outRepoBytes(2) = 1
        outputReport(portalHandle, outRepoBytes)
        System.Threading.Thread.Sleep(500)

        outRepoBytes(1) = &H51
        outRepoBytes(2) = &H20
        readBlock = 0
        Do
            outRepoBytes(3) = readBlock
            outputReport(portalHandle, outRepoBytes)
            flushHid(portalHandle)
            timeout = 0
            Do
                inputReport(portalHandle, inRepoBytes)
                timeout = timeout + 1
            Loop Until inRepoBytes(1) <> &H53 Or timeout = 4

            If timeout <> 4 Then
                Array.Copy(inRepoBytes, 4, skylanderBytes, readBlock * 16, 16)
                readBlock = readBlock + 1
            End If

        Loop While readBlock <= &H3F
        Array.Clear(outRepoBytes, 0, 33)
        skylanderBytes = decryptSkylander(skylanderBytes)
        System.IO.File.WriteAllBytes(SaveFileDialog1.FileName, skylanderBytes)
        ToolStripStatusLabel1.Text = "Save Completed to " & SaveFileDialog1.FileName
        SaveFileDialog1.FileName = ""
    End Sub


    Private Sub FixCRCsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FixCRCsToolStripMenuItem.Click
        'update checksums section A
        X = checksumCalc(2, 1, skylanderBytes)
        skylanderBytes(140) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(141) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(3, 1, skylanderBytes)
        skylanderBytes(138) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(139) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(4, 1, skylanderBytes)
        skylanderBytes(272) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(273) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(1, 1, skylanderBytes)
        skylanderBytes(142) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(143) = CByte("&H" & Strings.Right(X, 2))

        'update checksums section B
        X = checksumCalc(2, 2, skylanderBytes)
        skylanderBytes(588) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(589) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(3, 2, skylanderBytes)
        skylanderBytes(586) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(587) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(4, 2, skylanderBytes)
        skylanderBytes(720) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(721) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(1, 2, skylanderBytes)
        skylanderBytes(590) = CByte("&H" & Strings.Left(X, 2))
        skylanderBytes(591) = CByte("&H" & Strings.Right(X, 2))
    End Sub


    Sub readskylandData(ByRef skylanderData As Byte(), ByVal blank As Boolean)

        'get skylander ID
        skylanderID = SwapEndianness(skylanderData(16), skylanderData(17))

        SQLcmd = New OleDbCommand("SELECT * FROM Skylanders WHERE ID = " & skylanderID, SQLcnt)
        SQLrst = SQLcmd.ExecuteReader

        If Not SQLrst.HasRows Then
            'if figure ID is not in database
            Label7.Text = "Unknown ID: " & skylanderID
            Label8.Text = "- -"
            PictureBox1.Image = Nothing
            lockControls()
            ToolStripStatusLabel1.Text = "Unknown Skylander read"
            Exit Sub

        Else
            SQLrst.Read()
            'get figure name and element
            Label7.Text = SQLrst("skyname")
            Label8.Text = SQLrst("element")
            'load skylander picture if available
            PictureBox1.ImageLocation = exepath & "images\" & Trim(Str(skylanderID)) & ".jpg"
            'lock controls if a swapper bottom half, item or stage is opened
            If Label8.Text = "Item" Or Label8.Text = "Stage" Or blank Then
                lockControls()
                ToolStripStatusLabel1.Text = "Finished Reading"
                Exit Sub
            End If
        End If
        'get high index and fill the variable sections
        If skylanderData(137) > skylanderData(585) Then
            moneyA = 131
            expA = 128
            hatA = 148
            skill = 144
            indexVal = 1
        ElseIf skylanderData(137) < skylanderData(585) Then
            moneyA = 579
            expA = 576
            hatA = 596
            skill = 592
            indexVal = 2
        End If

        If skylanderData(274) > skylanderData(722) Then
            expB = 275
            expC = 280
            hatB = 277
            hatC = 284
            hatD = 286
            indexValB = 1
        ElseIf skylanderData(274) < skylanderData(722) Then
            expB = 723
            expC = 728
            hatB = 725
            hatC = 732
            hatD = 734
            indexValB = 2
        End If

        If SQLrst("special").ToString = "Vehicle" Then
            skylanderGold = SwapEndianness(skylanderData(expC), skylanderData(expC + 1))
            Label8.Text = Label8.Text & " Vehicle"
            TextBox2.Text = skylanderGold
            TextBox1.Text = 0
            ComboBox1.SelectedIndex = 0

            Exit Sub
        End If

        'get skylander hat
        skylanderHat = HatControl.gethat(skylanderData(hatA), skylanderData(hatB), skylanderData(hatC), skylanderData(hatD))
        ComboBox1.SelectedIndex = skylanderHat
        'get skylander gold
        skylanderGold = SwapEndianness(skylanderData(moneyA), skylanderData(moneyA + 1))
        TextBox2.Text = skylanderGold
        'get skylander xp
        skylanderEXP = SwapEndianness(skylanderData(expA), skylanderData(expA + 1)) + SwapEndianness(skylanderData(expB), skylanderData(expB + 1)) + SwapEndianness(skylanderData(expC), skylanderData(expC + 1))
        If skylanderData(expC + 2) = 1 Then
            skylanderEXP = skylanderEXP + 65536
        End If
        TextBox1.Text = skylanderEXP
        'get skylander skills
        skylanderSkill = SwapEndianness(skylanderData(skill), skylanderData(skill + 1))
        autom = True
        If skylanderSkill And 2 Then
            RadioButton1.Checked = True
        ElseIf (skylanderSkill And 1) And Not (skylanderSkill And 2) Then
            RadioButton2.Checked = True
        Else
            RadioButton1.Checked = False
            RadioButton2.Checked = False
        End If
        autom = False
        unlockControls()
        SQLrst.Close()
        ToolStripStatusLabel1.Text = "Finished Reading"
    End Sub


    Function writeskylandData(ByVal encryption As Boolean, ByRef skylanderData As Byte()) As Byte()

        'format money and place in array
        X = Hex(TextBox2.Text)
        X = Strings.Right("0000" & X, 4)
        If InStr(1, Label8.Text, "Vehicle") Then
            skylanderData(expC) = CByte("&H" & Strings.Right(X, 2))
            skylanderData(expC + 1) = CByte("&H" & Strings.Left(X, 2))
            GoTo vehicleSkip
        Else
            skylanderData(moneyA) = CByte("&H" & Strings.Right(X, 2))
            skylanderData(moneyA + 1) = CByte("&H" & Strings.Left(X, 2))
        End If

        'place hat in correct place in array

        skylanderHat = ComboBox1.SelectedIndex
        If skylanderHat < 46 Then
            skylanderData(hatA) = skylanderHat
            skylanderData(hatB) = 0
            skylanderData(hatC) = 0
            skylanderData(hatD) = 0
        End If
        If skylanderHat >= 46 And skylanderHat < 97 Then
            skylanderData(hatB) = skylanderHat
            skylanderData(hatC) = 0
            skylanderData(hatA) = 0
            skylanderData(hatD) = 0
        End If
        If skylanderHat >= 97 And skylanderHat < 256 Then
            skylanderData(hatC) = skylanderHat
            skylanderData(hatB) = 0
            skylanderData(hatA) = 0
            skylanderData(hatD) = 0
        End If
        If skylanderHat >= 256 Then
            skylanderData(hatD) = skylanderHat - 255
            skylanderData(hatC) = 0
            skylanderData(hatB) = 0
            skylanderData(hatA) = 0
        End If


        'set exp into the array
        If TextBox1.Text > 162035 Then
            X = Hex(TextBox1.Text - 162036)
            X = Strings.Right("0000" & X, 4)
            skylanderData(expC) = CByte("&H" & Strings.Right(X, 2))
            skylanderData(expC + 1) = CByte("&H" & Strings.Left(X, 2))
            skylanderData(expC + 2) = 1

            skylanderData(expB) = 12
            skylanderData(expB + 1) = 248

            skylanderData(expA) = 232
            skylanderData(expA + 1) = 128

        ElseIf TextBox1.Text <= 162035 And TextBox1.Text > 96500 Then
            X = Hex(TextBox1.Text - 96500)
            X = Strings.Right("0000" & X, 4)
            skylanderData(expC) = CByte("&H" & Strings.Right(X, 2))
            skylanderData(expC + 1) = CByte("&H" & Strings.Left(X, 2))
            skylanderData(expC + 2) = 0

            skylanderData(expB) = 12
            skylanderData(expB + 1) = 248

            skylanderData(expA) = 232
            skylanderData(expA + 1) = 128

        ElseIf TextBox1.Text <= 96500 And TextBox1.Text > 33000 Then
            X = Hex(TextBox1.Text - 33000)
            X = Strings.Right("0000" & X, 4)
            skylanderData(expB) = CByte("&H" & Strings.Right(X, 2))
            skylanderData(expB + 1) = CByte("&H" & Strings.Left(X, 2))

            skylanderData(expC) = 0
            skylanderData(expC + 1) = 0
            skylanderData(expC + 2) = 0

            skylanderData(expA) = 232
            skylanderData(expA + 1) = 128

        ElseIf TextBox1.Text <= 33000 Then
            X = Hex(TextBox1.Text)
            X = Strings.Right("0000" & X, 4)
            skylanderData(expA) = CByte("&H" & Strings.Right(X, 2))
            skylanderData(expA + 1) = CByte("&H" & Strings.Left(X, 2))

            skylanderData(expC) = 0
            skylanderData(expC + 1) = 0
            skylanderData(expC + 2) = 0

            skylanderData(expB) = 0
            skylanderData(expB + 1) = 0

        End If

        'set skill path

        If skylanderSkill And 1 Then
            X = Hex(skylanderSkill)
            X = Strings.Right("0000" & X, 4)
            skylanderData(expA + 16) = CByte("&H" & Strings.Right(X, 2))
            skylanderData(expA + 17) = CByte("&H" & Strings.Left(X, 2))
        End If

vehicleSkip:
        'update checksums
        X = checksumCalc(2, indexVal, skylanderData)
        skylanderData(expA + 12) = CByte("&H" & Strings.Left(X, 2))
        skylanderData(expA + 13) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(3, indexVal, skylanderData)
        skylanderData(expA + 10) = CByte("&H" & Strings.Left(X, 2))
        skylanderData(expA + 11) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(4, indexValB, skylanderData)
        skylanderData(expB - 3) = CByte("&H" & Strings.Left(X, 2))
        skylanderData(expB - 2) = CByte("&H" & Strings.Right(X, 2))

        X = checksumCalc(1, indexVal, skylanderData)
        skylanderData(expA + 14) = CByte("&H" & Strings.Left(X, 2))
        skylanderData(expA + 15) = CByte("&H" & Strings.Right(X, 2))


        If encryption = True Then
            skylanderData = encryptSkylander(skylanderData)
        End If

        Return skylanderData
    End Function

    'we make sure only numeric values are allowed in our textboxes
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    'level 20 is the max and 197500 points is needed for it, we change the level label to match each level threshold
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            If Int(TextBox1.Text) > 197500 Then
                TextBox1.Text = 197500
            End If
            If Int(TextBox1.Text) = 197500 Then
                Label9.Text = "Level: 20"
            End If
            If Int(TextBox1.Text) >= 174300 And Int(TextBox1.Text) < 197500 Then
                Label9.Text = "Level: 19"
            End If
            If Int(TextBox1.Text) >= 152600 And Int(TextBox1.Text) < 174300 Then
                Label9.Text = "Level: 18"
            End If
            If Int(TextBox1.Text) >= 132400 And Int(TextBox1.Text) < 152600 Then
                Label9.Text = "Level: 17"
            End If
            If Int(TextBox1.Text) >= 113700 And Int(TextBox1.Text) < 132400 Then
                Label9.Text = "Level: 16"
            End If
            If Int(TextBox1.Text) >= 96500 And Int(TextBox1.Text) < 113700 Then
                Label9.Text = "Level: 15"
            End If
            If Int(TextBox1.Text) >= 85800 And Int(TextBox1.Text) < 96500 Then
                Label9.Text = "Level: 14"
            End If
            If Int(TextBox1.Text) >= 69200 And Int(TextBox1.Text) < 85800 Then
                Label9.Text = "Level: 13"
            End If
            If Int(TextBox1.Text) >= 55000 And Int(TextBox1.Text) < 69200 Then
                Label9.Text = "Level: 12"
            End If
            If Int(TextBox1.Text) >= 43000 And Int(TextBox1.Text) < 55000 Then
                Label9.Text = "Level: 11"
            End If
            If Int(TextBox1.Text) >= 33000 And Int(TextBox1.Text) < 43000 Then
                Label9.Text = "Level: 10"
            End If
            If Int(TextBox1.Text) >= 24800 And Int(TextBox1.Text) < 33000 Then
                Label9.Text = "Level: 9"
            End If
            If Int(TextBox1.Text) >= 18200 And Int(TextBox1.Text) < 24800 Then
                Label9.Text = "Level: 8"
            End If
            If Int(TextBox1.Text) >= 13000 And Int(TextBox1.Text) < 18200 Then
                Label9.Text = "Level: 7"
            End If
            If Int(TextBox1.Text) >= 9000 And Int(TextBox1.Text) < 13000 Then
                Label9.Text = "Level: 6"
            End If
            If Int(TextBox1.Text) >= 6000 And Int(TextBox1.Text) < 9000 Then
                Label9.Text = "Level: 5"
            End If
            If Int(TextBox1.Text) >= 3800 And Int(TextBox1.Text) < 6000 Then
                Label9.Text = "Level: 4"
            End If
            If Int(TextBox1.Text) >= 2200 And Int(TextBox1.Text) < 3800 Then
                Label9.Text = "Level: 3"
            End If
            If Int(TextBox1.Text) >= 1000 And Int(TextBox1.Text) < 2200 Then
                Label9.Text = "Level: 2"
            End If
            If Int(TextBox1.Text) >= 0 And Int(TextBox1.Text) < 1000 Then
                Label9.Text = "Level: 1"
            End If
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        'max money is 65000
        If TextBox2.Text <> "" Then
            If Int(TextBox2.Text) > 65000 Then
                TextBox2.Text = 65000
            End If
        End If
    End Sub


    'this is to catch if the portal is removed
    Protected Overrides Sub WndProc(ByRef m As Message)

        If m.Msg = WM_DEVICECHANGE Then
            If (m.WParam.ToInt32 = DBT_DEVICEREMOVECOMPLETE) Then

                ' If WParam contains DBT_DEVICEREMOVAL, a device has been removed.
                ' Find out if it's the device we're communicating with.

                If checkDevice(m) Then
                    lockPortalControls()
                    ToolStripStatusLabel1.Text = "Portal Removed!"
                End If

            End If
        End If
        MyBase.WndProc(m)

    End Sub

    Private Sub lockControls()
        ComboBox1.SelectedIndex = 0
        TextBox1.Text = 0
        TextBox2.Text = 0
        ComboBox1.Enabled = False
        TextBox1.Enabled = False
        TextBox2.Enabled = False

        RadioButton1.Checked = False
        RadioButton2.Checked = False
        RadioButton1.Enabled = False
        RadioButton2.Enabled = False
    End Sub

    Private Sub unlockControls()
        ComboBox1.Enabled = True
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        RadioButton1.Enabled = True
        RadioButton2.Enabled = True
    End Sub

    Public Sub lockPortalControls()
        ReadSkylanderToolStripMenuItem.Enabled = False
        WriteSkylanderToolStripMenuItem.Enabled = False
        ReadSwapperOtherHalfToolStripMenuItem.Enabled = False
        WriteSwapperOtherHalfToolStripMenuItem.Enabled = False
        RawDumpToolStripMenuItem.Enabled = False
        ResetFigureToolStripMenuItem.Enabled = False
        ResetSwapperOtherHalfToolStripMenuItem.Enabled = False
        PortalToDecryptedFileToolStripMenuItem.Enabled = False
    End Sub

    Public Sub unlockPortalControls()
        ReadSkylanderToolStripMenuItem.Enabled = True
        WriteSkylanderToolStripMenuItem.Enabled = True
        ReadSwapperOtherHalfToolStripMenuItem.Enabled = True
        WriteSwapperOtherHalfToolStripMenuItem.Enabled = True
        RawDumpToolStripMenuItem.Enabled = True
        ResetFigureToolStripMenuItem.Enabled = True
        ResetSwapperOtherHalfToolStripMenuItem.Enabled = True
        PortalToDecryptedFileToolStripMenuItem.Enabled = True
    End Sub


    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.ShowDialog()
    End Sub

    'fill the metadata window and show it
    Private Sub ShowMetadataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowMetadataToolStripMenuItem.Click
        Dialog1.mapData(skylanderBytes)
        Dialog1.ShowDialog()
    End Sub

    'cleanup by closing the database and closing the hid handles to the portal
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SQLcnt.Close()
        CloseCommunications(portalHandle)
    End Sub

    'get the portal handle to work with
    Private Sub ConnectToPortalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectToPortalToolStripMenuItem.Click
        portalHandle = FindThePortal()
    End Sub


   
End Class
