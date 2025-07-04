﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BIMIssueManagerMarkupsEditor.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b)
                ? Brushes.Green
                : Brushes.OrangeRed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
