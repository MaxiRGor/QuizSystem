using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Airport
{
    public class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var values = (IEnumerable)value;
                // cast property as an ObservableCollection<object>
                ObservableCollection<Category> collection = new ObservableCollection<Category>(values.OfType<Category>());
                int count = 0;
                foreach(var cat in collection)
                {
                    count += cat.Themes.Count;
                }
                return count;

            }
            catch
            {
               return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
