using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Common.ModelView
{
    public class IndividualReportModel
    {
        public int UserIdN { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Position { get; set; }
        public DateTime Date { get; set; }
        public string TimeIn { get; set; }
        public string BreakOut { get; set; }
        public string BreakIn { get; set; }
        public string TimeOut { get; set; }
        public string Schedule { get; set; }
        public string Result { get; set; }
        public string Division { get; set; }

    }
}
