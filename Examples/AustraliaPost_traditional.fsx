/// Example GS1 Logisitics Label
/// More details on the label requirements can be founds here:
/// https://www.gs1.org/standards/gs1-logistic-label-guideline/1-3
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

let barcode x y content =
  Label.Collection [
    Label.BY 3 2.0 10
    Label.FO x y Left
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
  ]

/// Label fields

/// Label content

Label [

  text 60 186 48 60 "Deliver To:"
  text 453 186 48 48 "Phone"
  text 609 186 48 36 "123456789"
  text 60 237 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  text 60 294 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  text 60 354 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  text 60 414 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  text 60 468 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCD"
  text 684 468 48 36 "ABC"
  text 786 468 48 36 "1234"
  line 60 531 1060

  //^FO966,213^BXN,6,200,,,,_^FH^FD_5F101112345671234519112345123456712123451211_5F14201234_5F19212345678_5F18008123456789012^FS
  
  dataMatrix 966 213 "_5F101112345671234519112345123456712123451211_5F14201234_5F19212345678_5F18008123456789012"

  text 60 543 48 60 "Delivery Instructions:"
  text 60 603 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
  text 60 660 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
  text 60 720 48 36 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX"
  text 981 543 48 48 "12.3 kg"

  //^FO60,771^GB1060,4,4,B,0^FS
  line 60 771 1060
  text 60 783 36 48 "SIGNATURE ON DELIVERY REQUIRED"
  //^FO789,771^GB4,103,4,B,0^FS
  verticleLine 789 771 103
  text 801 783 36 36 "CON NO"
  text 939 783 36 36 "ABC1234567"
  text 801 831 36 36 "PARCEL"
  text 939 828 36 36 "1 OF 1"
  line 60 876 1060

  text 204 888 48 48 "AP Article ID: ABCDE1234567121234512119"
  barcode 93 939 ">;011123456712345191>6ABCDE1>5234567121234512119"
  text 204 1218 48 48 "AP Article ID: ABCDE1234567121234512119"

  
  line 60 1269 1060
  text 60 1287 42 30 "Aviation Security and Dangerous Goods Declaration"
  text 60 1332 36 24 "The sender acknowledges that this article may be carried by air and will be subject to aviation security and"
  text 60 1365 36 24 "clearing procedures. The sender declares that the article does not contain any dangerous or prohibited goods,"
  text 60 1401 36 24 "explosive or incendiary devices. A false declaration is a criminal offence."
  line 60 1440 1060


  text 60 1452 48 60 "Sender:"
  text 60 1500 42 30 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  text 60 1548 42 30 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  text 60 1593 42 30 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMN"
  text 60 1641 42 30 "ABCDEFGHIJKLMNOPQRSTUVWXYZABCD   ABC   1234"

  verticleLine 732 1440 259

  text 741 1452 48 60 "Order ID:"
  text 741 1494 48 60 "12345678"


  ]
|> string /// output the ZPL string to the console
