using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ReportUtility.Common.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ReportUtility
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            
        }

        private void signatoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maitenance m = new Maitenance();
            m.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            lvFile.AllowDrop = true;
            lvFile.Columns.Add("File");
            lvFile.Columns[0].Width = 600;

            try
            {
                var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1).AddMonths(1).AddDays(-1);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void generateReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = new List<string>();
            for (int i = 0; i < lvFile.Items.Count; i++)
            {
                files.Add(lvFile.Items[i].Text);
            }
            var bl = new IndividualReportBL();
            string period = string.Empty;
            var logs = bl.LogsByDateRange(files, ref period);
            string filename = string.Format("{0}\\IndividualDTR.rpt", System.IO.Directory.GetCurrentDirectory());
            ReportDocument rptDoc = new ReportDocument();

            ReportPreview rpt = new ReportPreview();
            rptDoc.Load(filename);

            ParameterFields crtParamFields;
            crtParamFields = new ParameterFields();
            crtParamFields.Add(CreateCRParam("RptTitle", Properties.Settings.Default.ReportTitle));
            crtParamFields.Add(CreateCRParam("Period", period));

            crtParamFields.Add(CreateCRParam("CertName", Properties.Settings.Default.CertifierName));
            crtParamFields.Add(CreateCRParam("CertPosition", Properties.Settings.Default.CertifierPosition));
            crtParamFields.Add(CreateCRParam("CertDept", Properties.Settings.Default.CertifierDept));

            crtParamFields.Add(CreateCRParam("ApproverName", Properties.Settings.Default.ApproverName));
            crtParamFields.Add(CreateCRParam("ApproverPosition", Properties.Settings.Default.ApproverPosition));
            crtParamFields.Add(CreateCRParam("ApproverDept", Properties.Settings.Default.ApproverDept));

            crv.ParameterFieldInfo = crtParamFields;

            rptDoc.SetDataSource(logs);

            crv.ReportSource = rptDoc;
           
            saveFile.RestoreDirectory = true;

         
            crv.AllowedExportFormats = (int)(ViewerExportFormats.PdfFormat);
            crv.ExportReport();
             
            //rpt.Params = crtParamFields;
            //rpt.ReportDoc = rptDoc;
            //rpt.ShowDialog();
        }

        private ParameterField CreateCRParam(string paramName, string paramValue)
        {
            ParameterDiscreteValue crtParamDiscreteValue;
            ParameterField crtParamField;
            

            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();

            crtParamDiscreteValue.Value = paramValue;
            crtParamField.ParameterFieldName = paramName;
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);

            return crtParamField;
        }
        private void group2_Enter(object sender, EventArgs e)
        {

        }

        private void lvFile_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
               
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).ToLower() == ".csv")
                    {
                        lvFile.Items.Add(file);
                    }
                    else
                    {
                        MessageBox.Show("Invalid file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
        }

        private void lvFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lvFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (lvFile.SelectedItems.Count > 0)
                    {
                        foreach (ListViewItem item in lvFile.SelectedItems)
                        {
                            lvFile.Items.Remove(item);
                        }
                    }
                }
            }
        }

        private void lvFile_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (lvFile.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvFile.SelectedItems)
                    {
                        lvFile.Items.Remove(item);
                    }
                }
            }
        }
    }
}
