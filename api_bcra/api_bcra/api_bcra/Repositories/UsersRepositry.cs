using api_bcra.Context;
using api_bcra.Models;
using api_bcra.Repositories.interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace api_bcra.Repositories
{
    public class UsersRepositry : IUsersRepository
    {
        private readonly BCRADbContext _context;
        public UsersRepositry(BCRADbContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User user)
        {
            try
            {
                var existsbyusername = await _context.Users.Where(u => u.Username == user.Username).FirstOrDefaultAsync();
                var existsbyemail = await _context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();

                if (existsbyusername == null && existsbyemail == null)
                {
                    var hash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Password = hash;
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return user;
                } else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
         
        public async Task<bool> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null) 
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            return false;
        }

        public async Task<User> Edit(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public async Task<User> Get(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id).Include(u => u.UserRoles).FirstOrDefaultAsync();

            if (user == null) {
                throw new Exception();
            } else { return user; }
        }

        public async Task<ICollection<User>> GetAll()
        {
            var users = await _context.Users.Include(u => u.UserRoles).ToListAsync();
            return users;
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _context.Users.Where(u => u.Username.Equals(username) ).Include(u => u.UserRoles).FirstOrDefaultAsync();
            return user;
        }
    }
}
