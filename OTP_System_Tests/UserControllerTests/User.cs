using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OTP_System.Server.Controllers;
using OTP_System_Server.Models;
using OTP_System_Server.Services;


namespace OTP_System_Tests.UserControllerTests
{
    internal class User
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);
        }

        [Test]
        public void Register_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var credentials = new CredentialsModel { Username = "testuser", Password = "testpassword" };
            var expectedUser = new UserModel { Username = credentials.Username, PasswordHash = "hashedPassword" }; 
            _userServiceMock.Setup(service => service.RegisterUser(credentials.Username, credentials.Password))
                            .Returns(expectedUser);

            // Act
            var result = _userController.Register(credentials) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.NotNull(result.Value);
            Assert.IsInstanceOf<UserModel>(result.Value);
            Assert.That((result.Value as UserModel).Username, Is.EqualTo(expectedUser.Username));
        }

        [Test]
        public void Authenticate_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var credentials = new CredentialsModel { Username = "testuser", Password = "testpassword" };
            var expectedUser = new UserModel { Username = credentials.Username, PasswordHash = "hashedPassword" }; 
            _userServiceMock.Setup(service => service.Authenticate(credentials.Username, credentials.Password))
                            .Returns(expectedUser);

            // Act
            var result = _userController.Authenticate(credentials) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.NotNull(result.Value);
            Assert.IsInstanceOf<UserModel>(result.Value);
            Assert.That((result.Value as UserModel).Username, Is.EqualTo(expectedUser.Username));
        }

        [Test]
        public void ValidateOtp_ValidOtp_ReturnsOk()
        {
            // Arrange
            var otpValidation = new OtpValiationModel { Username = "testuser", Otp = "123456" };
            _userServiceMock.Setup(service => service.ValidateOtp(otpValidation.Username, otpValidation.Otp))
                            .Returns(true);

            // Act
            var result = _userController.ValidateOtp(otpValidation) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.NotNull(result.Value);
            Assert.That(result.Value, Is.EqualTo("OTP is valid"));
        }
    }
}
