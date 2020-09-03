using System;
using System.Collections.Generic;
using System.Net;

namespace Backend.Common.Filters.ExceptionFilter
{
    public class StatusCodeException : Exception
    {
        public StatusCodeException()
            : base("Bad Request")
        {
        }

        public StatusCodeException(HttpStatusCode statusCode, List<string> errors) : base(string.Join(", ", errors))
        {
            HttpStatusCode = statusCode;
            Errors = errors;
        }

        public StatusCodeException(HttpStatusCode statusCode, string error) : base(error)
        {
            HttpStatusCode = statusCode;
            Errors = new List<string> {error};
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public List<string> Errors { get; set; }
    }
}