using ReportUtility.Helper.EventLogger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Helper
{
    public class BaseBLL : EventLoggerService
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public static string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public static bool IsEditMode(object value)
        {
            decimal val = Decimal.Parse(value.ToString());

            bool isEdit = false;

            if (val != 0)
            {
                isEdit = true;
            }
            else
            {
                isEdit = false;

                if (value is Guid)
                {
                    if (new Guid(value.ToString()).CompareTo(Guid.Empty) == 0)
                    {
                        isEdit = false;
                    }
                }
                else if (value.ToString().Length == 0)
                {
                    isEdit = false;
                }
                else if (string.IsNullOrEmpty(value.ToString()) || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    isEdit = false;
                }
            }

            return isEdit;

        }
    }
}
