using MyStorage_v02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for SignUpForm.xaml
    /// </summary>
    public partial class SignUpForm : Window
    {
        private Users UserTest { get; set; }
        public SignUpForm(double left, double top)
        {
            InitializeComponent();
            
            this.Left = left;
            this.Top = top;
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

        private void line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Plg.Points.Clear();
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.411, 0));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.173, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.465, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.42, 0));
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

        private void OnPasswordChanged2(object sender, RoutedEventArgs e)
        {
            if (tbPass2.Password.Length > 0)
            {
                Watermark2.Visibility = Visibility.Collapsed;
            }
            else
            {
                Watermark2.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window w = new MainWindow(this.Left, this.Top);
            w.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (tbEmail.Text.Length != 0 && tbName.Text.Length != 0 && tbSurname.Text.Length != 0 && tbPass.Password.Length != 0 && tbPass2.Password.Length != 0)
            {
                if (IsEmailValid(tbEmail.Text.ToLower()))
                {
                    if (tbName.Text.Length > 1 && tbName.Text.Length <= 15)
                    {
                        if (tbSurname.Text.Length > 1 && tbSurname.Text.Length <= 15)
                        {
                            if (tbPass.Password.Length > 6)
                            {
                                if (tbPass.Password == tbPass2.Password)
                                {
                                    Task task = Task.Run(Load);
                                    task.ContinueWith(LoadEnd);
                                }
                                else
                                {
                                    txt_validation.Text = "The passwords don't match";
                                }
                            }
                            else
                            {
                                txt_validation.Text = "Password must contain more than 6 symbols";
                            }
                        }
                        else
                        {
                            txt_validation.Text = "Surname lgth must be between 2 and 15 symbols";
                        }
                    }
                    else
                    {
                        txt_validation.Text = "Name lgth must be between 2 and 15 symbols";
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
            bool flag = true;
            try
            {
                using (MyStorageEntities db = new MyStorageEntities())
                {
                    var user = db.Users.AsEnumerable().Where(x => x.Email == Dispatcher.Invoke(new Func<string>(() => tbEmail.Text.ToLower()))).FirstOrDefault();
                    if (user != null)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }

            if (flag == false)
            {
                Dispatcher.Invoke(new Action(() => txt_validation.Text = ""));
                
                try
                {
                    using (MyStorageEntities db = new MyStorageEntities())
                    {
                        UserTest = new Users { Name = Dispatcher.Invoke(new Func<string>(() => tbName.Text)), Surname = Dispatcher.Invoke(new Func<string>(() => tbSurname.Text)), Email = Dispatcher.Invoke(new Func<string>(() => tbEmail.Text.ToLower())), MaxSize = 16106127360, Password = Dispatcher.Invoke(new Func<string>(() => tbPass.Password)), FullSize = 0 };
                    }
                    var task = Task.Run(MyMeth);
                }
                catch (Exception)
                {
                    Dispatcher.Invoke(new Action(() => txt_validation.Text = "Some error!"));
                }
            }
            else
            {
                Dispatcher.Invoke(new Action(() => txt_validation.Text = "This email already exists!"));
            }
        }

        private void MyMeth()
        {
            Thread.Sleep(100);
            Dispatcher.Invoke(new Action(() => {
                var window = new Email(tbEmail.Text.ToLower(), UserTest);
                window.Owner = this;
                window.ShowDialog();

            }));
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

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
