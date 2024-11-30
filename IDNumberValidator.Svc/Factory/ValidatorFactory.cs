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

        /// <summary>
        /// Dynamically create instance of the validator class based on the type that was provided.
        /// </summary>
        /// <param name="validatorType">Type of validator that will be invoked</param>
        /// <returns>returns an instance of a class of type <c>IIdNumberValidator</c>.</returns>
        /// <exception cref="InvalidOperationException">If the validator is not found and if the constructor invoked does not exists</exception>
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
