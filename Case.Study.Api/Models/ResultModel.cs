using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Models
{
    public class ResultModel
    {
        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        public string Return { get; set; }

        public bool IsSuccess { get; set; }

    }

    public class ResultModel<T>
    {
        public List<T> DataList { get; set; }

        public T Data { get; set; }

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSuccess { get; set; }
    }

}