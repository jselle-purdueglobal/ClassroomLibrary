using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using LibraryCatalog.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace LibraryCatalog.ViewModels;

public class LinkViewModel : ViewModelBase, IRoutableViewModel
{
    // Fields
    private IAuthService _authService;
    
    // Properties
    public string? UrlPathSegment => "link";
    public IScreen HostScreen { get;}
    [Reactive] public string LibraryCode { get; set; } = string.Empty;
    [Reactive] public string StatusMessage { get; set; } = string.Empty;
    public ReactiveCommand<Unit, Unit> LinkLibraryCommand { get; }

    public LinkViewModel(IScreen screen, IAuthService authService)
    {
        HostScreen = screen;
        LinkLibraryCommand = ReactiveCommand.CreateFromTask(LinkLibraryAsync);
        _authService = authService;
        
        this.WhenAnyValue(x => x.LibraryCode)
            .Subscribe(_ => StatusMessage = string.Empty);
    }

    private async Task LinkLibraryAsync()
    {
        var result = await _authService.LinkLibraryAsync(LibraryCode);
        if (result)
        {
            var catalogViewModel = Locator.Current.GetService<CatalogViewModel>();
            HostScreen.Router.Navigate.Execute(catalogViewModel!);
        }
        else
        {
            StatusMessage = "Failed to link library. Please check your code and try again.";
        }
    }
}