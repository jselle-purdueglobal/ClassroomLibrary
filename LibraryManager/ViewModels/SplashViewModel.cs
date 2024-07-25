using ReactiveUI;

namespace LibraryManager.ViewModels;

public class SplashViewModel(IScreen screen) : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment => "Splash";
    public IScreen HostScreen { get; } = screen;
}