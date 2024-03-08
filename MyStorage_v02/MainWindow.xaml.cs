using MyStorage_v02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.IO;

namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string EMAIL { get; set; }
        private string PATH { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            PATH = "config.txt";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public MainWindow(double left, double top)
        {
            InitializeComponent();
            this.Left = left;
            this.Top = top;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(PATH))
            {
                try
                {
                    using (FileStream fs = new FileStream("config.txt", FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            EMAIL = sr.ReadLine();    
                        }
                    }
                    Task task = Task.Run(Load2);
                    task.ContinueWith(LoadEnd);
                }
                catch (Exception) 
                {
                    MessageBox.Show("Error!");
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Plg.Points.Clear();
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.411, 0));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.173, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.465, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.42, 0));
        }

        private void cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tbPass.Password.Length > 0)
            {
                Watermark.Visibility = Visibility.Collapsed;
            }
            else
            {
                Watermark.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window w = new SignUpForm(this.Left, this.Top);
            w.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (tbEmail.Text.Length != 0 && tbPass.Password.Length != 0)
            {
                if (IsEmailValid(tbEmail.Text.ToLower()))
                {
                    if (tbPass.Password.Length > 6)
                    {
                        txt_validation.Text = "";
                        Task task = Task.Run(Load);
                        task.ContinueWith(LoadEnd);
                    }
                    else
                    {
                        txt_validation.Text = "Password must contain more than 6 symbols";
                    }
                }
                else
                {
                    txt_validation.Text = "Email has been entered incorrectly";
                }
            }
            else
            {
                txt_validation.Text = "Fill in all the fields";
            }
        }

        public void Load()
        {
            Dispatcher.Invoke(() => {
                this.load_img.Visibility = Visibility.Visible;
            });
            
            Thread.Sleep(1000);
            try
            {
                using (MyStorageEntities db = new MyStorageEntities())
                {
                    var user = db.Users.AsEnumerable().Where(x => x.Email == Dispatcher.Invoke(new Func<string>(() => tbEmail.Text.ToLower()))).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Password == Dispatcher.Invoke(new Func<string>(() => tbPass.Password)))
                        {
                            if(Dispatcher.Invoke(new Func<bool?>(() => TGB.IsChecked)) == true)
                            {
                                try
                                {
                                    using (FileStream fs = new FileStream("config.txt", FileMode.Create, FileAccess.Write))
                                    {
                                        using (StreamWriter sw = new StreamWriter(fs))
                                        {
                                            sw.WriteLine(user.Email);
                                        }
                                    }
                                }
                                catch (Exception) 
                                {
                                    MessageBox.Show("Some error!");
                                }
                            }

                            Dispatcher.Invoke(()=> { 
                                Window w = new ChooseForm(user);
                                w.Show();
                            });
                           
                            Dispatcher.Invoke(new Action(() => this.Close()));
                        }
                        else
                        {
                            Dispatcher.Invoke(new Action(() => txt_validation.Text = "Wrong password"));
                        }
                    }
                    else
                    {
                        Dispatcher.Invoke(new Action(() => txt_validation.Text = "This account doesn't exist!"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Load2()
        {
            Dispatcher.Invoke(() => {
                this.load_img.Visibility = Visibility.Visible;
            });
            Thread.Sleep(1000);
            try
            {
                using (MyStorageEntities db = new MyStorageEntities())
                {
                    var user = db.Users.AsEnumerable().Where(x => x.Email == Dispatcher.Invoke(new Func<string>(() => EMAIL))).FirstOrDefault();

                    if (user != null)
                    {
                        Dispatcher.Invoke(() => {
                            Window w = new ChooseForm(user);
                            w.Show();
                        });

                        Dispatcher.Invoke(new Action(() => this.Close()));
                    }
                    else
                    {
                        try
                        {
                            if (File.Exists("config.txt"))
                            {
                                File.Delete("config.txt");
                            }
                            
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadEnd(Task obj)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.load_img.Visibility = Visibility.Collapsed;
            }));

        }
        public bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        
    }
}
