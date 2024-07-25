using ReactiveUI;

namespace LibraryManager.ViewModels;

public class DashboardViewModel(IScreen screen) : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment => "Dashboard";
    public IScreen HostScreen { get; } = screen;
}