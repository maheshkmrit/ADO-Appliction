using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace SampleDataAccessApp
{
   
    class TransactionDemo
    {
        static readonly string strConnection = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;

        private static void addEmployee(string name, string address, int salary, string deptName, int managerId)
        {
            SqlTransaction transaction = null;
            SqlConnection con = new SqlConnection(strConnection);
            string cmdGetDeptId = $"Select dbo.GetDept('{deptName}') as DeptId";
            string cmdInsertDept = "InsertDept";
            int depId = 0;
            try
            {
                con.Open();
                transaction = con.BeginTransaction();
                
                
                
                SqlCommand cmd1 = new SqlCommand(cmdGetDeptId, con, transaction);
                depId = (int)cmd1.ExecuteScalar();
                if (depId == 0)
                {
                    //2nd Operation: Inserting the DeptName
                    SqlCommand cmd2 = new SqlCommand(cmdInsertDept, con, transaction);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@deptName", deptName);
                    cmd2.Parameters.AddWithValue("@deptId", 0);
                    cmd2.Parameters[1].Direction = ParameterDirection.Output;
                    cmd2.ExecuteNonQuery();
                    depId = (int)cmd2.Parameters[1].Value;
                }




                
                SqlCommand cmd3 = new SqlCommand("INSERTEMPLOYEE", con, transaction);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@empName", name);
                cmd3.Parameters.AddWithValue("@empAddress", address);
                cmd3.Parameters.AddWithValue("@empSalary", salary);
                cmd3.Parameters.AddWithValue("@deptId", depId);
                cmd3.Parameters.AddWithValue("@managerId", managerId);
                cmd3.Parameters.AddWithValue("@empId", 0);
                cmd3.Parameters[5].Direction = ParameterDirection.Output;
                cmd3.ExecuteNonQuery();
                transaction.Commit();
                Console.WriteLine("The Employee has been added to the database that got ID as " + cmd3.Parameters[5].Value);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw ex;
            }
        }
        static void Main(string[] args)
        {
            addEmployee("Rakesh", "Chandigadh", 75000, "Sales", 1005);
        }
    }
}