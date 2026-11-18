using TeamsMd.Core;

namespace TeamsMd.Core.Tests;

public class ClipboardPayloadTests
{
    // Review Focus 4: blank input must not produce anything to copy.
    [Theory]
    [InlineData("")]
    [InlineData("   \r\n\t")]
    [InlineData(null)]
    public void Blank_input_gives_null(string? md) =>
        Assert.Null(ClipboardPayload.FromMarkdown(md!));

    [Fact]
    public void Payload_has_html_and_original_markdown()
    {
        var p = ClipboardPayload.FromMarkdown("**b**")!;
        Assert.Equal("**b**", p.PlainText);
        Assert.Contains("<!--StartFragment--><strong>", p.CfHtml.Replace("<p>", ""));
    }
}
