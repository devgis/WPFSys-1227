using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using HRMSys.DAL;
using HRMSys.Model;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace HRMSys.UI
{
    /// <summary>
    /// EmployeeListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeListWindow : Window
    {
        public EmployeeListWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            columnDepartmentId.ItemsSource = DepartmentDAL.ListAll();
            columnEducationId.ItemsSource = IdNameDAL.GetByCategory("学历");
            datagrid.ItemsSource = EmployeeDAL.ListAll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            cbSearchByName.IsChecked = true;
            dpInDateStart.SelectedDate = DateTime.Today.AddMonths(-1);
            dpInDateEnd.SelectedDate = DateTime.Today;
            cmbDept.ItemsSource = DepartmentDAL.ListAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeEditWindow win = new EmployeeEditWindow();
            win.IsAddNew = true;
            if (win.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = (Employee)datagrid.SelectedItem;
            if (employee == null)
            {
                MessageBox.Show("请先选择!");
                return;
            }
            if (MessageBox.Show("真的要删除" + employee.Name + "吗?", "提示!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                EmployeeDAL.Delete(employee.Id);
                LoadData();
                Guid operatorid = CommonHelper.GetOperatorId();
                T_OperaLogDAL.Insert(operatorid, "删除职员:" + employee.Name);
            }
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = (Employee)datagrid.SelectedItem;
            if (employee == null)
            {
                MessageBox.Show("请先选择!");
                return;
            }
            EmployeeEditWindow emdit = new EmployeeEditWindow();
            emdit.IsAddNew = false;
            emdit.EdittingId = employee.Id;
            if (emdit.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<string> whereList = new List<string>();
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (cbSearchByName.IsChecked == true)
            {
                whereList.Add("Name=@Name");
                paramList.Add(new SqlParameter("@Name", txtName.Text));
            }
            if (cbSearchByInDate.IsChecked == true)
            {
                whereList.Add("InDate>=@InDateStart and InDate<=@InDateEnd");
                paramList.Add(new SqlParameter("@InDateStart", dpInDateStart.SelectedDate));
                paramList.Add(new SqlParameter("@InDateEnd", dpInDateEnd.SelectedDate));
            }
            if (cbSearchByDept.IsChecked == true)
            {
                whereList.Add("DepartmentId=@DepartmentId");
                paramList.Add(new SqlParameter("@DepartmentId", cmbDept.SelectedValue));
            }

            string whereSql = string.Join("  and  ", whereList);
            string sql = "select * from  T_Employee where IsStopped=0";
            if (whereSql.Length > 0)
            {
                sql = sql + "  and " + whereSql;
            }
            Employee[] result = EmployeeDAL.Search(sql, paramList);
            datagrid.ItemsSource = result;
        }

        private void Btn_ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sdfExport = new SaveFileDialog();
            sdfExport.Filter = "Excel文件|*.xls";
            if (sdfExport.ShowDialog() != true)
            {
                return;
            }
            string filename = sdfExport.FileName;
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("员工数据");
            IRow rowHeader = sheet.CreateRow(0);//表头行
            rowHeader.CreateCell(0, CellType.STRING).SetCellValue("姓名");
            rowHeader.CreateCell(1, CellType.STRING).SetCellValue("工号");
            rowHeader.CreateCell(2, CellType.STRING).SetCellValue("入职日期");
            rowHeader.CreateCell(3, CellType.STRING).SetCellValue("学历");
            rowHeader.CreateCell(4, CellType.STRING).SetCellValue("毕业院校");
            rowHeader.CreateCell(5, CellType.STRING).SetCellValue("基本工资");
            rowHeader.CreateCell(6, CellType.STRING).SetCellValue("部门");
            rowHeader.CreateCell(7, CellType.STRING).SetCellValue("职位");
            rowHeader.CreateCell(8, CellType.STRING).SetCellValue("合同签订日");
            rowHeader.CreateCell(9, CellType.STRING).SetCellValue("合同到期日");

            Employee[] employees = (Employee[])datagrid.ItemsSource;

            for (int i = 0; i < employees.Length; i++)
            {
                Employee employee = employees[i];
                
                IRow row = sheet.CreateRow(i + 1);
                IdName idname = new IdName();
                row.CreateCell(0, CellType.STRING).SetCellValue(employee.Name);
                row.CreateCell(1, CellType.STRING).SetCellValue(employee.Number);
                row.CreateCell(3, CellType.STRING).SetCellValue(IdNameDAL.GetById(employee.EducationId).Name);
                row.CreateCell(4, CellType.STRING).SetCellValue(employee.School);
                row.CreateCell(5, CellType.STRING).SetCellValue(employee.BaseSalary);
                  row.CreateCell(6, CellType.STRING).SetCellValue(DepartmentDAL.GetById(employee.DepartmentId).Name);
                row.CreateCell(7, CellType.STRING).SetCellValue(employee.Position);

                ICellStyle styledate = workbook.CreateCellStyle();
                IDataFormat format = workbook.CreateDataFormat();

                //格式具体有哪些请看单元格右键中的格式，有说明
                styledate.DataFormat = format.GetFormat("yyyy\"年\"m\"月\"d\"日\"");

                ICell cellInDate = row.CreateCell(2, CellType.NUMERIC);
                cellInDate.CellStyle = styledate;
                cellInDate.SetCellValue(employee.InDate);

                ICell cellInDateStart = row.CreateCell(8, CellType.NUMERIC);
                cellInDateStart.CellStyle = styledate;
                cellInDateStart.SetCellValue(employee.ContractStartDay);

                ICell cellInDateEnd = row.CreateCell(9, CellType.NUMERIC);
                cellInDateEnd.CellStyle = styledate;
                cellInDateEnd.SetCellValue(employee.ContractEndDay);
            }
            using (FileStream stream = File.OpenWrite(filename))
            {
                workbook.Write(stream);
            }
        }
    }
}