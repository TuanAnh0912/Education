using Education.Application.Interface;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace Education.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateControllerController : ControllerBase
    {
        private IAuthenService _authenService;
        public AuthenticateControllerController(IAuthenService authenService)
        {
            _authenService = authenService;
        }
        [HttpPost("login")]
        public async Task<ServiceResponse> CheckLogin([FromBody] LoginRequest data)
        {
            var rsLogin = await _authenService.LoginAsync(data.Username, data.Password);
            if (!rsLogin.Success)
            {
                return new ServiceResponse(rsLogin.Success, rsLogin.Message ?? "");
            }
            //setTokenCookie(rsLogin.RefreshToken);
            return new ServiceResponse(rsLogin.Success, rsLogin.Message ?? "", new
            {
                access_token = rsLogin.ToKen
            });
        }

    }
}