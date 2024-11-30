using IDNumberValidator.Svc.Factory;
using IDNumberValidator.Svc.IServices;
using Moq;
using System;

namespace IDNumberValidator.Svc.UnitTests
{
    [TestClass]
    public class ValidatorFactoryTests
    {
        private Mock<IAlgorithmFactory> _mockAlgorithmFactory;
        private ValidatorFactory _validatorFactory;

        [TestInitialize]
        public void Setup()
        {
            _mockAlgorithmFactory = new Mock<IAlgorithmFactory>();
            _validatorFactory = new ValidatorFactory(_mockAlgorithmFactory.Object);
        }

        [TestMethod]
        public void CreateValidator_ReturnsValidatorInstance_WhenValidatorTypeIsValid()
        {
            // Arrange
            const string validValidatorType = "CreditCard"; // Assuming `CreditCardValidator` exists in the namespace

            // Act
            var validator = _validatorFactory.CreateValidator(validValidatorType);

            // Assert
            Assert.IsNotNull(validator);
            Assert.IsInstanceOfType(validator, typeof(IIdNumberValidator));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateValidator_ThrowsInvalidOperationException_WhenValidatorClassNotFound()
        {
            // Arrange
            const string invalidValidatorType = "NonExistent";

            // Act
            _validatorFactory.CreateValidator(invalidValidatorType);

            // Assert: Exception is expected
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateValidator_ThrowsInvalidOperationException_WhenConstructorNotFound()
        {
            // Arrange
            const string validatorTypeWithoutConstructor = "InvalidConstructorValidator"; // Assuming no valid constructor exists

            // Act
            _validatorFactory.CreateValidator(validatorTypeWithoutConstructor);

            // Assert: Exception is expected
        }

        [TestMethod]
        public void CreateValidator_ThrowsInvalidOperationException_WithCorrectErrorMessage_WhenValidatorClassNotFound()
        {
            // Arrange
            const string invalidValidatorType = "NonExistent";

            // Act & Assert
            try
            {
                _validatorFactory.CreateValidator(invalidValidatorType);
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("Validator class 'NonExistent' not found.", ex.Message);
            }
        }

        [TestMethod]
        public void CreateValidator_ThrowsInvalidOperationException_WithCorrectErrorMessage_WhenConstructorNotFound()
        {
            // Arrange
            const string validatorTypeWithoutConstructor = "InvalidConstructorValidator"; // Assuming no valid constructor exists

            // Act & Assert
            try
            {
                _validatorFactory.CreateValidator(validatorTypeWithoutConstructor);
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("Validator class 'InvalidConstructorValidator' not found.", ex.Message);
            }
        }
    }

}
