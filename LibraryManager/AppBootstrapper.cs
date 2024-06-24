using System.ComponentModel.Design;
using LibraryManager.ViewModels;
using LibraryManager.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LibraryManager;

public class AppBootstrapper : ReactiveObject, IScreen
{
    public RoutingState Router { get; }

    public AppBootstrapper()
    {
        Router = new RoutingState();
        Router.NavigateAndReset.Execute(new LoginViewModel(this));
    }
    
    //Configure DI Container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IScreen>(this);
        services.AddTransient<IViewFor<LoginViewModel>, LoginView>();
    }
}