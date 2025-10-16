using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BritInsurance.Application.Dto
{
    public record LoginDto
    {
        public required string UserName { get; init; }

        public required string Password { get; init; }
    }

    public record RefreshTokenDto
    {
        public required string RefreshToken { get; init; }
    }
}
