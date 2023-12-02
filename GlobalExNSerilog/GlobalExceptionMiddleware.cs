using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Microsoft.AspNetCore.Mvc.Controllers;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public GlobalExceptionMiddleware(RequestDelegate next)
    {

        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

        }
        catch (Exception ex)
        {
            string controllerNames = "";
            string actionName = "";
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                controllerNames = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>().ControllerName;
                actionName = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>().ActionName;
            }


            string propertyValue0 = $"{controllerNames}/{actionName}";
            Log.Error(ex, "{route} - Error message- {message} ", propertyValue0, ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}