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