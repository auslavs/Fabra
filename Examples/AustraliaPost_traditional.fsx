/// Example Australia Post Label (Traditional)
/// 
/// NOTE: This label has been designed for use with a 300 dpi printer

/// Use nuget package, or local version
//#r "nuget: Fabra"
#r @"..\bin\Debug\net5.0\Fabra.dll"

/// Increase the number of characters printed to the console
fsi.PrintWidth <- 3000;;

open Fabra

/// Helper functions

let text x y h w content =
  Label.Collection [
    Label.FO x y Left
    Label.Text Orientation.N h w content
  ]

let line x y w =
  Label.Collection [
    Label.FO x y Left
    Label.GB w 4 4 LineColour.B 0
  ]

let verticleLine x y h =
  Label.Collection [
    Label.FO x y Left
    Label.GB 4 h 4 LineColour.B 0
  ]

let barcode content =
  Label.Collection [
    Label.BY 3 2.0 10
    Label.FO 93 939 Left
    Label.BC Orientation.N 261 N N N Mode.A content
  ]

let dataMatrix x y content =
  Label.Collection [
    Label.FO x y Left
    Label.BX Orientation.N 6 QL200 None None None (Some "_^FH") None content
  ]


/// Label template

let template =
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

/// Label fields

let Phone value = text 609 186 48 36 value
let AddrLine1 value = text 60 237 48 36 value
let AddrLine2 value = text 60 294 48 36 value
let AddrLine3 value = text 60 354 48 36 value
let AddrLine4 value = text 60 414 48 36 value
let Suburb value = text 60 468 48 36 value
let State value = text 684 468 48 36 value
let Postcode value = text 786 468 48 36 value
let DiLine1 value = text 60 603 48 36 value
let DiLine2 value = text 60 660 48 36 value
let DiLine3 value = text 60 720 48 36 value
let Weight value = text 981 543 48 48 "12.3 kg"
let ConsignmentNo value = text 939 783 36 36 value
let ConsignmentQty value = text 939 828 36 36 value
let ArticleId value =
  Label.Collection [
    text 204 888 48 48 $"AP Article ID: {value}"
    text 204 1218 48 48 $"AP Article ID: {value}"
  ]
let SenderAddrLine1 value = text 60 1500 42 30 value
let SenderAddrLine2 value = text 60 1548 42 30 value
let SenderAddrLine3 value = text 60 1593 42 30 value
let SenderAddrLine4 value = text 60 1641 42 30 value
let OrderId value = text 741 1494 48 60 value


/// Label content

Label [

  Phone "123456789"

  /// Address
  AddrLine1 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  AddrLine2 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  AddrLine3 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  AddrLine4 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  Suburb "ABCDEFGHIJKLMNOPQRSTUVWXYZABCD"
  State "ABC"
  Postcode "1234"
  
  /// Data Matrix Barcode
  dataMatrix 966 213 "_5F101112345671234519112345123456712123451211_5F14201234_5F19212345678_5F18008123456789012"

  /// Delivery Instructions
  DiLine1 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
  DiLine2 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
  DiLine3 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
  Weight "12.3 kg"

  /// Consignment Information
  ConsignmentNo "ABC1234567"
  ConsignmentQty "1 OF 1"

  // Article Information and Barcode
  ArticleId "ABCDE1234567121234512119"
  barcode ">;011123456712345191>6ABCDE1>5234567121234512119"

  // Sender Information
  SenderAddrLine1 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  SenderAddrLine2 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  SenderAddrLine3 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  SenderAddrLine4 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCD   ABC   1234"

  // Order Id
  OrderId "12345678"

  template
  ]
|> string /// output the ZPL string to the console
