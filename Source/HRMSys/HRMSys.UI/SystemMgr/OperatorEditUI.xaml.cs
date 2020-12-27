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
    /// OperatorEditUI.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorEditUI : Window
    {
        public OperatorEditUI()
        {
            InitializeComponent();
        }

        public bool Insert { get; set; }

        public Guid Editting { get; set; }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Insert)
            {
                
                Operator op = new Operator();
                op.RealName = txtRealName.Text;
                op.UserName = txtUser.Text.Trim();
                op.UserPass = CommonHelper.getMd5(oripass.Password + CommonHelper.GetPasswordSalt());
                int count=OperatorDAL.CheckRepeat(op.UserName);
                if (count > 0)
                {
                    MessageBox.Show("用户已经存在，请勿重复添加！");
                    return;
                }
                OperatorDAL.Insert(op);
                DialogResult = true;
                Guid operatorid = CommonHelper.GetOperatorId();
                T_OperaLogDAL.Insert(operatorid, "新增操作员" + txtUser.Text);
            }
            else
            {
                string pwd = oripass.Password;
                if (pwd.Length <= 0)//编辑时候保存现有密码
                {
                    OperatorDAL.Update(Editting, txtUser.Text, txtRealName.Text);
                }
                else //把密码设置成输入新密码
                {
                    string pwdMd5 = CommonHelper.getMd5(pwd + CommonHelper.GetPasswordSalt());
                    OperatorDAL.Update(Editting, txtUser.Text, txtRealName.Text, pwdMd5);
                }
                DialogResult = true;
                Guid operatorid = CommonHelper.GetOperatorId();
                T_OperaLogDAL.Insert(operatorid, "修改操作员" + txtUser.Text);
            }
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Insert)
            {
            }
            else
            {
                Operator op = OperatorDAL.GetById(Editting);

                txtUser.Text = op.UserName;
                txtRealName.Text = op.RealName;
            }
        }
    }
}