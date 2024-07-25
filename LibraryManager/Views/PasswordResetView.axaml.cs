using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using LibraryManager.ViewModels;
using ReactiveUI;

namespace LibraryManager.Views;

public partial class PasswordResetView : ReactiveUserControl<PasswordResetViewModel>
{
    public PasswordResetView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}