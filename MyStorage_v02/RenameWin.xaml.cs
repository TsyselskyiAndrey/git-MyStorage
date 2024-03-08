using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for RenameWin.xaml
    /// </summary>
    public partial class RenameWin : Window
    {
        public Storage Storage { get; set; }
        public RenameWin(Storage storage)
        {
            InitializeComponent();
            Storage = storage;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Plg.Points.Clear();
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.411, 0));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.173, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.465, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.42, 0));
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbCode.Text) || String.IsNullOrEmpty(tbCode.Text))
            {
                MessageBox.Show("Enter something!");
            }
            else
            {
                Storage.NewFileName = tbCode.Text;
                this.Close();
            }
            
        }
    }
}
