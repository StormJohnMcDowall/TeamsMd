namespace TeamsMd.Core;

/// <summary>The paste-test sample (samples/teams-test.md), embedded in the assembly.</summary>
public static class SampleMarkdown
{
    private const string ResourceName = "TeamsMd.Core.teams-test.md";

    public static string Text { get; } = Load();

    private static string Load()
    {
        using var stream = typeof(SampleMarkdown).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Missing embedded resource {ResourceName}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
