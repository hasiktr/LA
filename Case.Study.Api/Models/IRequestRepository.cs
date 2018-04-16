using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Models
{
    public interface IRequestRepository : IDisposable
    { 
        Request GetRequestByGuid(string Guid_);
        Request GetRequestByID(int RequestID_);
        void InsertRequest(Request Request_);
        void DeleteRequest(int Request_);
        void UpdateRequest(Request Request_);
        void Save();
    }
}