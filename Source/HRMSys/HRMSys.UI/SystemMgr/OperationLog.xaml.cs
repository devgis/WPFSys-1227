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
using System.Data.SqlClient;
using HRMSys.DAL;
using HRMSys.Model;

namespace HRMSys.UI.SystemMgr
{
    /// <summary>
    /// OperationLog.xaml 的交互逻辑
    /// </summary>
    public partial class OperationLog : Window
    {
        public OperationLog()
        {
            InitializeComponent();
            Operator[] operators=OperatorDAL.GetAll();

            cmbop.ItemsSource = operators;
            coloperator.ItemsSource = operators;
        }

        private void Btn_Search_Click(object sender, RoutedEventArgs e)
        {
            List<string> wherelist = new List<string>();
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (ckop.IsChecked == true)
            {
                wherelist.Add("OperatorId=@OperatorId");
                paramList.Add(new SqlParameter("@OperatorId", cmbop.SelectedValue));
            }
            if (ckopdate.IsChecked == true)
            {
                if (startdate.SelectedDate == null)
                {
                    MessageBox.Show("请先选择!");
                    return;
 
                }
                wherelist.Add("MakeDate Between @BeginDate and @EndDate");
                paramList.Add(new SqlParameter("@BeginDate", startdate.SelectedDate));
                paramList.Add(new SqlParameter("@EndDate", enddate.SelectedDate));

                   
            }
            if (ckdesc.IsChecked ==true)
            {
                wherelist.Add("ActionDesc like @ActionDesc");
                paramList.Add(new SqlParameter("@ActionDesc", "%"+txtdesc.Text.Trim()+"%"));


            }
            if (wherelist.Count <= 0)
            {
                MessageBox.Show("至少选择一个条件!");
                return;
            }
            string sql = "select * from T_OperationLog where " + string.Join(" and ", wherelist);
            T_OperaLog []logs = T_OperaLogDAL.Search(sql, paramList.ToArray());
            datagrid.ItemsSource = logs;


        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            T_OperaLogDAL.Deletelog();
            datagrid.ItemsSource = T_OperaLogDAL.GetAll();
        
        }
    }
}
