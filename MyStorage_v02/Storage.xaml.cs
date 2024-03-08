using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MyStorage_v02.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for Storage.xaml
    /// </summary>
    public partial class Storage : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static bool key = false;
        private static bool flag = true;

        public Task MyTask { get; set; }
        public Task MyTask2 { get; set; }
        public Task MyTask3 { get; set; }
        public Task MyTask4 { get; set; }
        public List<MyFile> MyFiles { get; set; }
        public List<MyFile> MyFiles2 { get; set; }
        public List<MyFile> MyFiles3 { get; set; }
        public List<MyFile> MyFiles4 { get; set; }

        public CancellationTokenSource Source { get; set; }
        public CancellationToken Token { get; set; }

        public ProcessControl myProgressBarUpload { get; set; }
        public ProcessControl myProgressBarDownload { get; set; }

        public string NewFileName { get; set; } = "";

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private long _memory;

        public long Memory
        {
            get { return _memory; }
            set { _memory = value; OnPropertyChanged(); }
        }

        private long _maxMemory;

        public long MaxMemory
        {
            get { return _maxMemory; }
            set { _maxMemory = value; OnPropertyChanged(); }
        }

        private BlobServiceClient blobServiceClient { get; set; }
        private BlobContainerClient containerClient { get; set; }
        private Users User { get; set; }
        public long uploadFileSize { get; private set; }

        private static DoubleAnimation da = new DoubleAnimation
        {
            To = 200,
            Duration = TimeSpan.FromSeconds(0.25)
        };
        private static DoubleAnimation da2 = new DoubleAnimation
        {
            To = 50,
            Duration = TimeSpan.FromSeconds(0.25)
        };
        private static DoubleAnimation da3 = new DoubleAnimation
        {
            To = 1,
            Duration = TimeSpan.FromSeconds(0.25)
        };
        private static DoubleAnimation da4 = new DoubleAnimation
        {
            To = 0,
            Duration = TimeSpan.FromSeconds(0.25)
        };

        

        public Storage(Users user)
        {
            InitializeComponent();
            blobServiceClient = new BlobServiceClient(ConfigurationManager.AppSettings.Get("StorageConnectionString"));
            containerClient = blobServiceClient.GetBlobContainerClient(ConfigurationManager.AppSettings.Get("ContainerName"));
            User = user;
            this.DataContext = this;
            MaxMemory = User.MaxSize;
            Source = new CancellationTokenSource();
            Token = Source.Token;
            MyFiles = new List<MyFile>();
            MyFiles2 = new List<MyFile>();
            MyFiles3 = new List<MyFile>();
            MyFiles4 = new List<MyFile>();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized && flag == true)
            {
                this.WindowState = WindowState.Normal;
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowState = WindowState.Maximized;
                this.ResizeMode = ResizeMode.CanResize;
                window.BorderThickness = new Thickness(0);
                chrome.ResizeBorderThickness = new Thickness(0);
                flag = false;
                
            }
            else
            {
                flag = true;
            }
            Plg.Points.Clear();
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.411, 0));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.173, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.465, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.42, 0));
            sv.Width = ToolBar.ActualWidth - 235;
            sv2.Width = ToolBar.ActualWidth - 235;
            sv3.Width = ToolBar.ActualWidth - 235;
            sv4.Width = ToolBar.ActualWidth - 235;
            sv5.Width = ToolBar.ActualWidth - 235;
        }



        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void square_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                window.BorderThickness = new Thickness(2);
                chrome.ResizeBorderThickness = new Thickness(2);
            }
            else
            {
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowState = WindowState.Maximized;
                this.ResizeMode = ResizeMode.CanResize;
            }
        }

        private void cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window w = new ChooseForm(User);
            w.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.CanResize;

           
        }

        private void LoadingRecentEnd(Task obj)
        {
            Dispatcher.Invoke(() => tbSearch2.Text = "");
            Dispatcher.Invoke(() => wp5.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => svSearch2.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => load5.Visibility = Visibility.Collapsed);
            Task loadmemory = new Task(LoadMemory);
            loadmemory.Start();
        }


        private void LoadingRecent()
        {
            Dispatcher.Invoke(() => wp5.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => svSearch2.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => load5.Visibility = Visibility.Visible);
            Thread thread = null;
            Task t = Task.Run(() =>
            {
                thread = Thread.CurrentThread;
                while (true)
                {
                    double a = 0;
                    Dispatcher.Invoke(() => {
                        a = this.window.ActualHeight / 2 - 20 - load5.ActualHeight / 2;
                        load5.Margin = new Thickness(-100, a, 0, 0);
                    });
                }
            });
            try
            {
                using (MyStorageEntities db = new MyStorageEntities())
                {
                    var files = db.Files.Where(x => x.UserId == User.id && x.IsRemoved == false).ToList<Files>();
                    Dispatcher.Invoke(() => wp5.Children.Clear());
                    Dispatcher.Invoke(() => MyFiles2.Clear());
                    foreach (var item in files)
                    {
                        if (item.DateOfAddition.AddHours(24) >= DateTime.Now)
                        {
                            Dispatcher.Invoke(() => wp5.Children.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                            Dispatcher.Invoke(() => MyFiles2.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Thread.Sleep(1000);
            if (thread != null)
            {
                thread.Abort();
            }
        }

        private void LoadingFavoriteEnd(Task obj)
        {
            Dispatcher.Invoke(() => tbSearch3.Text = "");
            Dispatcher.Invoke(() => wp4.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => svSearch3.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => load4.Visibility = Visibility.Collapsed);
            Task loadmemory = new Task(LoadMemory);
            loadmemory.Start();
        }

        private void LoadingFavorite()
        {
            Dispatcher.Invoke(() => wp4.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => svSearch3.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => load4.Visibility = Visibility.Visible);
            Thread thread = null;
            Task t = Task.Run(() =>
            {
                thread = Thread.CurrentThread;
                while (true)
                {
                    double a = 0;
                    Dispatcher.Invoke(() => {
                        a = this.window.ActualHeight / 2 - 20 - load4.ActualHeight / 2;
                        load4.Margin = new Thickness(-100, a, 0, 0);
                    });
                }
            });
            try
            {
                using (MyStorageEntities db = new MyStorageEntities())
                {
                    var files = db.Files.Where(x => x.UserId == User.id && x.IsFavorite == true && x.IsRemoved == false).ToList<Files>();
                    Dispatcher.Invoke(() => wp4.Children.Clear());
                    Dispatcher.Invoke(() => MyFiles3.Clear());
                    foreach (var item in files)
                    {
                        Dispatcher.Invoke(() => wp4.Children.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                        Dispatcher.Invoke(() => MyFiles3.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Thread.Sleep(1000);
            if (thread != null)
            {
                thread.Abort();
            }
        }


        private void LoadingTrashEnd(Task obj)
        {
            Dispatcher.Invoke(() => tbSearch4.Text = "");
            Dispatcher.Invoke(() => wp3.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => svSearch4.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => load3.Visibility = Visibility.Collapsed);
        }

        private void LoadingTrash()
        {
            Dispatcher.Invoke(() => wp3.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => svSearch4.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => load3.Visibility = Visibility.Visible);
            Thread thread = null;
            Task t = Task.Run(() =>
            {
                thread = Thread.CurrentThread;
                while (true)
                {
                    double a = 0;
                    Dispatcher.Invoke(() => {
                        a = this.window.ActualHeight / 2 - 20 - load3.ActualHeight / 2;
                        load3.Margin = new Thickness(-100, a, 0, 0);
                    });
                }
            });
            try
            {
                using (MyStorageEntities db = new MyStorageEntities())
                {
                    var files0 = db.Files.Where(x => x.UserId == User.id && x.IsRemoved == true).ToList<Files>();
                    foreach (var item in files0)
                    {
                        if (item.DateOfRemoval.AddDays(7) <= DateTime.Now)
                        {
                            db.Files.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    var files = db.Files.Where(x => x.UserId == User.id && x.IsRemoved == true).ToList<Files>();
                    Dispatcher.Invoke(() => wp3.Children.Clear());
                    Dispatcher.Invoke(() => MyFiles4.Clear());
                    foreach (var item in files)
                    {
                        Dispatcher.Invoke(() => wp3.Children.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                        Dispatcher.Invoke(() => MyFiles4.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Thread.Sleep(1000);
            if (thread != null)
            {
                thread.Abort();
            }
        }

        private void LoadingEnd(Task obj)
        {
            Dispatcher.Invoke(() => tbSearch1.Text = "");
            Dispatcher.Invoke(() => wp.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => svSearch1.Visibility = Visibility.Visible);
            Dispatcher.Invoke(() => load.Visibility = Visibility.Collapsed);
        }

        private void Loading()
        {
            Dispatcher.Invoke(() => wp.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => svSearch1.Visibility = Visibility.Collapsed);
            Dispatcher.Invoke(() => load.Visibility = Visibility.Visible);
            Thread thread = null;
            Task t = Task.Run(() =>
            {
                thread = Thread.CurrentThread;
                while (true)
                {
                    double a = 0;
                    Dispatcher.Invoke(() => {
                        a = this.window.ActualHeight / 2 - 20 - load.ActualHeight / 2;
                        load.Margin = new Thickness(-100, a, 0, 0);
                    });
                }
            });
            try
            {
                using (MyStorageEntities db = new MyStorageEntities())
                {
                    var files = db.Files.Where(x => x.UserId == User.id && x.IsRemoved == false).ToList<Files>();
                    Dispatcher.Invoke(() => wp.Children.Clear());
                    Dispatcher.Invoke(() => MyFiles.Clear());
                    Memory = 0;
                    foreach (var item in files)
                    {
                        Dispatcher.Invoke(() => Memory += item.SizeBytes);
                        Dispatcher.Invoke(() => MyFiles.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                        Dispatcher.Invoke(() => wp.Children.Add(new MyFile(item.Name, item.Type, item.SizeBytes, item.DateOfAddition, item.DateOfRemoval, item.IsFavorite, item.IsRemoved, this)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Thread.Sleep(1000);
            if (thread != null)
            {
                thread.Abort();
            }
        }

        private void TabMouseEnter(object sender, MouseEventArgs e)
        {
            ti1_txt.BeginAnimation(OpacityProperty, da3);
            ti2_txt.BeginAnimation(OpacityProperty, da3);
            ti3_txt.BeginAnimation(OpacityProperty, da3);
            ti4_txt.BeginAnimation(OpacityProperty, da3);
            ti5_txt.BeginAnimation(OpacityProperty, da3);
            ti7_txt.BeginAnimation(OpacityProperty, da3);
            addBtn.BeginAnimation(OpacityProperty, da3);
            progress.BeginAnimation(OpacityProperty, da3);
            tbSize.BeginAnimation(OpacityProperty, da3);

            ti1.BeginAnimation(WidthProperty, da);
            ti2.BeginAnimation(WidthProperty, da);
            ti3.BeginAnimation(WidthProperty, da);
            ti4.BeginAnimation(WidthProperty, da);
            ti5.BeginAnimation(WidthProperty, da);
            ti6.BeginAnimation(WidthProperty, da);
            ti7.BeginAnimation(WidthProperty, da);
            canvas.BeginAnimation(WidthProperty, da);
        }

        private void TabMouseLeave(object sender, MouseEventArgs e)
        {
            ti1_txt.BeginAnimation(OpacityProperty, da4);
            ti2_txt.BeginAnimation(OpacityProperty, da4);
            ti3_txt.BeginAnimation(OpacityProperty, da4);
            ti4_txt.BeginAnimation(OpacityProperty, da4);
            ti5_txt.BeginAnimation(OpacityProperty, da4);
            ti7_txt.BeginAnimation(OpacityProperty, da4);
            addBtn.BeginAnimation(OpacityProperty, da4);
            progress.BeginAnimation(OpacityProperty, da4);
            tbSize.BeginAnimation(OpacityProperty, da4);

            ti1.BeginAnimation(WidthProperty, da2);
            ti2.BeginAnimation(WidthProperty, da2);
            ti3.BeginAnimation(WidthProperty, da2);
            ti4.BeginAnimation(WidthProperty, da2);
            ti5.BeginAnimation(WidthProperty, da2);
            ti6.BeginAnimation(WidthProperty, da2);
            ti7.BeginAnimation(WidthProperty, da2);
            canvas.BeginAnimation(WidthProperty, da2);
        }

        private void CPMouseEnter(object sender, MouseEventArgs e)
        {
            da.To = 50;
            da3.To = 0;
            ti1_txt.BeginAnimation(OpacityProperty, da3);
            ti2_txt.BeginAnimation(OpacityProperty, da3);
            ti3_txt.BeginAnimation(OpacityProperty, da3);
            ti4_txt.BeginAnimation(OpacityProperty, da3);
            ti5_txt.BeginAnimation(OpacityProperty, da3);
            ti7_txt.BeginAnimation(OpacityProperty, da3);
            addBtn.BeginAnimation(OpacityProperty, da3);
            progress.BeginAnimation(OpacityProperty, da3);
            tbSize.BeginAnimation(OpacityProperty, da3);

            ti1.BeginAnimation(WidthProperty, da);
            ti2.BeginAnimation(WidthProperty, da);
            ti3.BeginAnimation(WidthProperty, da);
            ti4.BeginAnimation(WidthProperty, da);
            ti5.BeginAnimation(WidthProperty, da);
            ti6.BeginAnimation(WidthProperty, da);
            ti7.BeginAnimation(WidthProperty, da);
            canvas.BeginAnimation(WidthProperty, da);
        }

        private void CPMouseLeave(object sender, MouseEventArgs e)
        {
            da.To = 200;
            da3.To = 1;
            ti1_txt.BeginAnimation(OpacityProperty, da3);
            ti2_txt.BeginAnimation(OpacityProperty, da3);
            ti3_txt.BeginAnimation(OpacityProperty, da3);
            ti4_txt.BeginAnimation(OpacityProperty, da3);
            ti5_txt.BeginAnimation(OpacityProperty, da3);
            ti7_txt.BeginAnimation(OpacityProperty, da3);
            addBtn.BeginAnimation(OpacityProperty, da3);
            progress.BeginAnimation(OpacityProperty, da3);
            tbSize.BeginAnimation(OpacityProperty, da3);

            ti1.BeginAnimation(WidthProperty, da);
            ti2.BeginAnimation(WidthProperty, da);
            ti3.BeginAnimation(WidthProperty, da);
            ti4.BeginAnimation(WidthProperty, da);
            ti5.BeginAnimation(WidthProperty, da);
            ti6.BeginAnimation(WidthProperty, da);
            ti7.BeginAnimation(WidthProperty, da);
            canvas.BeginAnimation(WidthProperty, da);
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if(key == false)
            {
                Window w = new ChooseForm(User);
                w.Show();
                this.Close();
                key = true;
            }
            else
            {
                key = false;
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                sv.Opacity = 0.15;
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            
        }

        private void wp_Drop(object sender, DragEventArgs e)
        {
            sv.Opacity = 1;
            MessageBox.Show("Success!");
        }

        private void sv_PreviewDragLeave(object sender, DragEventArgs e)
        {
            sv.Opacity = 1;
        }

        private void addBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            img7.Opacity = 1;

            RotateTransform rtf = new RotateTransform();
            img7.RenderTransform = rtf;
            img7.RenderTransformOrigin = new Point(0.5, 0.5);
            img6.RenderTransform = rtf;
            img6.RenderTransformOrigin = new Point(0.5, 0.5);
            DoubleAnimation da = new DoubleAnimation();
            da.To = 180;
            da.Duration = TimeSpan.FromMilliseconds(300);
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(da);
            Storyboard.SetTarget(da, img7);
            Storyboard.SetTarget(da, img6);
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Angle"));
            storyboard.Begin();
        }

        private void addBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            img7.Opacity = 0.4;

            RotateTransform rtf = new RotateTransform();
            img7.RenderTransform = rtf;
            img7.RenderTransformOrigin = new Point(0.5, 0.5);
            img6.RenderTransform = rtf;
            img6.RenderTransformOrigin = new Point(0.5, 0.5);
            DoubleAnimation da = new DoubleAnimation();
            da.To = -180;
            da.Duration = TimeSpan.FromMilliseconds(300);
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(da);
            Storyboard.SetTarget(da, img7);
            Storyboard.SetTarget(da, img6);
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Angle"));
            storyboard.Begin();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if(myProgressBarUpload == null)
            {
                Task task = new Task(Uploading, Token);
                task.ContinueWith(UploadingEnd);
                task.Start();
            }
            else
            {
                MessageBox.Show("Wait the for previous files!");
            }
            
        }

        private void UploadingEnd(Task obj)
        {
            if(myProgressBarDownload == null)
            {
                ImageAnimationController controller;
                Dispatcher.Invoke(() => { controller = ImageBehavior.GetAnimationController(this.DownloadGif); this.DownloadGif.Visibility = Visibility.Collapsed; this.DownloadImg.Visibility = Visibility.Visible; controller.Pause(); });
            }

        }

        private void Uploading()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            ImageAnimationController controller;
            Dispatcher.Invoke(() => { controller = ImageBehavior.GetAnimationController(this.DownloadGif); this.DownloadGif.Visibility = Visibility.Visible; this.DownloadImg.Visibility = Visibility.Collapsed; controller.Play(); });
            
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var item in openFileDialog.FileNames)
                {
                    FileInfo file = new FileInfo(item);
                    uploadFileSize = file.Length;
                    if (uploadFileSize > 0)
                    {
                        if (uploadFileSize + Memory <= MaxMemory)
                        {
                            var progressHandler = new Progress<long>();
                            progressHandler.ProgressChanged += UploadProgressChanged;
                            BlobClient blobClient = containerClient.GetBlobClient(User.Email + "/" + System.IO.Path.GetFileName(item));

                            int id = -1;
                            string myFileName = "";
                            Dispatcher.Invoke(() => id = User.id);
                            Dispatcher.Invoke(() => myFileName = System.IO.Path.GetFileName(item));
                            Files checkfile;
                            using (MyStorageEntities db = new MyStorageEntities())
                            {
                                checkfile = db.Files.Where(x => x.UserId == id).Where(y => y.Name == myFileName).FirstOrDefault();
                            }
                            try
                            {
                                if (blobClient.Exists() || checkfile != null)
                                {
                                    MessageBox.Show($"A file with this name ({System.IO.Path.GetFileName(item)}) already exists (it may be in your bin)! Rename it and try again");
                                }
                                else
                                {
                                    Dispatcher.Invoke(() => {
                                        myProgressBarUpload = new ProcessControl(file.Name, 0, file.Length, 0, DateTime.Now, "Upload");
                                        spHistory.Children.Add(myProgressBarUpload);
                                    });


                                    try
                                    {
                                        blobClient.Upload(item, progressHandler: progressHandler, cancellationToken: Token);
                                        using (MyStorageEntities db = new MyStorageEntities())
                                        {
                                            var newfile = new Files { Name = file.Name, IsFavorite = false, IsRemoved = false, SizeBytes = file.Length, DateOfAddition = DateTime.Now, DateOfRemoval = DateTime.Now, Type = file.Extension, UserId = db.Users.Where(x => x.Email == User.Email).FirstOrDefault().id };
                                            db.Files.Add(newfile);
                                            db.SaveChanges();
                                        }
                                    }
                                    catch (Exception)
                                    { }


                                    myProgressBarUpload = null;
                                }
                                Dispatcher.Invoke(() => {
                                    if (tab.SelectedIndex == 0)
                                    {
                                        Task load = new Task(Loading);
                                        load.ContinueWith(LoadingEnd);
                                        load.Start();
                                    }
                                    else if (tab.SelectedIndex == 1)
                                    {
                                        Task load4 = new Task(LoadingRecent);
                                        load4.ContinueWith(LoadingRecentEnd);
                                        load4.Start();
                                    }

                                });
                            }
                            catch (Azure.RequestFailedException ex)
                            {
                                if (ex.ErrorCode == "BlobAlreadyExists")
                                {
                                    MessageBox.Show($"A file with this name ({System.IO.Path.GetFileName(item)}) already exists! Rename it and try again");
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show($"You have no memory for loading {System.IO.Path.GetFileName(item)}. Delete something or buy some extra space!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The file is too small!");
                    }
                    
                }
                
            }
        }
        public void Deleting(MyFile myFile)
        {
            BlobClient blobClient = containerClient.GetBlobClient(User.Email + "/" + myFile.Title);
            try
            {
                if (blobClient.Exists())
                {
                    
                    blobClient.Delete(snapshotsOption: DeleteSnapshotsOption.IncludeSnapshots);

                    int id = -1;
                    string myFileName = "";
                    Dispatcher.Invoke(() => id = User.id);
                    Dispatcher.Invoke(() => myFileName = myFile.Title);
                    using (MyStorageEntities db = new MyStorageEntities())
                    {
                        var delFile = db.Files.Where(x => x.UserId == id).Where(y => y.Name == myFileName).FirstOrDefault();
                        delFile.IsRemoved = true;
                        delFile.DateOfRemoval = DateTime.Now;
                        db.SaveChanges();
                    }

                    Dispatcher.Invoke(() => {
                        if (tab.SelectedIndex == 0)
                        {
                            Task load = new Task(Loading);
                            load.ContinueWith(LoadingEnd);
                            load.Start();
                        }
                        else if (tab.SelectedIndex == 2)
                        {
                            Task load3 = new Task(LoadingFavorite);
                            load3.ContinueWith(LoadingFavoriteEnd);
                            load3.Start();
                        }
                        else if (tab.SelectedIndex == 1)
                        {
                            Task load4 = new Task(LoadingRecent);
                            load4.ContinueWith(LoadingRecentEnd);
                            load4.Start();
                        }
                        
                    });
                    
                }
                else
                {
                    //MessageBox.Show($"A file with this name ({myFile.Title}) doesn't exist!");
                    int id = -1;
                    string myFileName = "";
                    Dispatcher.Invoke(() => id = User.id);
                    Dispatcher.Invoke(() => myFileName = myFile.Title);
                    using (MyStorageEntities db = new MyStorageEntities())
                    {
                        var delFile = db.Files.Where(x => x.UserId == id).Where(y => y.Name == myFileName).FirstOrDefault();
                        db.Files.Remove(delFile);
                        db.SaveChanges();
                    }
                    Task load2 = new Task(LoadingTrash);
                    load2.ContinueWith(LoadingTrashEnd);
                    load2.Start();
                }

            }
            catch (Azure.RequestFailedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            
        }

        private void LoadMemory()
        {
            using (MyStorageEntities db = new MyStorageEntities())
            {
                var files = db.Files.Where(x => x.UserId == User.id && x.IsRemoved == false).ToList<Files>();
                Memory = 0;
                foreach (var item in files)
                {
                    Dispatcher.Invoke(() => Memory += item.SizeBytes);
                }
            }
        }

        public void Downloading(MyFile myFile)
        {
            ImageAnimationController controller;
            Dispatcher.Invoke(() => { controller = ImageBehavior.GetAnimationController(this.DownloadGif); this.DownloadGif.Visibility = Visibility.Visible; this.DownloadImg.Visibility = Visibility.Collapsed; controller.Play(); });
            var blobClient = containerClient.GetBlobClient(User.Email + "/" + myFile.Title).Download().Value;
            try
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = @"C:\";
                dialog.IsFolderPicker = true;
                if (Dispatcher.Invoke(new Func<bool>(()=> dialog.ShowDialog() == CommonFileDialogResult.Ok)))
                {
                    var outputFile = File.OpenWrite(@""+dialog.FileName + @"\" + myFile.Title);
                    var downloadBuffer = new byte[81920];
                    int bytesRead;
                    int totalBytesDownloaded = 0;
                    Dispatcher.Invoke(() => {
                        myProgressBarDownload = new ProcessControl(myFile.Title, 0, myFile.Size, 0, DateTime.Now, "Download");
                        spHistory.Children.Add(myProgressBarDownload);
                    });
                    while ((bytesRead = blobClient.Content.Read(downloadBuffer, 0, downloadBuffer.Length)) != 0) 
                    {
                        outputFile.Write(downloadBuffer, 0, bytesRead);
                        totalBytesDownloaded += bytesRead;

                        myProgressBarDownload.Value = Convert.ToInt32(GetProgressPercentage(blobClient.ContentLength, totalBytesDownloaded));
                        myProgressBarDownload.Size = totalBytesDownloaded;
                    }
                    blobClient.Content.Close();
                    outputFile.Close();
                    myProgressBarDownload = null;
                }

            }
            catch (Azure.RequestFailedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void DownloadingEnd(Task obj)
        {
            if(myProgressBarUpload == null)
            {
                ImageAnimationController controller;
                Dispatcher.Invoke(() => { controller = ImageBehavior.GetAnimationController(this.DownloadGif); this.DownloadGif.Visibility = Visibility.Collapsed; this.DownloadImg.Visibility = Visibility.Visible; controller.Pause(); });
            }

        }

        public void Restoring(MyFile myFile)
        {
            var blobClient = containerClient.GetBlobClient(User.Email + "/" + myFile.Title);
            try
            {
                if (myFile.RemoveDate.AddDays(7) > DateTime.Now)
                {
                    if(Memory + myFile.Size <= MaxMemory)
                    {
                        blobClient.Undelete();
                        int id = -1;
                        string myFileName = "";
                        Dispatcher.Invoke(() => id = User.id);
                        Dispatcher.Invoke(() => myFileName = myFile.Title);
                        using (MyStorageEntities db = new MyStorageEntities())
                        {
                            var delFile = db.Files.Where(x => x.UserId == id).Where(y => y.Name == myFileName).FirstOrDefault();
                            delFile.IsRemoved = false;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        MessageBox.Show("You can't restore this file! You don't have enough memory!");
                    }
                }
                else
                {
                    MessageBox.Show("You can't restore this file! It has expired!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Task load2 = new Task(LoadingTrash);
            load2.ContinueWith(LoadingTrashEnd);
            load2.Start();
            
        }

        public void RestoringEnd(Task obj)
        {
            Task loadmemory = new Task(LoadMemory);
            loadmemory.Start();
        }

        public void Favorite(MyFile myFile, string mode)
        {
            if(mode == "plus")
            {
                try
                {
                    int id = -1;
                    string myFileName = "";
                    Dispatcher.Invoke(() => id = User.id);
                    Dispatcher.Invoke(() => myFileName = myFile.Title);
                    using (MyStorageEntities db = new MyStorageEntities())
                    {
                        var delFile = db.Files.Where(x => x.UserId == id).Where(y => y.Name == myFileName).FirstOrDefault();
                        delFile.IsFavorite = true;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            else
            {
                try
                {
                    int id = -1;
                    string myFileName = "";
                    Dispatcher.Invoke(() => id = User.id);
                    Dispatcher.Invoke(() => myFileName = myFile.Title);
                    using (MyStorageEntities db = new MyStorageEntities())
                    {
                        var delFile = db.Files.Where(x => x.UserId == id).Where(y => y.Name == myFileName).FirstOrDefault();
                        delFile.IsFavorite = false;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Task load3 = new Task(LoadingFavorite);
                load3.ContinueWith(LoadingFavoriteEnd);
                load3.Start();
            }
            
        }


        public void Renaming(MyFile myFile)
        {
            Dispatcher.Invoke(() => {
                var window = new RenameWin(this);
                window.Owner = this;
                window.ShowDialog();
            });

            if (NewFileName != "")
            {
                try
                {
                    var oldBlob = containerClient.GetBlobClient(User.Email + "/" + myFile.Title);
                    var newBlob = containerClient.GetBlobClient(User.Email + "/" + NewFileName + myFile.Type);
                    newBlob.StartCopyFromUri(oldBlob.Uri);
                    oldBlob.Delete(snapshotsOption: DeleteSnapshotsOption.IncludeSnapshots);
                    int id = -1;
                    string myFileName = "";
                    Dispatcher.Invoke(() => id = User.id);
                    Dispatcher.Invoke(() => myFileName = myFile.Title);
                    using (MyStorageEntities db = new MyStorageEntities())
                    {
                        var delFile = db.Files.Where(x => x.UserId == id).Where(y => y.Name == myFileName).FirstOrDefault();
                        delFile.Name = NewFileName + myFile.Type;
                        db.SaveChanges();
                    }
                    Dispatcher.Invoke(() => {
                        if (tab.SelectedIndex == 0)
                        {
                            Task load = new Task(Loading);
                            load.ContinueWith(LoadingEnd);
                            load.Start();
                        }
                        else if (tab.SelectedIndex == 2)
                        {
                            Task load3 = new Task(LoadingFavorite);
                            load3.ContinueWith(LoadingFavoriteEnd);
                            load3.Start();
                        }
                        else if (tab.SelectedIndex == 1)
                        {
                            Task load4 = new Task(LoadingRecent);
                            load4.ContinueWith(LoadingRecentEnd);
                            load4.Start();
                        }

                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                NewFileName = "";
            }
            
        }



        private double GetProgressPercentage(double totalSize, double currentSize)
        {
            return (currentSize / totalSize) * 100;
        }

        private void UploadProgressChanged(object sender, long bytesUploaded)
        {
            myProgressBarUpload.Value = Convert.ToInt32(GetProgressPercentage(uploadFileSize, bytesUploaded));
            myProgressBarUpload.Size = bytesUploaded;

        }

        private void window_Closed(object sender, EventArgs e)
        {
            Source.Cancel();
            Source.Dispose();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbSearch1.Text = "";
            tbSearch2.Text = "";
            tbSearch3.Text = "";
            tbSearch4.Text = "";
            if (tab.SelectedIndex == 0)
            {
                Task load = new Task(Loading);
                load.ContinueWith(LoadingEnd);
                load.Start();
            }
            else if(tab.SelectedIndex == 3)
            {
                Task load2 = new Task(LoadingTrash);
                load2.ContinueWith(LoadingTrashEnd);
                load2.Start();
            }
            else if (tab.SelectedIndex == 2)
            {
                Task load3 = new Task(LoadingFavorite);
                load3.ContinueWith(LoadingFavoriteEnd);
                load3.Start();
            }
            else if (tab.SelectedIndex == 1)
            {
                Task load4 = new Task(LoadingRecent);
                load4.ContinueWith(LoadingRecentEnd);
                load4.Start();
            }
        }


        private void LoadSearch()
        {
            Dispatcher.Invoke(() => {
                wp.Children.Clear();
            });
            string text = Dispatcher.Invoke(new Func<string>(() => tbSearch1.Text));
            if(String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text))
            {
                foreach (var item in MyFiles)
                {
                    Dispatcher.Invoke(() => wp.Children.Add(item));
                }
            }
            else
            {
                foreach (var item in MyFiles.Where(x => x.Title.ToLower().Contains(text.ToLower())).ToList<MyFile>())
                {
                    Dispatcher.Invoke(() => wp.Children.Add(item));
                }
            }
            
        }
        private void LoadSearch2()
        {
            Dispatcher.Invoke(() => {
                wp5.Children.Clear();
            });
            string text = Dispatcher.Invoke(new Func<string>(() => tbSearch2.Text));
            if (String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text))
            {
                foreach (var item in MyFiles2)
                {
                    Dispatcher.Invoke(() => wp5.Children.Add(item));
                }
            }
            else
            {
                foreach (var item in MyFiles2.Where(x => x.Title.ToLower().Contains(text.ToLower())).ToList<MyFile>())
                {
                    Dispatcher.Invoke(() => wp5.Children.Add(item));
                }
            }

        }

        private void LoadSearch3()
        {
            Dispatcher.Invoke(() => {
                wp4.Children.Clear();
            });
            string text = Dispatcher.Invoke(new Func<string>(() => tbSearch3.Text));
            if (String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text))
            {
                foreach (var item in MyFiles3)
                {
                    Dispatcher.Invoke(() => wp4.Children.Add(item));
                }
            }
            else
            {
                foreach (var item in MyFiles3.Where(x => x.Title.ToLower().Contains(text.ToLower())).ToList<MyFile>())
                {
                    Dispatcher.Invoke(() => wp4.Children.Add(item));
                }
            }

        }

        private void LoadSearch4()
        {
            Dispatcher.Invoke(() => {
                wp3.Children.Clear();
            });
            string text = Dispatcher.Invoke(new Func<string>(() => tbSearch4.Text));
            if (String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text))
            {
                foreach (var item in MyFiles4)
                {
                    Dispatcher.Invoke(() => wp3.Children.Add(item));
                }
            }
            else
            {
                foreach (var item in MyFiles4.Where(x => x.Title.ToLower().Contains(text.ToLower())).ToList<MyFile>())
                {
                    Dispatcher.Invoke(() => wp3.Children.Add(item));
                }
            }

        }

        private void tbSearch1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MyTask != null)
            {
                if (MyTask.Status == TaskStatus.Running)
                {
                    MyTask.Dispose();
                }
            }

            MyTask = new Task(LoadSearch);
            MyTask.Start();
        }

        private void tbSearch2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MyTask2 != null)
            {
                if (MyTask2.Status == TaskStatus.Running)
                {
                    MyTask2.Dispose();
                }
            }

            MyTask2 = new Task(LoadSearch2);
            MyTask2.Start();
        }

        private void tbSearch3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MyTask3 != null)
            {
                if (MyTask3.Status == TaskStatus.Running)
                {
                    MyTask3.Dispose();
                }
            }

            MyTask3 = new Task(LoadSearch3);
            MyTask3.Start();
        }

        private void tbSearch4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MyTask4 != null)
            {
                if (MyTask4.Status == TaskStatus.Running)
                {
                    MyTask4.Dispose();
                }
            }

            MyTask4 = new Task(LoadSearch4);
            MyTask4.Start();
        }
    }
}
