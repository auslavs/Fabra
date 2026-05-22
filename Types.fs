namespace Fabra

open System.Globalization

/// Barcode orientation
[<RequireQualifiedAccess>]
type Orientation =
  /// Normal
  | N
  /// Rotated 90 degrees (clockwise)
  | R
  /// Inverted 180 degrees
  | I
  /// Read from bottom up, 270 degrees
  | B
  override x.ToString() =
    match x with
    | Orientation.N -> "N"
    | Orientation.R -> "R"
    | Orientation.I -> "I"
    | Orientation.B -> "B"

/// Barcode mode
[<RequireQualifiedAccess>]
type Mode =
    /// No selected mode
    | N
    /// UCC Case Mode
    | U
    /// Automatic Mode
    | A
    /// UCC/EAN Mode
    | D
    override x.ToString() =
        match x with
        | Mode.N -> "N"
        | Mode.U -> "U"
        | Mode.A -> "A"
        | Mode.D -> "D"

/// Generic Yes or No value for when a ZPL command requires a Y or N argument
[<RequireQualifiedAccess>]
type YesNo =
    /// Yes
    | Y
    /// No
    | N
    override x.ToString() =
        match x with
        | YesNo.Y -> "Y"
        | YesNo.N -> "N"

/// Justification
type Justification =
  /// Left
  | Left
  /// Right
  | Right
  /// Justified
  | Justified
  override x.ToString() =
      match x with
      | Left -> "0"
      | Right -> "1"
      | Justified -> "2"

/// Text justification for the Field Block (^FB) command.
[<RequireQualifiedAccess>]
type FieldBlockJustification =
  /// Left
  | Left
  /// Centre
  | Centre
  /// Right
  | Right
  /// Justified
  | Justified
  override x.ToString() =
      match x with
      | FieldBlockJustification.Left -> "L"
      | FieldBlockJustification.Centre -> "C"
      | FieldBlockJustification.Right -> "R"
      | FieldBlockJustification.Justified -> "J"

/// Line Colour
[<RequireQualifiedAccess>]
type LineColour =
  /// Black
  | B
  /// White
  | W
  override x.ToString() =
    match x with
    | LineColour.B -> "B"
    | LineColour.W -> "W"

/// Field Data (^FD)
type FieldData =
    | FieldData of string
    override x.ToString() =
        let (FieldData str) = x
        $"^FD{str}^FS"

/// Resident or downloaded font for the ^A command.
/// Valid identifiers are A-Z and 0-9.
type Font =
    | Font of char
    override x.ToString() =
        let (Font c) = x
        string c

/// Scalable/Bitmapped Font (^A)
type Text =
    { Font: Font
      Orientation: Orientation
      Height: int
      Width: int
      Data: FieldData }
    override x.ToString() =
        $"^A{x.Font}{x.Orientation},{x.Height},{x.Width}{x.Data}"

/// Field Block (^FB)
/// A modifier emitted immediately before the ^FD it word-wraps.
type FieldBlock =
    { Width: int
      MaxLines: int
      LineSpacing: int
      Justification: FieldBlockJustification
      HangingIndent: int
      Data: FieldData }
    override x.ToString() =
        $"^FB{x.Width},{x.MaxLines},{x.LineSpacing},{x.Justification},{x.HangingIndent}{x.Data}"

/// Code 128 Bar Code, Subsets A, B, and C (^BC)
type Barcode =
    { Orientation: Orientation
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      UCC_CheckDigit: YesNo
      Mode: Mode
      Data: FieldData }
    override x.ToString() =
        $"^BC{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode},{x.UCC_CheckDigit},{x.Mode}{x.Data}"

/// Data Matrix Quality Level
type DataMatrixQualityLevel=
  | QL0
  | QL50
  | QL80
  | QL100
  | QL140
  | QL200
  override x.ToString() =
    match x with
    | QL0   -> "0"
    | QL50  -> "50"
    | QL80  -> "80"
    | QL100 -> "100"
    | QL140 -> "140"
    | QL200 -> "200"

/// Data Matrix Aspect Ratio
type DataMatrixAspectRatio=
  | Square
  | Rectangular
  override x.ToString() =
    match x with
    | Square   -> "1"
    | Rectangular  -> "2"

/// Data Matrix Bar Code (^BX)
type DataMatrixBarcode =
    { Orientation: Orientation
      DimensionalHeight: int
      QualityLevel: DataMatrixQualityLevel
      ColumnsToEncode: int option
      RowsToEncode: int option
      FormatId: int option
      EscapeSequenceControlCharacter: string option
      AspectRatio: DataMatrixAspectRatio option
      Data: FieldData }
    override x.ToString() =
        let inline (+.) s1 s2 = 
          match s2 with
          | Some x -> s1 + $",{x}"
          | None -> s1 + ","
        $"^BX{x.Orientation},{x.DimensionalHeight},{x.QualityLevel}" +. x.ColumnsToEncode  +. x.RowsToEncode +. x.FormatId +. x.EscapeSequenceControlCharacter +. x.AspectRatio + $"{x.Data}"

/// QR Code error correction level for the ^BQ command.
[<RequireQualifiedAccess>]
type QrErrorCorrection =
  /// Ultra-high reliability (~30% recovery)
  | H
  /// High reliability (~25% recovery)
  | Q
  /// Standard (~15% recovery)
  | M
  /// High density (~7% recovery)
  | L
  override x.ToString() =
    match x with
    | QrErrorCorrection.H -> "H"
    | QrErrorCorrection.Q -> "Q"
    | QrErrorCorrection.M -> "M"
    | QrErrorCorrection.L -> "L"

/// QR Code Bar Code (^BQ)
/// The error-correction level is repeated in the ^FD prefix and Fabra
/// always uses automatic input mode (A), so the field data is emitted as
/// ^FD{errorCorrection}A,{data}^FS.
type QrCode =
    { Orientation: Orientation
      Model: int
      Magnification: int
      ErrorCorrection: QrErrorCorrection
      Mask: int
      Data: string }
    override x.ToString() =
        $"^BQ{x.Orientation},{x.Model},{x.Magnification},{x.ErrorCorrection},{x.Mask}^FD{x.ErrorCorrection}A,{x.Data}^FS"

/// Field Origin (^FO)
type FieldOrigin =
    { X_Axis: int
      Y_Axis: int
      Z: Justification }
    override x.ToString() = $"^FO{x.X_Axis},{x.Y_Axis},{x.Z}"

/// Graphic Box (^GB)
type GraphicBox =
    { Width: int
      Height: int
      Thickness: int
      LineColour: LineColour
      Rounding: int }
    override x.ToString() =
        $"^GB{x.Width},{x.Height},{x.Thickness},{x.LineColour},{x.Rounding}^FS"

/// Bar Code Field Default (^BY)
type BarcodeFieldDefault =
    {
      // Module width
      Width: int
      // Wide bar to narrow bar width ratio
      Ratio: float
      //Barcode height
      Height: int }
    override x.ToString() =
        // Ratio is rendered with the invariant culture so a comma decimal
        // separator can never be mistaken for a ZPL field separator.
        let ratio = x.Ratio.ToString(CultureInfo.InvariantCulture)
        $"^BY{x.Width},{ratio},{x.Height}"

/// Comment (^FX)
type Comment =
    | Comment of string
    override x.ToString() =
        let (Comment str) = x
        $"^FX{str}^FS"

/// Label Home (^LH)
type LabelHome =
    { X_Axis: int
      Y_Axis: int }
    override x.ToString() = $"^LH{x.X_Axis},{x.Y_Axis}"

/// A label element/command.
/// Used for containing all label commands within a single collection/label.
type LabelElement =
    | FieldData of FieldData
    | Text of Text
    | FieldBlock of FieldBlock
    | Barcode of Barcode
    | DataMatrixBarcode of DataMatrixBarcode
    | QrCode of QrCode
    | FieldOrigin of FieldOrigin
    | GraphicBox of GraphicBox
    | BarcodeFieldDefault of BarcodeFieldDefault
    | Comment of Comment
    | LabelHome of LabelHome
    | Collection of LabelElement list