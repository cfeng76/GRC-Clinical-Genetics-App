using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace GRC_Clinical_Genetics_Application
{
    public partial class Dashboard : Form
    {
        private int userID;
        private string GRCNumber;
        private string status;
        private string patientFirstName;
        private string patientLastName;
        private int personalHealthNumber;
        private bool isUrgent = false;
        private bool listAll = false;
        private bool defaultData = true;

        DashboardClass dashboard = new DashboardClass();
        public Dashboard(int id)
        {
            InitializeComponent();
            this.userID = id;
            this.NameLabel.Text = dashboard.UpdateGreeting(userID);
            InitializeDataTable();
        }

        public void InitializeDataTable()
        {
            defaultData = true;
            StatusComboBox.SelectedItem = "Any";
            DataTable dt = dashboard.UpdateAppTable(defaultData);
            ApplicationListTableView.DataSource = dt;
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            Login logOut = new Login();
            this.Close();
            logOut.Show();
        }

        //limits textbox to numbers and '-'
        private void GRCNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void PHNTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            GRCNumber = GRCNumberTextBox.Text;
            status = StatusComboBox.SelectedItem.ToString();
            patientFirstName = PatientFirstNameTextBox.Text;
            patientLastName = PatientLastNameTextBox.Text;
            personalHealthNumber = (PHNTextBox.Text != "") ? Convert.ToInt32(PHNTextBox.Text) : 0;
            isUrgent = (UrgentCheckBox.CheckState == CheckState.Checked) ? true : false;
            listAll = (listAllCheckBox.CheckState == CheckState.Checked) ? true : false;

            defaultData = (GRCNumber == "" && status == "Any" && patientFirstName == "" && patientLastName == "" && PHNTextBox.Text == "" && !isUrgent && !listAll) ? true : false;
           
            DataTable dt = dashboard.UpdateAppTable(defaultData, GRCNumber, status, patientFirstName, patientLastName, personalHealthNumber, isUrgent, listAll);
            ApplicationListTableView.DataSource = dt;

        }
    }
}
