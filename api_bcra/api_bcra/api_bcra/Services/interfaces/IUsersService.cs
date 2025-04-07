using api_bcra.Models;
using api_bcra.Services.Responses;

namespace api_bcra.Services.interfaces
{
    public interface IUsersService
    {
        Task<UserActionResponse> GetAll();
        Task<UserActionResponse> Get(int id);
        Task<UserActionResponse> GetByUsername(string username);
        Task<UserActionResponse> Add(User user);
        Task<UserActionResponse> Edit(User user);
        Task<UserActionResponse> Delete(int id);
    }
}
