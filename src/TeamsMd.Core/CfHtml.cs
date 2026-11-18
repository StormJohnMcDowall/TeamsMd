using System.Text;

namespace TeamsMd.Core;

/// <summary>
/// Wraps an HTML fragment in the Windows "HTML Format" (CF_HTML) clipboard layout.
/// Offsets are UTF-8 byte positions, which is how WinForms writes this format.
/// </summary>
public static class CfHtml
{
    private const string HeaderTemplate =
        "Version:0.9\r\nStartHTML:{0:D10}\r\nEndHTML:{1:D10}\r\nStartFragment:{2:D10}\r\nEndFragment:{3:D10}\r\n";
    private const string Prefix = "<html><body>\r\n<!--StartFragment-->";
    private const string Suffix = "<!--EndFragment-->\r\n</body></html>";

    public static string Build(string htmlFragment)
    {
        // Every number is 10 digits, so the header length doesn't depend on the values.
        int headerLength = Encoding.UTF8.GetByteCount(string.Format(HeaderTemplate, 0, 0, 0, 0));
        int startHtml = headerLength;
        int startFragment = startHtml + Encoding.UTF8.GetByteCount(Prefix);
        int endFragment = startFragment + Encoding.UTF8.GetByteCount(htmlFragment);
        int endHtml = endFragment + Encoding.UTF8.GetByteCount(Suffix);

        return string.Format(HeaderTemplate, startHtml, endHtml, startFragment, endFragment)
            + Prefix + htmlFragment + Suffix;
    }
}
