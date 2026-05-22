# Fabra

An F# DSL for generating Zebra Programming Language (ZPL) labels.

## Project layout

- `Types.fs` — ZPL command domain types; each renders itself via `ToString()`.
- `Label.fs` — the `Label` type with `static member` factory functions, the
  internal `Render` module, and the public `ZPL.render` function.
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

Fabra implements roughly 10 of the ~200 ZPL commands — the subset needed
for simple static labels. Planned additions, ordered easy → hard:

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
- [ ] `^GF` — Graphic Field (bitmap images). Largest item; needs an
      image → monochrome-bitmap encoder.
  - [x] Phase 1: accept pre-encoded `^GFA` ASCII-hex data (`GraphicField`
        type; renders `^GFA,{b},{c},{d},{data}^FS`).
  - [ ] Phase 2: add the image-to-bitmap converter.

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
