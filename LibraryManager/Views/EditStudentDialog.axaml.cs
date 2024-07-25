using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using LibraryManager.ViewModels;

namespace LibraryManager.Views;

public partial class EditStudentDialog : ReactiveUserControl<StudentsViewModel>
{
    public EditStudentDialog()
    {
        InitializeComponent();
    }
}