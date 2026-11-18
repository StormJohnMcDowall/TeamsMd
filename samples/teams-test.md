# TeamsMd paste test

This file checks every element TeamsMd supports. Paste it in, press **Ctrl+Enter**, then paste into a Teams chat.

## Text styles

Plain text, **bold**, *italic*, ***bold italic***, ~~strikethrough~~, and `inline code`.

Accents and emoji: café, naïve, Zürich, 日本語, 😀 🚀 ✅

Angle brackets should show as text: List<string>, <b>not bold</b>, 2 < 3 > 1

## Line breaks

This line and the next are separated by a single Enter.
In standard Markdown they join into one line.

This paragraph comes after a blank line, so it should be separate.

## Lists

- First bullet
- Second bullet
  - Nested bullet
  - Another nested bullet
- Third bullet

1. Step one
2. Step two
3. Step three

## Task list

- [x] Done item (should show ☑)
- [ ] Not done item (should show ☐)

## Links

A [named link](https://learn.microsoft.com/microsoftteams/) and a bare one: https://github.com

## Quote

> This is a block quote.
> It spans two lines.

## Code block

```csharp
public static string Greet(string name)
{
    return $"Hello, {name}!";
}
```

## Table

| Feature       | Expected in Teams | Notes            |
|---------------|:-----------------:|------------------|
| Bold          | Yes               | **works**        |
| Code          | Yes               | `x = 1`          |
| Alignment     | Maybe             | centre column    |

---

That horizontal rule above should show as a line. End of test.
