using api_bcra.Models;
using api_bcra.Services.Responses;

namespace api_bcra.Services.interfaces
{
    public interface IMainService
    {
        Task<ScoringResponse> GetDebts(string cuit, int userId);
    }
}
