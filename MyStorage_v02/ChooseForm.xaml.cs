using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using MyStorage_v02.Models;

namespace MyStorage_v02
{
    /// <summary>
    /// Interaction logic for ChooseForm.xaml
    /// </summary>
    public partial class ChooseForm : Window
    {
        private Users User { get; set; }
        public ChooseForm(Users user)
        {
            InitializeComponent();
            User = user;
            name_tb.Text = User.Name;
            surname_tb.Text = User.Surname;
            border.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(4)
            });
            clue_tb.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0.1,
                To = 0.6,
                Duration = TimeSpan.FromSeconds(0.8),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            });
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Plg.Points.Clear();
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.411, 0));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 3.173, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.465, this.ActualHeight / 64.375));
            Plg.Points.Add(new Point(ToolBar.ActualWidth / 1.42, 0));
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

        private void right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RightAnimation();
        }

        private void left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LeftAnimation();
        }

        private void Method2(Task obj)
        {
            List<Image> images = Dispatcher.Invoke(new Func<List<Image>>(() => {
                List<Image> imagess = new List<Image>();
                imagess.Add(fakeCentre);
                imagess.Add(fakeRight);
                imagess.Add(fakeRight2);
                imagess.Add(fakeLeft2);
                imagess.Add(fakeLeft);
                return imagess;
            }));

            foreach (Image item in images)
            {
                if (Dispatcher.Invoke(new Func<double>(() => Canvas.GetLeft(item))) == 26)
                {
                    Dispatcher.Invoke(new Action(() => leftImage.Source = item.Source));
                }
                if (Dispatcher.Invoke(new Func<double>(() => Canvas.GetLeft(item))) == 251)
                {
                    Dispatcher.Invoke(new Action(() => centreImage.Source = item.Source));
                }
                if (Dispatcher.Invoke(new Func<double>(() => Canvas.GetLeft(item))) == 575)
                {
                    Dispatcher.Invoke(new Action(() => rightImage.Source = item.Source));
                }
                if (Dispatcher.Invoke(new Func<double>(() => Canvas.GetLeft(item))) == 775)
                {
                    Dispatcher.Invoke(new Action(() => right2Image.Source = item.Source));
                }
                if (Dispatcher.Invoke(new Func<double>(() => Canvas.GetLeft(item))) == -180)
                {
                    Dispatcher.Invoke(new Action(() => left2Image.Source = item.Source));
                }
            }



            Dispatcher.Invoke(new Action(() => {
                fakeCentre.BeginAnimation(Canvas.LeftProperty, null);
                fakeCentre.BeginAnimation(Canvas.TopProperty, null);
                fakeCentre.BeginAnimation(WidthProperty, null);
                fakeCentre.BeginAnimation(HeightProperty, null);

                fakeLeft.BeginAnimation(Canvas.LeftProperty, null);
                fakeLeft.BeginAnimation(Canvas.TopProperty, null);
                fakeLeft.BeginAnimation(WidthProperty, null);
                fakeLeft.BeginAnimation(HeightProperty, null);

                fakeLeft2.BeginAnimation(Canvas.LeftProperty, null);
                fakeLeft2.BeginAnimation(Canvas.TopProperty, null);
                fakeLeft2.BeginAnimation(WidthProperty, null);
                fakeLeft2.BeginAnimation(HeightProperty, null);

                fakeRight.BeginAnimation(Canvas.LeftProperty, null);
                fakeRight.BeginAnimation(Canvas.TopProperty, null);
                fakeRight.BeginAnimation(WidthProperty, null);
                fakeRight.BeginAnimation(HeightProperty, null);

                fakeRight2.BeginAnimation(Canvas.LeftProperty, null);
                fakeRight2.BeginAnimation(Canvas.TopProperty, null);
                fakeRight2.BeginAnimation(WidthProperty, null);
                fakeRight2.BeginAnimation(HeightProperty, null);

                fakeCentre.Source = centreImage.Source;
                Canvas.SetLeft(fakeCentre, Canvas.GetLeft(centreImage));
                Canvas.SetTop(fakeCentre, Canvas.GetTop(centreImage));


                fakeLeft.Source = leftImage.Source;
                Canvas.SetLeft(fakeLeft, Canvas.GetLeft(leftImage));


                fakeRight.Source = rightImage.Source;
                Canvas.SetLeft(fakeRight, Canvas.GetLeft(rightImage));

                fakeRight2.Source = right2Image.Source;
                Canvas.SetLeft(fakeRight2, Canvas.GetLeft(right2Image));

                fakeLeft2.Source = left2Image.Source;
                Canvas.SetLeft(fakeLeft2, Canvas.GetLeft(left2Image));

                fakeCentre.Visibility = Visibility.Hidden;
                fakeLeft.Visibility = Visibility.Hidden;
                fakeRight.Visibility = Visibility.Hidden;
                fakeRight2.Visibility = Visibility.Hidden;
                fakeLeft2.Visibility = Visibility.Hidden;


                leftImage.Visibility = Visibility.Visible;
                centreImage.Visibility = Visibility.Visible;
                rightImage.Visibility = Visibility.Visible;
                right2Image.Visibility = Visibility.Visible;
                left2Image.Visibility = Visibility.Visible;

            }));



            Dispatcher.Invoke(new Action(() => left.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => right.IsEnabled = true));

        }

        private void Method1()
        {
            Thread.Sleep(500);
        }

        private void RightAnimation()
        {

            right.IsEnabled = false;
            left.IsEnabled = false;
            fakeCentre.Visibility = Visibility.Visible;
            fakeLeft.Visibility = Visibility.Visible;
            fakeRight.Visibility = Visibility.Visible;
            fakeRight2.Visibility = Visibility.Visible;
            fakeLeft2.Visibility = Visibility.Hidden;


            leftImage.Visibility = Visibility.Hidden;
            centreImage.Visibility = Visibility.Hidden;
            rightImage.Visibility = Visibility.Hidden;
            right2Image.Visibility = Visibility.Hidden;
            left2Image.Visibility = Visibility.Hidden;



            //1 ---> 0
            fakeLeft.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeLeft),
                To = Canvas.GetLeft(fakeLeft2),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            //2 ---> 1 
            fakeCentre.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeCentre),
                To = Canvas.GetLeft(leftImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeCentre.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                From = Canvas.GetTop(fakeCentre),
                To = Canvas.GetTop(leftImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeCentre.BeginAnimation(Canvas.WidthProperty, new DoubleAnimation
            {
                From = fakeCentre.ActualWidth,
                To = leftImage.ActualWidth,
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeCentre.BeginAnimation(Canvas.HeightProperty, new DoubleAnimation
            {
                From = fakeCentre.ActualHeight,
                To = leftImage.ActualHeight,
                Duration = TimeSpan.FromSeconds(0.3)
            });

            //3 ---> 2
            fakeRight.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeRight),
                To = Canvas.GetLeft(centreImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });
            fakeRight.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                From = Canvas.GetTop(fakeRight),
                To = Canvas.GetTop(centreImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeRight.BeginAnimation(Canvas.WidthProperty, new DoubleAnimation
            {
                From = fakeRight.ActualWidth,
                To = centreImage.ActualWidth,
                Duration = TimeSpan.FromSeconds(0.3)
            });
            fakeRight.BeginAnimation(Canvas.HeightProperty, new DoubleAnimation
            {
                From = fakeRight.ActualHeight,
                To = centreImage.ActualHeight,
                Duration = TimeSpan.FromSeconds(0.3)
            });

            //4 ---> 3
            fakeRight2.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeRight2),
                To = Canvas.GetLeft(fakeRight),
                Duration = TimeSpan.FromSeconds(0.3)
            });



            //0 ---> 4
            fakeLeft2.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeLeft2),
                To = Canvas.GetLeft(fakeRight2),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            Task task1 = Task.Run(Method1);
            task1.ContinueWith(Method2);
        }

        private void LeftAnimation()
        {

            right.IsEnabled = false;
            left.IsEnabled = false;
            fakeCentre.Visibility = Visibility.Visible;
            fakeLeft.Visibility = Visibility.Visible;
            fakeRight.Visibility = Visibility.Visible;
            fakeRight2.Visibility = Visibility.Hidden;
            fakeLeft2.Visibility = Visibility.Visible;


            leftImage.Visibility = Visibility.Hidden;
            centreImage.Visibility = Visibility.Hidden;
            rightImage.Visibility = Visibility.Hidden;
            right2Image.Visibility = Visibility.Hidden;
            left2Image.Visibility = Visibility.Hidden;



            //0 ---> 1
            fakeLeft2.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeLeft2),
                To = Canvas.GetLeft(fakeLeft),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            //1 ---> 2 
            fakeLeft.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeLeft),
                To = Canvas.GetLeft(centreImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeLeft.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                From = Canvas.GetTop(fakeLeft),
                To = Canvas.GetTop(centreImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeLeft.BeginAnimation(Canvas.WidthProperty, new DoubleAnimation
            {
                From = fakeLeft.ActualWidth,
                To = centreImage.ActualWidth,
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeLeft.BeginAnimation(Canvas.HeightProperty, new DoubleAnimation
            {
                From = fakeLeft.ActualHeight,
                To = centreImage.ActualHeight,
                Duration = TimeSpan.FromSeconds(0.3)
            });

            //2 ---> 3 
            fakeCentre.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeCentre),
                To = Canvas.GetLeft(rightImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });
            fakeCentre.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                From = Canvas.GetTop(fakeCentre),
                To = Canvas.GetTop(rightImage),
                Duration = TimeSpan.FromSeconds(0.3)
            });

            fakeCentre.BeginAnimation(Canvas.WidthProperty, new DoubleAnimation
            {
                From = fakeCentre.ActualWidth,
                To = rightImage.ActualWidth,
                Duration = TimeSpan.FromSeconds(0.3)
            });
            fakeCentre.BeginAnimation(Canvas.HeightProperty, new DoubleAnimation
            {
                From = fakeCentre.ActualHeight,
                To = rightImage.ActualHeight,
                Duration = TimeSpan.FromSeconds(0.3)
            });

            //3 ---> 4
            fakeRight.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeRight),
                To = Canvas.GetLeft(fakeRight2),
                Duration = TimeSpan.FromSeconds(0.3)
            });



            //4 ---> 0
            fakeRight2.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = Canvas.GetLeft(fakeRight2),
                To = Canvas.GetLeft(fakeLeft2),
                Duration = TimeSpan.FromSeconds(0.3)
            });



            Task task1 = Task.Run(Method1);
            task1.ContinueWith(Method2);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (right.IsEnabled == false || left.IsEnabled == false)
                return;
            if (e.Key == Key.Enter)
            {
                string name = centreImage.Source.ToString().Split('/')[centreImage.Source.ToString().Split('/').Length - 1];
                if (name == "storage.png")
                {
                    Window w = new Storage(User);
                    w.Show();
                    this.Close();
                }
                //else if(name == "video.png")
                //{
                //    Window w = new Video(User);
                //    w.Show();
                //    this.Close();
                //}
            }
            else if (e.Key == Key.Right)
            {
                RightAnimation();
            }
            else if (e.Key == Key.Left)
            {
                LeftAnimation();
            }

        }
    }
}
