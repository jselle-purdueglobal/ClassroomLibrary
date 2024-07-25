using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace LibraryManager.Services;

public class UserContext : ReactiveObject
{
    [Reactive] public int LibraryId { get; set; }
    [Reactive] public string Username { get; set; } = string.Empty;

    public void Clear()
    {
        LibraryId = 0;
        Username = string.Empty;
    }
}