using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HRMSys.DAL;
using HRMSys.Model;
using Microsoft.Win32;

namespace HRMSys.UI
{
    /// <summary>
    /// EmployeeEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeEditWindow : Window
    {
        public EmployeeEditWindow()
        {
            InitializeComponent();
        }

        public Guid EdittingId { get; set; }

        public bool IsAddNew { get; set; }

        #region 检查非空字段

        private void CheckTextBoxNotNull(ref bool IsOk, params TextBox[] textboxes)
        {
            foreach (TextBox txtBox in textboxes)
            {
                if (txtBox.Text.Length <= 0)
                {
                    IsOk = false;
                    txtBox.Background = Brushes.Red;
                }
                else
                {
                    txtBox.Background = null;
                }
            }
        }

        private void CheckComboBoxNotNull(ref bool IsOk, params ComboBox[] cmbs)
        {
            foreach (ComboBox cmb in cmbs)
            {
                if (cmb.SelectedIndex < 0)
                {
                    IsOk = false;
                    cmb.Effect = new DropShadowEffect { Color = Colors.Red };
                }
                else
                {
                    cmb.Effect = null;
                }
            }
        }

        #endregion 检查非空字段

        private bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbGender.ItemsSource = IdNameDAL.GetByCategory("性别");
            cbMarriage.ItemsSource = IdNameDAL.GetByCategory("婚姻状况");
            cbPartyStatus.ItemsSource = IdNameDAL.GetByCategory("政治面貌");
            cbEducation.ItemsSource = IdNameDAL.GetByCategory("学历");
            cbDepatment.ItemsSource = DepartmentDAL.ListAll();

            if (IsAddNew)
            {
                Employee employee = new Employee();
                employee.InDate = DateTime.Today;
                employee.ContractStartDay = DateTime.Today;
                employee.ContractEndDay = DateTime.Today.AddYears(1);
                employee.Nationality = "汉族";
                employee.Email = "@163.com";
                employee.Number = new SettingDAL().GetValue("员工编号");
                employee.BirthDay = DateTime.Now;

                gridEmployee.DataContext = employee;
            }
            else
            {
                Employee employee = EmployeeDAL.GetById(EdittingId);
                gridEmployee.DataContext = employee;
                if (employee.Photo != null)
                {
                    ShowImg(employee.Photo);
                }
            }
        }

        private void ShowImg(byte[] imgBytes)
        {
            MemoryStream stream = new MemoryStream(imgBytes);
            BitmapImage bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.StreamSource = stream;
            bmpImg.EndInit();
            imgPhoto.Source = bmpImg;
        }

        private void txtSave_Click(object sender, RoutedEventArgs e)
        {
            bool IsOk = true;
            CheckTextBoxNotNull(ref  IsOk, txtName, txtNational, txtNativeAddr, txtAddr,
                    txtBaseSalary, txtTelNum, txtIdNum, txtPosition, txtNumber);
            CheckComboBoxNotNull(ref IsOk, cbGender, cbMarriage,
                cbPartyStatus, cbEducation, cbDepatment);
            if (!IsOk)
            {
                return;
            }
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("请输入正确email！");
                return;
            }
            try
            {
                if (IsAddNew)
                {
                    Employee employee = (Employee)gridEmployee.DataContext;
                    if (employee.Photo == null)
                    {
                        MessageBox.Show("请选择照片!");
                        return;
                    }
                  
                    EmployeeDAL.Insert(employee);
                    Guid operatorid = CommonHelper.GetOperatorId();
                    T_OperaLogDAL.Insert(operatorid, "新增职员:" + employee.Name);
                }
                else
                {
                    Employee employee = (Employee)gridEmployee.DataContext;
                    EmployeeDAL.Update(employee);
                    Guid operatorid = CommonHelper.GetOperatorId();
                    T_OperaLogDAL.Insert(operatorid, "修改职员:" + employee.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DialogResult = true;
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "JPG图片|*.jpg|PNG图片|*.png";
            if (op.ShowDialog().Value)
            {
                Employee employee = (Employee)gridEmployee.DataContext;
                string filename = op.FileName;
                employee.Photo = File.ReadAllBytes(filename);
                imgPhoto.Source = new BitmapImage(new Uri(filename));
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            CaptureWindow win = new CaptureWindow();

            if (win.ShowDialog() == true)
            {
                byte[] data = win.CaptureData;
                ShowImg(data);
                Employee employee = (Employee)gridEmployee.DataContext;
                employee.Photo = data;
            }
        }
    }
}