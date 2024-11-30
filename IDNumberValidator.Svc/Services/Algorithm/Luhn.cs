using IDNumberValidator.Svc.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace IDNumberValidator.Svc.Services.Algorithm
{
    internal class Luhn : IAlgorithm
    {
        private readonly IConfiguration _configuration;
        public Luhn(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Validates the input using an algorithm. 
        /// Use this if you want to provide your own algorithm
        /// </summary>
        /// <typeparam name="T">The type of the input data to validate.</typeparam>
        /// <typeparam name="TResult">The result type of the validation</typeparam>
        /// <param name="input">The input data to validate.</param>
        /// <param name="algorithm">Lambda function that will perform the validation</param>
        /// <returns>A value of type <typeparamref name="TResult"/> indicating the validation result.</returns>
        public TResult Perform<T, TResult>(T input, Func<T, TResult> algorithm)
        {
            return algorithm(input);
        }

        /// <summary>
        /// Validates the input using a luhn algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the input data to validate. Must be <see cref="string"/>.</typeparam>
        /// <typeparam name="TResult">The result type of the validation, typically a <see cref="bool"/>.</typeparam>
        /// <param name="input">The input data to validate. Must be a string representing a numeric ID.</param>
        /// <returns>
        /// A value of type <typeparamref name="TResult"/> indicating the validation result.
        /// For example, returns <c>true</c> if the input is valid according to the implemented algorithm; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the input is not a string or is not a valid numeric identifier.
        /// </exception>
        public TResult Perform<T, TResult>(T input)
        {
            if (input is string idNumber)
            {
                if (string.IsNullOrWhiteSpace(idNumber))
                    throw new ArgumentException("Input string mus not be empty.");

                if (!idNumber.All(char.IsDigit))
                    throw new ArgumentException("Input string must contain only numeric characters.");

                int sum = 0;
                bool alternate = false;
                for (int i = idNumber.Length - 1; i >= 0; i--)
                {
                    int n = int.Parse(idNumber[i].ToString());
                    if (alternate)
                    {
                        n *= 2;
                        if (n > 9) n -= 9;
                    }
                    sum += n;
                    alternate = !alternate;
                }
                bool isValid = sum % 10 == 0;

                return (TResult)(object)isValid;
            }

            throw new ArgumentException("Input must be a string representing a numeric ID.");
        }
    }
}
