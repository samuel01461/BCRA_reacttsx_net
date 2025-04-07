using api_bcra.Models;
using api_bcra.Services.Responses;

namespace api_bcra.Services.interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        Task<string> GenerateRefToken(User user);
        Task<TokensResponse> RefreshToken(string refToken);
    }
}
