namespace Fabra.Tests

open System
open System.IO
open System.Globalization
open Xunit
open Fabra

/// Golden-file tests: render the example labels and assert they match the
/// committed .zpl files in /Examples. These lock the exact ZPL output so any
/// unintended change to the rendering is caught.
module GoldenTests =

    /// Normalise line endings so the comparison is stable across platforms
    /// (StringBuilder.AppendLine emits Environment.NewLine).
    let private normalize (s: string) =
        s.Replace("\r\n", "\n").Replace("\r", "\n").TrimEnd()

    let private golden name =
        Path.Combine(AppContext.BaseDirectory, "golden", name)
        |> File.ReadAllText
        |> normalize

    [<Fact>]
    let ``GS1 logistics label renders to its golden ZPL`` () =
        Assert.Equal(golden "GS1.zpl", normalize (ZPL.render Examples.GS1.label))

    [<Fact>]
    let ``Australia Post traditional label renders to its golden ZPL`` () =
        Assert.Equal(golden "AustraliaPost_traditional.zpl", normalize (ZPL.render Examples.AustraliaPost.label))

    [<Fact>]
    let ``Label.ToString() still renders the golden ZPL`` () =
        Assert.Equal(golden "GS1.zpl", normalize (string Examples.GS1.label))
        Assert.Equal(golden "AustraliaPost_traditional.zpl", normalize (string Examples.AustraliaPost.label))

    [<Fact>]
    let ``^BY ratio renders with a dot under a comma-decimal culture`` () =
        let original = CultureInfo.CurrentCulture
        try
            CultureInfo.CurrentCulture <- CultureInfo.GetCultureInfo("de-DE")
            let zpl = ZPL.render (Label [ Label.BY 3 2.5 10 ])
            Assert.Contains("^BY3,2.5,10", zpl)
        finally
            CultureInfo.CurrentCulture <- original

    [<Fact>]
    let ``^FX renders a comment terminated by ^FS`` () =
        let zpl = ZPL.render (Label [ Label.FX "setup notes" ])
        Assert.Contains("^FXsetup notes^FS", zpl)

    [<Fact>]
    let ``^LH renders the label home position`` () =
        let zpl = ZPL.render (Label [ Label.LH 30 60 ])
        Assert.Contains("^LH30,60", zpl)

    [<Fact>]
    let ``^FX comment renders as its own statement among fields`` () =
        let zpl =
            ZPL.render (Label [
                Label.FO 60 60 Justification.Left
                Label.FX "note"
                Label.Text (Font '0') Orientation.N 30 30 "hi" ])
        Assert.Equal("^XA\n^FO60,60,0\n^FXnote^FS\n^A0N,30,30^FDhi^FS\n^XZ", normalize zpl)

    [<Fact>]
    let ``^A renders the selected font`` () =
        let zpl = ZPL.render (Label [ Label.Text (Font 'A') Orientation.N 30 20 "hi" ])
        Assert.Contains("^AAN,30,20^FDhi^FS", zpl)

    [<Fact>]
    let ``^FB renders a field block before its field data`` () =
        let zpl = ZPL.render (Label [ Label.FB 400 3 0 FieldBlockJustification.Left 0 "wrapped text" ])
        Assert.Contains("^FB400,3,0,L,0^FDwrapped text^FS", zpl)

    [<Fact>]
    let ``^FB justification renders the ZPL letter`` () =
        let zpl = ZPL.render (Label [ Label.FB 200 2 5 FieldBlockJustification.Centre 10 "x" ])
        Assert.Contains("^FB200,2,5,C,10^FDx^FS", zpl)

    [<Fact>]
    let ``^BQ renders the QR command with a prefixed field data`` () =
        let zpl = ZPL.render (Label [ Label.BQ Orientation.N 2 10 QrErrorCorrection.Q 7 "HELLO" ])
        Assert.Contains("^BQN,2,10,Q,7^FDQA,HELLO^FS", zpl)

    [<Fact>]
    let ``^BQ repeats the error-correction level in the ^FD prefix`` () =
        let zpl = ZPL.render (Label [ Label.BQ Orientation.N 2 4 QrErrorCorrection.H 5 "data" ])
        Assert.Contains("^BQN,2,4,H,5^FDHA,data^FS", zpl)
