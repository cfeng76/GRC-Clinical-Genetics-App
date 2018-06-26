using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;

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

        public SqlCommand MetricsCommand(int userID, int metricsID)
        {
            if (metricsID == 1){
                SqlCommand openCmd = new SqlCommand("SELECT COUNT([Status Name]) FROM [GRC].[dbo].[Orders], [GRC].[dbo].Patients, [GRC].[dbo].[Orders Status], [GRC].[dbo].employees where [Patient ID] = [GRC].[dbo].Patients.ID  and [GRC].[dbo].[Orders].[Status ID] = [GRC].[dbo].[Orders Status].[Status ID] and [Employee ID] = [GRC].[dbo].employees.ID and [Status Name] like '%open%' and [Employee ID] = '" + userID + "' ", GRC_Connection);
                return openCmd;
            }else
            {
                SqlCommand urgentCmd = new SqlCommand("SELECT COUNT([isUrgent]) FROM [GRC].[dbo].[Orders], [GRC].[dbo].Patients, [GRC].[dbo].[Orders Status], [GRC].[dbo].employees where [Patient ID] = [GRC].[dbo].Patients.ID  and [GRC].[dbo].[Orders].[Status ID] = [GRC].[dbo].[Orders Status].[Status ID] and [Employee ID] = [GRC].[dbo].employees.ID and IsUrgent = 1 and [Employee ID] = '" + userID + "' ", GRC_Connection);
                return urgentCmd;
            }
        }

        public SqlCommand PHNCommand()
        {
            SqlCommand phnCmd = new SqlCommand("SELECT [Personal Health Number], [First Name], [Last Name], [ZIP/Postal Code], [DOB] FROM [GRC].[dbo].[Patients]", GRC_Connection);
            return phnCmd;
        }

        public SqlCommand DemographicsCommand(string phn)
        {
            SqlCommand demCmd = new SqlCommand("SELECT [Personal Health Number], [First Name], [Last Name], [ZIP/Postal Code], [DOB] FROM [GRC].[dbo].[Patients] where [Personal Health Number] = '" + phn + "' ", GRC_Connection);
            return demCmd;
        }

        public SqlDataAdapter getDefaultDatatable(int id)
        {   //CHANGE: change [Employee ID] = 30 to the corresponding log in
            SqlDataAdapter dataTable = new SqlDataAdapter("SELECT [GRC ID], [Status Name] as 'Status', [Patients].[Last Name] + ', ' + [Patients].[First Name] as 'Patient', [Patients].[Personal Health Number] as 'PHN',  CONVERT(VARCHAR(10), Patients.DOB , 126) as 'Date of Birth', CASE when[IsUrgent] = 1 then 'Yes' else 'No' end as 'Is Urgent?', CASE when Orders.[Paperwork Received Date] IS NULL then 'No' else 'Yes' end as 'Paperwork Received?', [Received Date] as 'Application Submission Date' , [Employees].[First Name] + ' ' + Employees.[Last Name] as 'Submitted by' FROM [GRC].[dbo].[Orders], [GRC].[dbo].Patients, [GRC].[dbo].[Orders Status], [GRC].[dbo].employees where [Employee ID] = 30 and [Patient ID] = [GRC].[dbo].Patients.ID and [GRC].[dbo].[Orders].[Status ID] = [GRC].[dbo].[Orders Status].[Status ID] and [Employee ID] = [GRC].[dbo].employees.ID", GRC_Connection);
            return dataTable;
        }

        public SqlDataAdapter getCustomDatatable(string GRCnum, string status, string patientFirstName, string patientLastName, int PHN, bool isUrgent, bool showAll, int id)
        {
            string cmdString = "SELECT[GRC ID], [Status Name] as 'Status', [Patients].[Last Name] + ', ' + [Patients].[First Name] as 'Patient', [Patients].[Personal Health Number] as 'PHN',  CONVERT(VARCHAR(10), Patients.DOB , 126) as 'Date of Birth', CASE when[IsUrgent] = 1 then 'Yes' else 'No' end as 'Is Urgent?', CASE when Orders.[Paperwork Received Date] IS NULL then 'No' else 'Yes' end as 'Paperwork Received?', [Received Date] as 'Application Submission Date' , [Employees].[First Name] + ' ' + Employees.[Last Name] as 'Submitted by' FROM [GRC].[dbo].[Orders], [GRC].[dbo].Patients, [GRC].[dbo].[Orders Status], [GRC].[dbo].employees where [Patient ID] = [GRC].[dbo].Patients.ID and[GRC].[dbo].[Orders].[Status ID] = [GRC].[dbo].[Orders Status].[Status ID] and [Employee ID] = [GRC].[dbo].employees.ID";

            if (GRCnum != "")
            {
                cmdString = cmdString + " and [GRC ID] LIKE '%" + GRCnum + "%' ";
            }
            if(status != "Any")
            {
                cmdString = cmdString + " and [Status Name] LIKE '%" + status + "%' "; 
            }
            if(patientFirstName != "")
            {
                cmdString = cmdString + " and [Patients].[First Name] LIKE '%" + patientFirstName + "%'";
            }
            if (patientLastName != "")
            {
                cmdString = cmdString + " and [Patients].[Last Name] LIKE '%" + patientLastName + "%'";
            }
            if(PHN != 0)
            {
                cmdString = cmdString + " and [Patients].[Personal Health Number] LIKE '%" + PHN + "%'";
            }
            if(isUrgent == true)
            {
                cmdString = cmdString + " and [isUrgent] = 1";
            }
            if(showAll == false)
            {
                cmdString = cmdString + " and [Employee ID] = " + id.ToString();
            }

            cmdString = cmdString + " order by [Status Name]";
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