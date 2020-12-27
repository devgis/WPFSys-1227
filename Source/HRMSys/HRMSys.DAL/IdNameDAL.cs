using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMSys.Model;

namespace HRMSys.DAL
{
    public class IdNameDAL
    {
        public static IdName[] GetByCategory(string category)
        {
            DataTable table = SqlHelper.ExecuteDataSet("select Id,Name from T_IdName where Category=@Category",
                new SqlParameter("@Category", category));
            IdName[] items = new IdName[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                items[i] = Tomodel(table.Rows[i]);
            }

            return items;
        }
        public static IdName Tomodel(DataRow row)
        {
            IdName idname = new IdName();
            idname.Id = (Guid)row["Id"];
            idname.Name = (string)row["Name"];
            return idname;

        }

        public static IdName GetById(Guid id)
        {
            DataTable table = SqlHelper.ExecuteDataSet("select Id,Name from T_IdName where Id=@Id",
                new SqlParameter("@Id",id));
              IdName[] items = new IdName[table.Rows.Count];
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return Tomodel(table.Rows[0]);
              
            
                
            }
          


        }
    }
}