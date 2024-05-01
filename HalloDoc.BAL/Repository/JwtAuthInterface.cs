using HalloDoc.BAL.Interface;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    /// <summary>
    /// Generate, validate and access data of jwt token
    /// </summary>
    public class JwtAuthInterface : IJwtAuthInterface 
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration Configuration;
        public JwtAuthInterface(IConfiguration Configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.Configuration = Configuration;
        }
        public string GenerateJWTAuthetication(UserDataViewModel UserData)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, UserData.Username),
                new Claim(ClaimTypes.Role, UserData.Role),
                new Claim("UserId", UserData.UserId.ToString()),
                new Claim("FirstName", UserData.FirstName ?? string.Empty),
                new Claim("LastName" , UserData.LastName ?? string.Empty),
                new Claim("Roleid" , UserData.roleid ?? string.Empty),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires =
                DateTime.UtcNow.AddMinutes(100);

            var token = new JwtSecurityToken(
                Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityTokenHandler)
        {
            jwtSecurityTokenHandler = null;

            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                // Corrected access to the validatedToken
                jwtSecurityTokenHandler = (JwtSecurityToken)validatedToken;

                if (jwtSecurityTokenHandler != null)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        public UserDataViewModel AccessData(string token)
        {
            UserDataViewModel model = new();

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            model.UserId = jwt.Claims.First(x => x.Type == "UserId").Value;
            model.Username = jwt.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            model.Role = jwt.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            model.FirstName = jwt.Claims.First(x => x.Type == "FirstName").Value;
            model.LastName = jwt.Claims.First(y => y.Type == "LastName").Value;
            model.roleid = jwt.Claims.First(y => y.Type == "Roleid").Value;
            return model;

        }
    }
}