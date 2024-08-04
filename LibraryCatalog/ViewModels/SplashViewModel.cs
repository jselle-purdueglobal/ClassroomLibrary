using ReactiveUI;

namespace LibraryCatalog.ViewModels;

public class SplashViewModel(IScreen screen) : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment => "splash";
    public IScreen HostScreen { get; } = screen;
}