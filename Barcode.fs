namespace Fabra

[<AutoOpen>]
module Types = 

  type Orientation = 
      /// normal
      | N
      /// rotated 90 degrees (clockwise)
      | R
      /// inverted 180 degrees
      | I
      /// read from bottom up, 270 degrees
      | B

    type Mode = 
      /// No selected mode
      | N
      /// UCC Case Mode
      | U
      /// Automatic Mode
      | A
      /// UCC/EAN Mode
      | D

    type YesNo = Y | N
    
    type Barcode = {
        Orientation : Orientation
        Height : int
        PrintInterpretationLine : YesNo
        PrintInterpretationLineAboveCode : YesNo
        UCC_CheckDigit : YesNo
        Mode : Mode
    }

    