﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GRC_Clinical_Genetics_Application
{
    public partial class Dashboard : Form
    {
        private int userID;
        private string GRCNumber;
        private string ApplicationStatus;
        private string GRCStatus;
        private string patientFirstName;
        private string patientLastName;
        private int personalHealthNumber;
        private bool isUrgent = false;
        private bool listAll = false;
        private bool defaultData = true;
        private int openMetricsID = 1;
        private int urgentMetricsID = 2;
        private bool logOut = false;
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
            AppStatus.SelectedItem = "Any";
            DataTable dt = dashboard.UpdateAppTable(defaultData);
            ApplicationListTableView.DataSource = dt;

            UpdateMetricLabels();
        }
        private void UpdateMetricLabels()
        {
            NumLabel1.Text = dashboard.UpdateMetrics(openMetricsID).ToString();
            NumLabel2.Text = dashboard.UpdateMetrics(urgentMetricsID).ToString();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            logOut = true;
            this.Close();
            Application.Restart();
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!logOut)
            {
                Application.Exit();
            }
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
            ApplicationStatus = AppStatus.SelectedItem.ToString();
            GRCStatus = StatusComboBox.SelectedItem.ToString();
            patientFirstName = PatientFirstNameTextBox.Text;
            patientLastName = PatientLastNameTextBox.Text;
            personalHealthNumber = (PHNTextBox.Text != "") ? Convert.ToInt32(PHNTextBox.Text) : 0;
            isUrgent = (UrgentCheckBox.CheckState == CheckState.Checked) ? true : false;
            listAll = (listAllCheckBox.CheckState == CheckState.Checked) ? true : false;

            defaultData = (GRCNumber == "" && ApplicationStatus == "Any" && GRCStatus == "Any" && patientFirstName == "" && patientLastName == "" && PHNTextBox.Text == "" && !isUrgent && !listAll) ? true : false;
            ApplicationListTableView.DataSource = null;
            DataTable dt = dashboard.UpdateAppTable(defaultData, GRCNumber, GRCStatus, patientFirstName, patientLastName, personalHealthNumber, isUrgent, listAll, ApplicationStatus);
            ApplicationListTableView.DataSource = dt;
            UpdateMetricLabels();
        }

        private void NewApplicationButton_Click(object sender, EventArgs e)
        {
            ApplicationForm newApp = new ApplicationForm(userID, false);
            newApp.Show();
        }

        private void ApplicationListTableView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }
            int applicationNum = (ApplicationListTableView.Rows[e.RowIndex].Cells[0].Value != DBNull.Value) ? Convert.ToInt32(ApplicationListTableView.Rows[e.RowIndex].Cells[0].Value) : 0;
            if (applicationNum == 0)
            {
                return;
            }
            //open existing application
            ApplicationForm newApp = new ApplicationForm(userID, true, applicationNum);
            newApp.Show();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            foreach(Control c in this.Controls)
            {
                if(c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
                if(c is CheckBox)
                {
                    ((CheckBox)c).CheckState = CheckState.Unchecked;
                }
                if(c is ComboBox)
                {
                    ((ComboBox)c).SelectedItem = "Any";
                }
            }
        }

    }
}
