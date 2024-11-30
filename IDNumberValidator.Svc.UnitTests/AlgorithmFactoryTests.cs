using IDNumberValidator.Svc.Factory;
using IDNumberValidator.Svc.IServices;
using Microsoft.Extensions.Configuration;
using Moq;
using System;

namespace IDNumberValidator.Svc.UnitTests
{
    [TestClass]
    public class AlgorithmFactoryTests
    {
        private Mock<IConfiguration> _mockConfig;
        private Mock<IConfigurationSection> _mockConfigSection;
        private AlgorithmFactory _algorithmFactory;

        [TestInitialize]
        public void Setup()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfigSection = new Mock<IConfigurationSection>();
            _algorithmFactory = new AlgorithmFactory(_mockConfig.Object);
        }

        [TestMethod]
        public void CreateAlgorithm_ReturnsAlgorithmInstance_WhenValidatorTypeIsValid()
        {
            // Arrange
            const string validatorType = "CreditCard"; // Validator type key
            const string className = "IDNumberValidator.Svc.Services.Algorithm.Luhn"; // Fully qualified name of the class

            _mockConfigSection.Setup(section => section.Value).Returns(className);
            _mockConfig.Setup(config => config.GetSection($"Validators:{validatorType}")).Returns(_mockConfigSection.Object);

            // Act
            var algorithm = _algorithmFactory.CreateAlgorithm(validatorType);

            // Assert
            Assert.IsNotNull(algorithm);
            Assert.IsInstanceOfType(algorithm, typeof(IAlgorithm));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateAlgorithm_ThrowsInvalidOperationException_WhenAlgorithmClassNotFound()
        {
            // Arrange
            const string validatorType = "InvalidType"; // No valid class name in config

            _mockConfigSection.Setup(section => section.Value).Returns(string.Empty);
            _mockConfig.Setup(config => config.GetSection($"Validators:{validatorType}")).Returns(_mockConfigSection.Object);

            // Act
            _algorithmFactory.CreateAlgorithm(validatorType);

            // Assert: Exception is expected
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateAlgorithm_ThrowsInvalidOperationException_WhenConstructorNotFound()
        {
            // Arrange
            const string validatorType = "NoConstructor";
            const string className = "IDNumberValidator.Svc.Services.Algorithm.NoConstructorAlgorithm"; // Class without the expected constructor

            _mockConfigSection.Setup(section => section.Value).Returns(className);
            _mockConfig.Setup(config => config.GetSection($"Validators:{validatorType}")).Returns(_mockConfigSection.Object);

            // Act
            _algorithmFactory.CreateAlgorithm(validatorType);

            // Assert: Exception is expected
        }

        [TestMethod]
        public void CreateAlgorithm_ThrowsInvalidOperationException_WithCorrectMessage_WhenAlgorithmClassNotFound()
        {
            // Arrange
            const string validatorType = "InvalidType";

            _mockConfigSection.Setup(section => section.Value).Returns(string.Empty);
            _mockConfig.Setup(config => config.GetSection($"Validators:{validatorType}")).Returns(_mockConfigSection.Object);

            // Act & Assert
            try
            {
                _algorithmFactory.CreateAlgorithm(validatorType);
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("Algorithm class '' not found.", ex.Message);
            }
        }

        [TestMethod]
        public void CreateAlgorithm_ThrowsInvalidOperationException_WithCorrectMessage_WhenConstructorNotFound()
        {
            // Arrange
            const string validatorType = "NoConstructor";
            const string className = "IDNumberValidator.Svc.Services.Algorithm.NoConstructorAlgorithm"; // Class without expected constructor

            _mockConfigSection.Setup(section => section.Value).Returns(className);
            _mockConfig.Setup(config => config.GetSection($"Validators:{validatorType}")).Returns(_mockConfigSection.Object);

            // Act & Assert
            try
            {
                _algorithmFactory.CreateAlgorithm(validatorType);
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("Algorithm class 'IDNumberValidator.Svc.Services.Algorithm.NoConstructorAlgorithm' not found.", ex.Message);
            }
        }
    }


}
