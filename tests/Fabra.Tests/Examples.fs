namespace Fabra.Tests

open Fabra

/// Label builders ported verbatim from the .fsx scripts in /Examples.
/// Keeping them here lets the golden tests exercise the exact same DSL
/// usage the example scripts document.
module Examples =

    /// Example GS1 Logistics Label (see Examples/GS1.fsx).
    module GS1 =

        let private barcode_GS1Multi x y content =
            Label.Collection [
                Label.BY 3 2.0 10
                Label.FO x y Left
                Label.BC Orientation.N 378 YesNo.N YesNo.N YesNo.N Mode.A content
            ]

        let private barcode_SSCC x y content =
            Label.Collection [
                Label.BY 6 2.0 10
                Label.FO x y Left
                Label.BC Orientation.N 354 YesNo.N YesNo.N YesNo.N Mode.A content
            ]

        let private text x y h w content =
            Label.Collection [
                Label.FO x y Left
                Label.Text Orientation.N h w content
            ]

        let private line x y w =
            Label.Collection [
                Label.FO x y Left
                Label.GB w 1 1 LineColour.B 0
            ]

        let label =
            Label [
                text 60 60 84 84 "Example GS1 Logistics Label"

                line 60 141 1060
                text 60 153 72 60 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEF"
                text 60 213 72 60 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEF"
                line 60 282 1060

                text 60 306 54 54 "SSCC"
                text 60 354 117 66 "123456789012345678"

                text 768 306 54 54 "Material"
                text 768 354 117 66 "12345678"

                text 60 471 54 54 "Content"
                text 60 519 117 66 "12345678901234"

                text 768 471 54 54 "Quantity"
                text 768 519 117 66 "1234 CS"

                text 60 639 54 54 "Best Before (dd.mm.yy)"
                text 60 684 117 66 "12.34.56"

                text 768 639 54 54 "Batch"
                text 768 684 117 66 "123456789"

                barcode_GS1Multi 141 792 ">;>8021234567890123415563412371234>81012345678>69"
                text 177 1182 72 36 "(02)12345678901234(15)563412(37)1234(10)123456789"

                barcode_SSCC 123 1263 ">;>800123456789012345678"
                text 219 1629 72 72 "(00)123456789012345678"
            ]

    /// Example Australia Post Label - Traditional (see Examples/AustraliaPost_traditional.fsx).
    module AustraliaPost =

        let private text x y h w content =
            Label.Collection [
                Label.FO x y Left
                Label.Text Orientation.N h w content
            ]

        let private line x y w =
            Label.Collection [
                Label.FO x y Left
                Label.GB w 4 4 LineColour.B 0
            ]

        let private verticleLine x y h =
            Label.Collection [
                Label.FO x y Left
                Label.GB 4 h 4 LineColour.B 0
            ]

        let private barcode content =
            Label.Collection [
                Label.BY 3 2.0 10
                Label.FO 93 939 Left
                Label.BC Orientation.N 261 YesNo.N YesNo.N YesNo.N Mode.A content
            ]

        let private dataMatrix x y content =
            Label.Collection [
                Label.FO x y Left
                Label.BX Orientation.N 6 QL200 None None None (Some "_^FH") None content
            ]

        let private template =
            Label.Collection [
                text 60 186 48 60 "Deliver To:"
                text 453 186 48 48 "Phone"
                line 60 531 1060

                text 60 543 48 60 "Delivery Instructions:"

                line 60 771 1060
                text 60 783 36 48 "SIGNATURE ON DELIVERY REQUIRED"

                verticleLine 789 771 103
                text 801 783 36 36 "CON NO"
                text 801 831 36 36 "PARCEL"
                line 60 876 1060

                line 60 1269 1060
                text 60 1287 42 30 "Aviation Security and Dangerous Goods Declaration"
                text 60 1332 36 24 "The sender acknowledges that this article may be carried by air and will be subject to aviation security and"
                text 60 1365 36 24 "clearing procedures. The sender declares that the article does not contain any dangerous or prohibited goods,"
                text 60 1401 36 24 "explosive or incendiary devices. A false declaration is a criminal offence."
                line 60 1440 1060

                text 60 1452 48 60 "Sender:"

                verticleLine 732 1440 259

                text 741 1452 48 60 "Order ID:"
            ]

        let private Phone value = text 609 186 48 36 value
        let private AddrLine1 value = text 60 237 48 36 value
        let private AddrLine2 value = text 60 294 48 36 value
        let private AddrLine3 value = text 60 354 48 36 value
        let private AddrLine4 value = text 60 414 48 36 value
        let private Suburb value = text 60 468 48 36 value
        let private State value = text 684 468 48 36 value
        let private Postcode value = text 786 468 48 36 value
        let private DiLine1 value = text 60 603 48 36 value
        let private DiLine2 value = text 60 660 48 36 value
        let private DiLine3 value = text 60 720 48 36 value
        let private Weight value = text 981 543 48 48 "12.3 kg"
        let private ConsignmentNo value = text 939 783 36 36 value
        let private ConsignmentQty value = text 939 828 36 36 value
        let private ArticleId value =
            Label.Collection [
                text 204 888 48 48 $"AP Article ID: {value}"
                text 204 1218 48 48 $"AP Article ID: {value}"
            ]
        let private SenderAddrLine1 value = text 60 1500 42 30 value
        let private SenderAddrLine2 value = text 60 1548 42 30 value
        let private SenderAddrLine3 value = text 60 1593 42 30 value
        let private SenderAddrLine4 value = text 60 1641 42 30 value
        let private OrderId value = text 741 1494 48 60 value

        let label =
            Label [
                Phone "123456789"

                AddrLine1 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
                AddrLine2 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
                AddrLine3 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
                AddrLine4 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
                Suburb "ABCDEFGHIJKLMNOPQRSTUVWXYZABCD"
                State "ABC"
                Postcode "1234"

                dataMatrix 966 213 "_5F101112345671234519112345123456712123451211_5F14201234_5F19212345678_5F18008123456789012"

                DiLine1 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
                DiLine2 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
                DiLine3 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
                Weight "12.3 kg"

                ConsignmentNo "ABC1234567"
                ConsignmentQty "1 OF 1"

                ArticleId "ABCDE1234567121234512119"
                barcode ">;011123456712345191>6ABCDE1>5234567121234512119"

                SenderAddrLine1 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
                SenderAddrLine2 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
                SenderAddrLine3 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
                SenderAddrLine4 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCD   ABC   1234"

                OrderId "12345678"

                template
            ]
