using System.Threading.Tasks;
using LibraryManager.ViewModels;
using ReactiveUI;

namespace LibraryManager.Services
{
    public interface INavigationService
    {
        Task<TViewModel> NavigateToAsync<TViewModel>() where TViewModel : IRoutableViewModel;
    }
}