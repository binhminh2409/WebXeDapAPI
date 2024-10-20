using WebXeDapAPI.Data;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebXeDapAPI.Helper
{
    public class Token
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public Token(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        //Sinh ma token
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Secret").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime now = DateTime.Now; // Lấy thời gian hiện tại
            int expirationMinutes = 60; // Đặt thời gian hết hạn là 60 phút
            DateTime expiration = now.AddMinutes(expirationMinutes); // Tính thời gian hết hạn

            var token = new JwtSecurityToken(claims: claims, expires: expiration,
                signingCredentials: cred, issuer: _configuration["Jwt:ValidIssuer"],
            audience: _configuration["Jwt:ValidAudience"]);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        //Xac thuc token
        public ClaimsPrincipal ValidataToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _configuration["Jwt:ValidIssuer"],
                ValidAudience = _configuration["Jwt:ValidAudience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
        //kiem tra trang thai token
        public StatusToken CheckTokenStatus(int userId)
        {
            //Tìm Giá trị của token theo UserId
            var userTokens = _dbContext.AccessTokens
                .Where(x => x.UserId == userId && x.Status == StatusToken.Valid)
                .OrderByDescending(x => x.ExpirationDate)
                .FirstOrDefault();
            if (userTokens != null)
            {
                if (userTokens.ExpirationDate > DateTime.Now)
                {
                    return StatusToken.Valid;
                }
            }
            return StatusToken.Valid;
        }
        public int? ExtractUserIdFromToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Secret").Value!));

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:ValidAudience"],
                    ValidateLifetime = true,
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var nameIdentifierClaim = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (nameIdentifierClaim != null)
                {
                    if (int.TryParse(nameIdentifierClaim, out int userId))
                    {
                        return userId;
                    }
                }
                Console.WriteLine(principal);
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
