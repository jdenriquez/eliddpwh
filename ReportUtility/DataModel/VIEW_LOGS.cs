using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.DataModel
{
    public class VIEW_LOGS
    {
        public int UserIdN { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Position { get; set; }
        public DateTime Date { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? BreakOut { get; set; }
        public DateTime? BreakIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public string Schedule { get; set; }
        public string Result { get; set; }
        public string Division { get; set; }
        public int? DivisionId { get; set; }
    }
}
