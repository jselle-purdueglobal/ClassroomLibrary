using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace LibraryCatalog.Views;

public partial class MainWindow : ReactiveWindow<AppBootstrapper>, IActivatableView
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    public MainWindow()
    {
        InitializeComponent();
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.AttachDevTools();
        this.WhenActivated(disposables =>
        {
            // Handle activation logic here
            Disposable.Create(() => { /* Handle deactivation logic here */ }).DisposeWith(disposables);
        });
    }
}