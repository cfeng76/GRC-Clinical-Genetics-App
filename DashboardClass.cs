using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GRC_Clinical_Genetics_Application
{
    class DashboardClass
    {
        private string name;
        Connections dashCon = new Connections();

        public DashboardClass() { }

        public string UpdateGreeting(int id)
        {
            dashCon.GRC_Connection.Open();
            SqlCommand cmd = dashCon.NameCommand(id);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                name = sdr[1].ToString() + " " + sdr[2].ToString();
            }
            dashCon.GRC_Connection.Close();
            return name;
        }

        public DataTable UpdateAppTable(bool def, string GRCnum = "", string status = "", string patientName = "", int PHN = 0, bool isUrgent = false)
        {
            if (def)
            {
                //create default table with no parameters
            }else
            {
                //create table with optional search parameters 
            }
            //default values provided to search criterias
            
            DataTable data = new DataTable();

            return data;
        }

    }
}
