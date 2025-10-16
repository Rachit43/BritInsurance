using BritInsurance.Application.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BritInsurance.Infrastructure.Identity
{
    public class UserContext : IUserContext
    {
        public string UserName { get; private set; } = string.Empty;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            GetScope(httpContextAccessor);
        }

        private void GetScope(IHttpContextAccessor httpContextAccessor)
        {
            ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;

            if (user != null && user.Identity != null && user.Identity.IsAuthenticated && user.Identity.Name != null)
            {
                UserName = user.Identity.Name;
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
