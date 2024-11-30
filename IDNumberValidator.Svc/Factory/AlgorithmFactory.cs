using IDNumberValidator.Svc.IServices;
using Microsoft.Extensions.Configuration;
using System;

namespace IDNumberValidator.Svc.Factory
{
    public interface IAlgorithmFactory
    {
        IAlgorithm CreateAlgorithm(string validatorType);
    }

    internal class AlgorithmFactory : IAlgorithmFactory
    {
        private readonly IConfiguration _config;

        public AlgorithmFactory(IConfiguration config)
        {
            _config = config;
        }


        /// <summary>
        /// Dynamically create instance of the algorithm class based on the type that was provided.
        /// </summary>
        /// <param name="validatorType">Type of validator that will be used to find the algorithm.</param>
        /// <returns>returns an instance of a class of type <c>IAlgorithm</c>.</returns>
        /// <exception cref="InvalidOperationException">If the algorithm is not found and if the constructor invoked does not exists</exception>
        public IAlgorithm CreateAlgorithm(string validatorType)
        {
            string className = _config.GetValue<string>($"Validators:{validatorType}");

            var type = Type.GetType(className);
            if (type == null)
            {
                throw new InvalidOperationException($"Algorithm class '{className}' not found.");
            }

            var constructor = type.GetConstructor([typeof(IConfiguration)]);
            if (constructor == null)
            {
                throw new InvalidOperationException($"Constructor for '{className}' with IConfiguration parameter not found.");
            }

            return (IAlgorithm)constructor.Invoke([_config]);
        }
    }

}
