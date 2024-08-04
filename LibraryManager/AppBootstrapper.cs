using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using LibraryManager.Extensions;
using LibraryManager.Services;
using LibraryManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LibraryManager
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        public AppBootstrapper()
        {
            Router = new RoutingState();
            var services = new ServiceCollection();
            services.AddCommonServices(this);
            _serviceProvider = services.BuildServiceProvider();
            _authService = _serviceProvider.GetRequiredService<IAuthService>();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            // Navigate to SplashView first
            await Router.Navigate.Execute(_serviceProvider.GetRequiredService<SplashViewModel>());
            await Task.Delay(1000);
            
            try
            {
                var token = await _authService.GetAccessTokenAsync();
                if (token == null) return;
                await Router.Navigate.Execute(_serviceProvider.GetRequiredService<MainViewModel>());
            }
            catch (UnauthorizedAccessException)
            {
                await Router.Navigate.Execute(_serviceProvider.GetRequiredService<LoginViewModel>());
            }
        }
    }
}