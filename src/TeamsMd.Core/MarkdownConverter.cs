using Markdig;
using Markdig.Helpers;
using Markdig.Extensions.TaskLists;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace TeamsMd.Core;

/// <summary>Converts Markdown to plain semantic HTML suitable for pasting into Teams.</summary>
public static class MarkdownConverter
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UsePipeTables()
        .UseEmphasisExtras()
        .UseTaskLists()
        .UseAutoLinks()
        .DisableHtml()
        .Build();

    public static string ToHtml(string markdown)
    {
        var document = Markdown.Parse(markdown.Replace("\r\n", "\n"), Pipeline);

        // Teams strips CSS classes anyway; drop them so the output is plain semantic HTML.
        foreach (var node in document.Descendants().ToList())
        {
            node.TryGetAttributes()?.Classes?.Clear();

            // Teams strips <input> checkboxes, so show the state as a character instead.
            if (node is TaskList task)
                task.ReplaceBy(new LiteralInline(task.Checked ? "☑" : "☐"));

            // Teams replaces a link's text with the page title when the text equals the href,
            // so show raw URLs without their scheme.
            if (node is AutolinkInline { IsEmail: false } angle)
            {
                var link = new LinkInline(angle.Url, "") { IsAutoLink = true };
                link.AppendChild(new LiteralInline(StripScheme(angle.Url)));
                angle.ReplaceBy(link);
            }
            else if (node is LinkInline { IsAutoLink: true, FirstChild: LiteralInline text } bare
                     && text.Content.ToString() == bare.Url)
            {
                text.Content = new StringSlice(StripScheme(bare.Url));
            }
        }

        return document.ToHtml(Pipeline);
    }

    private static string StripScheme(string url)
    {
        foreach (var scheme in new[] { "https://", "http://" })
            if (url.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
                return url[scheme.Length..];
        return url;
    }
}
