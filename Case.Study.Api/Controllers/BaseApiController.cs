using Case.Study.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Case.Study.Api.Controllers
{ 
    public class BaseApiController : ApiController
    {
        //public ILoggerRepository _LoggerRepository;

        public BaseApiController()
        {
            //_LoggerRepository = new LoggerRepository(new MyTestDBEntities());
        }

    }
}