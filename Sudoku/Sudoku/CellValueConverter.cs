using System;
using System.Globalization;
using System.Windows.Data;

namespace Sudoku
{
    class CellValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toReturn = value.ToString();
            if (toReturn == "0")
            {
                toReturn = " ";
            }
            return toReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
