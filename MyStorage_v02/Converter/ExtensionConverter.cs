using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyStorage_v02
{
    class ExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string extension = System.Convert.ToString(value).ToLower();
            if(extension == ".zip" || extension == ".rar" || extension == ".gz" || extension == ".7z" || extension == ".jar" || extension == ".war")
            {
                return "Images/arhivfile.png";
            }
            else if (extension == ".mp4" || extension == ".avi" || extension == ".mpg" || extension == ".wmv" || extension == ".mov" || extension == ".flv" || extension == ".wm" || extension == ".wmv" || extension == ".webm" || extension == ".mpeg")
            {
                return "Images/videofile.png";
            }
            else if (extension == ".mp3" || extension == ".aac" || extension == ".adt" || extension == ".adts" || extension == ".wav" || extension == ".wave" || extension == ".aif" || extension == ".wma" || extension == ".wm" || extension == ".ac3" || extension == ".amr" || extension == ".aud" || extension == ".flac" || extension == ".iff" || extension == ".m3u" || extension == ".m3u8" || extension == ".m4a" || extension == ".m4b" || extension == ".mid" || extension == ".mpa" || extension == ".ra" || extension == ".midi")
            {
                return "Images/musicfile.png";
            }
            else if (extension == ".odt" || extension == ".pdf" || extension == ".doc" || extension == ".docx" || extension == ".text" || extension == ".log" || extension == ".err" || extension == ".txt" || extension == ".dot" || extension == ".mobi" || extension == ".ppt" || extension == ".rtf")
            {
                return "Images/textfile.png";
            }
            else if (extension == ".ods" || extension == ".xlr" || extension == ".xls" || extension == ".xlsb" || extension == ".xlsm" || extension == ".xlsx" || extension == ".xml")
            {
                return "Images/tablefile.png";
            }
            else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".icon" || extension == ".ico" || extension == ".icns" || extension == ".pct" || extension == ".ttf" || extension == ".gif" || extension == ".tif" || extension == ".tiff" || extension == ".eps" || extension == ".raw" || extension == ".psd")
            {
                return "Images/imagefile.png";
            }
            else
            {
                return "Images/unknownfile.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
