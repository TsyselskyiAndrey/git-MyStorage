using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
namespace MyStorage_v02
{
    class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long bytes = System.Convert.ToInt64(value);
            if (bytes > 1024)
            {
                double kb = bytes / 1024;
                if (kb > 1024)
                {
                    double mb = kb / 1024;
                    if (mb > 1024)
                    {
                        double gb = mb / 1024;
                        if (gb > 1024)
                        {
                            double tb = gb / 1024;
                            return Math.Round(tb, 2) + " TB";
                        }
                        else
                        {
                            return Math.Round(gb, 2) + " GB";
                        }
                    }
                    else
                    {
                        return Math.Round(mb, 2) + " MB";
                    }
                }
                else
                {
                    return Math.Round(kb, 2) + " KB";
                }
            }
            else
            {
                
                return Math.Round(System.Convert.ToDouble(bytes), 2) + " B";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
