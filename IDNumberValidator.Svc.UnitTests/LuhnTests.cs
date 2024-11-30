using IDNumberValidator.Svc.Services.Algorithm;
using Microsoft.Extensions.Configuration;
using System;

namespace IDNumberValidator.Svc.UnitTests
{
    [TestClass]
    public class LuhnTests
    {
        private IConfiguration _configuration;
        private Luhn _luhn;

        [TestInitialize]
        public void TestInitialize()
        {
            // Mock configuration or initialize as needed
            _configuration = new ConfigurationBuilder().Build();
            _luhn = new Luhn(_configuration);
        }

        [DataTestMethod]
        [DataRow("4532015112830366", true)] // Valid Luhn number
        [DataRow("6011514433546201", true)] // Another valid Luhn number
        [DataRow("4532015112830367", false)] // Invalid Luhn number
        [DataRow("6011514433546202", false)] // Another invalid Luhn number
        public void Perform_ValidatesUsingLuhnAlgorithm(string idNumber, bool expected)
        {
            // Act
            bool result = _luhn.Perform<string, bool>(idNumber);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Perform_ThrowsArgumentException_WhenInputIsNotString()
        {
            // Arrange
            int invalidInput = 123456;

            // Act
            _luhn.Perform<int, bool>(invalidInput);

            // Exception is expected
        }

        [DataTestMethod]
        [DataRow("12345abc67890", "Input string must contain only numeric characters.")] // Non-numeric characters
        [DataRow("", "Input string mus not be empty.")] // Empty string
        [DataRow(" ", "Input string mus not be empty.")] // Whitespace only
        public void Perform_ThrowsArgumentException_WhenInputIsInvalidString(string invalidIdNumber, string expectedMessage)
        {
            // Act & Assert
            try
            {
                _luhn.Perform<string, bool>(invalidIdNumber);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                // Assert that the exception message matches the expected message
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public void Perform_ValidatesUsingCustomAlgorithm()
        {
            // Arrange
            string input = "123456789";
            Func<string, bool> customAlgorithm = id => id.Length == 9;

            // Act
            bool result = _luhn.Perform(input, customAlgorithm);

            // Assert
            Assert.IsTrue(result);
        }
    }

}
