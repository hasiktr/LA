using Case.Study.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Utilities
{
    public class Logger
    {
        private static object _Lock = new object();
        private static Logger _Logger;
        private static ILoggerRepository _LoggerRepository;

        public static Logger Log
        {
            get
            {
                if (_Logger == null)
                {
                    lock (_Lock)
                    {
                        if (_Logger == null)
                        {
                            _Logger = new Logger();
                            _LoggerRepository = new LoggerRepository(new MyTestDBEntities());
                        }
                    }
                }
                return _Logger;
            }
        }

        private Logger()
        {
        }
        
        public void Add(string Error_, ErrorType Type_)
        {
            _LoggerRepository.AddLog(new ErrorLog()
            {
                Error_Code = Error_,
                Error_DateTime = DateTime.Now
            });
            _LoggerRepository.Save();
        }

        public void Add(Exception Exception_, ErrorType Type_ = ErrorType.CriticalError)
        {
            _LoggerRepository.AddLog(new ErrorLog()
            {
                Error_Code = Exception_.Message ?? "",
                Error_DateTime = DateTime.Now,
                Error_InnerException = Exception_.InnerException == null ? "" : Exception_.InnerException.ToString(),
                Error_StackTrace = Exception_.StackTrace == null ? "" : Exception_.StackTrace.ToString()
            });
            _LoggerRepository.Save();
        }



    }
}