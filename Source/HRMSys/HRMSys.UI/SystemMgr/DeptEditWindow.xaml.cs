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
using HRMSys.Model;
using HRMSys.DAL;

namespace HRMSys.UI.SystemMgr
{
    /// <summary>
    /// DeptEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeptEditWindow : Window
    {
        public DeptEditWindow()
        {
            InitializeComponent();
            
        }
        public bool IsAddNew { get; set; }
        public Guid IsEditingId { get; set; }

        private void txtSave_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("部门名称不能为空！");
                    return;
            }
            if(IsAddNew)
            {
                DepartmentDAL.Insert(name);
            }
            else
            {
                DepartmentDAL.Update(IsEditingId, name);
            }
            DialogResult = true;
        }

        private void txtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsAddNew)
            {
                Department dept = DepartmentDAL.GetById(IsEditingId);
                txtName.Text = dept.Name;
            }
            txtName.Focus();
        }
    }
}
