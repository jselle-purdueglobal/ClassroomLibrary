using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace LibraryManager.ViewModels;

public class 
    LoginViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => "Login";
    public IScreen HostScreen { get; }

    public string Password;
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    public LoginViewModel(IScreen screen)
    {
        HostScreen = screen;
        var canLogin = this.WhenAnyValue(
            x => x.Username,
            x => x.Password,
            (username, password) =>
                !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password));

        LoginCommand = ReactiveCommand.CreateFromTask(ExecuteLoginAsync, canLogin);
    }

    private async Task ExecuteLoginAsync()
    {
        // Login logic here
        await Task.Delay(1000); // Simulate async work
        // Handle post-login logic
    }
}