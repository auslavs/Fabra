namespace Fabra

open System.Text

/// <summary>
/// The base Label type for the ZPL label.
/// This is a list of the ZPL commands which will make up the label.
/// </summary>
type Label =
  | Label of LabelElement list
  
  /// <summary>
  /// Field Data (^FD)
  /// The ^FD command defines the data string for a field. The field data can be any printable character except those used as command prefixes (^ and ~).
  /// </summary>
  /// <param name="a">Data to be printed</param>
  /// <returns>LabelElement.FieldData</returns>
  static member inline FD a = 
    FieldData.FieldData a
    |> LabelElement.FieldData

  /// <summary>
  /// Scalable/Bitmapped Font (^A)
  /// 
  /// The ^A command specifies the font to use in a text field. ^A designates the font for the current ^FD statement or field.
  /// The font specified by ^A is used only once for that ^FD entry.
  /// If a value for ^A is not specified again, the default ^CF font is used for the next ^FD entry.
  /// </summary>
  /// <param name="o">Orientation</param>
  /// <param name="h">Character Height (in dots)</param>
  /// <param name="w">Width (in dots)</param>
  /// <param name="fd">Field Data</param>
  /// <returns>LabelElement.Text</returns>
  static member inline Text o h w (fd: string) =
    { Orientation = o
      Height = h
      Width = w
      Data = (FieldData.FieldData fd) }
    |> LabelElement.Text

    
  /// <summary>
  /// Code 128 Bar Code, Subsets A, B, and C (^BC)
  ///
  /// The ^BC command creates the Code 128 bar code, a high-density, variable length, continuous,alphanumeric symbology. 
  /// It was designed for complexly encoded product identification.
  /// 
  /// Code 128 has three subsets of characters. 
  /// There are 106 encoded printing characters in each set, and each character can have up to three different meanings, depending on the character subset being used.
  /// 
  /// Each Code 128 character consists of six elements: three bars and three spaces.
  /// • ^BC supports a fixed print ratio.
  /// • Field data (^FD) is limited to the width (or length, if rotated) of the label.
  /// </summary>
  /// <param name="o">Orientation</param>
  /// <param name="h">Height</param>
  /// <param name="f">Print interpretation line</param>
  /// <param name="g">print interpretation line above code</param>
  /// <param name="e">UCC check digit</param>
  /// <param name="m">Mode</param>
  /// <param name="fd">Field Data</param>
  /// <returns>LabelElement.Barcode</returns>
  static member inline BC o h f g e m (fd: string) =
    { Orientation = o
      Height = h
      PrintInterpretationLine = f
      PrintInterpretationLineAboveCode = g
      UCC_CheckDigit = e
      Mode = m
      Data = (FieldData.FieldData fd) }
    |> LabelElement.Barcode

  /// <summary>
  /// Data Matrix Bar Code (^BX)
  /// 
  /// The ^BX command creates a two-dimensional matrix symbology made up of square modules arranged within a perimeter finder pattern.
  /// </summary>
  /// <param name="o">Orientation</param>
  /// <param name="h">Dimensional height of individual symbol elements</param>
  /// <param name="s">Quality level</param>
  /// <param name="c">Columns to encode</param>
  /// <param name="r">Rows to encode</param>
  /// <param name="f">Format ID (0 to 6) — not used with quality set at 200</param>
  /// <param name="g">Escape sequence control character</param>
  /// <param name="a">Aspect ratio</param>
  /// <param name="fd">Field Data</param>
  /// <returns></returns>
  static member inline BX o h s c r f g a (fd: string) =
    { Orientation = o
      DimensionalHeight = h
      QualityLevel = s
      ColumnsToEncode = c
      RowsToEncode = r
      FormatId = f
      EscapeSequenceControlCharacter = g
      AspectRatio = a
      Data = (FieldData.FieldData fd) }
    |> LabelElement.DataMatrixBarcode

  /// <summary>
  /// Field Origin (^FO)
  ///
  /// The ^FO command sets a field origin, relative to the label home (^LH) position.
  /// ^FO sets the upper-left corner of the field area by defining points along the x-axis and y-axis independent of the rotation.
  /// </summary>
  /// <param name="x">X-axis location (in dots). Values: 0 to 32000. Default: 0</param>
  /// <param name="y">Y-axis location (in dots). Values: 0 to 32000. Default: 0</param>
  /// <param name="z">Justification. Values: 0 = left justification, 1 = right justification, 2 = auto justification (script dependent). Default: last accepted ^FW value or ^FW default</param>
  /// <returns>LabelElement.FieldOrigin</returns>
  static member inline FO x y z =
    { X_Axis = x; Y_Axis = y; Z = z }
    |> LabelElement.FieldOrigin

  /// <summary>
  /// Graphic Box (^GB)
  ///
  /// The ^GB command is used to draw boxes and lines as part of a label format.
  /// Boxes and lines are used to highlight important information, divide labels into distinct areas, or to improve the appearance of a label.
  /// The same format command is used for drawing either boxes or lines.
  /// </summary>
  /// <param name="w">Box width (in dots). Values: value of t to 32000. Default: value used for thickness (t) or 1</param>
  /// <param name="h">Box height (in dots). Values: value of t to 32000. Default: value used for thickness (t) or 1</param>
  /// <param name="t">Border thickness (in dots). Values: 1 to 32000. Default: 1</param>
  /// <param name="c">Line color. Values: B = black, W = white. Default: B</param>
  /// <param name="r">Degree of corner rounding. Values: 0 (no rounding) to 8 (heaviest rounding). Default: 0</param>
  /// <returns>LabelElement.GraphicBox</returns>
  static member inline GB w h t c r =
    { GraphicBox.Width = w
      Height = h
      Thickness = t
      LineColour = c
      Rounding = r }
    |> LabelElement.GraphicBox

  /// Bar Code Field Default (^BY)
  ///
  /// The ^BY command is used to change the default values for the module width (in dots), the wide bar to narrow bar width ratio and the bar code height (in dots).
  /// It can be used as often as necessary within a label format.
  static member inline BY w r h =
    { Width = w; Ratio = r; Height = h }
    |> LabelElement.BarcodeFieldDefault

  static member inline Collection lst = LabelElement.Collection lst

  /// <summary>
  /// Gernerates the label in ZPL format
  /// </summary>
  /// <returns>ZPL string</returns>
  override x.ToString() =
      let rec loop (input: LabelElement list) (sb: StringBuilder) =
          match input with
          | [] -> sb
          | head :: tail ->
              match head with
              | FieldData fd ->
                  sb.AppendLine(fd.ToString()) |> ignore
                  loop tail sb
              | Text txt ->
                  sb.AppendLine(txt.ToString()) |> ignore
                  loop tail sb
              | Barcode bc ->
                  sb.AppendLine(bc.ToString()) |> ignore
                  loop tail sb
              | DataMatrixBarcode bc ->
                  sb.AppendLine(bc.ToString()) |> ignore
                  loop tail sb
              | FieldOrigin fo ->
                  sb.AppendLine(fo.ToString()) |> ignore
                  loop tail sb
              | GraphicBox gb ->
                  sb.AppendLine(gb.ToString()) |> ignore
                  loop tail sb
              | BarcodeFieldDefault bfd ->
                  sb.AppendLine(bfd.ToString()) |> ignore
                  loop tail sb
              | Collection co ->
                  let newInput = tail |> List.append co
                  loop newInput sb

      let (Label lst) = x
      let sb = new StringBuilder()
      sb.AppendLine("^XA") |> ignore
      loop lst sb |> ignore
      sb.AppendLine("^XZ") |> ignore
      sb.ToString()
