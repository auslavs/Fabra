namespace Fabra

/// Barcode orientation
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
    | N -> "N"
    | R -> "R"
    | I -> "I"
    | B -> "B"

/// Barcode mode
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
        | N -> "N"
        | U -> "U"
        | A -> "A"
        | D -> "D"

/// Generic Yes or No value for when a ZPL command requires a Y or N argument
type YesNo =
    /// Yes
    | Y
    /// No
    | N
    override x.ToString() =
        match x with
        | Y -> "Y"
        | N -> "N"

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

/// Line Colour
type LineColour =
  /// Black
  | B
  /// White
  | W
  override x.ToString() =
    match x with
    | B -> "B"
    | W -> "W"

/// Field Data (^FD)
type FieldData =
    | FieldData of string
    override x.ToString() =
        let (FieldData str) = x
        $"^FD{str}^FS"

/// Scalable/Bitmapped Font (^A)
type Text =
    { Orientation: Orientation
      Height: int
      Width: int
      Data: FieldData }
    override x.ToString() =
        $"^A0{x.Orientation},{x.Height},{x.Width}{x.Data}"

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
        $"^BC{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode},{x.UCC_CheckDigit}{x.Data}"

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
    override x.ToString() = $"^BY{x.Width},{x.Ratio},{x.Height}"

/// A label element/command.
/// Used for containing all label commands within a single collection/label.
type LabelElement =
    | FieldData of FieldData
    | Text of Text
    | Barcode of Barcode
    | DataMatrixBarcode of DataMatrixBarcode
    | FieldOrigin of FieldOrigin
    | GraphicBox of GraphicBox
    | BarcodeFieldDefault of BarcodeFieldDefault
    | Collection of LabelElement list