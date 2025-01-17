﻿using IDNumberValidator.Svc.Factory;
using IDNumberValidator.Svc.IServices;
using IDNumberValidator.Svc.Model;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.Services
{
    internal class IdNumberValidatiorService : IIdNumberValidatorService
    {
        readonly IValidatorFactory _validatorFactory;
        public IdNumberValidatiorService(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        /// <summary>
        /// Validate Id Number based on type. Validator is different per type. 
        /// Currently supports: Credit Card
        /// </summary>
        /// <param name="request"><c>IdNumberValidationRequest</c> containing the Id Number and Type to validate</param>
        /// <param name="ct">Cancellation token from the HTTPContext</param>
        /// <returns>A <c>IdNumberValidationResult</c> containing the result.</returns>
        /// <exception cref="ArgumentException">IdNumber is not provided or Contains letters and symbols.</exception>
        /// <exception cref="NotSupportedException">Type provided is not yet supported</exception>
        /// <exception cref="InvalidOperationException">Type provided is invalid</exception>
        public async Task<IdNumberValidationResult> ValidateIdNumber(IdNumberValidationRequest request, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (request == null || string.IsNullOrWhiteSpace(request.IdNumber))
            {
                throw new ArgumentException("IdNumber is required.");
            }

            string sanitizedNumber = Regex.Replace(request.IdNumber, @"\s+", "");
            if (!Regex.IsMatch(sanitizedNumber, @"^\d+$"))
            {
                throw new ArgumentException("IdNumber must contain only digits.");
            }

            switch (request.Type)
            {
                case IdType.CreditCard:
                    var validator = _validatorFactory.CreateValidator("CreditCard");
                    return new IdNumberValidationResult
                    {
                        Valid = await Task.FromResult(validator.Validate(sanitizedNumber)),
                        Type = IdType.CreditCard.ToString()
                    };

                case IdType.SocialSecurity:
                    throw new NotSupportedException("Validation for Social Security Number (Type 2) is under construction.");

                default:
                    throw new InvalidOperationException("Invalid Type. Supported types are CreditCard (1) and SocialSecurity (2).");
            }
        }
    }
}
