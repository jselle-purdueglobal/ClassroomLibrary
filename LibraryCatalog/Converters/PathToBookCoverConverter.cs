using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace LibraryCatalog.Converters;

public class PathToBookCoverConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string path && !string.IsNullOrEmpty(path))
        {
            return new Bitmap(AssetLoader.Open(new Uri(path)));
        }
        
        return new Bitmap(AssetLoader.Open(new Uri("avares://LibraryCatalog/Assets/no_cover.png")));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}