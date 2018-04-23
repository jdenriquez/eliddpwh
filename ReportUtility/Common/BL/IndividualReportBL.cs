using ReportUtility.Common.ModelView;
using ReportUtility.DAL;
using ReportUtility.Helper;
using ReportUtility.Helper.EventLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Common.BL
{
    public class IndividualReportBL : BaseBLL
    {
        public IList<IndividualReportModel> LogsByDateRange(List<string> files, ref string period)
        {
            var dtr = new List<IndividualReportModel>();
            var dal = new IndividualReportDAL(GetConnectionString());
            try
            {
                var dates = new List<DateTime>();
                               
                var logs = ReadCSV(files);
                DateTime startDate = logs.Min(p => p.Date);
                DateTime endDate = logs.Max(p => p.Date);
                period = string.Format("Period: {0}-{1}", startDate.ToString("MM/dd/yyyy"), endDate.ToString("MM/dd/yyyy"));
                var breakLogs = dal.LogsByDateRange(startDate, endDate).ToList();
                dtr = (from log in logs
                       join breakLog in breakLogs on new { log.UserId, log.Date } equals new { breakLog.UserId, breakLog.Date } into leftLog
                       from breakLog in leftLog.DefaultIfEmpty()
                       select new IndividualReportModel
                       {
                           UserIdN = log.UserIdN,
                           UserId = log.UserId,
                           Username = log.Username,
                           Division = log.Division,
                           Position = log.Position,
                           Date = log.Date,
                           TimeIn = log.TimeIn,
                           BreakOut = breakLog == null ? string.Empty : log.BreakOut,
                           BreakIn = breakLog == null ? string.Empty : log.BreakIn,
                           TimeOut = log.TimeOut,
                           Schedule = log.Schedule,
                           Result = log.Result
                       }
                      ).ToList();

            }
            catch (Exception ex)
            {
                this.AddError(new EventLogger()
                {
                    Message = ex.Message
                });
                dal.RollbackTransaction();
            }
            finally
            {
                dal.Dispose();
            }
            return dtr;
        }
      
        private IList<IndividualReportModel> ReadCSV(List<string> files)
        {
            var logs = new List<IndividualReportModel>();
            foreach (var file in files)
            {
                int i = 0;
                var reader = new StreamReader(File.OpenRead(file));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (i > 0)
                    {
                        
                        if (line.Contains(","))
                        {
                            var data = line.Split(',');
                            var log = new IndividualReportModel()
                            {
                                Division = data[3].ToString(),
                                UserId = data[1].ToString(),
                                Username = data[2].ToString(),
                                Date = DateTime.Parse(data[0].ToString()),
                                TimeIn = data[5].ToString(),

                                BreakOut = null,
                                BreakIn = null,
                                TimeOut = data[6].ToString(),
                                Schedule = data[4].ToString(),
                                Result = data[7].ToString(),
                            };
                            logs.Add(log);
                        }
                    }
                    i++;
                    
                }
            }
            
            return logs;
        }
    }
}
