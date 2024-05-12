using Education.Application.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.DataValidation;
using System.ComponentModel;
using System.Reflection;

namespace Education.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseServiceController<User>
    {
        readonly IUserService _userService;
        readonly ICheckExamsService _checkExamsService;
        public UserController(IUserService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            _userService = serviceProvider.GetRequiredService<IUserService>();
            _checkExamsService = serviceProvider.GetRequiredService<ICheckExamsService>();
            this.currentType = typeof(User);
        }
        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task<ServiceResponse> CreateUser([FromBody] UserRequestModel data)
        {
            var user = new User()
            {
                UserName = data.UserName,
                Password = data.Password,
                FullName = data.FullName
            };
            return await _userService.Add(user);
        }
        [HttpGet("excel")]
        [AllowAnonymous]
        public async Task<IActionResult> Excel()
        {
            var lstData = new List<StudentDetects>() { new StudentDetects()
                {
                    StudentName = "Bùi Tuấn Anh",
                    LstResult = new List<string>(){"A","B","D"},
                    ExamsCode = "BHGSU",
                    StudentCode = "B-0825"

                },
                new StudentDetects()
                {
                    StudentName = "Bùi Tuấn Minh",
                    LstResult = new List<string>(){"A","B","D"},
                    ExamsCode = "BHGSU",
                    StudentCode = "B-0829"
                }

            };
            var streamData = await _checkExamsService.ExportData(lstData, "TUấn anh");
            streamData.Position = 0;
            return File(streamData.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "test.xlsx");
        }
        [HttpPost("excel")]
        [AllowAnonymous]
        public async Task<IActionResult> Import(IFormFile formFile)
        {
             await _checkExamsService.ImportStudentsResultExams(formFile);
            return Ok();
        }
        [HttpPost("users-block")]
        [AllowAnonymous]
        public async Task<IActionResult> UserBlock(UserBlockRequestModel data)
        {
            var res = await _userService.InsertUserExam(data);
            return Ok(res);
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