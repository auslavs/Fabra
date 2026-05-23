# Fabra

An F# DSL for generating Zebra Programming Language (ZPL) labels.

## Project layout

- `Types.fs` — ZPL command domain types; each renders itself via `ToString()`.
- `Label.fs` — the `Label` type with `static member` factory functions, the
  internal `Render` module, and the public `ZPL.render` function.
- `Fabra.Imaging/` — optional companion package (`net8.0`) that converts
  image files to `^GF` graphic fields via SixLabors.ImageSharp. Keeps the
  image dependency out of the zero-dependency core.
- `Examples/` — `.fsx` scripts plus their expected `.zpl` golden output.
- `tests/Fabra.Tests/` — xUnit golden-file and rendering tests.

## Build & test

```
dotnet build Fabra.sln -c Release
dotnet test Fabra.sln
```

The golden tests assert the example labels render to the committed
`Examples/*.zpl` files. If a rendering change is intentional, regenerate
those files and review the diff before committing.

## Adding a ZPL command

Each command follows the same pattern:

1. Add a domain type to `Types.fs` with a `ToString()` that emits the ZPL.
2. Add a case to the `LabelElement` union.
3. Add a `static member` factory to the `Label` type in `Label.fs`.
4. Add a branch to the `Render.label` loop in `Label.fs`.
5. Add a test in `tests/Fabra.Tests`.

## Pull requests

- Always request a GitHub Copilot review on a new pull request.
- Evaluate each Copilot comment on its merits. Reject suggestions that
  aren't useful or are incorrect, replying to the thread with a brief
  reason (cite the ZPL spec where relevant). Apply the useful ones and
  reply to the thread confirming the change.

## ZPL command roadmap

Fabra implements about 25 of the ~200 ZPL commands. The original
easy → hard plan below is complete; the full command set and what remains
is catalogued under "ZPL command catalogue". Planned additions:

- [x] `^FX` — Comment. New `Comment` type; renders `^FX{text}^FS`.
- [x] `^LH` — Label Home. New `LabelHome` record (x, y); renders `^LH{x},{y}`.
- [x] `^A` font selector — the `Text` type currently hardcodes font `0`
      (`^A0`). Add a font parameter (`A`–`Z`, `0`–`9`) so other resident
      fonts can be selected. Breaking change to the `Label.Text` factory.
- [x] `^FB` — Field Block (word-wrapped, multi-line text). New `FieldBlock`
      type (width, max lines, line spacing, justification, hanging indent);
      a modifier emitted before the `^FD` it applies to.
- [x] `^BQ` — QR Code. New `QrCode` type (orientation, model,
      magnification, error correction, mask); renders `^BQ...` plus `^FD`.
- [x] `^GF` — Graphic Field (bitmap images). Largest item; needed an
      image → monochrome-bitmap encoder.
  - [x] Phase 1: accept pre-encoded `^GFA` ASCII-hex data (`GraphicField`
        type; renders `^GFA,{b},{c},{d},{data}^FS`).
  - [x] Phase 2: image-to-bitmap converter in the `Fabra.Imaging` package
        (`ImageField.fromFile`/`fromStream`); luminance threshold (default)
        or Floyd–Steinberg dithering.

## ZPL command catalogue

Coverage of the ZPL II command set grouped by family. `[x]` = implemented,
`[ ]` = not yet implemented. This lists the major commands and is not
guaranteed exhaustive; consult the Zebra ZPL II Programming Guide for the
authoritative spec and parameter details. New commands follow the five
steps in "Adding a ZPL command" above.

The `^XA`/`^XZ` start/end-format wrappers and the trailing `^FS` field
separator are emitted automatically by the renderer and have no factory.

### Fields & formatting

- [x] `^FD` — Field Data
- [x] `^FO` — Field Origin
- [x] `^FB` — Field Block
- [x] `^FR` — Field Reverse Print
- [x] `^FH` — Field Hexadecimal Indicator
- [x] `^FX` — Comment
- [ ] `^FT` — Field Typeset
- [ ] `^FV` — Field Variable
- [ ] `^FW` — Field Orientation (default)
- [ ] `^FN` — Field Number
- [ ] `^FP` — Field Parameter (character spacing/direction)
- [ ] `^FM` — Multiple Field Origin Locations
- [ ] `^FC` — Field Clock (real-time clock)
- [ ] `^FL` — Font Linking
- [ ] `^CO` — Cache On

### Fonts & text

- [x] `^A` — Scalable/bitmap font
- [x] `^CF` — Change Alphanumeric Default Font
- [x] `^CI` — Change International Font/Encoding
- [ ] `^A@` — Use named font
- [ ] `^CW` — Font Identifier
- [ ] `^SL` — Set Mode/Language (RTC)
- [ ] `~DB` / `~DS` — Download bitmap / scalable font
- [ ] `^TB` — Text Block

### Bar codes

- [x] `^BY` — Bar Code Field Default
- [x] `^BC` — Code 128
- [x] `^B2` — Interleaved 2 of 5
- [x] `^B3` — Code 39
- [x] `^BE` — EAN-13
- [x] `^BU` — UPC-A
- [x] `^BX` — Data Matrix
- [x] `^BQ` — QR Code
- [ ] `^B1` — Code 11
- [ ] `^B4` — Code 49
- [ ] `^B5` — Planet Code
- [ ] `^B7` — PDF417
- [ ] `^B8` — EAN-8
- [ ] `^B9` — UPC-E
- [ ] `^BA` — Code 93
- [ ] `^BB` — CODABLOCK
- [ ] `^BD` — UPS MaxiCode
- [ ] `^BF` — Micro-PDF417
- [ ] `^BI` — Industrial 2 of 5
- [ ] `^BJ` — Standard 2 of 5
- [ ] `^BK` — ANSI Codabar
- [ ] `^BL` — LOGMARS
- [ ] `^BM` — MSI
- [ ] `^BO` — Aztec
- [ ] `^BP` — Plessey
- [ ] `^BR` — GS1 DataBar (RSS)
- [ ] `^BS` — UPC/EAN extensions
- [ ] `^BT` — TLC39
- [ ] `^BZ` — POSTNET

### Graphics

- [x] `^GB` — Graphic Box
- [x] `^GC` — Graphic Circle
- [x] `^GD` — Graphic Diagonal Line
- [x] `^GE` — Graphic Ellipse
- [x] `^GF` — Graphic Field (see also `Fabra.Imaging`)
- [ ] `^GS` — Graphic Symbol
- [ ] `^IM` — Image Move
- [ ] `^IL` — Image Load
- [ ] `^IS` — Image Save
- [ ] `~DG` / `~DY` — Download graphic / objects
- [ ] `^XG` — Recall graphic

### Label & format setup

- [x] `^LH` — Label Home
- [x] `^LL` — Label Length
- [ ] `^LS` — Label Shift
- [ ] `^LT` — Label Top
- [ ] `^LR` — Label Reverse Print
- [ ] `^PO` — Print Orientation (invert)
- [ ] `^PM` — Print Mirror Image
- [ ] `^PF` — Slew given number of dots
- [ ] `^XF` — Recall format
- [ ] `^XB` — Suppress Backfeed

### Media & printer configuration

- [x] `^MD` — Media Darkness
- [x] `^PW` — Print Width
- [x] `^PQ` — Print Quantity
- [ ] `^PR` — Print Rate (speed)
- [ ] `^MN` — Media Tracking
- [ ] `^MT` — Media Type
- [ ] `^MM` — Print Mode
- [ ] `^MU` — Set Units
- [ ] `^MF` — Media Feed (power-up/head-close)
- [ ] `^ML` — Maximum Label Length
- [ ] `^SS` — Set Media Sensors
- [ ] `^CM` — Change Memory Letter Designation
- [ ] `^CC` / `~CC` — Change Caret prefix
- [ ] `^CD` / `~CD` — Change Delimiter
- [ ] `^CT` / `~CT` — Change Tilde prefix

### Serialization, clock & data

- [ ] `^SN` — Serialization Data
- [ ] `^SF` — Serialization Field
- [ ] `^ST` — Set Date and Time (RTC)
- [ ] `^SE` — Select Encoding table

### Control (`~`) commands

- [ ] `~SD` — Set Darkness
- [ ] `~JA` — Cancel All
- [ ] `~JL` — Set Label Length
- [ ] `~JR` — Power-On Reset
- [ ] `~JS` — Change Backfeed Sequence
- [ ] `~JX` — Cancel Current Format
- [ ] `~PS` / `~PP` — Print Start / Pause
- [ ] `~HS` — Host Status Return
- [ ] `~HI` — Host Identification
- [ ] `~HM` — Host Memory Status
- [ ] `~WC` — Print Configuration Label

### RFID (`^R` / `~R`)

- [ ] `^RS` — RFID Setup
- [ ] `^RF` — RFID Read/Write
- [ ] `^RB` — Define EPC Data Structure
- [ ] `^RI` — Get RFID Tag ID
- [ ] `^RM` — Enable RFID Motion
- [ ] `^RR` — Specify RFID Retries
- [ ] `^RT` — Read RFID Tag
- [ ] `^RW` — Set RFID Read/Write Power
- [ ] `^RZ` — Set RFID Tag Password
- [ ] `~RV` — Report RFID Validation
- [ ] `^WV` — Verify RFID Write

## Open design items

- **Input validation.** Fabra performs no validation anywhere — numeric
  ranges (`^FO`, `^GB`, `^BX`, …) and `^FD`/`^A` content trust the caller,
  with valid ranges only documented in XML docs. A Copilot review of the
  `^A` font selector asked whether the `Font` identifier (`A`–`Z`, `0`–`9`)
  should be validated. Undecided: keep trusting the caller, add validating
  smart constructors per type, or adopt library-wide validation. If
  adopted, apply it consistently across all commands rather than ad hoc.

- **Feliz-style props API.** Commands currently take positional arguments
  via `static member` factories (e.g.
  `Label.BC Orientation.N 378 YesNo.N YesNo.N YesNo.N Mode.A data`), which
  reads poorly for commands with many parameters or defaults. An
  alternative is a Feliz-style list of props — e.g.
  `Label.qr [ qr.model 2; qr.magnification 10; qr.data "…" ]` — built from
  a per-command prop DU folded onto a defaulted record (no need for
  Feliz's erased-type machinery). Trades the compile-time "all required
  fields set" guarantee for default-backed optional props, and would be a
  library-wide change applied consistently rather than per command (ties
  into the validation item above). Worth a throwaway spike on one command
  before committing. Keep the current positional style until then.
