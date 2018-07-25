using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GRC_Clinical_Genetics_Application
{
    class ApplicationFormClass
    {
        private string firstName;
        private string lastName;
        private string postalCode;
        private string dob;
        private int physicianID;
        private string physicianLastName;
        Connections AppCon = new Connections();
        public ApplicationFormClass()
        {

        }

        public AutoCompleteStringCollection Search(int typeOfSearch, int col = 0)
        {
            AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
            AppCon.GRC_Connection.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr;
            if (typeOfSearch == 1)
            {
                cmd = AppCon.PatientSearchCommand();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    MyCollection.Add(sdr[col].ToString());
                }
            }
            else if(typeOfSearch == 2)
            {
                cmd = AppCon.PhysicianSearchCommand();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    MyCollection.Add(sdr[0].ToString() + ", " + sdr[1].ToString() + " (" + sdr[2].ToString() + ")");
                }
            }
           
            AppCon.GRC_Connection.Close();
            return MyCollection;
        }

        public void UpdateDemographics(string PHN)
        {
            AppCon.GRC_Connection.Open();
            SqlCommand cmd = AppCon.DemographicsCommand(PHN);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                firstName = sdr[1].ToString();
                lastName = sdr[2].ToString();
                postalCode = sdr[3].ToString();
                dob = sdr[4].ToString(); 
            }
            AppCon.GRC_Connection.Close();
        }

        public bool PatientExists(string PHN, string fName, string lName, string DOB)
        {
            bool isPatient = false;
            AppCon.GRC_Connection.Open();
            SqlCommand cmd = AppCon.NewPatient(PHN, fName, lName, DOB);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                if (Convert.ToInt32(sdr[0].ToString()) == 1)
                {
                    isPatient = true;
                }else{
                    isPatient = false;
                }
            }
            AppCon.GRC_Connection.Close();
            return isPatient;
        }

        public bool CorrectPHN(string PHN, string fName, string lName, string DOB)
        {
            Console.WriteLine(fName + " " + lName);
            bool pHNIsCorrect = true;
            AppCon.GRC_Connection.Open();
            SqlCommand cmd = AppCon.CheckPHN(fName, lName, DOB);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if(PHN != sdr[0].ToString() && fName == sdr[1].ToString() && lName == sdr[2].ToString() && DOB == sdr[3].ToString())
                {
                    pHNIsCorrect = false;
                }else
                {
                    pHNIsCorrect = true;
                }
            }
            AppCon.GRC_Connection.Close();
            return pHNIsCorrect;
        }

        public bool FieldsCorrect(string PHN, bool noPHN, string alternateID, string alternateExplanation, string fName, string lName, string post)
        {
            bool isComplete = false;
            if ((PHN != "" && PHN.Length > 8 && !noPHN) ||
               (PHN == "" && noPHN && alternateID != "" && alternateExplanation != "")){
                isComplete = true;
            }
            else{
                isComplete = false;
                MessageBox.Show("Please enter a valid Personal Health Number or Alternate ID with details.");
                return isComplete;
            }

            if (fName != "" && lName != ""){
                isComplete = true;
            }
            else{
                isComplete = false;
                MessageBox.Show("Please enter a full name.");
                return isComplete;
            }

            if(post != "" && post.Length >= 6){
                isComplete = true;
            }else{
                isComplete = false;
                MessageBox.Show("Please enter a valid Postal Code/ZIP");
                return isComplete;
            }

            return isComplete;
        }

        public string GetFirstName(){
            return firstName;
        }

        public string GetLastName()
        {
            return lastName;
        }

        public string GetZIP()
        {
            return postalCode;
        }

        public DateTime GetDOB(DateTime minDate)
        {
            DateTime date = Convert.ToDateTime(dob);
           
            if(date.Year < minDate.Year)
            {
                date = minDate;
            }
            return date;
        }

        public DataTable UpdateClinicalContacts(string orderPhys)
        {
            physicianLastName = orderPhys.Split(',')[0];
            SetPhysID(physicianLastName);

            DataTable list = new DataTable();
            AppCon.GRC_Connection.Open();
            SqlDataAdapter adapt = AppCon.GetContactList(physicianID);
            adapt.Fill(list);
            AppCon.GRC_Connection.Close();
            return list;
        }

        public void SetPhysID(string physLast)
        {
            AppCon.GRC_Connection.Open();
            SqlCommand cmd = AppCon.GetPhysicianIDCommand(physLast);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                physicianID = Convert.ToInt32(sdr[0].ToString());
            }
            AppCon.GRC_Connection.Close();
        }
    }
}
