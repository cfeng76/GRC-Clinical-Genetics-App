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
        Dashboard dsb;
        ApplicationFormClass app = new ApplicationFormClass();

        public ApplicationForm(Dashboard dashboard){
            InitializeComponent();
            dsb = dashboard;
            PHNTextBox.AutoCompleteCustomSource = app.Search(0);
            FirstNameTextBox.AutoCompleteCustomSource = app.Search(1);
            LastNameTextBox.AutoCompleteCustomSource = app.Search(2);
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
                MessageBox.Show("Fields entered correctly");
                /*if(!IsExistingPatient){
                    option to add to db
                  }
                 */
            }
            
        }
    }
}
