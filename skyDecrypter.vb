Imports System.Security.Cryptography
Imports System.Text

Module skyDecrypter

    'decrypts a full skylander byte array
    Public Function decryptSkylander(ByVal skyData As Byte()) As Byte()
        Dim actikey = New Byte() {&H20, &H43, &H6F, &H70, &H79, &H72, &H69, &H67, &H68, &H74, &H20, &H28, &H43, &H29, &H20, &H32, &H30, &H31, &H30, &H20, &H41, &H63, &H74, &H69, &H76, &H69, &H73, &H69, &H6F, &H6E, &H2E, &H20, &H41, &H6C, &H6C, &H20, &H52, &H69, &H67, &H68, &H74, &H73, &H20, &H52, &H65, &H73, &H65, &H72, &H76, &H65, &H64, &H2E, &H20}
        Dim keyInput(85) As Byte
        Dim decSky(1023) As Byte
        Dim encData(15) As Byte
        Dim decData(15) As Byte
        Dim key(15) As Byte
        Dim h As Integer

        h = 0
        'we prepare the byte series used to generate the AES key
        Array.Copy(skyData, 0, keyInput, 0, 32)
        Array.Copy(actikey, 0, keyInput, 33, 53)

        'we avoid decrypting blocks up to 8 since they are never encrypted
        'we also avoid decrypting Imaginator checksums
        'we also avoid decrypting the access control blocks
        Do
            If ((h - 3) Mod 4 = 0) Or h < 8 Or h = 34 Or h = 62 Then

                Array.Copy(skyData, h * 16, decSky, h * 16, 16)

            Else

                keyInput(32) = h
                Array.Copy(skyData, 16 * h, encData, 0, 16)
                key = CalculateMD5Hash(keyInput)
                decData = AESD(encData, key)
                Array.Copy(decData, 0, decSky, h * 16, 16)

            End If
            h = h + 1
        Loop While h <= 63

        Return decSky
    End Function

    'encrypts a full skylander byte array
    Public Function encryptSkylander(ByVal skyData As Byte()) As Byte()
        Dim actikey = New Byte() {&H20, &H43, &H6F, &H70, &H79, &H72, &H69, &H67, &H68, &H74, &H20, &H28, &H43, &H29, &H20, &H32, &H30, &H31, &H30, &H20, &H41, &H63, &H74, &H69, &H76, &H69, &H73, &H69, &H6F, &H6E, &H2E, &H20, &H41, &H6C, &H6C, &H20, &H52, &H69, &H67, &H68, &H74, &H73, &H20, &H52, &H65, &H73, &H65, &H72, &H76, &H65, &H64, &H2E, &H20}
        Dim keyInput(85) As Byte
        Dim encSky(1023) As Byte
        Dim encData(15) As Byte
        Dim decData(15) As Byte
        Dim key(15) As Byte
        Dim h As Integer

        h = 0
        'we prepare the byte series used to generate the AES key
        Array.Copy(skyData, 0, keyInput, 0, 32)
        Array.Copy(actikey, 0, keyInput, 33, 53)

        'we avoid encrypting blocks up to 8 since they are never encrypted
        'we also avoid encrypting Imaginator checksums
        'we also avoid encrypting the access control blocks
        Do
            If ((h - 3) Mod 4 = 0) Or h < 8 Or h = 34 Or h = 62 Then

                Array.Copy(skyData, h * 16, encSky, h * 16, 16)

            Else

                keyInput(32) = h
                Array.Copy(skyData, 16 * h, decData, 0, 16)
                key = CalculateMD5Hash(keyInput)
                encData = AESE(decData, key)
                Array.Copy(encData, 0, encSky, h * 16, 16)

            End If
            h = h + 1
        Loop While h <= 63

        Return encSky
    End Function

    'generates a 0'd skylander array
    Public Function cleanSkylander(ByVal skyData As Byte()) As Byte()
        Dim clnSky(1023) As Byte
        Dim blnkBytes(15) As Byte
        Dim h As Integer

        'we 0 everything but blocks lower than 5 or the Imaginator checksums
        Do
            If ((h - 3) Mod 4 = 0) Or h < 5 Or h = 34 Or h = 62 Then

                Array.Copy(skyData, h * 16, clnSky, h * 16, 16)

            Else

                Array.Copy(blnkBytes, 0, clnSky, h * 16, 16)

            End If
            h = h + 1
        Loop While h <= 63

        Return clnSky

    End Function

    'this function is to detect blank/new skylanders
    Public Function checkBlankSkylander(ByVal skyData As Byte()) As Boolean
        Dim h As Integer
        Dim areaA As Boolean = False
        Dim areaB As Boolean = False

        h = 128
        Do
            If skyData(h) <> 0 Then
                areaA = True
                Exit Do
            End If
            h = h + 1
        Loop While h <= 144

        h = 576
        Do
            If skyData(h) <> 0 Then
                areaB = True
                Exit Do
            End If
            h = h + 1
        Loop While h <= 592


        If areaA And areaB Then
            Return False
        End If

        Return True

    End Function

    'we require the MD5 to generate the AES key
    Public Function CalculateMD5Hash(ByVal input As Byte()) As Byte()
        Dim md5 As MD5 = md5.Create()
        Dim hash As Byte() = md5.ComputeHash(input)
        Return hash
    End Function


    'AES encryption and decryption functions
    Public Function AESE(ByVal input As Byte(), ByVal key As Byte()) As Byte()
        Dim AES As New RijndaelManaged
        AES.Padding = PaddingMode.Zeros
        AES.Key = key
        AES.Mode = CipherMode.ECB
        Dim DESEncrypter As ICryptoTransform = AES.CreateEncryptor
        Dim Buffer As Byte() = input
        Return DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length)
    End Function

    Public Function AESD(ByVal input As Byte(), ByVal key As Byte()) As Byte()
        Dim AES As New RijndaelManaged
        AES.Padding = PaddingMode.Zeros
        AES.Key = key
        AES.Mode = CipherMode.ECB
        Dim DESDecrypter As ICryptoTransform = AES.CreateDecryptor
        Dim Buffer As Byte() = input
        Return DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length)
    End Function
End Module
