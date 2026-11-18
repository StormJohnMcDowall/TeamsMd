using System.Runtime.InteropServices;
using TeamsMd.Core;

namespace TeamsMd.App;

internal sealed class MainForm : Form
{
    private readonly TextBox _input;
    private readonly Label _status;

    // Primary button colours: brand purple, darker on hover/press.
    private static readonly Color Primary = Color.FromArgb(0x5B, 0x5F, 0xC7);
    private static readonly Color PrimaryHover = Color.FromArgb(0x4F, 0x52, 0xB2);
    private static readonly Color PrimaryPressed = Color.FromArgb(0x44, 0x47, 0x91);

    public MainForm()
    {
        SuspendLayout();

        // Sizes below are written for 96 DPI; scale them up on high-DPI displays.
        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode = AutoScaleMode.Dpi;

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
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(160, 40),
            Padding = new Padding(14, 6, 14, 6),
            Anchor = AnchorStyles.Right,
            Font = new Font(Font.FontFamily, 10f, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            BackColor = Primary,
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false,
        };
        copyButton.FlatAppearance.BorderSize = 0;
        copyButton.FlatAppearance.MouseOverBackColor = PrimaryHover;
        copyButton.FlatAppearance.MouseDownBackColor = PrimaryPressed;
        copyButton.Click += (_, _) => CopyForTeams();

        _status = new Label
        {
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Text = "Ctrl+Enter to copy",
        };

        // Bottom strip sizes itself to the button, so the text can't be clipped.
        var bottom = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(8),
        };
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bottom.Controls.Add(_status, 0, 0);
        bottom.Controls.Add(copyButton, 1, 0);

        Controls.Add(_input);
        Controls.Add(bottom);

        ResumeLayout(performLayout: true);
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
