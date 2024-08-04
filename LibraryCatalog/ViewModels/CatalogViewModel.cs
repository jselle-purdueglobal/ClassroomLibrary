using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using LibraryCatalog.Models;
using LibraryCatalog.Services;
using LibraryCatalog.Views;
using ReactiveUI.Fody.Helpers;

namespace LibraryCatalog.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

public class CatalogViewModel : ViewModelBase, IRoutableViewModel, IActivatableViewModel
{
    // Fields
    private readonly SourceList<Book> _sourceBooks = new();
    private readonly ReadOnlyObservableCollection<Book> _books;
    private readonly IApiService _apiService;
    private readonly LibraryContext _libraryContext;
    private string _searchQuery = string.Empty; 
    
    // Properties
    public ReadOnlyObservableCollection<Book> Books => _books;
    public string UrlPathSegment => "catalog"; 
    public IScreen HostScreen { get; }
    public ReactiveCommand<Book, Unit> BookSelectedCommand { get; }
    public ReactiveCommand<Unit, Unit> CheckInCommand { get; }
    public ViewModelActivator Activator { get; } = new();
    [Reactive] public Book SelectedCheckoutBook { get; set; }
    
    [Reactive] public Book? SelectedCheckInBook { get; set; }
    [Reactive] public ObservableCollection<Checkout>? ActiveCheckouts { get; set; }
    [Reactive] public Student SelectedCheckOutStudent { get; set; }
    [Reactive] public Student SelectedCheckInStudent { get; set; }
    [Reactive] public ObservableCollection<Student> AvailableStudents { get; set; }
    [Reactive] public string SearchQuery { get; set; }

    // Constructor
    public CatalogViewModel(IScreen screen, IApiService apiService, LibraryContext libraryContext)
    {
        HostScreen = screen;
        _apiService = apiService;
        _libraryContext = libraryContext;
        ActiveCheckouts = new ObservableCollection<Checkout>();
        BookSelectedCommand = ReactiveCommand.CreateFromTask<Book>(OnBookSelected);
        CheckInCommand = ReactiveCommand.CreateFromTask(OnCheckInSelected);
        var filter = this.WhenAnyValue(x => x.SearchQuery)
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Select(query => new Func<Book, bool>(book =>
                string.IsNullOrWhiteSpace(query) ||
                book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                book.Authors.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                book.Illustrators.Contains(query, StringComparison.OrdinalIgnoreCase)));

        this.WhenAnyValue(x => x.SelectedCheckInStudent)
            .WhereNotNull()
            .Subscribe(GetCheckInBook);

        _sourceBooks.Connect()
            .Filter(filter)
            .Bind(out _books)
            .Subscribe();
        
        this.WhenActivated((disposables) =>
        {
            Task.Run(InitializeCatalogAsync);

            Disposable
                .Create(() => { })
                .DisposeWith(disposables);
        });
    }

    // Get Check In Book
    private async void GetCheckInBook(Student student)
    {
        var bookId = ActiveCheckouts.FirstOrDefault(c => c.StudentId == student.StudentId)!.BookId;
        SelectedCheckInBook = await _apiService.GetCheckInBookAsync(bookId);
    }
    
    // Initialize
    private async Task InitializeCatalogAsync()
    {
        var checkouts = await _apiService.GetActiveCheckoutsAsync(_libraryContext.LibraryId);
        ActiveCheckouts = new ObservableCollection<Checkout>(checkouts ?? Enumerable.Empty<Checkout>());
        
        var books = await _apiService.GetBooksAsync();
        _sourceBooks.Edit(list =>
        {
            list.Clear();
            list.AddRange(books);
        });
    }
    
    // Checkout
    private async Task OnBookSelected(Book selectedBook)
    {
        SelectedCheckoutBook = selectedBook;
        var activeCheckouts = await _apiService.GetActiveCheckoutsAsync(_libraryContext.LibraryId);
        var students = await _apiService.GetStudentsAsync(_libraryContext.LibraryId);
        
        AvailableStudents = activeCheckouts == null ? new ObservableCollection<Student>(students) : 
            new ObservableCollection<Student>(students.Where(s => activeCheckouts.All(c => c.StudentId != s.StudentId)).ToList());
        
        var checkOutDialog = new CheckOutDialog() { DataContext = this };

        var cd = new ContentDialog
        {
            PrimaryButtonText = "Check Out",
            CloseButtonText = "Cancel",
            Title = "Check Out",
            Content = checkOutDialog,
            IsPrimaryButtonEnabled = true,
            IsSecondaryButtonEnabled = false
        };

        var result = await cd.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var checkout = new AddCheckoutDto
            {
                StudentId = SelectedCheckOutStudent.StudentId,
                BookId = SelectedCheckoutBook.BookId,
                LibraryId = _libraryContext.LibraryId
            };
            await _apiService.AddCheckoutAsync(checkout);
        }
    }
    
    // Check In
    private async Task OnCheckInSelected()
    {
        SelectedCheckInBook = null;
        var students = await _apiService.GetStudentsAsync(_libraryContext.LibraryId);
        var checkouts = await _apiService.GetActiveCheckoutsAsync(_libraryContext.LibraryId);
        ActiveCheckouts = new ObservableCollection<Checkout>(checkouts ?? Enumerable.Empty<Checkout>());
        AvailableStudents = ActiveCheckouts == null ? [] : 
            new ObservableCollection<Student>(students.Where(s => ActiveCheckouts.Any(c => c.StudentId == s.StudentId)).ToList());
        
        var checkInDialog = new CheckInDialog { DataContext = this };

        var cd = new ContentDialog
        {
            PrimaryButtonText = "Check In",
            CloseButtonText = "Cancel",
            Title = "Check In",
            Content = checkInDialog,
            IsPrimaryButtonEnabled = true,
            IsSecondaryButtonEnabled = false
        };

        var result = await cd.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var selectedCheckout = ActiveCheckouts!.FirstOrDefault(c => c.BookId == SelectedCheckInBook.BookId);
            var checkInDto = new CheckInDto
            {
                CheckoutId = selectedCheckout!.CheckoutId,
                ReturnDate = DateTime.UtcNow
            };
            await _apiService.AddCheckInAsync(checkInDto);
        }
    }
    
}