using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IUserService:IBaseService<User>
    {
        Task<ServiceResponse> InsertUserExam(UserBlockRequestModel data);
        Task<ServiceResponse> InitLogin(string userID);
        //Task<ServiceResponse> GetAllRole();
        // Task<ServiceResponse> InsertRolePermisstion(RolePermisstionRequestModel model);
        // Task<ServiceResponse> SendMail();
        //Task<ServiceResponse> ResetPassword(string newPassword);
    }
}
