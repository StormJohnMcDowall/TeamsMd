using TeamsMd.Core;

namespace TeamsMd.Core.Tests;

public class DraftStoreTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "teamsmd-test-" + Guid.NewGuid());

    public void Dispose()
    {
        if (Directory.Exists(_dir)) Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void Load_is_null_before_anything_is_saved() =>
        Assert.Null(new DraftStore(_dir).Load());

    [Fact]
    public void Saved_text_round_trips_exactly()
    {
        var text = "# Hi\r\n\r\ncafé 日本 😀\r\n- [x] done";
        Assert.True(new DraftStore(_dir).Save(text));
        Assert.Equal(text, new DraftStore(_dir).Load());
    }

    // A cleared box must stay cleared: empty draft is "", not "no draft".
    [Fact]
    public void Empty_draft_loads_as_empty_not_null()
    {
        new DraftStore(_dir).Save("");
        Assert.Equal("", new DraftStore(_dir).Load());
    }

    [Fact]
    public void Later_save_replaces_earlier()
    {
        var store = new DraftStore(_dir);
        store.Save("first, longer text");
        store.Save("second");
        Assert.Equal("second", store.Load());
    }
}
