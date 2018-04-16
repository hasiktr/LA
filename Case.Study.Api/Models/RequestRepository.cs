using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Models
{
    public class RequestRepository : IRequestRepository, IDisposable
    {
        protected MyTestDBEntities _DBContext = null;

        public RequestRepository(MyTestDBEntities Context_)
        {
            this._DBContext = Context_;
        }

        public Request GetRequestByGuid(string Guid_)
        {
            return _DBContext.Requests.Where(p => p.Request_Guid == Guid_).SingleOrDefault();
        }

        public Request GetRequestByID(int RequestID_)
        {
            return _DBContext.Requests.Find(RequestID_);
        }

        public void InsertRequest(Request Request_)
        {
            _DBContext.Requests.Add(Request_);
        }

        public void DeleteRequest(int Request_)
        {
            throw new NotImplementedException();
        }

        public void UpdateRequest(Request Request_)
        {
            _DBContext.Entry(Request_).State = EntityState.Modified;
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