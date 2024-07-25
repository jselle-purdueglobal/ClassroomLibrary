using ReactiveUI;

namespace LibraryManager.ViewModels;

public class BooksViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => "Books";
    public IScreen HostScreen { get; }
}