using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMSys.Model;

namespace HRMSys.DAL
{
    public class DepartmentDAL
    {
        public static Department[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select *from T_Department where IsStopped=0");
            Department[] deps = new Department[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                deps[i] = ToModel(dt.Rows[i]);
            }
            return deps;
        }

        private static Department ToModel(DataRow row)
        {
            Department depart = new Department();
            depart.Name = (string)SqlHelper.FromDbValue( row["Name"]);
            depart.Id = (Guid)SqlHelper.FromDbValue( row["Id"]);
            return depart;
        }

        public static Department GetById(Guid id)
        {
            DataTable table = SqlHelper.ExecuteDataSet("select *from T_Department where Id=@Id",
                new SqlParameter("@Id", id));

            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return ToModel(table.Rows[0]);
            }
        }

        public static void Update(Guid id, string name)
        {
            SqlHelper.ExecuteNonQuery("Update T_Department set Name=@Name where Id=@Id",
                new SqlParameter("@Name", name),
                new SqlParameter("@Id", id));
        }

        public static void Insert(string name)
        {
            SqlHelper.ExecuteNonQuery("Insert into T_Department values(newid(),@Name,0)",
                new SqlParameter("@Name", name));
        }

        public static void Delete(Guid id)
        {
            SqlHelper.ExecuteNonQuery("Update T_Department set IsStopped=1 where Id=@Id",
                new SqlParameter("@Id", id));
        }
    }
}