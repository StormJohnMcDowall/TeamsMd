using TeamsMd.Core;

namespace TeamsMd.Core.Tests;

public class MarkdownConverterTests
{
    [Fact]
    public void Bold_and_italic()
    {
        var h = MarkdownConverter.ToHtml("**b** *i*");
        Assert.Contains("<strong>b</strong>", h);
        Assert.Contains("<em>i</em>", h);
    }

    [Fact]
    public void Heading_list_link_code()
    {
        var h = MarkdownConverter.ToHtml("# T\n\n- a\n- b\n\n[x](https://e.com) `c`\n\n```cs\nvar x = 1;\n```");
        Assert.Contains("<h1>T</h1>", h);
        Assert.Contains("<li>a</li>", h);
        Assert.Contains("<a href=\"https://e.com\">x</a>", h);
        Assert.Contains("<code>c</code>", h);
        Assert.Contains("<pre><code>var x = 1;", h);
    }

    [Fact]
    public void Strikethrough() =>
        Assert.Contains("<del>s</del>", MarkdownConverter.ToHtml("~~s~~"));

    [Fact]
    public void Pipe_table() =>
        Assert.Contains("<table>", MarkdownConverter.ToHtml("| a | b |\n|---|---|\n| 1 | 2 |"));

    [Fact]
    public void Autolink() =>
        Assert.Contains("<a href=\"https://e.com\">https://e.com</a>", MarkdownConverter.ToHtml("see https://e.com"));

    // Fenced code language and task lists both add class attributes by default; Teams strips them.
    [Fact]
    public void No_class_attributes() =>
        Assert.DoesNotContain("class=", MarkdownConverter.ToHtml("```cs\nx\n```\n\n- [x] done"));

    // Teams strips <input> form controls, so checkbox state must be carried as text.
    [Fact]
    public void Task_list_state_survives_as_text()
    {
        var h = MarkdownConverter.ToHtml("- [x] done\n- [ ] todo");
        Assert.DoesNotContain("<input", h);
        Assert.Contains("☑ done", h);
        Assert.Contains("☐ todo", h);
    }

    // Review Focus 2: angle brackets in prose must stay visible text.
    [Fact]
    public void Angle_brackets_are_literal()
    {
        var h = MarkdownConverter.ToHtml("Use List<string> and <b>x</b>");
        Assert.Contains("List&lt;string&gt;", h);
        Assert.Contains("&lt;b&gt;", h);
    }

    // Review Focus 3: Windows line endings give the same output.
    [Fact]
    public void Crlf_matches_lf() =>
        Assert.Equal(MarkdownConverter.ToHtml("# T\n\n- a\n- b"), MarkdownConverter.ToHtml("# T\r\n\r\n- a\r\n- b"));
}
