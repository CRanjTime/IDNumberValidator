using IDNumberValidator.Svc.IServices;
using System;

namespace IDNumberValidator.Svc.Factory
{
    public interface IValidatorFactory
    {
        IIdNumberValidator CreateValidator(string validatorType);
    }

    internal class ValidatorFactory : IValidatorFactory
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

            var constructor = type.GetConstructor([typeof(IAlgorithmFactory)]);
            if (constructor == null)
            {
                throw new InvalidOperationException($"Constructor for '{validatorType}' with IConfiguration parameter not found.");
            }

            return (IIdNumberValidator)constructor.Invoke([_algorithmFactory]);
        }
    }
}
