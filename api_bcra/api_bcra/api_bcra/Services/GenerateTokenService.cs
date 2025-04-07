using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api_bcra.Context;
using api_bcra.Models;
using api_bcra.Services.interfaces;
using api_bcra.Services.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api_bcra.Services
{
    public class GenerateTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly BCRADbContext _context;

        public GenerateTokenService(IConfiguration configuration, BCRADbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<TokensResponse> RefreshToken(string refToken)
        {
            var check_refToken = await _context.RefreshTokens.Where(r => r.Token.Equals(refToken)).FirstOrDefaultAsync();
            if (check_refToken == null)
            {
                return new TokensResponse { Refresh_Token = "", Access_Token = "" };
            } else
            {
                if (check_refToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new TokensResponse { Refresh_Token = "", Access_Token = "" };
                }

                check_refToken.Used = 1;
                _context.Entry(check_refToken).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var user = await _context.Users.Where(u => u.Id == check_refToken.IdUser).Include(u => u.UserRoles).FirstOrDefaultAsync();

                var new_refToken = await GenerateRefToken(user);
                var new_token = GenerateToken(user);

                return new TokensResponse { Refresh_Token = new_refToken, Access_Token = new_token };
            }
        }
        public async Task<string> GenerateRefToken(User user)
        {
            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddDays(7);
            var new_token = new RefreshToken { Token  = token, IdUser = user.Id, ExpiryDate = expiry, CreationDate = DateTime.UtcNow, Used = 0 };
            _context.RefreshTokens.Add(new_token);
            await _context.SaveChangesAsync();
            return token;
        }
        public string GenerateToken(User user)
        {
            var jwt_config = _configuration.GetSection("JWT_Config");
            var jwt_issuer = jwt_config.GetValue<string>("Issuer");
            var jwt_audience = jwt_config.GetValue<string>("Audience");
            var jwt_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_config.GetValue<string>("SigningKey")));
            var jwt_expiration = jwt_config.GetValue<int>("ExpiryInMinutes");
            var expiry = DateTime.UtcNow.AddMinutes(jwt_expiration);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.UserRoles.First().IdRole.ToString()),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                }),
                Audience = jwt_audience,
                Expires = expiry,
                Issuer = jwt_issuer,
                SigningCredentials = new SigningCredentials(jwt_key, SecurityAlgorithms.HmacSha256)
            };

            var token_handler = new JwtSecurityTokenHandler();
            var create_token = token_handler.CreateToken(descriptor);
            var write_token = token_handler.WriteToken(create_token);
            return write_token;
        }
    }
}
