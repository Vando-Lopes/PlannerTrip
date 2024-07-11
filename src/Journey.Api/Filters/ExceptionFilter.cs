using Journey.Communication.Responses;
using Journey.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Journey.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is JourneyException) 
            {
                var journeyException = (JourneyException)context.Exception;
                var responseJson = new ReponseErrorsJson(journeyException.GetErrorMessages());
                context.HttpContext.Response.StatusCode = (int)journeyException.GetStatusCode();
                context.Result = new ObjectResult(responseJson);
            }
            else
            {
                var responseJson = new ReponseErrorsJson(["Erro desconhecido"]);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Result = new BadRequestObjectResult(responseJson);
            }
        }
    }
}
