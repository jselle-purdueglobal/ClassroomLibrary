using System;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace LibraryCatalog.Models;

public class Book : ReactiveObject
{
    [Reactive] public int BookId { get; set; }
    [Reactive] public required string Title { get; set; }
    [Reactive] public required string Authors { get; set; }
    [Reactive] public string Illustrators { get; set; } = string.Empty;
    [Reactive] public string ImagePath { get; set; } = string.Empty;
}