using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMSys.Model;
using System.Data;
using System.Data.SqlClient;

namespace HRMSys.DAL
{
    public class T_OperaLogDAL
    {
        private static T_OperaLog ToModel(DataRow row)
        {
            T_OperaLog model = new T_OperaLog();
            model.Id = (System.Guid)row["Id"];
            model.OperatorId = (System.Guid)row["OperatorId"];
            model.MakeDate = (System.DateTime)row["MakeDate"];
            model.ActionDesc = (string)row["ActionDesc"];
            return model;
        }

        public static  void Insert(Guid operatorId,string actionDesc)
        {
  
            SqlHelper.ExecuteNonQuery("insert into T_OperationLog (Id,OperatorId,MakeDate,ActionDesc) values (newid(),@operatorId,getdate(),@ActionDesc)",
                new SqlParameter("@operatorId", operatorId),

                new SqlParameter("@ActionDesc", actionDesc));
        }
        public static T_OperaLog[] Search(string sql,SqlParameter[] parameters)
        {
            DataTable table = SqlHelper.ExecuteDataSet(sql, parameters);
            T_OperaLog[] logs = new T_OperaLog[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {

                logs[i] = ToModel(table.Rows[i]);
            }
            return logs;

 
        }
        public static T_OperaLog[] GetAll()
        {

            DataTable table = SqlHelper.ExecuteDataSet("select *from T_OperationLog");
            T_OperaLog[] items = new T_OperaLog[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count;i++ )
            {
                items[i] = ToModel(table.Rows[i]);
            }
            return items;
        }
        public static void  Deletelog()
        {
            SqlHelper.ExecuteNonQuery("delete from T_OperationLog");

        }



    }
}
