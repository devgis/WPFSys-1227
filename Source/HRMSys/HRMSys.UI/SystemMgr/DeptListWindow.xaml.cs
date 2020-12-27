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

namespace HRMSys.UI.SystemMgr
{
    /// <summary>
    /// DeptListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeptListWindow : Window
    {
        public DeptListWindow()
        {
            InitializeComponent();
        
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ReloadData();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DeptEditWindow win = new DeptEditWindow();
            win.IsAddNew = true;
            if (win.ShowDialog() == true)
            {
                ReloadData();
            }

        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Department dept = datagrid.SelectedItem as Department;
            if (dept == null)
            {
                MessageBox.Show("没有选中任何行!");
                return;
            }
            if (MessageBox.Show("是否真的要删除?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                DepartmentDAL.Delete(dept.Id);
            }
           
            ReloadData();
        }
        private void ReloadData()
        {
            datagrid.ItemsSource = DepartmentDAL.ListAll();
        }
        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Department dept = datagrid.SelectedItem as Department;
            if (dept != null)
            {
                DeptEditWindow win = new DeptEditWindow();
                win.IsAddNew = false;
                win.IsEditingId = dept.Id;
                if (win.ShowDialog() == true)
                {
                    ReloadData();
                }
            }
        }
    }
}
