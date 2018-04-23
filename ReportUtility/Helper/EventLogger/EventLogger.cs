using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Helper.EventLogger
{
    public class EventLogger
    {

        private decimal _eventloggersid;

        private long _userid;

        private int _eventtype;

        private int _messagetype;

        private string _methodname;

        private string _classname;

        private string _projectname;

        private string _message;

        private DateTime _datetimelog;


        public decimal EventLoggersId
        {
            get { return _eventloggersid; }
            set { _eventloggersid = value; }
        }

        public long UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }

        public int EventType
        {
            get { return _eventtype; }
            set { _eventtype = value; }
        }

        public int MessageType
        {
            get { return _messagetype; }
            set { _messagetype = value; }
        }

        public string MethodName
        {
            get { return _methodname; }
            set { _methodname = value; }
        }

        public string Classname
        {
            get { return _classname; }
            set { _classname = value; }
        }

        public string ProjectName
        {
            get { return _projectname; }
            set { _projectname = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public DateTime DateTimeLog
        {
            get { return _datetimelog; }
            set { _datetimelog = value; }
        }


        public EventLogger()
        {
        }

        public EventLogger(
            long userid,
            int eventtype,
            int messagetype,
            string methodname,
            string classname,
            string projectname,
            string message,
            DateTime datetimelog)
        {

            _userid = userid;
            _eventtype = eventtype;
            _messagetype = messagetype;
            _methodname = methodname;
            _classname = classname;
            _projectname = projectname;
            _message = message;
            _datetimelog = datetimelog;
        }

        public EventLogger(
            decimal eventloggersid,
            long userid,
            int eventtype,
            int messagetype,
            string methodname,
            string classname,
            string projectname,
            string message,
            DateTime datetimelog)
        {

            _eventloggersid = eventloggersid;
            _userid = userid;
            _eventtype = eventtype;
            _messagetype = messagetype;
            _methodname = methodname;
            _classname = classname;
            _projectname = projectname;
            _message = message;
            _datetimelog = datetimelog;
        }
    }
}
