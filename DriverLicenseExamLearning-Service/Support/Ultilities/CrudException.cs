using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.Support.Ultilities
{
    public class CrudException<T> : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public T Error { get; set; }

        public CrudException(HttpStatusCode status, string msg, T error) : base(msg)
        {
            Status = status;
            Error = error;
        }
    }
}

