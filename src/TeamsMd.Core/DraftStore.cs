using System.Text;

namespace TeamsMd.Core;

/// <summary>Keeps the text box contents in a file so they survive restarts.</summary>
public sealed class DraftStore(string settingsDir)
{
    private readonly string _path = Path.Combine(settingsDir, "draft.md");

    /// <summary>The saved draft, or null if nothing has ever been saved (or it can't be read).</summary>
    public string? Load()
    {
        try
        {
            return File.Exists(_path) ? File.ReadAllText(_path, Encoding.UTF8) : null;
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    /// <summary>Saves the draft. Returns false if it couldn't be written.</summary>
    public bool Save(string text)
    {
        try
        {
            Directory.CreateDirectory(settingsDir);
            // Write a temp file then swap it in, so a crash mid-write can't corrupt the draft.
            var temp = _path + ".tmp";
            File.WriteAllText(temp, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            File.Move(temp, _path, overwrite: true);
            return true;
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}
