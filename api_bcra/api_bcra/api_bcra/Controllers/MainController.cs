using System.Security.Claims;
using api_bcra.Services.interfaces;
using api_bcra.Services.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_bcra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        private readonly IMainService _mainService;
        public MainController(IMainService mainService)
        {
            _mainService = mainService;
        }

        [HttpGet("{cuit}")]
        public async Task<IActionResult> GetDebts(string cuit)
        {
            var userId = Int32.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            var score = await _mainService.GetDebts(cuit, userId);
            return Ok(score);
        }
    }
}
