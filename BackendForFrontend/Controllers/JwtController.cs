using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BackendForFrontend.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly JwtHelpers _jwtHelpers;

        public JwtController(JwtHelpers jwtHelpers)
        {
            _jwtHelpers = jwtHelpers;
        }


        // 這裡用來產生我們的Token
        [AllowAnonymous]
        [HttpPost("~/gentoken")]
        public IActionResult GenToken(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }
            var token = _jwtHelpers.GenerateToken(username);
            return Ok(token);
        }

        // 來看看我們在Claims裡有有哪些屬性和內容
        [HttpGet("~/claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        // 回傳我們剛剛在產Token時輸入的username
        [HttpGet("~/username")]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }

        // 傳回Jwt的id
        [HttpGet("~/jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }
    }
}
