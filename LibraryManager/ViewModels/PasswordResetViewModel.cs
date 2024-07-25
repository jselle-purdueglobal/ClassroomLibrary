using System;
using System.Reactive;
using System.Threading.Tasks;
using LibraryManager.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace LibraryManager.ViewModels;

public class PasswordResetViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => "PasswordReset";
    public IScreen HostScreen { get; }

    private readonly IApiClient _apiClient;
    private readonly INavigationService _navigationService;

    [Reactive] public string NewPassword { get; set; }
    [Reactive] public string ConfirmPassword { get; set; }
    [Reactive] public string ErrorMessage { get; set; }

    public ReactiveCommand<Unit, Unit> ResetPasswordCommand { get; }

    public PasswordResetViewModel(IScreen screen, INavigationService navigationService, IApiClient apiClient)
    {
        HostScreen = screen;
        _apiClient = apiClient;
        _navigationService = navigationService;

        var canResetPassword = this.WhenAnyValue(
            x => x.NewPassword,
            x => x.ConfirmPassword,
            (newPassword, confirmPassword) =>
                !string.IsNullOrWhiteSpace(newPassword) &&
                !string.IsNullOrWhiteSpace(confirmPassword) &&
                newPassword.Length >= 8 &&
                newPassword == confirmPassword);

        ResetPasswordCommand = ReactiveCommand.CreateFromTask(ExecuteResetPasswordAsync, canResetPassword);

        this.WhenAnyValue(x => x.NewPassword, x => x.ConfirmPassword)
            .Subscribe(_ => ErrorMessage = string.Empty);
    }

    private async Task ExecuteResetPasswordAsync()
    {
        try
        {
            var result = await _apiClient.ResetPasswordAsync(NewPassword);
            if (result)
            {
                await _navigationService.NavigateToAsync<MainViewModel>();
            }
            else
            {
                ErrorMessage = "Failed to reset password. Please try again.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to reset password. Please try again.";
        }
    }
}