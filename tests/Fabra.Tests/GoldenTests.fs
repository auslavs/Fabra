namespace Fabra.Tests

open System
open System.IO
open Xunit

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
        Assert.Equal(golden "GS1.zpl", normalize (string Examples.GS1.label))

    [<Fact>]
    let ``Australia Post traditional label renders to its golden ZPL`` () =
        Assert.Equal(golden "AustraliaPost_traditional.zpl", normalize (string Examples.AustraliaPost.label))
