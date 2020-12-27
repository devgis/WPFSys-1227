using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HRMSys.DAL;
using HRMSys.Model;

namespace HRMSys.UI
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtuser.Text;
            string password = pass.Password;
            Operator op = OperatorDAL.GetByUserName(userName);
            if (op == null)
            {
                MessageBox.Show("用户名或者密码错误!");
                return;
            }
            else
            {
                string dbmd5 = op.UserPass;
                string mymd5 = CommonHelper.getMd5(password + CommonHelper.GetPasswordSalt());
                if (dbmd5 == mymd5)
                {
                   
                    MainWindow wm = new MainWindow();
                    wm.Show();
            
                    this.Close();
                    T_OperaLogDAL.Insert(op.Id, "登陆成功!");
                    //把登录操作者的Id保存到全局变量
                    Application.Current.Properties["OperatorId"] = op.Id;

                }
                else
                {
                    T_OperaLogDAL.Insert(op.Id, op.RealName+"登陆错误!");
                    MessageBox.Show("用户名或者密码错误!");
                    return;
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private bool _closinganimation = true;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = _closinganimation;
            _closinganimation = false;

            System.Windows.Media.Animation.Storyboard sb = new System.Windows.Media.Animation.Storyboard();
            System.Windows.Media.Animation.DoubleAnimation dh = new System.Windows.Media.Animation.DoubleAnimation();
            System.Windows.Media.Animation.DoubleAnimation dw = new System.Windows.Media.Animation.DoubleAnimation();
            System.Windows.Media.Animation.DoubleAnimation dop = new System.Windows.Media.Animation.DoubleAnimation();
            dop.Duration = dh.Duration = dw.Duration = sb.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            dop.To = dh.To = dw.To = 0;
            System.Windows.Media.Animation.Storyboard.SetTarget(dop, this);
            System.Windows.Media.Animation.Storyboard.SetTarget(dh, this);
            System.Windows.Media.Animation.Storyboard.SetTarget(dw, this);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(dop, new PropertyPath("Opacity", new object[] { }));

            sb.Children.Add(dop);
            sb.Completed += new EventHandler(sb_Completed);
            sb.Begin();
        }

        private void sb_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.BtnLogin_Click(sender, null);
            }
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}