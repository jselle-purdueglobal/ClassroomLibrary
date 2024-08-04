using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using LibraryCatalog.ViewModels;
using ReactiveUI;

namespace LibraryCatalog.Views;

public partial class SplashView : ReactiveUserControl<SplashViewModel>
{
    public SplashView()
    {
        InitializeComponent();
    }
}