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
        private int userID;

        public DashboardClass() { }

        public string UpdateGreeting(int id)
        {
            userID = id;
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

        public DataTable UpdateAppTable(bool def, string GRCnum = "", string status = "", string patientFirstName = "", string patientLastName = "", int PHN = 0, bool isUrgent = false, bool showAll = false)
        {
            DataTable data = new DataTable();

            Console.WriteLine("Default table: " + def);
            Console.WriteLine("////////////////////////////////////////");
            Console.WriteLine("GRC#: " + GRCnum);
            Console.WriteLine("Status: " + status);
            Console.WriteLine("Name: " + patientFirstName + " " + patientLastName);
            Console.WriteLine("PHN: " + PHN);
            Console.WriteLine("isUrgent?: " + isUrgent);
            Console.WriteLine("list All?: " + showAll);

            if (def)//default table
            {
                dashCon.GRC_Connection.Open();
                SqlDataAdapter adapt = dashCon.getDefaultDatatable(userID);
                adapt.Fill(data);
                dashCon.GRC_Connection.Close();
                return data;
            }else
            {
                //create table with optional search parameters 
                dashCon.GRC_Connection.Open();
                SqlDataAdapter adt = dashCon.getCustomDatatable(GRCnum, status, patientFirstName, patientLastName, PHN, isUrgent, showAll);
                adt.Fill(data);
                dashCon.GRC_Connection.Close();
                return data;
            }
        }

    }
}
