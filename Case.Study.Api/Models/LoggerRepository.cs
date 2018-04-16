using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Models
{
    public class LoggerRepository : ILoggerRepository, IDisposable
    {
        protected MyTestDBEntities _DBContext = null;

        public LoggerRepository(MyTestDBEntities Context_)
        {
            _DBContext = Context_;
        }

        public void AddLog(ErrorLog Error_)
        {
            _DBContext.ErrorLogs.Add(Error_);
        }

        public void Save()
        {
            _DBContext.SaveChanges();
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _DBContext.Dispose();
                }
            }
            this.disposed = true;
        }
         
   

        #endregion


    }
}