using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Utilities
{
    public enum RequestStatus
    {
        QUE = 1,
        ERR = 2,
        CMP = 3
    }

    public enum AuthorizationTypes
    {
        User = 1,
        Moderator = 2,
        Administrator = 3
    }

    public enum ErrorType
    {
        CriticalError = 1,
        UserError = 2,
        Warning = 3

    }
}