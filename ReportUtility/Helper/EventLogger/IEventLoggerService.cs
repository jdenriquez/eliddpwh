using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Helper.EventLogger
{
    public interface IEventLoggerService
    {
        string EventLogMessage { get; }
        bool HasError { get; }

        #region Save

        void AddError(EventLogger data);
        void AddError(params object[] eventloggers);

        #endregion

        #region Insert

        void InsertError(EventLogger data);
        void InsertError(params object[] EventLogger);

        #endregion

        #region Get All

        IList<EventLogger> EventLoggersGetAll();

        #endregion

        #region Get by KeyColumn

        EventLogger EventLoggersGetByEventLoggersId(decimal eventloggersid);

        #endregion

        #region Get by Column

        IList<EventLogger> EventLoggersGetByUserId(long userid);


        IList<EventLogger> EventLoggersGetByEventType(int eventtype);


        IList<EventLogger> EventLoggersGetByMessageType(int messagetype);


        IList<EventLogger> EventLoggersGetByMethodName(string methodname);


        IList<EventLogger> EventLoggersGetByClassname(string classname);


        IList<EventLogger> EventLoggersGetByProjectName(string projectname);


        IList<EventLogger> EventLoggersGetByMessage(string message);


        IList<EventLogger> EventLoggersGetByDateTimeLog(DateTime datetimelog);


        IList<EventLogger> EventLoggersGetByRangeDateTimeLog(DateTime _startDate, DateTime _endDate);

        #endregion

        #region Custom Insert
        #endregion

        #region Custom Update
        #endregion

        #region Custom Delete
        #endregion

        #region Custom Get
        #endregion
    }
}
