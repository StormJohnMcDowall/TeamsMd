using System.Runtime.InteropServices;
using TeamsMd.Core;

namespace TeamsMd.App;

internal sealed class MainForm : Form
{
    private readonly TextBox _input;
    private readonly Label _status;

    public MainForm()
    {
        Text = "TeamsMd";
        ClientSize = new Size(700, 500);
        StartPosition = FormStartPosition.CenterScreen;

        _input = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            AcceptsReturn = true,
            AcceptsTab = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 10f),
        };

        var copyButton = new Button
        {
            Text = "Copy for Teams",
            AutoSize = true,
            Dock = DockStyle.Right,
        };
        copyButton.Click += (_, _) => CopyForTeams();

        _status = new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Text = "Ctrl+Enter to copy",
        };

        var bottom = new Panel { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(6) };
        bottom.Controls.Add(_status);
        bottom.Controls.Add(copyButton);

        Controls.Add(_input);
        Controls.Add(bottom);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.Enter))
        {
            CopyForTeams();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void CopyForTeams()
    {
        var payload = ClipboardPayload.FromMarkdown(_input.Text);
        if (payload is null)
        {
            _status.Text = "Nothing to copy";
            return;
        }

        // WinForms writes the Html format as UTF-8, matching CfHtml's byte offsets.
        var data = new DataObject();
        data.SetData(DataFormats.Html, payload.CfHtml);
        data.SetData(DataFormats.UnicodeText, payload.PlainText);

        try
        {
            Clipboard.SetDataObject(data, copy: true, retryTimes: 10, retryDelay: 100);
            _status.Text = "Copied";
        }
        catch (ExternalException)
        {
            _status.Text = "Clipboard busy — try again";
        }
    }
}
