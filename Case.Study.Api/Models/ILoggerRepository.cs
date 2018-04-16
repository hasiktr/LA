using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Models
{
    public interface ILoggerRepository : IDisposable
    { 
        void AddLog(ErrorLog Error_);
        void Save();
    }
}