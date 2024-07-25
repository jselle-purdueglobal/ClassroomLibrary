using System;
using System.Reactive;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using FluentAvalonia.UI.Controls;
using ReactiveUI.Fody.Helpers;

namespace LibraryManager.ViewModels;

public class MainViewModel : ReactiveObject, IRoutableViewModel
{
    public string UrlPathSegment => "Main";
    public IScreen HostScreen { get; }

    private readonly IServiceProvider _serviceProvider;
    [Reactive] public NavigationViewItem SelectedMenuItem { get; set; }
    [Reactive] public IRoutableViewModel CurrentViewModel { get; private set; }

    public MainViewModel(IScreen screen, IServiceProvider serviceProvider)
    {
        HostScreen = screen;
        _serviceProvider = serviceProvider;

        this.WhenAnyValue(x => x.SelectedMenuItem)
            .WhereNotNull()
            .Subscribe(NavigateToSelectedItem);

        // Set initial view
        SelectedMenuItem = new NavigationViewItem { Content = "Dashboard", Tag = "Dashboard" };
    }

    private void NavigateToSelectedItem(NavigationViewItem item)
    {
        var tag = item.Tag as string;
        CurrentViewModel = tag switch
        {
            "Dashboard" => _serviceProvider.GetRequiredService<DashboardViewModel>(),
            "Books" => _serviceProvider.GetRequiredService<BooksViewModel>(),
            "Students" => _serviceProvider.GetRequiredService<StudentsViewModel>(),
            _ => throw new ArgumentException("Invalid view")
        };
    }
}