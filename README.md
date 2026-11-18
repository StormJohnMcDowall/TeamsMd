# TeamsMd

A small Windows app: type or paste Markdown, click **Copy for Teams** (or press **Ctrl+Enter**), then paste into Microsoft Teams with the formatting intact.

It puts two things on the clipboard: the formatted HTML (for Teams, Outlook, Word, etc.) and your original Markdown as plain text (for Notepad and other plain-text apps).

Supported: headings, bold, italic, strikethrough, lists, task lists, links, inline code, code blocks and tables (Teams may restyle tables).

## Build

Requires the .NET 10 SDK (works on Windows or Linux).

```bash
dotnet test
dotnet publish src/TeamsMd.App -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -o publish
```

The app is `publish/TeamsMd.exe`, a single file that runs without installing .NET.
