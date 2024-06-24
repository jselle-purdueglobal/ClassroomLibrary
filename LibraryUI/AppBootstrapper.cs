using System;
using System.Reactive;
using LibraryUI.ViewModels;
using LibraryUI.Views;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace LibraryUI;

public class AppBootstrapper : ReactiveObject, IScreen
{
    public AppBootstrapper()
    {
        Router = new RoutingState();
        ConfigureServices();
    }
    
    public RoutingState Router { get; }

    // Configure dependency injection container
    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IScreen>(this);
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MainWindow>();

        var serviceProvider = services.BuildServiceProvider();
        Locator.CurrentMutable.RegisterConstant(serviceProvider, typeof(IServiceProvider));
    }

    public void RegisterParts()
    {
        Locator.CurrentMutable.RegisterViewsForViewModels(typeof(AppBootstrapper).Assembly);
    }

    public IObservable<Unit> Initialize()
    {
        RegisterParts();
        ConfigureServices();
        var mainWindowViewModel = Locator.Current.GetService<MainWindowViewModel>();
        return Router.Navigate.Execute(mainWindowViewModel);
    } 
}