using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace LibraryCatalog.Services;

public class LibraryContext : ReactiveObject
{
    [Reactive] public int LibraryId { get; set; }
    [Reactive] public string LibraryName { get; set; } = string.Empty;
    [Reactive] public string AccessToken { get; set; } = string.Empty;
}