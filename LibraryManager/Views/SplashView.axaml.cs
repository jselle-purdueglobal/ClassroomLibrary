using Avalonia.ReactiveUI;
using LibraryManager.ViewModels;

namespace LibraryManager.Views;

public partial class SplashView : ReactiveUserControl<SplashViewModel>
{
    public SplashView()
    {
        InitializeComponent();
    }
}