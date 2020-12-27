using System;
using System.Collections.Generic;
using System.Linq;
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

namespace HRMSys.UI
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            SettingDAL dal = new SettingDAL();
            txtName.Text = dal.GetValue("公司名称");
            txtWeb.Text = dal.GetValue("公司网站");
            ckPro.IsChecked = dal.GetBoolValue("启动生日提醒");
            txtremind.Text = dal.GetValue("生日提醒天数");
            txtNumber.Text = dal.GetValue("员工编号");

        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            SettingDAL dal = new SettingDAL();
            dal.SetValue("公司名称", txtName.Text);
           dal.SetValue("公司网站",txtWeb.Text);
             dal.SetValue("启动生日提醒",(bool)ckPro.IsChecked);
            dal.SetValue("生日提醒天数",txtremind.Text);
            dal.SetValue("员工编号", txtNumber.Text);
            DialogResult = true;




        }
    }
}
