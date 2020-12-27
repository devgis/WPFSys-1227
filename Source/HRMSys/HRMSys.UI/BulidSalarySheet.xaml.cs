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
using HRMSys.Model;

namespace HRMSys.UI
{
    /// <summary>
    /// BulidSalarySheet.xaml 的交互逻辑
    /// </summary>
    public partial class BulidSalarySheet : Window
    {
        public BulidSalarySheet()
        {
            InitializeComponent();

        }

        private void Btn_Produce_Click(object sender, RoutedEventArgs e)
        {
            int year = (int)cmbyear.SelectedValue;
            int month = (int)cmbmonth.SelectedValue;
            Guid depid = (Guid)cmbdepart.SelectedValue;
            SalarySheetDAL dal = new SalarySheetDAL();
            if (dal.IsExits(year, month, depid))
            {
                if (MessageBox.Show("工资单已经存在，是否重新生成工资单？", "提示",
                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                    dal.Clear(year, month, depid);
                    colemployee.ItemsSource = EmployeeDAL.ListAllDept(depid);
                }
                else
                {
                    return;
                }




            }
            dal.Bulid(year, month, depid);
            dataGrid1.ItemsSource = SalarySheetDAL.GetSalarySheetItem(year, month, depid);
         



        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> listyears = new List<int>();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 5; i++)
            {

                listyears.Add(i);
            }
            List<int> months = new List<int>();
            for (int i = 1; i <= 12; i++)
            {

                months.Add(i);

            }
            cmbyear.ItemsSource = listyears;
            cmbmonth.ItemsSource = months;
            cmbyear.SelectedValue = DateTime.Now.Year;
            cmbmonth.SelectedValue = DateTime.Now.Month;
            cmbdepart.ItemsSource = DepartmentDAL.ListAll();




        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            SalarySheetItem item = (SalarySheetItem)e.Row.DataContext;
            SalarySheetDAL.Update(item);

        }
    }
}
