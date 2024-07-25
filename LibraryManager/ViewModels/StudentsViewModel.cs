using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using LibraryManager.Models;
using LibraryManager.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using LibraryManager.Services;

namespace LibraryManager.ViewModels;

public class StudentsViewModel : ViewModelBase, IRoutableViewModel, IActivatableViewModel
{
    // Initialize Sample Data  
    
    // Reactive Routing Setup
    public string UrlPathSegment => "Students";
    public IScreen HostScreen { get; }

    private readonly IStudentService _studentService;
    private readonly UserContext _userContext;
    
    // Properties
    [Reactive] public ObservableCollection<StudentItemViewModel> StudentItems { get; set; }
    [Reactive] public ObservableCollection<Student> StudentsToDelete { get; set; }
    [Reactive] public string EditFirstName { get; set; } = string.Empty;
    [Reactive] public string EditLastName { get; set; } = string.Empty;
    [Reactive] public string AddFirstName { get; set; } = string.Empty;
    [Reactive] public string AddLastName { get; set; } = string.Empty;
    public ViewModelActivator Activator { get; } = new();

    // Commands
    public ReactiveCommand<StudentItemViewModel, Unit> EditStudentCommand { get; }
    public ReactiveCommand<StudentItemViewModel, Unit> ToggleDeleteCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteAllCommand { get; }
    public ReactiveCommand<Unit, Unit> AddStudentCommand { get; }
    
    // Constructor
    public StudentsViewModel(IScreen screen, IStudentService studentService, UserContext userContext)
    {
        HostScreen = screen;
        _studentService = studentService;
        _userContext = userContext;
        StudentItems = [];
        
        // Initialize Properties
        StudentsToDelete = new ObservableCollection<Student>();

        this.WhenActivated((disposables) =>
        {
            Task.Run(LoadStudentsAsync);

            Disposable
                .Create(() => { })
                .DisposeWith(disposables);
        });
        
        // Initialize Commands
        EditStudentCommand = ReactiveCommand.CreateFromTask<StudentItemViewModel>(EditStudentAsync);
        ToggleDeleteCommand = ReactiveCommand.Create<StudentItemViewModel>(ToggleDelete);
        AddStudentCommand = ReactiveCommand.CreateFromTask(AddStudentAsync);
        
        var deleteEnabled = this.WhenAnyValue(x => x.StudentsToDelete.Count)
            .Select(count => count > 0);
        
        DeleteAllCommand = ReactiveCommand.CreateFromTask(DeleteAllAsync, deleteEnabled);
    }
    
    // Load Students
    private async Task LoadStudentsAsync()
    {
        StudentItems.Clear();
        var students = await _studentService.GetLibraryStudentsAsync(_userContext.LibraryId);
        
        foreach (var student in students)
        {
            StudentItems.Add(new StudentItemViewModel
            {
                Student = student,
                IsMarkedForDeletion = false
            });
        }
    }
    
    // Edit Student
    private async Task EditStudentAsync(StudentItemViewModel studentItem)
    {
        EditFirstName = studentItem.Student.FirstName;
        EditLastName = studentItem.Student.LastName;

        var editDialog = new EditStudentDialog() { DataContext = this };

        var cd = new ContentDialog
        {
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            Title = "Edit Student",
            Content = editDialog,
            IsPrimaryButtonEnabled = true,
            IsSecondaryButtonEnabled = false
        };

        var result = await cd.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var student = studentItem.Student;
            await _studentService.UpdateStudentNameAsync(student.StudentId, EditFirstName, EditLastName);
            await LoadStudentsAsync();
        }
    }
    
    // Toggle Delete
    private void ToggleDelete(StudentItemViewModel studentItem)
    {
        studentItem.IsMarkedForDeletion = !studentItem.IsMarkedForDeletion;
        var student = studentItem.Student;
        
        if (studentItem.IsMarkedForDeletion)
        {
            if (StudentsToDelete.All(s => s.StudentId != student.StudentId))
            {
                StudentsToDelete.Add(student);
            }
        }
        else
        {
            var studentToRemove = StudentsToDelete.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (studentToRemove != null)
            {
                StudentsToDelete.Remove(studentToRemove);
            }
        }
    }
    
    // Delete All
    private async Task DeleteAllAsync()
    {
        var deleteDialog = new ConfirmDeleteDialog() { DataContext = this };

        var cd = new ContentDialog
        {
            PrimaryButtonText = "Confirm",
            CloseButtonText = "Cancel",
            Title = "Are you Sure?",
            Content = deleteDialog,
            IsPrimaryButtonEnabled = true,
            IsSecondaryButtonEnabled = false
        };

        var result = await cd.ShowAsync();
        
        if (result == ContentDialogResult.Primary)
        {
            var studentIds = StudentsToDelete.Select(s => s.StudentId).ToList();
            await _studentService.DeleteStudentsAsync(studentIds);
            await LoadStudentsAsync();
            StudentsToDelete.Clear();
        }
    }
    
    // Add Student
    private async Task AddStudentAsync()
    {
        var addDialog = new AddStudentDialog() { DataContext = this };
        
        var cd = new ContentDialog
        {
            PrimaryButtonText = "Add",
            CloseButtonText = "Cancel",
            Title = "Add Student",
            Content = addDialog,
            IsPrimaryButtonEnabled = true,
            IsSecondaryButtonEnabled = false
        };

        var result = await cd.ShowAsync();
        
        if (result == ContentDialogResult.Primary)
        {
            var student = new Student()
            {
                FirstName = AddFirstName,
                LastName = AddLastName,
                LibraryId = _userContext.LibraryId
            };
            await _studentService.AddStudentAsync(student);
            await LoadStudentsAsync();
            
            AddFirstName = string.Empty;
            AddLastName = string.Empty;
        }
    }
}