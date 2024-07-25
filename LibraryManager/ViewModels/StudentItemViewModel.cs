using System.Reactive;
using System.Threading.Tasks;
using LibraryManager.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace LibraryManager.ViewModels;

public class StudentItemViewModel : ReactiveObject
{
    public Student Student { get; init; } = null!;
    [Reactive] public bool IsMarkedForDeletion { get; set; }
}