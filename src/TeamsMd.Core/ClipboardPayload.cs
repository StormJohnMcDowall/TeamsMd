namespace TeamsMd.Core;

/// <summary>What goes on the clipboard: CF_HTML for rich paste, the original Markdown as plain text.</summary>
public sealed record ClipboardPayload(string CfHtml, string PlainText)
{
    /// <summary>Returns null when there is nothing to copy (null, empty or whitespace input).</summary>
    public static ClipboardPayload? FromMarkdown(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return null;

        return new ClipboardPayload(Core.CfHtml.Build(MarkdownConverter.ToHtml(markdown)), markdown);
    }
}
