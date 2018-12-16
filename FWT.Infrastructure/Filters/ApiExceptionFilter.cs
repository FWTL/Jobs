using System;
using System.IO;
using System.Text;
using FluentValidation;
using FWTL.Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace FWTL.Infrastructure.Filters
{
    public sealed class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hosting;

        private readonly ILogger _logger;

        public ApiExceptionAttribute(ILogger logger, IHostingEnvironment hosting)
        {
            _logger = logger;
            _hosting = hosting;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var exception = context.Exception as ValidationException;
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new ValidationResultModel(exception));
                return;
            }

            var exceptionId = Guid.NewGuid();
            context.HttpContext.Response.StatusCode = 500;
            var sb = new StringBuilder();
            sb.AppendLine("ErrorId: " + exceptionId);
            sb.AppendLine(context.HttpContext.Request.GetDisplayUrl());

            if (context.HttpContext.Request.Body.CanSeek)
            {
                context.HttpContext.Request.Body.Position = 0;
                using (var reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    sb.AppendLine(reader.ReadToEnd());
                }
            }

            sb.AppendLine();
            sb.Append(context.Exception);

            _logger.Error(sb.ToString());

            if (_hosting.IsDevelopment())
            {
                context.Result = new ContentResult() { Content = sb.ToString() };
            }
            else
            {
                context.Result = new ContentResult() { Content = exceptionId.ToString() };
            }
        }
    }
}