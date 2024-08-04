using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using LibraryCatalog.Services;
using LibraryCatalog.ViewModels;
using LibraryCatalog.Views;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;

namespace LibraryCatalog;

public class AppBootstrapper : ReactiveObject, IScreen, IActivatableViewModel
{
    // Properties
    public RoutingState Router { get; }
    public ViewModelActivator Activator { get; }

    public AppBootstrapper()
    {
        Activator = new ViewModelActivator();
        Router = new RoutingState();
        
        RegisterServices();
        InitializeAsync();
    }
    
    // Register Services
    private void RegisterServices()
    {
        var services = new ServiceCollection();
        
        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "keys")));
        
        var serviceProvider = services.BuildServiceProvider();
        
        Locator.CurrentMutable.RegisterLazySingleton(() => 
            serviceProvider.GetRequiredService<IDataProtectionProvider>());
        Locator.CurrentMutable.RegisterLazySingleton(() => 
            new LibraryContext());

        var libraryContext = Locator.Current.GetService<LibraryContext>();

        Locator.CurrentMutable.RegisterLazySingleton<IApiService>(() =>
            new ApiService("http://localhost:5218/", libraryContext!));
        Locator.CurrentMutable.Register(() =>
            new CatalogView(), typeof(IViewFor<CatalogViewModel>));
        Locator.CurrentMutable.Register(() =>
            new CheckOutDialog(), typeof(IViewFor<CatalogViewModel>), "CheckOutDialog");
        Locator.CurrentMutable.Register(() =>
            new CheckInDialog(), typeof(IViewFor<CatalogViewModel>), "CheckInDialog");
        Locator.CurrentMutable.Register(() =>
            new SplashView(), typeof(IViewFor<SplashViewModel>));
        Locator.CurrentMutable.Register(() =>
            new LinkView(), typeof(IViewFor<LinkViewModel>));

        var apiService = Locator.Current.GetService<IApiService>();
        var dataProtection = Locator.Current.GetService<IDataProtectionProvider>();
        
        
        Locator.CurrentMutable.Register(() =>
            new CatalogViewModel(this, apiService!, libraryContext!));
        Locator.CurrentMutable.Register(() =>
            new SplashViewModel(this));
        Locator.CurrentMutable.Register(() => 
            new AuthService(apiService!, libraryContext!, dataProtection!));

        var authService = Locator.Current.GetService<AuthService>();
        
        Locator.CurrentMutable.Register(() =>
            new LinkViewModel(this, authService!));
    }
    
    // Initialize
    private async void InitializeAsync()
    {
        var splashViewModel = Locator.Current.GetService<SplashViewModel>();
        var authService = Locator.Current.GetService<AuthService>();
        var catalogViewModel = Locator.Current.GetService<CatalogViewModel>();
        var linkViewModel = Locator.Current.GetService<LinkViewModel>();

        await Router.Navigate.Execute(splashViewModel!);
        await Task.Delay(2000);

        var isLinked = await authService!.ValidateLibraryLinkAsync();
        if (isLinked)
        {
            await Router.Navigate.Execute(catalogViewModel!);
        }
        else
        {
            await Router.Navigate.Execute(linkViewModel!);
        }
    }
}