using System.CodeDom.Compiler;
using System.Security.Claims;
using api_bcra.Models;
using api_bcra.Services.interfaces;
using api_bcra.Services.Responses;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_bcra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenService _tokenService;
        public LoginController(IUsersService usersService, ITokenService tokenService) {
            _usersService = usersService;
            _tokenService = tokenService;
        }

        [HttpGet("{refresh_token}")]
        public async Task<IActionResult> RefreshToken(string refresh_token)
        {
            var check_refToken = await _tokenService.RefreshToken(refresh_token);
            if (check_refToken.Refresh_Token == "" && check_refToken.Access_Token == "")
            {
                return Unauthorized();
            }
            return Ok(check_refToken);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             
            var user = await _usersService.GetByUsername(login.Username);

            if (user.StatusCode == 404)
            {
                return NotFound();
            }

            if (user.StatusCode == 500)
            {
                return BadRequest();
            }

            var check_password = BCrypt.Net.BCrypt.Verify(login.Password, user._User.Password);

            if (check_password)
            {
                var token = _tokenService.GenerateToken(user._User);
                var refresh_token = await _tokenService.GenerateRefToken(user._User);
                return Ok(new TokensResponse { Refresh_Token = refresh_token, Access_Token = token} );
            }

            return Unauthorized();
        }
    }
}
