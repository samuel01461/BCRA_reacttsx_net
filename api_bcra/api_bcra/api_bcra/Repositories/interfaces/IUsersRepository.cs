using api_bcra.Models;

namespace api_bcra.Repositories.interfaces
{
    public interface IUsersRepository
    {
        Task<ICollection<User>> GetAll();
        Task<User> Get(int id);
        Task<User> GetByUsername(string username);
        Task<User> Add(User user);
        Task<User> Edit(User user);
        Task<bool> Delete(int id);
    }
}
