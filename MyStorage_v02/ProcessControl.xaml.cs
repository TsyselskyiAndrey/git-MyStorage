using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for ProcessControl.xaml
    /// </summary>
    public partial class ProcessControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string Title { get; set; }

        private long _size;
        public long Size
        {
            get { return _size; }
            set { _size = value; OnPropertyChanged(); }
        }

        public long MaximumSize { get; set; }
        
        private int _value;
        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }
        public DateTime Date { get; set; }

        private string _mode;

        public string Mode
        {
            get { return _mode; }
            set { _mode = value; OnPropertyChanged(); }
        }

        public ProcessControl(string title, long size, long maximumSize, int value, DateTime date, string mode)
        {
            InitializeComponent();
            this.DataContext = this;
            Title = title;
            Value = value;
            MaximumSize = maximumSize;
            Size = size;
            Date = date;
            Mode = mode;
        }

        private void pb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(Value == 100)
            {
                gif.Visibility = Visibility.Collapsed;
                img.Visibility = Visibility.Visible;
            }
        }

    }
}
