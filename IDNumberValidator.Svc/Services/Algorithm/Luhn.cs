using IDNumberValidator.Svc.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.Services.Algorithm
{
    public class Luhn : IAlgorithm
    {
        private readonly IConfiguration _configuration;
        public Luhn(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TResult Perform<T, TResult>(T input, Func<T, TResult> algorithm)
        {
            return algorithm(input);
        }

        public TResult Perform<T, TResult>(T input)
        {
            if (input is string idNumber)
            {
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
