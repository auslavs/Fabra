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
- [ ] `^FB` — Field Block (word-wrapped, multi-line text). New `FieldBlock`
      type (width, max lines, line spacing, justification, hanging indent);
      a modifier emitted before the `^FD` it applies to.
- [ ] `^BQ` — QR Code. New `QrCode` type (orientation, model,
      magnification, error correction, mask); renders `^BQ...` plus `^FD`.
- [ ] `^GF` — Graphic Field (bitmap images). Largest item; needs an
      image → monochrome-bitmap encoder. Phase 1: accept pre-encoded
      `^GFA` ASCII-hex data. Phase 2: add the image-to-bitmap converter.
