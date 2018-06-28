using System;
using System.Collections.Generic;
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
        Connections AppCon = new Connections();
        public ApplicationFormClass()
        {

        }

        public AutoCompleteStringCollection Search(int col)
        {
            AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
            AppCon.GRC_Connection.Open();
            SqlCommand cmd = AppCon.PHNCommand();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                MyCollection.Add(sdr[col].ToString());
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

        public bool FieldsCorrect(string PHN, bool noPHN, string alternateID, string alternateExplanation, string firstName, string lastName, string postalCode)
        {
            bool isComplete = false;
            if ((PHN != "" && !noPHN) ||
               (PHN == "" && noPHN && alternateID != "" && alternateExplanation != "")){
                isComplete = true;
            }
            else{
                isComplete = false;
                MessageBox.Show("Please enter a valid Personal Health Number or Alternate ID with details.");
                return isComplete;
            }

            if (firstName != "" && lastName != ""){
                isComplete = true;
            }
            else{
                isComplete = false;
                MessageBox.Show("Please enter a full name.");
                return isComplete;
            }

            if(postalCode != "" && postalCode.Length >= 6){
                isComplete = true;
            }else{
                isComplete = false;
                MessageBox.Show("Please enter a valid Postal Code/ZIP");
                return isComplete;
            }

            return isComplete;
        }

        public string GetFirstName()
        {
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
            Console.WriteLine(date);
            if(date.Year < minDate.Year)
            {
                date = minDate;
            }
            return date;
        }
    }
}
