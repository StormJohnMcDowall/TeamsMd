# TeamsMd — design

## Intent
A small Windows desktop app: type or paste Markdown into a text box, click one button, and the formatted version lands on the clipboard ready to paste into Microsoft Teams. Teams doesn't render Markdown you paste in, so you lose the formatting or have to redo it by hand.

## Goals
- One window: a multi-line text box and a **Copy for Teams** button.
- Paste into Teams keeps headings, bold/italic, strikethrough, lists, links, inline code and code blocks.
- Tables are best effort (Teams may restyle or simplify them).
- Ships as one standalone `.exe` that runs without installing anything.

## Non-goals
- Live preview of the formatted text.
- Settings, themes, file open/save, installer, auto-update.
- Converting in the other direction (Teams → Markdown).

## Anti-goals
- A copy that reports success but pastes as raw Markdown or garbled text.
- Needing the .NET runtime or admin rights to run.

## Approaches considered
1. **C# / WinForms + Markdig** — chosen. The usual way to build a Windows app, with a mature Markdown converter and good clipboard support. The standalone build is large (tens of MB).
2. **Rust + pulldown-cmark** — gives a small, dependency-free exe, but means more code and a less standard UI for a one-button tool.
3. **Tauri (web UI)** — only worth it for a live preview, which is out of scope.

## Design
- **Stack:** .NET 10 (current LTS), WinForms, Markdig with its common extensions (pipe tables, strikethrough, task lists, autolinks). *(Claude's call)*
- **Conversion:** Markdown → HTML via Markdig, using plain semantic tags only (no CSS classes, which Teams strips). *(Claude's call)*
- **Clipboard:** one clipboard write containing two formats. The first is HTML in the Windows CF_HTML format, with the `Version/StartHTML/EndHTML/StartFragment/EndFragment` header worked out from UTF-8 byte offsets. The second is the original Markdown as plain text, for apps that don't accept HTML. *(Claude's call)*
- **UI:** resizable window, monospace text box, button at the bottom, **Ctrl+Enter** shortcut for the same action, and a status line that shows "Copied" or the error. *(Claude's call)*
- **Build:** cross-compiled from Linux (`EnableWindowsTargeting`) with `dotnet publish -r win-x64 --self-contained -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true`.
- **Code layout:** conversion and CF_HTML packaging live in a small library with no UI, covered by unit tests that run on Linux. The WinForms project is a thin shell around it.

## Verification
- Automated: unit tests for Markdown → HTML and for the CF_HTML header offsets, including non-ASCII text.
- Manual (user, on Windows): run the exe, paste a sample covering every supported element into Teams, and confirm the formatting survives.

## Left to the builder
Project and namespace names, the window's default size, and the exact text of the status messages.
