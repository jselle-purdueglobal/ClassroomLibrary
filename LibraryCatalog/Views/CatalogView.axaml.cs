using LibraryCatalog.ViewModels;
using Avalonia.ReactiveUI;

namespace LibraryCatalog.Views;

public partial class CatalogView : ReactiveUserControl<CatalogViewModel>
{
    public CatalogView()
    {
        InitializeComponent();
    }
}