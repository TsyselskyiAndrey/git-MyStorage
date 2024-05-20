using Azure.Storage.Blobs;
using MyStorage_v02.Models;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for MyFile.xaml
    /// </summary>
    public partial class MyFile : UserControl
    {
        private Storage Storage { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime RemoveDate { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsRemoved { get; set; }

        public MyFile(string title, string type, long size, DateTime addDate, DateTime removeDate, bool isFavorite, bool isRemoved, Storage storage)
        {
            InitializeComponent();
            this.DataContext = this;
            Storage = storage;
            Title = title;
            Type = type;
            Size = size;
            AddDate = addDate;
            RemoveDate = removeDate;
            IsFavorite = isFavorite;
            IsRemoved = isRemoved;
            if (IsFavorite == true)
            {
                star.Source = new BitmapImage(new Uri("Images/star4.png", UriKind.Relative));
                adfav.Header = " Remove from favorites";
            }
            else
            {
                star.Source = new BitmapImage(new Uri("Images/star5.png", UriKind.Relative));
                adfav.Header = " Add to favorites";
            }
            if (IsRemoved == true)
            {
                restore.Visibility = Visibility.Visible;
                download.Visibility = Visibility.Collapsed;
                star.Visibility = Visibility.Collapsed;

                dl.Visibility = Visibility.Collapsed;
                ren.Visibility = Visibility.Collapsed;
                adfav.Visibility = Visibility.Collapsed;
                res.Visibility = Visibility.Visible;

            }
            else
            {
                restore.Visibility = Visibility.Collapsed;
                download.Visibility = Visibility.Visible;
                star.Visibility = Visibility.Visible;

                dl.Visibility = Visibility.Visible;
                ren.Visibility = Visibility.Visible;
                adfav.Visibility = Visibility.Visible;
                res.Visibility = Visibility.Collapsed;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            border.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.15)
            });
            border.BeginAnimation(Canvas.TopProperty, new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.15)
            });
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            border.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation()
            {
                To = 8,
                Duration = TimeSpan.FromSeconds(0.15)
            });
            border.BeginAnimation(Canvas.TopProperty, new DoubleAnimation()
            {
                To = 8,
                Duration = TimeSpan.FromSeconds(0.15)
            });
        }

        private void star_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsFavorite == true)
            {
                star.Source = new BitmapImage(new Uri("Images/star3.png", UriKind.Relative));
                IsFavorite = false;
                Task task = new Task(() => Storage.Favorite(this, "minus"));
                task.Start();
                adfav.Header = " Add to favorites";
            }
            else
            {
                star.Source = new BitmapImage(new Uri("Images/star4.png", UriKind.Relative));
                IsFavorite = true;
                Task task = new Task(() => Storage.Favorite(this, "plus"));
                task.Start();
                adfav.Header = " Remove from favorites";
            }
        }

        private void star_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsFavorite != true)
            {
                star.Source = new BitmapImage(new Uri("Images/star3.png", UriKind.Relative));
            }
        }

        private void star_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsFavorite == true)
            {
                star.Source = new BitmapImage(new Uri("Images/star4.png", UriKind.Relative));
            }
            else
            {
                star.Source = new BitmapImage(new Uri("Images/star5.png", UriKind.Relative));
            }
        }

        private void download_MouseLeave(object sender, MouseEventArgs e)
        {
            download.Source = new BitmapImage(new Uri("Images/download1.png", UriKind.Relative));
        }

        private void download_MouseEnter(object sender, MouseEventArgs e)
        {
            download.Source = new BitmapImage(new Uri("Images/download2.png", UriKind.Relative));
        }

        private void download_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Storage.myProgressBarDownload == null)
            {
                Task task = new Task(() => Storage.Downloading(this));
                task.ContinueWith(Storage.DownloadingEnd);
                task.Start();
            }
            else
            {
                MessageBox.Show("Wait for the previous files!");
            }
        }

        private void trash_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Task task = new Task(() => Storage.Deleting(this));
            task.Start();

        }

        private void trash_MouseLeave(object sender, MouseEventArgs e)
        {
            trash.Source = new BitmapImage(new Uri("Images/trashbox1.png", UriKind.Relative));
        }

        private void trash_MouseEnter(object sender, MouseEventArgs e)
        {
            trash.Source = new BitmapImage(new Uri("Images/trashbox2.png", UriKind.Relative));
        }

        private void restore_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Task task = new Task(() => Storage.Restoring(this));
            task.ContinueWith(Storage.RestoringEnd);
            task.Start();
        }

        private void restore_MouseEnter(object sender, MouseEventArgs e)
        {
            restore.Source = new BitmapImage(new Uri("Images/restore2.png", UriKind.Relative));
        }

        private void restore_MouseLeave(object sender, MouseEventArgs e)
        {
            restore.Source = new BitmapImage(new Uri("Images/restore1.png", UriKind.Relative));
        }

        private void dl_Click(object sender, RoutedEventArgs e)
        {
            if (Storage.myProgressBarDownload == null)
            {
                Task task = new Task(() => Storage.Downloading(this));
                task.ContinueWith(Storage.DownloadingEnd);
                task.Start();
            }
            else
            {
                MessageBox.Show("Wait for the previous files!");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(() => Storage.Deleting(this));
            task.Start();
        }

        private void res_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(() => Storage.Restoring(this));
            task.ContinueWith(Storage.RestoringEnd);
            task.Start();
        }

        private void ren_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(() => Storage.Renaming(this));
            task.Start();
        }

        private void adfav_Click(object sender, RoutedEventArgs e)
        {
            if (IsFavorite == true)
            {
                star.Source = new BitmapImage(new Uri("Images/star3.png", UriKind.Relative));
                IsFavorite = false;
                Task task = new Task(() => Storage.Favorite(this, "minus"));
                task.Start();
                adfav.Header = " Add to favorites";
            }
            else
            {
                star.Source = new BitmapImage(new Uri("Images/star4.png", UriKind.Relative));
                IsFavorite = true;
                Task task = new Task(() => Storage.Favorite(this, "plus"));
                task.Start();
                adfav.Header = " Remove from favorites";
            }
        }

    }
}
