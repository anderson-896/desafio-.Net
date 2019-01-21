using CrossCutting.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace WebApi.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private bool active;
        public ExceptionFilter(bool active = true)
        {
            this.active = active;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            var message = string.Format(ValidationMessages.GENERIC_ERROR, exception.ToString());

            var response = ResponseEnvelope
                .CreateErrorResponseEnvelope(ValidationMessageHelper
                .Create(message));

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);            
        }
    }
}