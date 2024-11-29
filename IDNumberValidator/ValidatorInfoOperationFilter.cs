using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace IDNumberValidator.Api
{
    public class ValidatorInfoOperationFilter : IOperationFilter
    {
        private readonly IConfiguration _configuration;

        public ValidatorInfoOperationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "ValidateIdNumber")
            {
                var creditCardValidator = _configuration.GetValue<string>("Validators:CreditCard").Split('.').Last();

                operation.Description += $@"<h3>Available Validators:</h3>
                                            <p><strong>(1) Credit Card</strong>: {creditCardValidator}</p>";

                if (!operation.Responses.ContainsKey("400"))
                    operation.Responses["400"] = new OpenApiResponse { Description = "Bad Request" };
                if (!operation.Responses.ContainsKey("500"))
                    operation.Responses["500"] = new OpenApiResponse { Description = "Internal Server Error" };

                operation.Responses["200"].Description = $"Validation success.";
                operation.Responses["400"].Description = $"Unupported Id Type. Ongoing Development";
                operation.Responses["500"].Description = $"Invalid Id Type. Provided a type that was not in the list.";
            }
        }
    }
}
