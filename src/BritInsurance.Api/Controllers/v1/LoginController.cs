using Asp.Versioning;
using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BritInsurance.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/login")]
    [ApiController]
    public class LoginController(ILogger<LoginController> logger, ILoginService loginService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(UserIdentityDto), StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] LoginDto loginRequest)
        {
            return Ok(loginService.Login(loginRequest.UserName, loginRequest.Password));
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(UserIdentityDto), StatusCodes.Status200OK)]
        public IActionResult Refresh([FromBody] RefreshTokenDto refreshToken)
        {
            return Ok(loginService.RefreshToken(refreshToken.RefreshToken));
        }
    }
}