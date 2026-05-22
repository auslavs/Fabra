namespace Fabra.Imaging

open System.IO
open System.Text
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open Fabra

/// Algorithm for reducing an image to ZPL's 1-bit (black/white) monochrome.
[<RequireQualifiedAccess>]
type Dithering =
    /// Per-pixel luminance threshold — no dithering. Ideal for logos and line art.
    | Threshold
    /// Floyd–Steinberg error-diffusion dithering. Better for photos and gradients.
    | FloydSteinberg

/// Converts images to ZPL Graphic Field (^GF) elements via SixLabors.ImageSharp.
module ImageField =

    /// Default luminance cutoff (0-255): pixels darker than this print as black dots.
    [<Literal>]
    let DefaultThreshold = 128

    /// Pack a width×height monochrome predicate into ^GFA bytes (8 pixels per
    /// byte, most-significant bit leftmost, each row padded to a whole byte).
    let private pack (width: int) (height: int) (isBlack: int -> int -> bool) : GraphicField =
        let bytesPerRow = (width + 7) / 8
        let sb = StringBuilder(bytesPerRow * height * 2)
        for y in 0 .. height - 1 do
            for byteIndex in 0 .. bytesPerRow - 1 do
                let mutable b = 0
                for bit in 0 .. 7 do
                    let x = byteIndex * 8 + bit
                    if x < width && isBlack x y then
                        b <- b ||| (0x80 >>> bit)
                sb.Append(b.ToString("X2")) |> ignore
        let total = bytesPerRow * height
        { GraphicField.BinaryByteCount = total
          GraphicFieldCount = total
          BytesPerRow = bytesPerRow
          Data = sb.ToString() }

    let private encode (dithering: Dithering) (threshold: int) (image: Image<L8>) : GraphicField =
        let width = image.Width
        let height = image.Height
        match dithering with
        | Dithering.Threshold ->
            pack width height (fun x y -> int image.[x, y].PackedValue < threshold)
        | Dithering.FloydSteinberg ->
            let buf = Array2D.init height width (fun y x -> float image.[x, y].PackedValue)
            let black = Array2D.zeroCreate<bool> height width
            for y in 0 .. height - 1 do
                for x in 0 .. width - 1 do
                    let oldValue = buf.[y, x]
                    let newValue = if oldValue < float threshold then 0.0 else 255.0
                    black.[y, x] <- newValue = 0.0
                    let err = oldValue - newValue
                    if x + 1 < width then
                        buf.[y, x + 1] <- buf.[y, x + 1] + err * 7.0 / 16.0
                    if y + 1 < height then
                        if x > 0 then buf.[y + 1, x - 1] <- buf.[y + 1, x - 1] + err * 3.0 / 16.0
                        buf.[y + 1, x] <- buf.[y + 1, x] + err * 5.0 / 16.0
                        if x + 1 < width then buf.[y + 1, x + 1] <- buf.[y + 1, x + 1] + err * 1.0 / 16.0
            pack width height (fun x y -> black.[y, x])

    /// Convert an image stream to a ^GF graphic field using the given dithering
    /// algorithm and luminance cutoff (0-255).
    let fromStreamWith (dithering: Dithering) (threshold: int) (stream: Stream) : LabelElement =
        use image = Image.Load<L8>(stream)
        encode dithering threshold image |> LabelElement.GraphicField

    /// Convert an image stream using a luminance threshold of 128 (no dithering).
    let fromStream (stream: Stream) : LabelElement =
        fromStreamWith Dithering.Threshold DefaultThreshold stream

    /// Convert an image file to a ^GF graphic field using the given dithering
    /// algorithm and luminance cutoff (0-255).
    let fromFileWith (dithering: Dithering) (threshold: int) (path: string) : LabelElement =
        use stream = File.OpenRead(path)
        fromStreamWith dithering threshold stream

    /// Convert an image file using a luminance threshold of 128 (no dithering).
    let fromFile (path: string) : LabelElement =
        fromFileWith Dithering.Threshold DefaultThreshold path
