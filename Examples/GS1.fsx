/// Example GS1 Logisitics Label
/// More details on the label requirements can be founds here:
/// https://www.gs1.org/standards/gs1-logistic-label-guideline/1-3
/// 
/// NOTE: This label has been designed for use with a 300 dpi printer

#r @"..\bin\Debug\net5.0\Fabra.dll"

/// Increase the number of characters printed to the console
fsi.PrintWidth <- 2000;;

open Fabra

/// Helper functions

let barcode_GS1Multi x y content =
  Label.Collection [
    Label.BY 3 2.0 10
    Label.FO x y Left
    Label.BC Orientation.N 378 N N N Mode.A content
  ]

let barcode_SSCC x y content =
  Label.Collection [
    Label.BY 6 2.0 10
    Label.FO x y Left
    Label.BC Orientation.N 354 N N N Mode.A content
  ]

let text x y h w content =
  Label.Collection [
    Label.FO x y Left
    Label.Text Orientation.N h w content
  ]

let line x y w =
  Label.Collection [
    Label.FO x y Left
    Label.GB w 1 1 LineColour.B 0
  ]
  
/// Label content

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
|> string /// output the ZPL string to the console
