namespace Fabra.Tests

open System.IO
open Xunit
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open Fabra
open Fabra.Imaging

/// Tests for the Fabra.Imaging image → ^GF converter. Images are built in
/// memory and round-tripped through PNG (lossless) so the expected bytes are
/// deterministic.
module ImageTests =

    let private black = Rgba32(0uy, 0uy, 0uy, 255uy)
    let private white = Rgba32(255uy, 255uy, 255uy, 255uy)

    let private pngStream (width: int) (height: int) (paint: Image<Rgba32> -> unit) : MemoryStream =
        use image = new Image<Rgba32>(width, height)
        paint image
        let ms = new MemoryStream()
        image.SaveAsPng(ms)
        ms.Position <- 0L
        ms

    [<Fact>]
    let ``threshold packs an 8x1 black/white row to F0`` () =
        use ms =
            pngStream 8 1 (fun image ->
                for x in 0 .. 3 do image.[x, 0] <- black
                for x in 4 .. 7 do image.[x, 0] <- white)
        let zpl = ZPL.render (Label [ ImageField.fromStream ms ])
        Assert.Contains("^GFA,1,1,1,F0^FS", zpl)

    [<Fact>]
    let ``rows are padded to a whole byte`` () =
        // 9 black pixels: row spans two bytes -> FF then 1000_0000 (0x80).
        use ms =
            pngStream 9 1 (fun image ->
                for x in 0 .. 8 do image.[x, 0] <- black)
        let zpl = ZPL.render (Label [ ImageField.fromStream ms ])
        Assert.Contains("^GFA,2,2,2,FF80^FS", zpl)

    [<Fact>]
    let ``a fully white image produces all-zero bytes`` () =
        use ms =
            pngStream 8 2 (fun image ->
                for y in 0 .. 1 do
                    for x in 0 .. 7 do image.[x, y] <- white)
        let zpl = ZPL.render (Label [ ImageField.fromStream ms ])
        Assert.Contains("^GFA,2,2,1,0000^FS", zpl)
