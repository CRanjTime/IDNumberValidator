using IDNumberValidator.Svc.Factory;
using IDNumberValidator.Svc.IServices;
using IDNumberValidator.Svc.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.Services.Validator
{
    public class CreditCardValidator : IIdNumberValidator
    {
        readonly IAlgorithmFactory _algorithmFactory;
        public CreditCardValidator(IAlgorithmFactory algorithmFactory)
        {
            _algorithmFactory = algorithmFactory;
        }
        public bool Validate(string idNumber)
        {
            var algorithm = _algorithmFactory.CreateAlgorithm("CreditCard");
            return algorithm.Perform<string, bool>(idNumber);
        }
    }
}
