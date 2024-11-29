using IDNumberValidator.Svc.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.Factory
{
    public interface IValidatorFactory
    {
        IIdNumberValidator CreateValidator(string validatorType);
    }

    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IAlgorithmFactory _algorithmFactory;

        public ValidatorFactory(IAlgorithmFactory algorithmFactory)
        {
            _algorithmFactory = algorithmFactory;
        }

        public IIdNumberValidator CreateValidator(string validatorType)
        {
            var type = Type.GetType($"IDNumberValidator.Svc.Services.Validator.{validatorType}Validator");
            if (type == null)
            {
                throw new InvalidOperationException($"Validator class '{validatorType}' not found.");
            }

            var constructor = type.GetConstructor(new[] { typeof(IAlgorithmFactory) });
            if (constructor == null)
            {
                throw new InvalidOperationException($"Constructor for '{validatorType}' with IConfiguration parameter not found.");
            }

            return (IIdNumberValidator)constructor.Invoke(new object[] { _algorithmFactory });
        }
    }
}
