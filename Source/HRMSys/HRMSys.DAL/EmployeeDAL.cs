using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMSys.Model;

namespace HRMSys.DAL
{
    public class EmployeeDAL
    {
        public static Employee ToModel(DataRow row)
        {
            Employee employee = new Employee();
            employee.Address = (string)row["Address"];
            employee.BaseSalary = (int)row["BaseSalary"];
            employee.BirthDay = (DateTime)row["BirthDay"];
            employee.ContractEndDay = (DateTime)row["ContractEndDay"];
            employee.ContractStartDay = (DateTime)row["ContractStartDay"];
            employee.DepartmentId = (Guid)row["DepartmentId"];
            employee.EducationId = (Guid)row["EducationId"];
            employee.Email = (string)row["Email"];
            employee.EmergencyContact = (string)SqlHelper.FromDbValue(row["EmergencyContact"]);
            employee.GenderId = (Guid)row["GenderId"];
            employee.Id = (Guid)row["Id"];
            employee.IdNum = (string)row["IdNum"];
            employee.InDate = (DateTime)row["InDate"];
            employee.Major = (string)row["Major"];
            employee.MarriageId = (Guid)row["MarriageId"];
            employee.Name = (string)row["Name"];
            employee.Nationality = (string)row["Nationality"];
            employee.NativeAddr = (string)row["NativeAddr"];
            employee.Number = (string)row["Number"];
            employee.PartyStatusId = (Guid)row["PartyStatusId"];
            employee.Position = (string)row["Position"];
            employee.Remarks = (string)SqlHelper.FromDbValue(row["Remarks"]);
            employee.Resume = (string)SqlHelper.FromDbValue(row["Resume"]);
            employee.School = (string)SqlHelper.FromDbValue(row["School"]);
            employee.TelNum = (string)row["TelNum"];

            //todo:如果员工非常多，那么Photo会增加内存占用
            employee.Photo = (byte[])SqlHelper.FromDbValue(row["Photo"]);
            return employee;
        }

        public static Employee[] ListAll()
        {
            DataTable table = SqlHelper.ExecuteDataSet("select * from T_Employee where IsStopped=0");
            Employee[] items = new Employee[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                items[i] = ToModel(table.Rows[i]);
            }
            return items;
        }

        public static void Delete(Guid id)
        {
            SqlHelper.ExecuteNonQuery("Update T_Employee set IsStopped=1 where Id=@Id",
            new SqlParameter("@Id", id));
        }

        public static Employee GetById(Guid id)
        {
            DataTable table = SqlHelper.ExecuteDataSet("select * from T_Employee where Id=@Id",
            new SqlParameter("@Id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count == 1)
            {
                return ToModel(table.Rows[0]);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Insert(Employee employee)
        {
            SqlHelper.ExecuteNonQuery(@"INSERT INTO [T_Employee]
           ([Id],[Number],[Name],[BirthDay],[InDate],[MarriageId],[PartyStatusId],[Nationality]
           ,[NativeAddr],[EducationId],[Major],[School],[Address],[BaseSalary],[Email]
           ,[IdNum],[TelNum],[EmergencyContact],[DepartmentId],[Position],[ContractStartDay]
           ,[ContractEndDay],[Resume],[Remarks],[IsStopped],[GenderId],Photo)
            VALUES(newid(),@Number,@Name,@BirthDay,@InDate,@MarriageId,@PartyStatusId,@Nationality
           ,@NativeAddr,@EducationId,@Major,@School,@Address,@BaseSalary,@Email
           ,@IdNum,@TelNum,@EmergencyContact,@DepartmentId,@Position,@ContractStartDay
           ,@ContractEndDay,@Resume,@Remarks,0,@GenderId,@Photo)", new SqlParameter("@Number", employee.Number)
                                                          , new SqlParameter("@Name", employee.Name)
                                                          , new SqlParameter("@BirthDay", employee.BirthDay)
                                                          , new SqlParameter("@InDate", employee.InDate)
                                                          , new SqlParameter("@MarriageId", employee.MarriageId)
                                                          , new SqlParameter("@PartyStatusId", employee.PartyStatusId)
                                                          , new SqlParameter("@Nationality", employee.Nationality)
                                                          , new SqlParameter("@NativeAddr", employee.NativeAddr)
                                                          , new SqlParameter("@EducationId", employee.EducationId)
                                                          , new SqlParameter("@Major", SqlHelper.ToDbvalue(employee.Major))
                                                          , new SqlParameter("@School", SqlHelper.ToDbvalue(employee.School))
                                                          , new SqlParameter("@Address", employee.Address)
                                                          , new SqlParameter("@BaseSalary", employee.BaseSalary)
                                                          , new SqlParameter("@Email", SqlHelper.ToDbvalue(employee.Email))
                                                          , new SqlParameter("@IdNum", employee.IdNum)
                                                          , new SqlParameter("@TelNum", employee.TelNum)
                                                          , new SqlParameter("@EmergencyContact", SqlHelper.ToDbvalue(employee.EmergencyContact))
                                                          , new SqlParameter("@DepartmentId", employee.DepartmentId)
                                                          , new SqlParameter("@Position", employee.Position)
                                                          , new SqlParameter("@ContractStartDay", employee.ContractStartDay)
                                                          , new SqlParameter("@ContractEndDay", employee.ContractEndDay)
                                                          , new SqlParameter("@Resume", SqlHelper.ToDbvalue(employee.Resume))
                                                          , new SqlParameter("@Remarks", SqlHelper.ToDbvalue(employee.Remarks))
                                                          , new SqlParameter("@GenderId", employee.GenderId)
                                                          , new SqlParameter("@Photo", SqlHelper.ToDbvalue(employee.Photo)));
        }

        public static void Update(Employee employee)
        {
            SqlHelper.ExecuteNonQuery(@"Update T_Employee set
                [Number]=@Number,[Name]=@Name,[BirthDay]=@BirthDay,[InDate]=@InDate,
                [MarriageId]=@MarriageId,[PartyStatusId]=@PartyStatusId,[Nationality]=@Nationality,
                [NativeAddr]=@NativeAddr,[EducationId]=@EducationId,[Major]=@Major,[School]=@School,
                [Address]=@Address,[BaseSalary]=@BaseSalary,[Email]=@Email,
                [IdNum]=@IdNum,[TelNum]=@TelNum,[EmergencyContact]=@EmergencyContact,
                [DepartmentId]=@DepartmentId,[Position]=@Position,[ContractStartDay]=@ContractStartDay,
                [ContractEndDay]=@ContractEndDay,[Resume]=@Resume,[Remarks]=@Remarks,[GenderId]=@GenderId
                ,Photo=@Photo
                Where Id=@Id", new SqlParameter("@Number", employee.Number)
            , new SqlParameter("@Name", employee.Name)
            , new SqlParameter("@BirthDay", employee.BirthDay)
            , new SqlParameter("@InDate", employee.InDate)
            , new SqlParameter("@MarriageId", employee.MarriageId)
            , new SqlParameter("@PartyStatusId", employee.PartyStatusId)
            , new SqlParameter("@Nationality", employee.Nationality)
            , new SqlParameter("@NativeAddr", employee.NativeAddr)
            , new SqlParameter("@EducationId", employee.EducationId)
            , new SqlParameter("@Major", SqlHelper.ToDbvalue(employee.Major))
            , new SqlParameter("@School", SqlHelper.ToDbvalue(employee.School))
            , new SqlParameter("@Address", employee.Address)
            , new SqlParameter("@BaseSalary", employee.BaseSalary)
            , new SqlParameter("@Email", SqlHelper.ToDbvalue(employee.Email))
            , new SqlParameter("@IdNum", employee.IdNum)
            , new SqlParameter("@TelNum", employee.TelNum)
            , new SqlParameter("@EmergencyContact", SqlHelper.ToDbvalue(employee.EmergencyContact))
            , new SqlParameter("@DepartmentId", employee.DepartmentId)
            , new SqlParameter("@Position", employee.Position)
            , new SqlParameter("@ContractStartDay", employee.ContractStartDay)
            , new SqlParameter("@ContractEndDay", employee.ContractEndDay)
            , new SqlParameter("@Resume", SqlHelper.ToDbvalue(employee.Resume))
            , new SqlParameter("@Remarks", SqlHelper.ToDbvalue(employee.Remarks))
            , new SqlParameter("@GenderId", employee.GenderId)
            , new SqlParameter("@Photo", SqlHelper.ToDbvalue(employee.Photo))
            , new SqlParameter("@Id", employee.Id));
        }
        /// <summary>
        /// 查询部门所有员工
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Employee[] ListAllDept(Guid deptid)
        {
            DataTable table = SqlHelper.ExecuteDataSet("select *from T_Employee where DepartmentId=@DepartmentId",
                new SqlParameter("@DepartmentId", deptid));
            return ToEmployees(table);

        }
        public static Employee[] Search(string sql, List<SqlParameter> parameters)
        {
            DataTable table = SqlHelper.ExecuteDataSet(sql, parameters.ToArray());
            Employee[] items = new Employee[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                items[i] = ToModel(table.Rows[i]);
            }
            return items;
        }
        private static Employee[] ToEmployees(DataTable table)
        {
            Employee[] items = new Employee[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                items[i] = ToModel(table.Rows[i]);
            }
            return items;
        }
        //搜索三天之内过生日的员工
        public Employee[] Search3DaysBirthDay()
        {
            string sql = @"select * from T_Employee
where DATEDIFF(day,GetDate(),Convert(varchar(5),DatePart(yyyy,GetDate()))+
		'-'+Convert(varchar(5),DATEPART(mm,BirthDay))
		+'-'+Convert(varchar(5),DATEPART(dd,BirthDay))) between 0 and 3";
            DataTable table = SqlHelper.ExecuteDataSet(sql);
            return ToEmployees(table);
        }
       

    }
}