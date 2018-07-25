using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GRC_Clinical_Genetics_Application
{
    public partial class ApplicationForm : Form
    {
        private string PHN;
        private string firstName;
        private string lastName;
        private string postalCode;
        private string DOB;
        private string alternateID;
        private string alternateExplanation;
        private bool noPHN;
        private bool createNewPatient = false;
        private int demographics = 1;
        private int clinicalInfo = 2;
        private int testSearch = 3;
        private string orderingPhysician;
        Dashboard dsb;
        ApplicationFormClass app = new ApplicationFormClass();

        public ApplicationForm(Dashboard dashboard){
            InitializeComponent();
            dsb = dashboard;
            PHNTextBox.AutoCompleteCustomSource = app.Search(demographics, 0);
            FirstNameTextBox.AutoCompleteCustomSource = app.Search(demographics, 1);
            LastNameTextBox.AutoCompleteCustomSource = app.Search(demographics, 2);

            OrderingPhysicianTextBox.AutoCompleteCustomSource = app.Search(clinicalInfo);
        }

        private void PHNTextBox_KeyPress(object sender, KeyPressEventArgs e){
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)){
                e.Handled = true;
            }
        }

        private void NoPHNCheckBox_CheckedChanged(object sender, EventArgs e){
            if (NoPHNCheckBox.CheckState == CheckState.Checked){
                AlternateIDLabel.Show();
                ExplanationAltIDLabel.Show();
                AlternateIDTextbox.Show();
                AlternateIDExplanationTextbox.Show();
                PHNTextBox.Text = "";
                PHNTextBox.ReadOnly = true;
            }else if (NoPHNCheckBox.CheckState == CheckState.Unchecked){
                AlternateIDLabel.Hide();
                ExplanationAltIDLabel.Hide();
                AlternateIDTextbox.Hide();
                AlternateIDExplanationTextbox.Hide();
                AlternateIDTextbox.Text = "";
                AlternateIDExplanationTextbox.Text = "";
                PHNTextBox.ReadOnly = false;
            }
            
        }
        private void UrgentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (UrgentCheckBox.CheckState == CheckState.Checked)
            {
                UrgentExplLabel.Show();
                UrgentExplTextBox.Show();
            }
            else if (UrgentCheckBox.CheckState == CheckState.Unchecked)
            {
                UrgentExplLabel.Hide();
                UrgentExplTextBox.Hide();
            }
        }

        private void FamilyHistoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FamilyHistoryCheckBox.CheckState == CheckState.Checked)
            {
                FamilyDetailsLabel.Show();
                FamilyHistoryTextBox.Show();
            }
            else if (FamilyHistoryCheckBox.CheckState == CheckState.Unchecked)
            {
                FamilyDetailsLabel.Hide();
                FamilyHistoryTextBox.Hide();
            }
        }

        private void EthnicityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EthnicityCheckBox.CheckState == CheckState.Checked)
            {
                EthnicityRiskLabel.Show();
                EthnicityRiskTextBox.Show();
            }
            else if (EthnicityCheckBox.CheckState == CheckState.Unchecked)
            {
                EthnicityRiskLabel.Hide();
                EthnicityRiskTextBox.Hide();
            }
        }

        private void OtherTestingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OtherTestingCheckBox.CheckState == CheckState.Checked)
            {
                OtherTestingLabel.Show();
                OtherTestingTextBox.Show();
            }
            else if (OtherTestingCheckBox.CheckState == CheckState.Unchecked)
            {
                OtherTestingLabel.Hide();
                OtherTestingTextBox.Hide();
            }
        }

        private void PHNTextBox_TextChanged(object sender, EventArgs e)
        {
            PHN = PHNTextBox.Text;
            app.UpdateDemographics(PHN);
            FirstNameTextBox.Text = app.GetFirstName();
            LastNameTextBox.Text = app.GetLastName();
            PostalCodeTextBox.Text = app.GetZIP();
            DOBPicker.Value = app.GetDOB(DOBPicker.MinDate);
        }

        private void OrderingPhysicianTextBox_TextChanged(object sender, EventArgs e)
        {
            orderingPhysician = OrderingPhysicianTextBox.Text;
            DataTable dt = app.UpdateClinicalContacts(orderingPhysician);
            PrimaryClinicalContactComboBox.DisplayMember = "Clinical Contact";
            PrimaryClinicalContactComboBox.DataSource = dt;
            AltClinicalContactComboBox.DisplayMember = PrimaryClinicalContactComboBox.DisplayMember;
            AltClinicalContactComboBox.DataSource = dt;
        }

        private void NewPatientButton_Click(object sender, EventArgs e)
        {
            PHN = PHNTextBox.Text;
            noPHN = (NoPHNCheckBox.CheckState == CheckState.Checked) ? true : false;
            alternateID = AlternateIDTextbox.Text;
            alternateExplanation = AlternateIDExplanationTextbox.Text;
            firstName = FirstNameTextBox.Text;
            lastName = LastNameTextBox.Text;
            postalCode = PostalCodeTextBox.Text;
            DOB = DOBPicker.Value.ToString();

            if(app.FieldsCorrect(PHN, noPHN, alternateID, alternateExplanation, firstName, lastName, postalCode))
            {
                if (!app.PatientExists(PHN, firstName, lastName, DOB) && !noPHN && app.CorrectPHN(PHN, firstName, lastName, DOB))
                {
                    DialogResult dr = MessageBox.Show("Patient does not exist! Would you like to create a new record for this patient?", "New Patient!", MessageBoxButtons.YesNo);
                    createNewPatient = (dr == DialogResult.Yes) ? true : false;
                }
                else if (!app.PatientExists(PHN, firstName, lastName, DOB) && !noPHN && !app.CorrectPHN(PHN, firstName, lastName, DOB))
                {
                    DialogResult res = MessageBox.Show("A Patient exists with name " + firstName + lastName + ". Are you sure you have entered the correct PHN?", "Similar record found!", MessageBoxButtons.YesNo);
                    createNewPatient = (res == DialogResult.Yes) ? true : false;
                }
                else
                {
                    MessageBox.Show("Patient already exists!");
                    //ADD: update postal code if different
                }
            }

        }

        
    }
}
