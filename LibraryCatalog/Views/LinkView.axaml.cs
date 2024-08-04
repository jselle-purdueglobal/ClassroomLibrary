using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using LibraryCatalog.ViewModels;

namespace LibraryCatalog.Views;

public partial class LinkView : ReactiveUserControl<LinkViewModel>
{
    public LinkView()
    {
        InitializeComponent();
    }
}