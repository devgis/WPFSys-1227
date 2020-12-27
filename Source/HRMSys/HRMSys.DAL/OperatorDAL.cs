using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMSys.Model;

namespace HRMSys.DAL
{
    public class OperatorDAL
    {
        public static void DeletebyId(Guid id)
        {
            SqlHelper.ExecuteNonQuery("update T_Operator set IsDelete=1 where Id=@Id",
                new SqlParameter("@Id", id));
        }

        public static Operator[] GetAll()
        {
            DataTable dt = SqlHelper.ExecuteDataSet("Select *from T_Operator where IsDelete=0");

            Operator[] operators = new Operator[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                operators[i] = ToOperator(dt.Rows[i]);
            }
            return operators;
        }

        public static void Insert(Operator op)
        {
            SqlHelper.ExecuteNonQuery("Insert into T_Operator (Id,UserName,UserPass,IsDelete,RealName,IsLocked) values(newid(),@UserName,@UserPass,0,@RealName,0)",
                new SqlParameter("@UserName", op.UserName), new SqlParameter("@UserPass", op.UserPass), new SqlParameter("RealName", op.RealName));
        }

        private static Operator ToOperator(DataRow row)
        {
            Operator op = new Operator();
            op.Id = (Guid)row["Id"];
            op.UserName = (string)row["UserName"];
            op.UserPass = (string)row["UserPass"];
            op.IsDelete = (bool)row["IsDelete"];
            op.RealName = (string)row["RealName"];
            op.IsLocked = (bool)row["IsLocked"];
            return op;
        }

        /// <summary>
        /// 不更新密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        public static void Update(Guid id, string userName, string realName)
        {
            SqlHelper.ExecuteNonQuery("Update T_Operator set UserName=@UserName,RealName=@RealName where Id=@Id",
                new SqlParameter("@UserName", userName), new SqlParameter("@RealName", realName),
                new SqlParameter("@Id", id));
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="pwd"></param>
        public static void Update(Guid id, string userName, string realName, string pwd)
        {
            SqlHelper.ExecuteNonQuery("Update T_Operator set UserName=@UserName,RealName=@RealName,UserPass=@UserPass where Id=@Id",
                           new SqlParameter("@UserName", userName), new SqlParameter("@RealName", realName),
                           new SqlParameter("@UserPass", pwd), new SqlParameter("@Id", id));
        }

        public static Operator GetById(Guid id)
        {
            DataTable table = SqlHelper.ExecuteDataSet("select *from T_Operator where  Id=@Id",
             new SqlParameter("@Id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            if (table.Rows.Count > 1)
            {
                throw new Exception("存在重复的Id!");
            }
            else
            {
                return ToOperator(table.Rows[0]);
            }
        }

        public static Operator GetByUserName(string userName)
        {
            DataTable table = SqlHelper.ExecuteDataSet("select *from T_Operator where UserName=@UserName and IsDelete=0",
                new SqlParameter("@UserName", userName));

            if (table.Rows.Count <= 0)
            {
                return null;
            }
            if (table.Rows.Count > 1)
            {
                throw new Exception("存在重复的用户名!");
                
            }
            else
            {
                return ToOperator(table.Rows[0]);
            }
        }
        public static int CheckRepeat(string userName)
        {
            int count = SqlHelper.ExecuteScalar("select count(*) from T_Operator where UserName=@UserName and IsDelete=0",
                new SqlParameter("UserName", userName));
            return count;
            
            
        }
    }
}