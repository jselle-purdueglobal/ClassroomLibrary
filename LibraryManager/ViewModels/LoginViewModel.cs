using System;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using LibraryManager.Models;
using LibraryManager.Services;
using LibraryManager.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace LibraryManager.ViewModels;

public class LoginViewModel : ViewModelBase, IRoutableViewModel
{
    private INavigationService _navigationService;
    private IAuthService _authService;
    public string UrlPathSegment => "Login";
    public IScreen HostScreen { get; }
    
    [Reactive] public string Username { get; set; }
    [Reactive] public string Password { get; set; }
    [Reactive] public string ErrorMessage { get; set; }
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    public LoginViewModel(IScreen screen, INavigationService navigationService, IAuthService authService)
    {
        HostScreen = screen;
        _authService = authService;
        _navigationService = navigationService;
        var canLogin = this.WhenAnyValue(
            x => x.Username,
            x => x.Password,
            (username, password) =>
                !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password));

        LoginCommand = ReactiveCommand.CreateFromTask(ExecuteLoginAsync, canLogin);
        
        // Clear error message when username or password changes
        this.WhenAnyValue(x => x.Username, x => x.Password)
            .Subscribe(_ => ErrorMessage = string.Empty);
    }

    private async Task ExecuteLoginAsync()
    {
        try
        {
            var result = await _authService.LoginAsync(Username, Password);
            if (result.IsFirstLogin)
            {
                await _navigationService.NavigateToAsync<PasswordResetViewModel>();
            }
            else
            {
                await _navigationService.NavigateToAsync<MainViewModel>();   
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}