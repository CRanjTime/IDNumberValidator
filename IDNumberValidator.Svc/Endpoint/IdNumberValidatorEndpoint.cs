using IDNumberValidator.Svc.IServices;
using IDNumberValidator.Svc.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.Endpoint
{
    internal static class IdNumberValidatorEndpoint
    {
        public static IApplicationBuilder UseIdNumberValidatorService(this IApplicationBuilder builder, IConfiguration config)
        {
            var route = "/ValidateIdNumber";
            if (builder is WebApplication app)
            {
                app.MapPost(route, async ([FromBody] IdNumberValidationRequest request, IIdNumberValidatorService service, HttpContext ctx) =>
                {
                    return await ValidateIdNumber(request, service, ctx);
                }).WithName("ValidateIdNumber");
            }

            return builder;
        }

        private static async Task<IResult> ValidateIdNumber(IdNumberValidationRequest request, IIdNumberValidatorService service, HttpContext ctx)
        {
            try
            {
                IdNumberValidationResult result = await service.ValidateIdNumber(request, ctx.RequestAborted);
                return Results.Ok(result);
            }
            catch (Exception ex) when (HandleException(ex) is IResult result)
            {
                return result;
            }
        }

        static IResult HandleException(Exception ex) =>
        ex switch
        {
            ArgumentException => Results.BadRequest(new { error = ex.Message }),
            NotSupportedException => Results.BadRequest(new { error = ex.Message }),
            InvalidOperationException => Results.Problem(
                statusCode: 500,
                title: ex.Message,
                detail: ex.InnerException?.Message),
            _ => Results.Problem(
                statusCode: 500,
                title: "An unexpected error occurred.",
                detail: ex.Message)
        };
    }
}
