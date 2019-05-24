Option Explicit On
Module CRCcalculator
    Dim CRCTab(255) As Long '               Array for single byte CRCs loaded from table

    'swap from little endian to big endian and viceversa
    Public Function SwapEndianness(ByVal byte1 As Byte, ByVal byte2 As Byte) As Long
        Dim bytebuffer As String

        bytebuffer = IIf(byte1 < &H10, "0", "") & Hex$(byte1)
        bytebuffer = Hex(byte2) & bytebuffer
        bytebuffer = Int("&H" & bytebuffer)
        Return bytebuffer

    End Function

    'CRC16 CCITT function
    Public Function crc16(ByVal databyte() As Byte) As String

        Dim X As Long
        Dim TempLng As Integer
        Dim CRC As Long

        CRC = &HFFFF&                 '                     Load initial value (normally 0xFFFF)

        For X = 0 To UBound(databyte) '                             Loop through data bytes
            TempLng = ((CRC \ &H100&) Xor databyte(X)) '           Shift left (>>8) XOR with data
            CRC = ((CRC * &H100&) And &HFFFF&) Xor CRCTab(TempLng) ' Shift right (<<8) prevent overflow, XOR with table
        Next
        crc16 = Right("0000" & Hex(CRC), 4)
    End Function


    'calculate the checksums of a skylander by type
    Public Function checksumCalc(ByVal chkType As Integer, ByVal sector As Integer, ByRef skylanderData As Byte()) As String
        Dim chksum1, chksum2, chksum3, chksum4 As Integer

        If sector = 1 Then
            chksum1 = 128
            chksum2 = 144
            chksum3 = 208
            chksum4 = 274
        ElseIf sector = 2 Then
            chksum1 = 576
            chksum2 = 592
            chksum3 = 656
            chksum4 = 722
        End If


        Dim crcbuffer() As Byte
        Dim i, a
        Select Case chkType
            Case 1
                'type 1 checksum, first block, 2 last bytes replaced with 0500
                ReDim crcbuffer(15)
                i = 0
                Do
                    crcbuffer(i) = (skylanderData(chksum1 + i))
                    i = i + 1
                Loop While i < 14
                crcbuffer(14) = 5
                crcbuffer(15) = 0
            Case 2
                'type 2 checksum first 3 blocks
                ReDim crcbuffer(47)
                i = 0
                a = 0
                Do
                    If i = 32 Then
                        a = a + 16
                    End If
                    crcbuffer(i) = (skylanderData(chksum2 + a))
                    i = i + 1
                    a = a + 1
                Loop While i < 47
            Case 3
                'type 3 checksum, next 3 blocks + 224 0s bytes
                ReDim crcbuffer(271)
                i = 0
                a = 0
                Do
                    If i = 32 Then
                        a = a + 16
                    End If
                    crcbuffer(i) = (skylanderData(chksum3 + a))
                    i = i + 1
                    a = a + 1
                Loop While i < 47
            Case 4
                'type 4 checksum, last 62 bytes, xor'd by E953
                ReDim crcbuffer(61)
                i = 0
                a = 0
                Do
                    If i = 30 Then
                        a = a + 16
                    End If
                    crcbuffer(i) = (skylanderData(chksum4 + a))
                    i = i + 1
                    a = a + 1
                Loop While i < 61
                a = crc16(crcbuffer)
                a = Hex(CLng("&H" & a) Xor &HE953&)
                a = Right("0000" & a, 4)
                checksumCalc = Right(a, 2) & Left(a, 2)
                Exit Function
        End Select
        i = Right("0000" & crc16(crcbuffer), 4)
        checksumCalc = Right(i, 2) & Left(i, 2)
    End Function



    'CRC-16 CCITT table
    Public Sub inittable()

        CRCTab(0) = &H0&
        CRCTab(1) = &H1021&
        CRCTab(2) = &H2042&
        CRCTab(3) = &H3063&
        CRCTab(4) = &H4084&
        CRCTab(5) = &H50A5&
        CRCTab(6) = &H60C6&
        CRCTab(7) = &H70E7&
        CRCTab(8) = &H8108&
        CRCTab(9) = &H9129&
        CRCTab(10) = &HA14A&
        CRCTab(11) = &HB16B&
        CRCTab(12) = &HC18C&
        CRCTab(13) = &HD1AD&
        CRCTab(14) = &HE1CE&
        CRCTab(15) = &HF1EF&
        CRCTab(16) = &H1231&
        CRCTab(17) = &H210&
        CRCTab(18) = &H3273&
        CRCTab(19) = &H2252&
        CRCTab(20) = &H52B5&
        CRCTab(21) = &H4294&
        CRCTab(22) = &H72F7&
        CRCTab(23) = &H62D6&
        CRCTab(24) = &H9339&
        CRCTab(25) = &H8318&
        CRCTab(26) = &HB37B&
        CRCTab(27) = &HA35A&
        CRCTab(28) = &HD3BD&
        CRCTab(29) = &HC39C&
        CRCTab(30) = &HF3FF&
        CRCTab(31) = &HE3DE&
        CRCTab(32) = &H2462&
        CRCTab(33) = &H3443&
        CRCTab(34) = &H420&
        CRCTab(35) = &H1401&
        CRCTab(36) = &H64E6&
        CRCTab(37) = &H74C7&
        CRCTab(38) = &H44A4&
        CRCTab(39) = &H5485&
        CRCTab(40) = &HA56A&
        CRCTab(41) = &HB54B&
        CRCTab(42) = &H8528&
        CRCTab(43) = &H9509&
        CRCTab(44) = &HE5EE&
        CRCTab(45) = &HF5CF&
        CRCTab(46) = &HC5AC&
        CRCTab(47) = &HD58D&
        CRCTab(48) = &H3653&
        CRCTab(49) = &H2672&
        CRCTab(50) = &H1611&
        CRCTab(51) = &H630&
        CRCTab(52) = &H76D7&
        CRCTab(53) = &H66F6&
        CRCTab(54) = &H5695&
        CRCTab(55) = &H46B4&
        CRCTab(56) = &HB75B&
        CRCTab(57) = &HA77A&
        CRCTab(58) = &H9719&
        CRCTab(59) = &H8738&
        CRCTab(60) = &HF7DF&
        CRCTab(61) = &HE7FE&
        CRCTab(62) = &HD79D&
        CRCTab(63) = &HC7BC&
        CRCTab(64) = &H48C4&
        CRCTab(65) = &H58E5&
        CRCTab(66) = &H6886&
        CRCTab(67) = &H78A7&
        CRCTab(68) = &H840&
        CRCTab(69) = &H1861&
        CRCTab(70) = &H2802&
        CRCTab(71) = &H3823&
        CRCTab(72) = &HC9CC&
        CRCTab(73) = &HD9ED&
        CRCTab(74) = &HE98E&
        CRCTab(75) = &HF9AF&
        CRCTab(76) = &H8948&
        CRCTab(77) = &H9969&
        CRCTab(78) = &HA90A&
        CRCTab(79) = &HB92B&
        CRCTab(80) = &H5AF5&
        CRCTab(81) = &H4AD4&
        CRCTab(82) = &H7AB7&
        CRCTab(83) = &H6A96&
        CRCTab(84) = &H1A71&
        CRCTab(85) = &HA50&
        CRCTab(86) = &H3A33&
        CRCTab(87) = &H2A12&
        CRCTab(88) = &HDBFD&
        CRCTab(89) = &HCBDC&
        CRCTab(90) = &HFBBF&
        CRCTab(91) = &HEB9E&
        CRCTab(92) = &H9B79&
        CRCTab(93) = &H8B58&
        CRCTab(94) = &HBB3B&
        CRCTab(95) = &HAB1A&
        CRCTab(96) = &H6CA6&
        CRCTab(97) = &H7C87&
        CRCTab(98) = &H4CE4&
        CRCTab(99) = &H5CC5&
        CRCTab(100) = &H2C22&
        CRCTab(101) = &H3C03&
        CRCTab(102) = &HC60&
        CRCTab(103) = &H1C41&
        CRCTab(104) = &HEDAE&
        CRCTab(105) = &HFD8F&
        CRCTab(106) = &HCDEC&
        CRCTab(107) = &HDDCD&
        CRCTab(108) = &HAD2A&
        CRCTab(109) = &HBD0B&
        CRCTab(110) = &H8D68&
        CRCTab(111) = &H9D49&
        CRCTab(112) = &H7E97&
        CRCTab(113) = &H6EB6&
        CRCTab(114) = &H5ED5&
        CRCTab(115) = &H4EF4&
        CRCTab(116) = &H3E13&
        CRCTab(117) = &H2E32&
        CRCTab(118) = &H1E51&
        CRCTab(119) = &HE70&
        CRCTab(120) = &HFF9F&
        CRCTab(121) = &HEFBE&
        CRCTab(122) = &HDFDD&
        CRCTab(123) = &HCFFC&
        CRCTab(124) = &HBF1B&
        CRCTab(125) = &HAF3A&
        CRCTab(126) = &H9F59&
        CRCTab(127) = &H8F78&
        CRCTab(128) = &H9188&
        CRCTab(129) = &H81A9&
        CRCTab(130) = &HB1CA&
        CRCTab(131) = &HA1EB&
        CRCTab(132) = &HD10C&
        CRCTab(133) = &HC12D&
        CRCTab(134) = &HF14E&
        CRCTab(135) = &HE16F&
        CRCTab(136) = &H1080&
        CRCTab(137) = &HA1&
        CRCTab(138) = &H30C2&
        CRCTab(139) = &H20E3&
        CRCTab(140) = &H5004&
        CRCTab(141) = &H4025&
        CRCTab(142) = &H7046&
        CRCTab(143) = &H6067&
        CRCTab(144) = &H83B9&
        CRCTab(145) = &H9398&
        CRCTab(146) = &HA3FB&
        CRCTab(147) = &HB3DA&
        CRCTab(148) = &HC33D&
        CRCTab(149) = &HD31C&
        CRCTab(150) = &HE37F&
        CRCTab(151) = &HF35E&
        CRCTab(152) = &H2B1&
        CRCTab(153) = &H1290&
        CRCTab(154) = &H22F3&
        CRCTab(155) = &H32D2&
        CRCTab(156) = &H4235&
        CRCTab(157) = &H5214&
        CRCTab(158) = &H6277&
        CRCTab(159) = &H7256&
        CRCTab(160) = &HB5EA&
        CRCTab(161) = &HA5CB&
        CRCTab(162) = &H95A8&
        CRCTab(163) = &H8589&
        CRCTab(164) = &HF56E&
        CRCTab(165) = &HE54F&
        CRCTab(166) = &HD52C&
        CRCTab(167) = &HC50D&
        CRCTab(168) = &H34E2&
        CRCTab(169) = &H24C3&
        CRCTab(170) = &H14A0&
        CRCTab(171) = &H481&
        CRCTab(172) = &H7466&
        CRCTab(173) = &H6447&
        CRCTab(174) = &H5424&
        CRCTab(175) = &H4405&
        CRCTab(176) = &HA7DB&
        CRCTab(177) = &HB7FA&
        CRCTab(178) = &H8799&
        CRCTab(179) = &H97B8&
        CRCTab(180) = &HE75F&
        CRCTab(181) = &HF77E&
        CRCTab(182) = &HC71D&
        CRCTab(183) = &HD73C&
        CRCTab(184) = &H26D3&
        CRCTab(185) = &H36F2&
        CRCTab(186) = &H691&
        CRCTab(187) = &H16B0&
        CRCTab(188) = &H6657&
        CRCTab(189) = &H7676&
        CRCTab(190) = &H4615&
        CRCTab(191) = &H5634&
        CRCTab(192) = &HD94C&
        CRCTab(193) = &HC96D&
        CRCTab(194) = &HF90E&
        CRCTab(195) = &HE92F&
        CRCTab(196) = &H99C8&
        CRCTab(197) = &H89E9&
        CRCTab(198) = &HB98A&
        CRCTab(199) = &HA9AB&
        CRCTab(200) = &H5844&
        CRCTab(201) = &H4865&
        CRCTab(202) = &H7806&
        CRCTab(203) = &H6827&
        CRCTab(204) = &H18C0&
        CRCTab(205) = &H8E1&
        CRCTab(206) = &H3882&
        CRCTab(207) = &H28A3&
        CRCTab(208) = &HCB7D&
        CRCTab(209) = &HDB5C&
        CRCTab(210) = &HEB3F&
        CRCTab(211) = &HFB1E&
        CRCTab(212) = &H8BF9&
        CRCTab(213) = &H9BD8&
        CRCTab(214) = &HABBB&
        CRCTab(215) = &HBB9A&
        CRCTab(216) = &H4A75&
        CRCTab(217) = &H5A54&
        CRCTab(218) = &H6A37&
        CRCTab(219) = &H7A16&
        CRCTab(220) = &HAF1&
        CRCTab(221) = &H1AD0&
        CRCTab(222) = &H2AB3&
        CRCTab(223) = &H3A92&
        CRCTab(224) = &HFD2E&
        CRCTab(225) = &HED0F&
        CRCTab(226) = &HDD6C&
        CRCTab(227) = &HCD4D&
        CRCTab(228) = &HBDAA&
        CRCTab(229) = &HAD8B&
        CRCTab(230) = &H9DE8&
        CRCTab(231) = &H8DC9&
        CRCTab(232) = &H7C26&
        CRCTab(233) = &H6C07&
        CRCTab(234) = &H5C64&
        CRCTab(235) = &H4C45&
        CRCTab(236) = &H3CA2&
        CRCTab(237) = &H2C83&
        CRCTab(238) = &H1CE0&
        CRCTab(239) = &HCC1&
        CRCTab(240) = &HEF1F&
        CRCTab(241) = &HFF3E&
        CRCTab(242) = &HCF5D&
        CRCTab(243) = &HDF7C&
        CRCTab(244) = &HAF9B&
        CRCTab(245) = &HBFBA&
        CRCTab(246) = &H8FD9&
        CRCTab(247) = &H9FF8&
        CRCTab(248) = &H6E17&
        CRCTab(249) = &H7E36&
        CRCTab(250) = &H4E55&
        CRCTab(251) = &H5E74&
        CRCTab(252) = &H2E93&
        CRCTab(253) = &H3EB2&
        CRCTab(254) = &HED1&
        CRCTab(255) = &H1EF0&

    End Sub
End Module
