using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;


namespace HRMSys.UI
{
    internal class CommonHelper
    {
        public static string getMd5(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        //把一些可变的值写入appconfig.

        public static string GetPasswordSalt()
        {
            string salt = ConfigurationManager.AppSettings["passwordSalt"];
            return salt;
        }
        //获得登陆者的Id
        public static Guid GetOperatorId()
        {
            Guid operatorid = (Guid)Application.Current.Properties["OperatorId"];
            return operatorid;
        }
    }
}