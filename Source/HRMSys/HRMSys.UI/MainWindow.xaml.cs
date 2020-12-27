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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HRMSys.DAL;
using HRMSys.Model;
using HRMSys.UI.SystemMgr;
using System.Windows.Threading;


namespace HRMSys.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer ShowTimer; 

        public MainWindow()
        {
            InitializeComponent();
            ShowTimer = new DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurrentTimer);
            ShowTimer.Interval = new TimeSpan(0,0,0,1);
            ShowTimer.Start();
            this.Title = new SettingDAL().GetValue("公司名称") + "人事管理系统";
            EmployeeDAL employeeDAL = new EmployeeDAL();
            Employee[] birthdayEmployees =
                employeeDAL.Search3DaysBirthDay();
            if (new SettingDAL().GetBoolValue("启动生日提醒"))
            {
                //检测一个月之内合同到期的员工
                if (birthdayEmployees.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < birthdayEmployees.Length; i++)
                    {
                        sb.Append(birthdayEmployees[0].Name).Append(",");
                    }
                    sb.Append("三天之内过生日");
                    //MessageBox.Show(sb.ToString());
                    PopupWindow popupWin = new PopupWindow();
                    popupWin.Left = SystemParameters.WorkArea.Width - popupWin.Width;
                    popupWin.Top = SystemParameters.WorkArea.Height - popupWin.Height;
                    popupWin.Message = sb.ToString();
                    popupWin.Show();
                }
            }

        }

        private void ShowCurrentTimer(object sender, EventArgs e)
        {
            txttime.Text = "当前时间:";
            txttime.Text += DateTime.Now.ToString("yyyy年MM月dd日");
            txttime.Text += " ";
            txttime.Text += DateTime.Now.ToString("HH:mm:ss");
        }
        private void OPMG_Click(object sender, RoutedEventArgs e)
        {
           
            OperatorListUI listUI = new OperatorListUI();
            listUI.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EmployeeListWindow listemployee = new EmployeeListWindow();
            listemployee.ShowDialog();
        }

        private void Setiing_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow wm = new SettingWindow();
            wm.ShowDialog();
        }
        private void Operatorlogs_Click(object sender, RoutedEventArgs e)
        {
            OperationLog log = new OperationLog();
            log.ShowDialog();
        }

        private void Depmgr_Click(object sender, RoutedEventArgs e)
        {

            DeptListWindow dp = new DeptListWindow();
            dp.ShowDialog();
        }

        private void producesalary_Click(object sender, RoutedEventArgs e)
        {
            BulidSalarySheet salary = new BulidSalarySheet();
            salary.ShowDialog();

        }

    }
}