using Markdig;
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
        }

        return document.ToHtml(Pipeline);
    }
}
