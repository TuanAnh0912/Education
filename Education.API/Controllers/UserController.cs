using Education.Application.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection;

namespace Education.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : BaseServiceController<User>
    {
        readonly IUserService _userService;
        public UserController(IUserService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            _userService = serviceProvider.GetRequiredService<IUserService>();
            this.currentType = typeof(License);
        }
        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task<ServiceResponse> CreateUser([FromBody] User user)
        {
            return await _userService.Add(user);
        }
        //[HttpGet("role-permission")]
        //public async Task<ServiceResponse> GetAllRole()
        //{
        //    var listRole = new List<string>() { "role.View" };
        //    var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
        //    if (checkPermisstion)
        //    {
        //        return await _userService.GetAllRole();
        //    }
        //    return new ServiceResponse(false, "Bạn không có quyền");
        //}
        //[HttpPost("new-role")]
        //public async Task<ServiceResponse> AddRolePermisstion([FromBody] RolePermisstionRequestModel model)
        //{
        //    var listRole = new List<string>() { "role.Add" };
        //    var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
        //    if (checkPermisstion)
        //    {
        //        return await _userService.InsertRolePermisstion(model);
        //    }
        //    return new ServiceResponse(false, "Bạn không có quyền");
        //}
        //[HttpGet("link-reset")]
        //public Task<ServiceResponse> GetLinkResetPassword()
        //{
        //    return _userService.SendMail();
        //}
        //[HttpPost("reset-password")]
        //public Task<ServiceResponse> GetLinkResetPassword(RequestResetPasswordModel data)
        //{
        //    return _userService.ResetPassword(data.NewPassword);
        //}
    }

}