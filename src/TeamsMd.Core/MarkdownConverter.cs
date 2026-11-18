using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;

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
        foreach (var node in document.Descendants())
            node.TryGetAttributes()?.Classes?.Clear();

        return document.ToHtml(Pipeline);
    }
}
