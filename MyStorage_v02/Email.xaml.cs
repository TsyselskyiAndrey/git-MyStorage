using MyStorage_v02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
using System.Windows.Threading;

namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for Email.xaml
    /// </summary>
    public partial class Email : Window
    {
        public string Mail { get; set; } = "email@gmail.com";
        private Users User { get; set; }
        private int Code { get; set; }
        public Email(string mail, Users user)
        {
            InitializeComponent();
            User = user;
            Mail = mail;
            Task task = new Task(Method);
            task.ContinueWith(Method2);
            task.Start();
            Task maintask = new Task(MainMethod);
            maintask.Start();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(Method);
            task.ContinueWith(Method2);
            task.Start();
            Task maintask = new Task(MainMethod);
            maintask.Start();
        }

        private void MainMethod()
        {
            Random rnd = new Random();
            MailMessage message = new MailMessage();
            message.From = new MailAddress("tsyselskyiandrey2@gmail.com");

            message.To.Add(new MailAddress(Mail));

            message.Subject = "My Storage";
            Code = rnd.Next(100000, 1000000);
            message.Body = "Your Verification Code: " + Code.ToString();

            string server = "smtp.gmail.com";

            SmtpClient smtpClient = new SmtpClient(server)
            {
                Port = 587,
                Credentials = new NetworkCredential("tsyselskyiandrey2@gmail.com", "nwfhdbwzngfegtjc"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            try
            {
                smtpClient.Send(message);
            }
            catch (SmtpException sm)
            {
                MessageBox.Show(sm.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Method()
        {
            int time = 30;
            while (time != 0)
            {
                try
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        btnSendAgain.Content = time;
                        btnSendAgain.IsEnabled = false;
                    }));
                    time--;
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    Environment.Exit(0);
                }
               
            }
            
        }

        private void Method2(Task obj)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                btnSendAgain.Content = "Send again";
                btnSendAgain.IsEnabled = true;
            }));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(Code.ToString() == tbCode.Text)
            {
                txt_validation.Text = "";
                Task task = Task.Run(Load);
                task.ContinueWith(LoadEnd);
            }
            else
            {
                txt_validation.Text = "The code is incorrect! Try gain";
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
                    db.Users.Add(User);
                    db.SaveChanges();
                }
                Dispatcher.Invoke(() => {
                    Window w = new ChooseForm(User);
                    w.Show();
                });

                Dispatcher.Invoke(new Action(() => this.Close()));
                Dispatcher.Invoke(new Action(() => this.Owner.Close()));
            }
            catch (Exception)
            {
                Dispatcher.Invoke(new Action(() => txt_validation.Text = "Some error!"));
            }
        }

        private void tbCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string numbers = "0123456789";
            for (int i = 0; i < e.Text.Length; i++)
            {
                if (!numbers.Contains(e.Text[i]))
                {
                    e.Handled = true;
                }
            }
            
        }
        private void LoadEnd(Task obj)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.load_img.Visibility = Visibility.Collapsed;
            }));

        }

       
    }
}
