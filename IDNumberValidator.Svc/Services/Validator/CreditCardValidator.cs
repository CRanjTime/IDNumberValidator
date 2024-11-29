using IDNumberValidator.Svc.Factory;
using IDNumberValidator.Svc.IServices;

namespace IDNumberValidator.Svc.Services.Validator
{
    internal class CreditCardValidator : IIdNumberValidator
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
