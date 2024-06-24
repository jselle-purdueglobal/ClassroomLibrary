using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LibraryUI.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}