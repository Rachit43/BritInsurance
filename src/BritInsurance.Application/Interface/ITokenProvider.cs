using BritInsurance.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BritInsurance.Application.Interface
{
    public interface ITokenProvider
    {
        UserIdentityDto GenerateJwtToken(string user, string[] roles);
    }
}
