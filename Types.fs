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

/// Diagonal orientation for the Graphic Diagonal Line (^GD) command.
[<RequireQualifiedAccess>]
type Diagonal =
  /// Right-leaning diagonal, '\' (default).
  | R
  /// Left-leaning diagonal, '/'.
  | L
  override x.ToString() =
    match x with
    | Diagonal.R -> "R"
    | Diagonal.L -> "L"

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

/// Graphic Field (^GF)
/// Phase 1: pre-encoded ASCII-hex (^GFA) bitmap data supplied by the
/// caller. The data string is emitted verbatim.
type GraphicField =
    { BinaryByteCount: int
      GraphicFieldCount: int
      BytesPerRow: int
      Data: string }
    override x.ToString() =
        $"^GFA,{x.BinaryByteCount},{x.GraphicFieldCount},{x.BytesPerRow},{x.Data}^FS"

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

/// Change Alphanumeric Default Font (^CF)
type ChangeFont =
    { Font: Font
      Height: int
      Width: int }
    override x.ToString() = $"^CF{x.Font},{x.Height},{x.Width}"

/// Field Hexadecimal Indicator (^FH)
type FieldHexadecimal =
    | FieldHexadecimal of char
    override x.ToString() =
        let (FieldHexadecimal c) = x
        $"^FH{c}"

/// Change International Font/Encoding (^CI)
type ChangeInternational =
    | ChangeInternational of int
    override x.ToString() =
        let (ChangeInternational n) = x
        $"^CI{n}"

/// Label Length (^LL)
type LabelLength =
    | LabelLength of int
    override x.ToString() =
        let (LabelLength n) = x
        $"^LL{n}"

/// Print Width (^PW)
type PrintWidth =
    | PrintWidth of int
    override x.ToString() =
        let (PrintWidth n) = x
        $"^PW{n}"

/// Media Darkness (^MD)
type MediaDarkness =
    | MediaDarkness of int
    override x.ToString() =
        let (MediaDarkness n) = x
        $"^MD{n}"

/// Print Quantity (^PQ)
type PrintQuantity =
    { Quantity: int
      Pause: int
      Replicates: int
      OverridePause: YesNo
      CutOnError: YesNo }
    override x.ToString() =
        $"^PQ{x.Quantity},{x.Pause},{x.Replicates},{x.OverridePause},{x.CutOnError}"

/// Graphic Circle (^GC)
type GraphicCircle =
    { Diameter: int
      Thickness: int
      LineColour: LineColour }
    override x.ToString() =
        $"^GC{x.Diameter},{x.Thickness},{x.LineColour}^FS"

/// Graphic Diagonal Line (^GD)
type GraphicDiagonal =
    { Width: int
      Height: int
      Thickness: int
      LineColour: LineColour
      Direction: Diagonal }
    override x.ToString() =
        $"^GD{x.Width},{x.Height},{x.Thickness},{x.LineColour},{x.Direction}^FS"

/// Graphic Ellipse (^GE)
type GraphicEllipse =
    { Width: int
      Height: int
      Thickness: int
      LineColour: LineColour }
    override x.ToString() =
        $"^GE{x.Width},{x.Height},{x.Thickness},{x.LineColour}^FS"

/// Code 39 Bar Code (^B3)
type Code39 =
    { Orientation: Orientation
      CheckDigit: YesNo
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^B3{x.Orientation},{x.CheckDigit},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode}{x.Data}"

/// Interleaved 2 of 5 Bar Code (^B2)
type Interleaved2of5 =
    { Orientation: Orientation
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      CheckDigit: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^B2{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode},{x.CheckDigit}{x.Data}"

/// EAN-13 Bar Code (^BE)
type Ean13 =
    { Orientation: Orientation
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^BE{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode}{x.Data}"

/// UPC-A Bar Code (^BU)
type UpcA =
    { Orientation: Orientation
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      PrintCheckDigit: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^BU{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode},{x.PrintCheckDigit}{x.Data}"

/// EAN-8 Bar Code (^B8)
type Ean8 =
    { Orientation: Orientation
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^B8{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode}{x.Data}"

/// UPC-E Bar Code (^B9)
type UpcE =
    { Orientation: Orientation
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      PrintCheckDigit: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^B9{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode},{x.PrintCheckDigit}{x.Data}"

/// Code 93 Bar Code (^BA)
type Code93 =
    { Orientation: Orientation
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      PrintCheckDigit: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^BA{x.Orientation},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode},{x.PrintCheckDigit}{x.Data}"

/// Code 11 Bar Code (^B1)
type Code11 =
    { Orientation: Orientation
      CheckDigit: YesNo
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^B1{x.Orientation},{x.CheckDigit},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode}{x.Data}"

/// PDF417 Bar Code (^B7)
type Pdf417 =
    { Orientation: Orientation
      Height: int
      SecurityLevel: int
      Columns: int
      Rows: int
      Truncate: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^B7{x.Orientation},{x.Height},{x.SecurityLevel},{x.Columns},{x.Rows},{x.Truncate}{x.Data}"

/// Field Typeset (^FT)
/// Like ^FO but positions the field relative to a typeset baseline.
type FieldTypeset =
    { X_Axis: int
      Y_Axis: int
      Z: Justification }
    override x.ToString() = $"^FT{x.X_Axis},{x.Y_Axis},{x.Z}"

/// Field Variable (^FV)
type FieldVariable =
    | FieldVariable of string
    override x.ToString() =
        let (FieldVariable str) = x
        $"^FV{str}^FS"

/// Field Orientation default (^FW)
type FieldOrientation =
    | FieldOrientation of Orientation
    override x.ToString() =
        let (FieldOrientation o) = x
        $"^FW{o}"

/// Print Orientation (^PO)
[<RequireQualifiedAccess>]
type PrintOrientation =
    /// Normal
    | Normal
    /// Invert 180 degrees
    | Invert
    override x.ToString() =
        match x with
        | PrintOrientation.Normal -> "^PON"
        | PrintOrientation.Invert -> "^POI"

/// Label Shift (^LS)
type LabelShift =
    | LabelShift of int
    override x.ToString() =
        let (LabelShift n) = x
        $"^LS{n}"

/// Label Top (^LT)
type LabelTop =
    | LabelTop of int
    override x.ToString() =
        let (LabelTop n) = x
        $"^LT{n}"

/// Media Type (^MT)
[<RequireQualifiedAccess>]
type MediaType =
    /// Thermal transfer
    | ThermalTransfer
    /// Direct thermal
    | DirectThermal
    override x.ToString() =
        match x with
        | MediaType.ThermalTransfer -> "^MTT"
        | MediaType.DirectThermal -> "^MTD"

/// Field direction for the Field Parameter (^FP) command.
[<RequireQualifiedAccess>]
type FieldDirection =
    /// Horizontal
    | H
    /// Vertical
    | V
    /// Reverse
    | R
    override x.ToString() =
        match x with
        | FieldDirection.H -> "H"
        | FieldDirection.V -> "V"
        | FieldDirection.R -> "R"

/// Plessey Bar Code (^BP)
type Plessey =
    { Orientation: Orientation
      PrintCheckDigit: YesNo
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      Data: FieldData }
    override x.ToString() =
        $"^BP{x.Orientation},{x.PrintCheckDigit},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode}{x.Data}"

/// ANSI Codabar Bar Code (^BK)
type Codabar =
    { Orientation: Orientation
      CheckDigit: YesNo
      Height: int
      PrintInterpretationLine: YesNo
      PrintInterpretationLineAboveCode: YesNo
      StartCharacter: char
      StopCharacter: char
      Data: FieldData }
    override x.ToString() =
        $"^BK{x.Orientation},{x.CheckDigit},{x.Height},{x.PrintInterpretationLine},{x.PrintInterpretationLineAboveCode},{x.StartCharacter},{x.StopCharacter}{x.Data}"

/// Aztec Bar Code (^BO)
type Aztec =
    { Orientation: Orientation
      Magnification: int
      ExtendedChannel: YesNo
      ErrorControl: int
      MenuSymbol: YesNo
      SymbolCount: int
      Data: FieldData }
    override x.ToString() =
        $"^BO{x.Orientation},{x.Magnification},{x.ExtendedChannel},{x.ErrorControl},{x.MenuSymbol},{x.SymbolCount}{x.Data}"

/// Graphic Symbol (^GS)
type GraphicSymbol =
    { Orientation: Orientation
      Height: int
      Width: int
      Data: FieldData }
    override x.ToString() =
        $"^GS{x.Orientation},{x.Height},{x.Width}{x.Data}"

/// Field Number (^FN)
type FieldNumber =
    | FieldNumber of int
    override x.ToString() =
        let (FieldNumber n) = x
        $"^FN{n}"

/// Field Parameter (^FP)
type FieldParameter =
    { Direction: FieldDirection
      Gap: int }
    override x.ToString() = $"^FP{x.Direction},{x.Gap}"

/// Print Mirror Image (^PM)
type PrintMirror =
    | PrintMirror of YesNo
    override x.ToString() =
        let (PrintMirror y) = x
        $"^PM{y}"

/// Slew given number of dots (^PF)
type Slew =
    | Slew of int
    override x.ToString() =
        let (Slew n) = x
        $"^PF{n}"

/// Label Reverse Print (^LR)
type LabelReverse =
    | LabelReverse of YesNo
    override x.ToString() =
        let (LabelReverse y) = x
        $"^LR{y}"

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
    | GraphicField of GraphicField
    | BarcodeFieldDefault of BarcodeFieldDefault
    | Comment of Comment
    | LabelHome of LabelHome
    | ChangeFont of ChangeFont
    | FieldReverse
    | FieldHexadecimal of FieldHexadecimal
    | ChangeInternational of ChangeInternational
    | LabelLength of LabelLength
    | PrintWidth of PrintWidth
    | MediaDarkness of MediaDarkness
    | PrintQuantity of PrintQuantity
    | GraphicCircle of GraphicCircle
    | GraphicDiagonal of GraphicDiagonal
    | GraphicEllipse of GraphicEllipse
    | Code39 of Code39
    | Interleaved2of5 of Interleaved2of5
    | Ean13 of Ean13
    | UpcA of UpcA
    | Ean8 of Ean8
    | UpcE of UpcE
    | Code93 of Code93
    | Code11 of Code11
    | Pdf417 of Pdf417
    | FieldTypeset of FieldTypeset
    | FieldVariable of FieldVariable
    | FieldOrientation of FieldOrientation
    | PrintOrientation of PrintOrientation
    | LabelShift of LabelShift
    | LabelTop of LabelTop
    | MediaType of MediaType
    | Plessey of Plessey
    | Codabar of Codabar
    | Aztec of Aztec
    | GraphicSymbol of GraphicSymbol
    | FieldNumber of FieldNumber
    | FieldParameter of FieldParameter
    | PrintMirror of PrintMirror
    | Slew of Slew
    | LabelReverse of LabelReverse
    | Collection of LabelElement list