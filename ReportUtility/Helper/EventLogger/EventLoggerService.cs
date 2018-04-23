using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Helper.EventLogger
{
    public class EventLoggerService : CollectionBase, IEventLoggerService
    {
        #region private variable

        #endregion

        public string EventLogMessage
        {
            get
            {
                StringBuilder errors = new StringBuilder();
                foreach (EventLogger list in base.List)
                {
                    if (errors.Length > 0)
                    {
                        errors.Append(Environment.NewLine);
                    }
                    errors.Append(list.Message.ToString());
                }
                return errors.ToString();
            }
        }

        public bool HasError
        {
            get
            {
                return base.Count > 0;
            }
        }

        public EventLogger this[int index]
        {
            get
            {
                return (EventLogger)base.List[index];
            }
        }

        public void EventLog(EventLogger eventLogger)
        {
            base.List.Remove(eventLogger);
        }

        #region Save

        public void AddError(EventLogger data)
        {
            base.List.Add(data);
        }

        public void AddError(params object[] eventloggers)
        {
            EventLogger data = new EventLogger()
            {
                EventLoggersId = decimal.Parse(eventloggers[0].ToString()),
                UserId = long.Parse(eventloggers[1].ToString()),
                EventType = int.Parse(eventloggers[2].ToString()),
                MessageType = int.Parse(eventloggers[3].ToString()),
                MethodName = eventloggers[4].ToString(),
                Classname = eventloggers[5].ToString(),
                ProjectName = eventloggers[6].ToString(),
                Message = eventloggers[7].ToString(),
                DateTimeLog = DateTime.Parse(eventloggers[8].ToString())
            };

            base.List.Add(data);
        }
        #endregion


        #region Insert

        public void InsertError(EventLogger data)
        {
            base.List.Add(data);
        }

        public void InsertError(params object[] eventloggers)
        {
            EventLogger data = new EventLogger()
            {
                EventLoggersId = decimal.Parse(eventloggers[0].ToString()),
                UserId = long.Parse(eventloggers[1].ToString()),
                EventType = int.Parse(eventloggers[2].ToString()),
                MessageType = int.Parse(eventloggers[3].ToString()),
                MethodName = eventloggers[4].ToString(),
                Classname = eventloggers[5].ToString(),
                ProjectName = eventloggers[6].ToString(),
                Message = eventloggers[7].ToString(),
                DateTimeLog = DateTime.Parse(eventloggers[8].ToString())
            };

            base.List.Add(data);
        }
        #endregion

        #region Get All

        public IList<EventLogger> EventLoggersGetAll()
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }
        #endregion

        #region Get by KeyColumn

        public EventLogger EventLoggersGetByEventLoggersId(decimal eventloggersid)
        {
            EventLogger item = new EventLogger();
            return item;
        }
        #endregion

        #region Get by Column

        public IList<EventLogger> EventLoggersGetByUserId(long userid)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }

        public IList<EventLogger> EventLoggersGetByEventType(int eventtype)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }

        public IList<EventLogger> EventLoggersGetByMessageType(int messagetype)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }

        public IList<EventLogger> EventLoggersGetByMethodName(string methodname)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }

        public IList<EventLogger> EventLoggersGetByClassname(string classname)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }

        public IList<EventLogger> EventLoggersGetByProjectName(string projectname)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }

        public IList<EventLogger> EventLoggersGetByMessage(string message)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }

        public IList<EventLogger> EventLoggersGetByDateTimeLog(DateTime datetimelog)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }
        #region Get by Column

        public IList<EventLogger> EventLoggersGetByRangeDateTimeLog(DateTime _startDate, DateTime _endDate)
        {
            IList<EventLogger> items = new List<EventLogger>();
            return items;
        }
        #endregion
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