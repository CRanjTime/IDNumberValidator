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

        /// <summary>
        /// Validate the <param name="idNumber"/> using an algorithm specified in the config.
        /// </summary>
        /// <param name="idNumber">Id Number to validate</param>
        /// <returns>Returns <c>true</c> if the input is valid; otherwise, <c>false</c>.</returns>
        public bool Validate(string idNumber)
        {
            var algorithm = _algorithmFactory.CreateAlgorithm("CreditCard");
            return algorithm.Perform<string, bool>(idNumber);
        }
    }
}
