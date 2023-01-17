using SampleDataAccessA.Practicle.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SampleDataAccessA.Practicle
{
    class PatientManagerApp
    {
    }

    namespace Entities
    {
        class Doctor
        {
            public int DocId { get; set; }
            public string DocName { get; set; }
            public int DocAge { get; set; }
            public string DocAddress { get; set; }
        }

        class Patient
        {
            public int PatId { get; set; }
            public string PatName { get; set; }
            public int PatAge { get; set; }
            public string PatDisease { get; set; }
            public string PatAddress { get; set; }
            public int DocId { get; set; }



        }
    }

    namespace Dalayer
    {
        interface IDataAccessComponent
        {
            void AddNewDoctor(Doctor doc);
            void UpdateDoctor(Doctor doc);
            void DeleteDoctor(int id);
            List<Doctor> GetAllDoctors();
            List<Patient> GetAllPatients();

            void AddNewPatient(Patient pat);
            void UpdatePatient(Patient pat);
            void DeletePatient(int id);
        }
        class DataComponent : IDataAccessComponent
        {
            private string strCon = string.Empty;

            #region AllStatements
            const string STRINSERT = "InsertDoctor";
            const string STRUPDATE = "Update tblDoctor Set DocName = @docName, DocAge = @DocAge, DocAddress = @docAddress WHERE DocId = @docId";
            const string STRALL = "SELECT * FROM tblDoctor";
            const string STRALLDEPTS = "SELECT * FROM tblDoctor";
            const string STRDELETE = "DELETE FROM tblDoctor WHERE docId = @id";



            const string STRINSERTPATIENT = "InsertPatient";
            const string STRUPDATEPATIENT = "Update tblPatient Set PatName = @patName, PatAge = @patAge, PatDisease = @patDisease, PatAddress = @patAddress, DocId = @docId WHERE PatId = @patId";
            const string STRALLPATIENT = "SELECT * FROM tblPatient";
            const string STRDELETEPATIENT = "DELETE FROM tblPatient WHERE patId = @id";



            #endregion

            #region HELPERS
            private void NonQueryExecute(string query, SqlParameter[] parameters, CommandType type)
            {
                SqlConnection con = new SqlConnection(strCon);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = type;
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }


            private DataTable GetRecords(string query, SqlParameter[] parameters, CommandType type = CommandType.Text)
            {
                SqlConnection con = new SqlConnection(strCon);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = type;
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                try
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    DataTable table = new DataTable("Records");
                    table.Load(reader);
                    return table;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            #endregion


            public DataComponent(string connectionString)
            {
                strCon = connectionString;
            }

            #region IDataAccessComponentImpl
            public void AddNewDoctor(Doctor doc)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@docName", doc.DocName));
                parameters.Add(new SqlParameter("@docAge", doc.DocAge));
                parameters.Add(new SqlParameter("@docAddress", doc.DocAddress));
                parameters.Add(new SqlParameter("@docId", doc.DocId));

                try
                {
                    NonQueryExecute(STRINSERT, parameters.ToArray(), CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void DeleteDoctor(int id)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@id", id));

                try
                {
                    NonQueryExecute(STRDELETE, parameters.ToArray(), CommandType.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            

            public List<Doctor> GetAllDoctors()
            {
                var table = GetRecords(STRALL, null);
                List<Doctor> doclist = new List<Doctor>();
                foreach (DataRow row in table.Rows)
                {

                    Doctor doc = new Doctor
                    {
                        DocId = (int)row[0],
                        DocName = row[1].ToString(),
                        DocAddress = row[2].ToString(),
                        
                    };
                    doclist.Add(doc);
                }
                return doclist;
            }

            public void UpdateDoctor(Doctor doc)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@docName", doc.DocName));
                parameters.Add(new SqlParameter("@docAge", doc.DocAge));
                parameters.Add(new SqlParameter("@docAddress", doc.DocAddress));
                parameters.Add(new SqlParameter("@docId", doc.DocId));

                try
                {
                    NonQueryExecute(STRUPDATE, parameters.ToArray(), CommandType.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public List<Patient> GetAllPatients()
            {
                var table = GetRecords(STRALLPATIENT, null);
                List<Patient> patlist = new List<Patient>();
                foreach (DataRow row in table.Rows)
                {

                    Patient pat = new Patient
                    {
                        PatId = (int)row[0],
                        PatName = row[1].ToString(),
                        PatAge = (int)row[2],
                        PatDisease = row[3].ToString(),
                        PatAddress = row[4].ToString()



                    };
                    patlist.Add(pat);
                }
                return patlist;
                
            }

            public void AddNewPatient(Patient pat)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("@patName", pat.PatName));
                parameters.Add(new SqlParameter("@patAge", pat.PatAge));
                parameters.Add(new SqlParameter("@patDisease", pat.PatDisease));
                parameters.Add(new SqlParameter("@patAddress", pat.PatAddress));
                parameters.Add(new SqlParameter("@docId", pat.DocId));
                parameters.Add(new SqlParameter("@patId", pat.PatId));

                try
                {
                    NonQueryExecute(STRINSERTPATIENT, parameters.ToArray(), CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //throw new NotImplementedException();
            }

            public void UpdatePatient(Patient pat)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@patName", pat.PatName));
                parameters.Add(new SqlParameter("@patAge", pat.PatAge));
                parameters.Add(new SqlParameter("@patDisease", pat.PatDisease));
                parameters.Add(new SqlParameter("@patAddress", pat.PatAddress));
                parameters.Add(new SqlParameter("@docId", pat.DocId));
                parameters.Add(new SqlParameter("@patId", pat.PatId));

                try
                {
                    NonQueryExecute(STRUPDATEPATIENT, parameters.ToArray(), CommandType.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //throw new NotImplementedException();
            }

            public void DeletePatient(int id)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@id", id));

                try
                {
                    NonQueryExecute(STRDELETEPATIENT, parameters.ToArray(), CommandType.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //throw new NotImplementedException();
            }

            #endregion
        }
    }
    namespace UILayer
    {
        using SampleDataAccessA.Practicle.Dalayer;
        using SimpleFrameWorkApp;
        using System.Configuration;
        class MainProgram
        {
            static IDataAccessComponent component = null;
            static string connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            public static void MyUI()
            {
                string MenuHeader = "\t\t\tPatient Manager Application\t\t\t\n";
                string Menu = "Press 1 ------------------- to Add Patient\n" +
                    "Press 2 ------------------- to View All Patient\n" +
                    "Press 3 ------------------- to Delete Patient\n" +
                    "Press 4 ------------------- to Update Patient\n" +
                    "Press x ------------------- to Exit";

                bool res = true;

                while(res)
                {
                    Console.WriteLine(MenuHeader);
                    Console.WriteLine(Menu);
                RETRY:
                    string choice = Console.ReadLine().ToLower();
                    switch (choice)
                    {
                        case "1":
                            string name = Utilities.Prompt("Enter the Patient Name");
                            int age = Utilities.GetNumber("Enter patient Age");
                            string disease = Utilities.Prompt("Enter Disease");
                            string address = Utilities.Prompt("Enter patient Address");
                            Console.WriteLine("Enter Id From Listed Doctor");
                            Console.WriteLine("Doctor Name     Doctor Id");
                            var data = component.GetAllDoctors();
                            foreach (var doc in data)
                                Console.WriteLine($"{ doc.DocName}         { doc.DocId}");

                            int docId = Utilities.GetNumber("Enter Doctor ID");
                            component.AddNewPatient(new Patient
                            {   PatName = name,
                                PatAge = age,
                                PatDisease = disease,
                                PatAddress = address,
                                DocId = docId
                                
                            });
                            Console.WriteLine("Patient Added Successful");

                            break;
                            
                        case "2":
                            var patData = component.GetAllPatients();
                            foreach (var pat in patData)
                                Console.WriteLine($"Patient Name: { pat.PatName}\nPatient Age: {pat.PatAge}\nPatient Disease: {pat.PatDisease}\nPatient Address: {pat.PatAddress}\n");
                            break;
                            
                        case "3":
                            int id = Utilities.GetNumber("Enter patient id to Delete");
                            component.DeletePatient(id);
                            break;
                        case "4":
                            int NewId = Utilities.GetNumber("Enter Patient to Update for Patient Updation");
                            string Newname = Utilities.Prompt("Enter the Patient Name to Update");
                            int Newage = Utilities.GetNumber("Enter patient Age to Update");
                            string Newdisease = Utilities.Prompt("Enter Disease to Update");
                            string Newaddress = Utilities.Prompt("Enter patient Address to Update");
                            Console.WriteLine("Enter Id From Listed Doctor");
                            Console.WriteLine("Doctor Name     Doctor Id");
                            var Olddata = component.GetAllDoctors();
                            foreach (var doc in Olddata)
                                Console.WriteLine($"{ doc.DocName}         { doc.DocId}");

                            int NewdocId = Utilities.GetNumber("Enter Doctor ID to Update");

                            component.UpdatePatient(
                                new Patient
                                {
                                    PatName = Newname,
                                    PatAge = Newage,
                                    PatDisease = Newdisease,
                                    PatAddress = Newaddress,
                                    PatId = NewId,
                                    DocId = NewdocId
                                });

                            break;
                        case "x":
                            res = false;
                            break;
                        default:
                            break;
                    }

                }
            }
            
            static void Main(string[] args)
            {
                component = new DataComponent(connectionString);
                //component.AddNewEmployee(new Employee
                //{
                //    EmpName = Utilities.Prompt("Enter the EmpName"),
                //    EmpAddress = Utilities.Prompt("Enter the Address"),
                //    EmpSalary = Utilities.GetNumber("Enter the salary"),
                //    DeptId = Utilities.GetNumber("Enter the DeptId")
                //});

                //component.UpdateEmployee(new Employee
                //{
                //    EmpId = 1196,
                //    EmpName = "Samuel L Jackson",
                //    EmpAddress = "LA",
                //    EmpSalary = 60000
                //});

                //component.DeleteEmployee(1196);
                //var data = component.GetAllDepts();
                //foreach(var dept in data)
                //    Console.WriteLine(dept.DeptName);

                //var data = component.GetAllDoctors();
                //foreach (var doc in data)
                //    Console.WriteLine(doc.DocName);

                //component.AddNewDoctor(
                //    new Doctor
                //    {
                //        DocName = "Rk Batra",
                //        DocAge = 34,
                //        DocAddress = "Himachal"
                //    }
                //    );

                //component.AddNewPatient(
                //    new Patient {
                //    PatName="Kally",
                //    PatAge=38,
                //    PatDisease="Nausea",
                //    PatAddress="Huston",
                //    DocId=100});
                //MyUI();
                //component.UpdateDoctor(
                //    new Doctor {
                //        DocName = "Austin",
                //        DocAge=34,
                //        DocAddress="Chichago",
                //        DocId = 104
                //    });

                //Console.WriteLine("Updated Successful");

                //component.UpdatePatient(
                //    new Patient
                //    {
                //        PatName = "Balke",
                //        PatAddress = "Scotland",
                //        PatDisease = "Dizziness",
                //        DocId = 102,
                //        PatAge = 35,
                //        PatId = 3
                //    });
                //Console.WriteLine("Updated Successful");
                MyUI();


            }
        }
    }
}
