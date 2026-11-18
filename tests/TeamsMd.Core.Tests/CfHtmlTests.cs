using System.Text;
using System.Text.RegularExpressions;
using TeamsMd.Core;

namespace TeamsMd.Core.Tests;

public class CfHtmlTests
{
    private static int Offset(string cf, string key) =>
        int.Parse(Regex.Match(cf, "^" + key + ":(\\d{10})\r\n", RegexOptions.Multiline).Groups[1].Value);

    [Theory]
    [InlineData("<p>hi</p>")]
    [InlineData("<p>café — 日本 😀</p>")] // Review Focus 1: multi-byte UTF-8
    public void Offsets_point_at_fragment(string frag)
    {
        var cf = CfHtml.Build(frag);
        var bytes = Encoding.UTF8.GetBytes(cf);
        string Slice(int a, int b) => Encoding.UTF8.GetString(bytes, a, b - a);

        Assert.StartsWith("Version:0.9\r\n", cf);
        Assert.Equal(frag, Slice(Offset(cf, "StartFragment"), Offset(cf, "EndFragment")));
        Assert.StartsWith("<html>", Slice(Offset(cf, "StartHTML"), bytes.Length));
        Assert.Equal(bytes.Length, Offset(cf, "EndHTML"));
    }
}
