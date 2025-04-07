using api_bcra.Models;
using api_bcra.Repositories;
using api_bcra.Repositories.interfaces;
using api_bcra.Services.interfaces;
using api_bcra.Services.Responses;
using Microsoft.AspNetCore.Http;

namespace api_bcra.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        public UsersService(IUsersRepository userRepository) {
            _usersRepository = userRepository;
        }
        public async Task<UserActionResponse> Add(User user)
        {
            try
            {
                var add = await _usersRepository.Add(user);
                if (add != null)
                {
                    return new UserActionResponse { _User = user, StatusCode = 200, ErrorMessage = "Usuario creado correctamente" };
                } else
                {
                    return new UserActionResponse { _User = null, StatusCode = 500, ErrorMessage = "Ocurrio un error al crear el usuario" };
                }
            }
            catch (Exception ex) 
            {
                return new UserActionResponse { _User = null, StatusCode = 500, ErrorMessage = ex.Message };
            }
        }

        public async Task<UserActionResponse> Delete(int id)
        {
            try
            {
                var delete_user = await _usersRepository.Delete(id);

                if (delete_user)
                {
                    return new UserActionResponse { StatusCode = 200, ErrorMessage = "Usuario eliminado correctamente" };
                }
                else
                {
                    return new UserActionResponse { StatusCode = 500, ErrorMessage = "Ocurrio un error al eliminar el usuario" };
                }
            }
            catch (Exception ex)
            {
                return new UserActionResponse { StatusCode = 500, ErrorMessage = ex.Message };
            }
        }

        public async Task<UserActionResponse> Edit(User user)
        {
            try
            {
                var edit = await _usersRepository.Edit(user);

                if (edit != null)
                {
                    return new UserActionResponse { _User = user, StatusCode = 200, ErrorMessage = "Usuario modificado correctamente" };
                }
                else
                {
                    return new UserActionResponse { _User = null, StatusCode = 500, ErrorMessage = "Ocurrio un error al modificar el usuario" };
                }
            }
            catch (Exception ex)
            {
                return new UserActionResponse { _User = null, StatusCode = 500, ErrorMessage = ex.Message };
            }
        }

        public async Task<UserActionResponse> Get(int id)
        {
            try
            {
                var get_user = await _usersRepository.Get(id);

                if (get_user != null)
                {
                    return new UserActionResponse { _User = get_user, StatusCode = 200, ErrorMessage = "" };
                }
                else
                {
                    return new UserActionResponse { _User = null, StatusCode = 404, ErrorMessage = "Usuario no encontrado" };
                }
            }
            catch (Exception ex)
            {
                return new UserActionResponse { _User = null, StatusCode = 500, ErrorMessage = ex.Message };
            }
        }

        public async Task<UserActionResponse> GetAll()
        {
            try
            {
                var get_users = await _usersRepository.GetAll();

                if (get_users.Count > 0)
                {
                    return new UserActionResponse { _Users = get_users, StatusCode = 200 };
                }
                else
                {
                    return new UserActionResponse { _Users = [], StatusCode = 404, ErrorMessage = "Usuarios no encontrados" };
                }
            }
            catch (Exception ex)
            {
                return new UserActionResponse { _Users = [], StatusCode = 500, ErrorMessage = ex.Message };
            }
        }

        public async Task<UserActionResponse> GetByUsername(string username)
        {
            try
            {
                var get_user = await _usersRepository.GetByUsername(username);

                if (get_user != null)
                {
                    return new UserActionResponse { _User = get_user, StatusCode = 200, ErrorMessage = "" };
                }
                else
                {
                    return new UserActionResponse { _User = null, StatusCode = 404, ErrorMessage = "Usuario no encontrado" };
                }
            }
            catch (Exception ex)
            {
                return new UserActionResponse { _User = null, StatusCode = 500, ErrorMessage = ex.Message };
            }
        }
    }
}
