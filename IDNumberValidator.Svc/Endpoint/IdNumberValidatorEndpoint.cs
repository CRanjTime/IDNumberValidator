using IDNumberValidator.Svc.IServices;
using IDNumberValidator.Svc.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

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
                    try
                    {
                        IdNumberValidationResult result = await service.ValidateIdNumber(request, ctx.RequestAborted);
                        return Results.Ok(result);
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (NotSupportedException ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (InvalidOperationException ex)
                    {
                        return Results.Problem(statusCode: 500, title: ex.Message, detail: ex.InnerException?.Message);
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(statusCode: 500, title: "An unexpected error occurred.", detail: ex.Message);
                    }
                }).WithName("ValidateIdNumber");
            }

            return builder;
        }
    }
}
