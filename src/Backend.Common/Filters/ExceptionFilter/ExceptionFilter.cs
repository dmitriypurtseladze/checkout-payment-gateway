using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Backend.Common.Filters.ExceptionFilter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Log.Error(context.Exception, context.Exception.Message);

            context.Exception = new Exception("an error occured");
        }
    }
}