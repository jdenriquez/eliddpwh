using ReportUtility.DataModel;
using ReportUtility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.DAL
{
    public class IndividualReportDAL : BaseDAL
    {
        public IndividualReportDAL(string connectionString) : base(connectionString)
        {
        }

        public IndividualReportDAL(string connectionName, string connectionString)
            : base(connectionName, connectionString)
        {

        }

        public IndividualReportDAL(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {

        }

        private static VIEW_LOGS ConvertReaderToEntity(dynamic reader)
        {
            if (!reader.Read())
            {
                return null;
            }

            VIEW_LOGS item = new VIEW_LOGS();

            //item.DivisionId = string.IsNullOrEmpty(reader["DivisionId"].ToString()) ? null : int.Parse(reader["DivisionId"].ToString());
            //item.Division = reader["Division"].ToString();
            item.UserIdN = int.Parse(reader["UserIdN"].ToString());
            item.UserId = reader["UserId"].ToString();
            //item.Username = reader["Username"].ToString();
            //item.Position = reader["Position"].ToString();

            item.Date = DateTime.Parse(reader["Date"].ToString());
            //item.TimeIn = string.IsNullOrEmpty(reader["TimeIn"].ToString())? null : DateTime.Parse(reader["TimeIn"].ToString());

            item.BreakOut= string.IsNullOrEmpty(reader["BreakOut"].ToString()) ? null : DateTime.Parse(reader["BreakOut"].ToString());
            item.BreakIn = string.IsNullOrEmpty(reader["BreakIn"].ToString())  ? null : DateTime.Parse(reader["BreakIn"].ToString());
            //item.TimeOut = string.IsNullOrEmpty(reader["TimeOut"].ToString())  ? null : DateTime.Parse(reader["TimeIn"].ToString());
            //item.Schedule = reader["Schedule"].ToString();
            //item.Result = reader["Result"].ToString();

            return item;
        }

        private static VIEW_LOGS ConvertReaderToEntityUser(dynamic reader)
        {
            if (!reader.Read())
            {
                return null;
            }

            VIEW_LOGS item = new VIEW_LOGS();
            Console.WriteLine(reader["UserId"].ToString());
            item.UserIdN = int.Parse(reader["UserIdN"].ToString());
            item.UserId = reader["UserId"].ToString();
            item.DivisionId = string.IsNullOrEmpty(reader["DivisionId"].ToString()) ? null : int.Parse(reader["DivisionId"].ToString());
            item.Division = reader["Division"].ToString();
            item.Username = reader["Username"].ToString();
            item.Position = reader["Position"].ToString();

            return item;
        }

        private static IList<VIEW_LOGS> ConvertReaderToList(dynamic reader)
        {
            List<VIEW_LOGS> listItem = new List<VIEW_LOGS>();
            VIEW_LOGS item = null;
            while (reader.Read())
            {
                item = ConvertReaderToEntity(reader);
                if (item == null)
                {
                    break;
                }
                listItem.Add(item);
            }

            return listItem;
        }

        private static IList<VIEW_LOGS> ConvertReaderToListUser(dynamic reader)
        {
            List<VIEW_LOGS> listItem = new List<VIEW_LOGS>();
            VIEW_LOGS item = null;
            while (reader.Read())
            {
                item = ConvertReaderToEntityUser(reader);
                if (item == null)
                {
                    break;
                }
                listItem.Add(item);
                Console.WriteLine(item.Username);
            }

            return listItem;
        }

        public IList<VIEW_LOGS> LogsByDateRange(DateTime startDate, DateTime endDate)
        {
            StringBuilder script = new StringBuilder();

            script.Append("SELECT u.nUserIdn as UserIdN, u.sUserID as UserId, ");
            script.Append("CONVERT(DATETIME, CONVERT(VARCHAR, DATEADD(s, l.ndatetime, '1970-01-01 00:00:00'), 101), 101) AS Date, ");
            //.Append("MAX(CASE WHEN l.nTNAEvent = 0 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS TimeIn, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 1 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS BreakOut, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 2 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS BreakIn ");
            //script.Append("MAX(CASE WHEN l.nTNAEvent = 3 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS TimeOut, ");
            //script.Append("ds.sName AS Schedule, '' AS Result ");
            script.Append("FROM TB_USER u ");
            //script.Append("LEFT JOIN TB_USER_DEPT d ");
            //script.Append("ON u.nDepartmentIdn = d.nDepartmentIdn ");
            script.Append("LEFT JOIN TB_EVENT_LOG l ");
            script.Append("ON u.nUserIdn = l.nUserID ");
            //script.Append("LEFT JOIN TB_TA_RESULT r ");
            //script.Append("ON r.nDateTime = l.nDateTime AND r.nUserIdn = l.nUserID ");
            script.Append(string.Format("WHERE nTNAEvent IN (1,2) AND CONVERT(DATETIME,CONVERT(VARCHAR,DATEADD(s,l.ndatetime,'1970-01-01 00:00:00'),101),101) BETWEEN '{0}' AND '{1}'", startDate.ToShortDateString(), endDate.ToShortDateString()));
            script.Append("GROUP BY  u.nUserIdn, u.sUserID, ");
            script.Append("CONVERT(DATETIME, CONVERT(VARCHAR, DATEADD(s, l.ndatetime, '1970-01-01 00:00:00'), 101), 101) ");
            //script.Append("ds.sName ");
            //script.Append("ORDER BY u.sUserName ");
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script.ToString()))
            {
                
                
                    return ConvertReaderToList(reader);
                
                    
            }
        }

        public IList<VIEW_LOGS> LogsByDateRangeUserId(DateTime startDate, DateTime endDate, string userid)
        {
            StringBuilder script = new StringBuilder();

            script.Append("SELECT u.nUserIdn as UserIdN, d.nDepartmentIdn as DivisionId, d.sName as Division, u.sUserID as UserId,u.sUserName as Username, '' as Position, ");
            script.Append("CONVERT(DATETIME, CONVERT(VARCHAR, DATEADD(s, l.ndatetime, '1970-01-01 00:00:00'), 101), 101) AS Date, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 0 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS TimeIn, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 1 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS BreakOut, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 2 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS BreakIn, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 3 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS TimeOut, ");
            script.Append("ds.sName AS Schedule, '' AS Result ");
            script.Append("FROM TB_USER u ");
            script.Append("LEFT JOIN TB_USER_DEPT d ");
            script.Append("ON u.nDepartmentIdn = d.nDepartmentIdn ");
            script.Append("LEFT JOIN TB_EVENT_LOG l ");
            script.Append("ON u.nUserIdn = l.nUserID ");
            script.Append("LEFT JOIN TB_TA_RESULT r ");
            script.Append("ON r.nDateTime = l.nDateTime AND r.nUserIdn = l.nUserID ");
            script.Append("LEFT JOIN TB_TA_DAILYSCHEDULE ds ");
            script.Append("ON ds.nDailyScheduleIdn = r.nDailyScheduleIdn ");
            script.Append(string.Format("WHERE CONVERT(DATETIME,CONVERT(VARCHAR,DATEADD(s,l.ndatetime,'1970-01-01 00:00:00'),101),101) BETWEEN '{0}' AND '{1}' ", startDate.ToShortDateString(), endDate.ToShortDateString()));
            script.Append(string.Format("AND u.sUserID IN ({0}) ", userid));
            script.Append("GROUP BY u.nUserIdn, d.nDepartmentIdn, d.sName, u.sUserID, u.sUserName,  ");
            script.Append("CONVERT(DATETIME, CONVERT(VARCHAR, DATEADD(s, l.ndatetime, '1970-01-01 00:00:00'), 101), 101), ");
            script.Append("ds.sName ");
            script.Append("ORDER BY u.sUserName ");
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script.ToString()))
            {
                return ConvertReaderToList(reader);
            }
        }

        public IList<VIEW_LOGS> LogsByDateRangeDeptId(DateTime startDate, DateTime endDate, string deptId)
        {
            StringBuilder script = new StringBuilder();

            script.Append("SELECT u.nUserIdn as UserIdN, d.nDepartmentIdn as DivisionId, d.sName as Division, u.sUserID as UserId,u.sUserName as Username, '' as Position, ");
            script.Append("CONVERT(DATETIME, CONVERT(VARCHAR, DATEADD(s, l.ndatetime, '1970-01-01 00:00:00'), 101), 101) AS Date, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 0 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS TimeIn, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 1 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS BreakOut, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 2 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS BreakIn, ");
            script.Append("MAX(CASE WHEN l.nTNAEvent = 3 THEN DATEADD(s, l.ndatetime, '1970-01-01 00:00:00') ELSE NULL END) AS TimeOut, ");
            script.Append("ds.sName AS Schedule, '' AS Result ");
            script.Append("FROM TB_USER u ");
            script.Append("LEFT JOIN TB_USER_DEPT d ");
            script.Append("ON u.nDepartmentIdn = d.nDepartmentIdn ");
            script.Append("LEFT JOIN TB_EVENT_LOG l ");
            script.Append("ON u.nUserIdn = l.nUserID ");
            script.Append("LEFT JOIN TB_TA_RESULT r ");
            script.Append("ON r.nDateTime = l.nDateTime AND r.nUserIdn = l.nUserID ");
            script.Append("LEFT JOIN TB_TA_DAILYSCHEDULE ds ");
            script.Append("ON ds.nDailyScheduleIdn = r.nDailyScheduleIdn ");
            script.Append(string.Format("WHERE CONVERT(DATETIME,CONVERT(VARCHAR,DATEADD(s,l.ndatetime,'1970-01-01 00:00:00'),101),101) BETWEEN '{0}' AND '{1}' ", startDate.ToShortDateString(), endDate.ToShortDateString()));
            script.Append(string.Format("AND u.nDepartmentIdn IN ({0}) ", deptId));
            script.Append("GROUP BY u.nUserIdn, d.nDepartmentIdn, d.sName, u.sUserID, u.sUserName,  ");
            script.Append("CONVERT(DATETIME, CONVERT(VARCHAR, DATEADD(s, l.ndatetime, '1970-01-01 00:00:00'), 101), 101), ");
            script.Append("ds.sName ");
            script.Append("ORDER BY u.sUserName ");
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script.ToString()))
            {
                return ConvertReaderToList(reader);
            }
        }

        public IList<VIEW_LOGS> UserAll()
        {
            StringBuilder script = new StringBuilder();

            script.Append("SELECT u.nUserIdn as UserIdN, d.nDepartmentIdn as DivisionId, d.sName as Division, u.sUserID as UserId,u.sUserName as Username, '' as Position ");
            script.Append("FROM TB_USER u ");
            script.Append("LEFT JOIN TB_USER_DEPT d ");
            script.Append("ON u.nDepartmentIdn = d.nDepartmentIdn ");
            script.Append("ORDER BY u.sUserName ");
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script.ToString()))
            {


                return ConvertReaderToListUser(reader);


            }
        }

        public IList<VIEW_LOGS> UserByUserId(string userid)
        {
            StringBuilder script = new StringBuilder();

            script.Append("SELECT u.nUserIdn as UserIdN, d.nDepartmentIdn as DivisionId, d.sName as Division, u.sUserID as UserId,u.sUserName as Username, '' as Position ");
            script.Append("FROM TB_USER u ");
            script.Append("LEFT JOIN TB_USER_DEPT d ");
            script.Append("ON u.nDepartmentIdn = d.nDepartmentIdn ");
            script.Append(string.Format("WHERE u.sUserID IN ({0}) ", userid));
            script.Append("ORDER BY u.sUserName ");
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script.ToString()))
            {
                return ConvertReaderToListUser(reader);
            }
        }

        public IList<VIEW_LOGS> UserByDeptId(string deptId)
        {
            StringBuilder script = new StringBuilder();

            script.Append("SELECT u.nUserIdn as UserIdN, d.nDepartmentIdn as DivisionId, d.sName as Division, u.sUserID as UserId,u.sUserName as Username, '' as Position, ");
            script.Append("FROM TB_USER u ");
            script.Append("LEFT JOIN TB_USER_DEPT d ");
            script.Append("ON u.nDepartmentIdn = d.nDepartmentIdn ");
            script.Append(string.Format("WHERE u.nDepartmentIdn IN ({0}) ", deptId));
            script.Append("ORDER BY u.sUserName ");
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script.ToString()))
            {
                return ConvertReaderToListUser(reader);
            }
        }
    }
}
