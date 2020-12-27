using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using HRMSys.Model;
using System.Data;

namespace HRMSys.DAL
{
    public class SalarySheetDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 

        public bool IsExits(int year, int month, Guid depaertid)
        {

            object obj = SqlHelper.ExecuteScalar1(@"select count(*) from T_SalarySheet where Year=@Year and Month=@Month
                 and DePartmentId=@DePartmentId",
                                                  new SqlParameter("@Year", year),
                                                  new SqlParameter("@Month", month),
                                                  new SqlParameter("@DePartmentId", depaertid));
            return Convert.ToInt32(obj) > 0;



        }
        /// <summary>
        /// 清理已经生成的工资单
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="depaertid"></param>
        public void Clear(int year, int month, Guid depaertid)
        {
            object obj = SqlHelper.ExecuteScalar1(@"select Id from T_SalarySheet where Year=@Year and Month=@Month
              and   DePartmentId=@DePartmentId",
                                          new SqlParameter("@Year", year),
                                          new SqlParameter("@Month", month),
                                          new SqlParameter("@DePartmentId", depaertid));
            Guid sheetId = (Guid)obj;
            SqlHelper.ExecuteNonQuery("delete from T_SalarySheetItem where sheetId=@sheetId",
                new SqlParameter("@sheetId", sheetId));
            SqlHelper.ExecuteNonQuery("delete from T_SalarySheet where Id=@Id",
               new SqlParameter("@Id", sheetId));



        }
        public void Bulid(int year, int month, Guid dept)
        {
            Guid sheetId = Guid.NewGuid();
            SqlHelper.ExecuteNonQuery("insert into T_SalarySheet values(@Id,@Year,@Month,@DePartmentId)",
           new SqlParameter("@Id", sheetId),

           new SqlParameter("@Year", year),
           new SqlParameter("@Month", month),
           new SqlParameter("@DePartmentId", dept));

            Employee[] employees = EmployeeDAL.ListAllDept(dept);
            foreach (Employee employee in employees)
            {
                SqlHelper.ExecuteNonQuery(@"insert into T_SalarySheetItem values (newid(),@SheetId,@EmployeeId,0,
                    0,0,0)", new SqlParameter("@SheetId", sheetId),
                           new SqlParameter("@EmployeeId", employee.Id));

            }


        }
        public static SalarySheetItem[] GetSalarySheetItem(int year, int month, Guid deptid)
        {

            DataTable tableMain = SqlHelper.ExecuteDataSet(@"select * from T_SalarySheet where Year=@Year and Month=@Month
                 and DePartmentId=@DePartmentId",
                                         new SqlParameter("@Year", year),
                                         new SqlParameter("@Month", month),
                                         new SqlParameter("@DePartmentId", deptid));
            //先查主表 Id，再根据主表Id查询字表信息！
            if (tableMain.Rows.Count == 1)
            {
                Guid sheetId = (Guid)tableMain.Rows[0]["Id"];
                DataTable table = SqlHelper.ExecuteDataSet(@"select * from T_SalarySheetItem where
              SheetId=@SheetId", new SqlParameter("@SheetId", sheetId));

                SalarySheetItem[] items = new SalarySheetItem[table.Rows.Count];

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    SalarySheetItem salary = new SalarySheetItem();
                    DataRow row = table.Rows[i];
                    salary.Id = (Guid)row["Id"];
                    salary.BaseSalary = (decimal)row["BaseSalary"];
                    salary.Bounds = (decimal)row["Bounds"];
                    salary.EmployeeId = (Guid)row["EmployeeId"];
                    salary.Fine = (decimal)row["Fine"];
                    salary.Other = (decimal)row["Other"];
                    salary.SheetId = (Guid)row["SheetId"];
                    items[i] = salary;


                }
                return items;
            }
            else if (tableMain.Rows.Count <= 0)
            {
                return new SalarySheetItem[0];

            }
            else
            {
                throw new Exception("ERROR");
            }





        }
        public static void Update(SalarySheetItem item)
        {
            SqlHelper.ExecuteNonQuery(@"update T_SalarySheetItem set BaseSalary=@BaseSalary,
           Bounds=@Bounds,Fine=@Fine,Other=@Other where Id=@Id",
                                                               new SqlParameter("@BaseSalary", item.BaseSalary),
                                                               new SqlParameter("@Bounds", item.Bounds),
                                                               new SqlParameter("@Fine", item.Fine),
                                                               new SqlParameter("@Other", item.Other),
                                                               new SqlParameter("@Id", item.Id));


        }


    }
}
