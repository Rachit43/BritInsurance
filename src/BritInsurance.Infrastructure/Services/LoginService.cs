using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BritInsurance.Infrastructure.Services
{
    public class LoginService : ILoginService
    {
        private readonly ITokenProvider _tokenProvider;

        public LoginService(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public UserIdentityDto Login(string userName, string password)
        {
            if (userName == "test" && password == "password")
            {
                var roles = new[] { ApplicationRoles.View }; // Placeholder for user roles
                return _tokenProvider.GenerateJwtToken(userName, roles);
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
        }

        public UserIdentityDto RefreshToken(string refreshToken)
        {
            // ValidateRefreshToken(refreshToken);

            var userName = "test";
            var roles = new[] { ApplicationRoles.View };
            return _tokenProvider.GenerateJwtToken(userName, roles);
        }   
    }
}
