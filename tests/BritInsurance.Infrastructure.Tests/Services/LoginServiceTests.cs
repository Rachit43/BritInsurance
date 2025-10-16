using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Infrastructure.Config;
using BritInsurance.Infrastructure.Services;
using Moq;

namespace BritInsurance.Infrastructure.Tests.Services
{
    public class LoginServiceTests
    {
        private readonly Mock<ITokenProvider> _mockTokenProvider;

        public LoginServiceTests()
        {
            _mockTokenProvider = new();
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginService = new LoginService(_mockTokenProvider.Object);
            var userName = "test";
            var password = "password";
            var roles = new[] { ApplicationRoles.View };
            var expectedToken = new UserIdentityDto() { AccessToken = "a", RefreshToken = "b", UserName = "c" };
            _mockTokenProvider.Setup(tp => tp.GenerateJwtToken(userName, roles)).Returns(expectedToken);

            // Act
            var result = loginService.Login(userName, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result);

            // Verify
            _mockTokenProvider.Verify(tp => tp.GenerateJwtToken(userName, roles), Times.Once);
            _mockTokenProvider.VerifyAll();
        }

        [Fact]
        public void Login_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var loginService = new LoginService(_mockTokenProvider.Object);
            var userName = "test1";
            var password = "password";
            var roles = new[] { ApplicationRoles.View };
            var expectedToken = new UserIdentityDto() { AccessToken = "a", RefreshToken = "b", UserName = "c" };

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() => loginService.Login(userName, password));

            // Verify
            _mockTokenProvider.Verify(tp => tp.GenerateJwtToken(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
            _mockTokenProvider.VerifyAll();
        }
    }
}
