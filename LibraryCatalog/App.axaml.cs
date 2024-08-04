using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FluentAvalonia.Styling;
using LibraryCatalog.ViewModels;
using LibraryCatalog.Views;
using ReactiveUI;
using Splat;

namespace LibraryCatalog;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            Locator.CurrentMutable.RegisterViewsForViewModels(typeof(App).Assembly);
            
            Styles.Add(new FluentAvaloniaTheme());
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new AppBootstrapper()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}