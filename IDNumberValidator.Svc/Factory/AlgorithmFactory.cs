using IDNumberValidator.Svc.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.Factory
{
    public interface IAlgorithmFactory
    {
        IAlgorithm CreateAlgorithm(string validatorType);
    }

    public class AlgorithmFactory : IAlgorithmFactory
    {
        private readonly IConfiguration _config;

        public AlgorithmFactory(IConfiguration config)
        {
            _config = config;
        }

        public IAlgorithm CreateAlgorithm(string validatorType)
        {
            string className = _config.GetValue<string>($"Validators:{validatorType}");

            var type = Type.GetType(className);
            if (type == null)
            {
                throw new InvalidOperationException($"Algorithm class '{className}' not found.");
            }

            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });
            if (constructor == null)
            {
                throw new InvalidOperationException($"Constructor for '{className}' with IConfiguration parameter not found.");
            }

            return (IAlgorithm)constructor.Invoke(new object[] { _config });
        }
    }

}
