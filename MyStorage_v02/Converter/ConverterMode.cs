using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyStorage_v02
{
    class ConverterMode : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string mode = System.Convert.ToString(value);
            if(mode == "Upload")
            {
                return "Images/uploadgif.gif";
            }
            else if(mode == "Download")
            {
                return "Images/downloadgif.gif";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
