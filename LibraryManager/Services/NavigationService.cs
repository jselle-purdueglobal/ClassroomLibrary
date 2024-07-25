using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using LibraryManager.ViewModels;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManager.Services
{
    public class NavigationService(IServiceProvider serviceProvider, IScreen screen) : INavigationService
    {
        public async Task<TViewModel> NavigateToAsync<TViewModel>() where TViewModel : IRoutableViewModel
        {
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();
            await screen.Router.Navigate.Execute(viewModel);
            return viewModel;
        }
    }
}