using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportUtility
{
    public partial class Maitenance : Form
    {
        public Maitenance()
        {
            InitializeComponent();
        }

        private void Maitenance_Load(object sender, EventArgs e)
        {
            txtReportTitle.Text = Properties.Settings.Default.ReportTitle;

            txtCertifierName.Text = Properties.Settings.Default.CertifierName;
            txtCertifierDept.Text = Properties.Settings.Default.CertifierDept;
            txtCertifierPos.Text = Properties.Settings.Default.CertifierPosition;

            txtApproverName.Text = Properties.Settings.Default.ApproverName;
            txtApproverDept.Text = Properties.Settings.Default.ApproverDept;
            txtApproverPos.Text = Properties.Settings.Default.ApproverPosition;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ReportTitle = txtReportTitle.Text;
                Properties.Settings.Default.CertifierName = txtCertifierName.Text;
                Properties.Settings.Default.CertifierDept = txtCertifierDept.Text;
                Properties.Settings.Default.CertifierPosition = txtCertifierPos.Text;

                Properties.Settings.Default.ApproverName = txtApproverName.Text;
                Properties.Settings.Default.ApproverDept = txtApproverDept.Text;
                Properties.Settings.Default.ApproverPosition = txtApproverPos.Text;
                Properties.Settings.Default.Save();

                MessageBox.Show("Settings successfully saved.", "Setting Saved", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
