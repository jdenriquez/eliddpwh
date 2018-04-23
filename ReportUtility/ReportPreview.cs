using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportUtility
{
    public partial class ReportPreview : Form
    {
        public ReportDocument ReportDoc { get; set; }
        public ParameterFields Params { get; set; }
        public ReportPreview()
        {
            InitializeComponent();
        }

        private void ReportPreview_Load(object sender, EventArgs e)
        {
            
            crystalReportViewer1.ReportSource = ReportDoc;
            crystalReportViewer1.ParameterFieldInfo = Params;
        }
    }
}
