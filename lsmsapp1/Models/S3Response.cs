using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace lsmsapp1.Models
{
    public class S3Response
    {
        public HttpStatusCode status { get; set; }

        public string  Message { get; set; }
    }
}
