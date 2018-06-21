using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;

namespace GRC_Clinical_Genetics_Application
{
    class Connections
    {
        public SqlConnection GRC_Connection = new SqlConnection("Server=WSSQLC011N02\\TSTINST01;database=GRC;integrated security=true;");
        private string sConnection = Properties.Settings.Default.GRCConnectionString;
        public Connections()
        {
            
        }

        public SqlCommand LoginCommand(string user)
        {
            SqlCommand loginCommand = new SqlCommand("Select [login], [password], [PwdReset], [ID] from [GRC].[dbo].[Employees] where [login]='" + user + "' ", GRC_Connection);
            return loginCommand;
        }

        public SqlCommand NameCommand(int id)
        {
            SqlCommand cmd = new SqlCommand("Select [ID], [First Name], [Last Name] from [GRC].[dbo].[Employees] where id =" + id , GRC_Connection);
            return cmd;
        }

        public SqlCommand UpdateCommand(string newPass, string user)
        {
            SqlCommand updateCommand = new SqlCommand("Update [GRC].[dbo].[Employees] SET [password] = '" + newPass + "', [PwdReset] = 0 where [login] = '" + user + "' ", GRC_Connection);
            return updateCommand;
        }

        public SqlDataAdapter getDefaultDatatable(int id)
        {
            SqlDataAdapter dataTable = new SqlDataAdapter("SELECT [GRC ID], [Status Name] as 'Status', [Patients].[Last Name] + ', ' + [Patients].[First Name] as 'Patient', [Patients].[Personal Health Number] as 'PHN',  CONVERT(VARCHAR(10), Patients.DOB , 126) as 'Date of Birth', CASE when[IsUrgent] = 1 then 'Yes' else 'No' end as 'Is Urgent?', [Received Date] as 'Date Sent' , [Employees].[First Name] + ' ' + Employees.[Last Name] as 'Sent by' FROM [GRC].[dbo].[Orders], [GRC].[dbo].Patients, [GRC].[dbo].[Orders Status], [GRC].[dbo].employees where [Employee ID] = 30 and [Patient ID] = [GRC].[dbo].Patients.ID and [GRC].[dbo].[Orders].[Status ID] = [GRC].[dbo].[Orders Status].[Status ID] and [Employee ID] = [GRC].[dbo].employees.ID", GRC_Connection);
            return dataTable;
        }

        public SqlDataAdapter getCustomDatatable(string GRCnum = "", string status = "", string patientFirstName = "", string patientLastName = "", int PHN = 0, bool isUrgent = false, bool showAll = false)
        {
            string cmdString = "SELECT[GRC ID], [Status Name] as 'Status', [Patients].[Last Name] + ', ' + [Patients].[First Name] as 'Patient', [Patients].[Personal Health Number] as 'PHN',  CONVERT(VARCHAR(10), Patients.DOB , 126) as 'Date of Birth', CASE when[IsUrgent] = 1 then 'Yes' else 'No' end as 'Is Urgent?', [Received Date] as 'Date Sent' , [Employees].[First Name] + ' ' + Employees.[Last Name] as 'Sent by' FROM[GRC].[dbo].[Orders], [GRC].[dbo].Patients, [GRC].[dbo].[Orders Status], [GRC].[dbo].employees where[Employee ID] = 30 and[Patient ID] = [GRC].[dbo].Patients.ID and[GRC].[dbo].[Orders].[Status ID] = [GRC].[dbo].[Orders Status].[Status ID] and [Employee ID] = [GRC].[dbo].employees.ID";
            
            //if not empty, add the 'and ______ = _______' to the end of the cmd string 

            SqlDataAdapter dataTable = new SqlDataAdapter(cmdString, GRC_Connection);
            return dataTable;
        }

    }
}



////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////* SQL COMMANDS *//////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////
//
// OPEN CONNECTION FIRST AND ALWAYS CLOSE CONNECTION AFTER USE
//
// READ: SqlCommand cmd = connection._________Command(obj);
//       SqlDataReader sdr = cmd.ExecuteReader();
//       while (sdr.Read()){
//           sdr[] is the array with the columns of data
//       }
//
// UPDATE: SqlCommand cmd = connection.________Command(obj); 
//         cmd.ExecuteNonQuery();
//