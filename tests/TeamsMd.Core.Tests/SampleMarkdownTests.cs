using TeamsMd.Core;

namespace TeamsMd.Core.Tests;

public class SampleMarkdownTests
{
    [Fact]
    public void Sample_is_the_teams_paste_test()
    {
        Assert.StartsWith("# TeamsMd paste test", SampleMarkdown.Text);
        Assert.Contains("- [x] Done item", SampleMarkdown.Text);
    }
}
