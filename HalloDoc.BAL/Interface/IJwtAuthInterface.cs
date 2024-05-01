using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Interface
{
    /// <summary>
    /// Generate, validate and access data of jwt token
    /// </summary>
    public interface IJwtAuthInterface
    {
        string GenerateJWTAuthetication(UserDataViewModel UserData);
        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityTokenHandler);
        UserDataViewModel AccessData(string token);
    }
}
