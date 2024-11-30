using IDNumberValidator.Svc.Factory;
using IDNumberValidator.Svc.IServices;
using IDNumberValidator.Svc.Model;
using IDNumberValidator.Svc.Services;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.UnitTests
{
    [TestClass]
    public class IdNumberValidatiorServiceTests
    {
        private Mock<IValidatorFactory> _mockValidatorFactory;
        private IdNumberValidatiorService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockValidatorFactory = new Mock<IValidatorFactory>();
            _service = new IdNumberValidatiorService(_mockValidatorFactory.Object);
        }

        [TestMethod]
        public async Task ValidateIdNumber_ReturnsValidResult_WhenCreditCardIsValid()
        {
            // Arrange
            var request = new IdNumberValidationRequest
            {
                IdNumber = "4111111111111111",
                Type = IdType.CreditCard
            };

            var mockValidator = new Mock<IIdNumberValidator>();
            mockValidator.Setup(v => v.Validate(It.IsAny<string>())).Returns(true);

            _mockValidatorFactory.Setup(f => f.CreateValidator("CreditCard")).Returns(mockValidator.Object);

            // Act
            var result = await _service.ValidateIdNumber(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Valid);
            Assert.AreEqual("CreditCard", result.Type);
        }

        [TestMethod]
        public async Task ValidateIdNumber_ThrowsArgumentException_WhenIdNumberIsNullOrEmpty()
        {
            // Arrange
            var request = new IdNumberValidationRequest
            {
                IdNumber = "",
                Type = IdType.CreditCard
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _service.ValidateIdNumber(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task ValidateIdNumber_ThrowsArgumentException_WhenIdNumberContainsNonDigits()
        {
            // Arrange
            var request = new IdNumberValidationRequest
            {
                IdNumber = "4111-1111-1111-111X",
                Type = IdType.CreditCard
            };

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _service.ValidateIdNumber(request, CancellationToken.None));

            Assert.AreEqual("IdNumber must contain only digits.", exception.Message);
        }

        [TestMethod]
        public async Task ValidateIdNumber_ThrowsNotSupportedException_WhenTypeIsSocialSecurity()
        {
            // Arrange
            var request = new IdNumberValidationRequest
            {
                IdNumber = "123456789",
                Type = IdType.SocialSecurity
            };

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<NotSupportedException>(() =>
                _service.ValidateIdNumber(request, CancellationToken.None));

            Assert.AreEqual("Validation for Social Security Number (Type 2) is under construction.", exception.Message);
        }

        [TestMethod]
        public async Task ValidateIdNumber_ThrowsInvalidOperationException_WhenTypeIsInvalid()
        {
            // Arrange
            var request = new IdNumberValidationRequest
            {
                IdNumber = "123456789",
                Type = (IdType)999 // Invalid enum value
            };

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _service.ValidateIdNumber(request, CancellationToken.None));

            Assert.AreEqual("Invalid Type. Supported types are CreditCard (1) and SocialSecurity (2).", exception.Message);
        }

        [TestMethod]
        public async Task ValidateIdNumber_ThrowsOperationCanceledException_WhenCancellationTokenIsTriggered()
        {
            // Arrange
            var request = new IdNumberValidationRequest
            {
                IdNumber = "4111111111111111",
                Type = IdType.CreditCard
            };

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
                _service.ValidateIdNumber(request, cts.Token));
        }
    }

}
