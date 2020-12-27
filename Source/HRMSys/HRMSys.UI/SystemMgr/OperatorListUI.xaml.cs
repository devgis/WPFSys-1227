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
    /// OperatorListUI.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorListUI : Window
    {
        public OperatorListUI()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            OperatorEditUI editUi = new OperatorEditUI();
            editUi.Insert = true;
            if (editUi.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            gridOperator.ItemsSource = OperatorDAL.GetAll();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Operator op = (Operator)gridOperator.SelectedItem;
            if (op == null)
            {
                MessageBox.Show("请先选择行!");
                return;
            }
            OperatorEditUI editUi = new OperatorEditUI();
            editUi.Insert = false;

            editUi.Editting = op.Id;
            if (editUi.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Operator op = (Operator)gridOperator.SelectedItem;
            if (op == null)
            {
                MessageBox.Show("请先选择行!");
                return;
            }
            if (MessageBox.Show("真的要删除" + op.UserName + "吗?", "提示!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                OperatorDAL.DeletebyId(op.Id);
                LoadData();
                Guid operatorid = CommonHelper.GetOperatorId();
               T_OperaLogDAL.Insert(operatorid, "删除管理员" + op.UserName);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

      
    }
}