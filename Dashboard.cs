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
        private string status;
        private string GRCNumber;
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

        //placeholder text for first and last name textboxes
        private void PatientFirstNameTextBox_Click(object sender, EventArgs e)
        {
            if (PatientFirstNameTextBox.Text == "First Name")
            {
                PatientFirstNameTextBox.Text = "";
                PatientFirstNameTextBox.ForeColor = Color.Black;
            }
        }

        private void PatientLastNameTextBox_Click(object sender, EventArgs e)
        {
            if (PatientLastNameTextBox.Text == "Last Name")
            {
                PatientLastNameTextBox.Text = "";
                PatientLastNameTextBox.ForeColor = Color.Black;
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
            //dashboard.UpdateAppTable();
        }
    }
}
