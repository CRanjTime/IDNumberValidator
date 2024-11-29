using IDNumberValidator.Svc.Endpoint;
using IDNumberValidator.Svc.Factory;
using IDNumberValidator.Svc.IServices;
using IDNumberValidator.Svc.Services;
using IDNumberValidator.Svc.Services.Algorithm;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IDNumberValidator.Svc
{
    public static class Extension
    {
        public static IServiceCollection AddIdNumberValidator(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAlgorithmFactory, AlgorithmFactory>();
            services.AddScoped<IValidatorFactory, ValidatorFactory>();
            services.AddScoped<IIdNumberValidatorService, IdNumberValidatiorService>();
            return services;
        }

        public static IApplicationBuilder UseIdNumberValidator(this IApplicationBuilder builder, IConfiguration config) =>
            builder.UseIdNumberValidatorService(config);
    }
}
