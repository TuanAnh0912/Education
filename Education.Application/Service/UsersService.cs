using Education.Application.Helpers;
using Education.Application.Interface;
using Education.Core.Interface;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Education.Core.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Education.Application.Service.Base;

namespace Education.Application.Service
{
    public class UsersService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleUserRepository _roleUserRepository;
        private readonly IExamTestRepository _examTestRepository;
        private readonly IJwtUtils _jwtUtils;
        public UsersService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _roleUserRepository = serviceProvider.GetRequiredService<IRoleUserRepository>();
            _examTestRepository = serviceProvider.GetRequiredService<IExamTestRepository>();
            _jwtUtils = serviceProvider.GetRequiredService<IJwtUtils>();
        }

        // SELECT * from system_permission sp JOIN permission p ON p.PermissionID = sp.PermissionID 
        //JOIN user_permission up ON up.SystemPermissionName = sp.SystemPermissionName WHERE up.TotalBit & p.Bit > 0 ;
        public async override Task<ServiceResponse> Add(User entity)
        {
            if (entity == null)
            {
                return new ServiceResponse(false, "Dữ liệu trống");
            }
            entity.UserID = Guid.NewGuid();
            // var validateUser = ValidateUser(entity);
            var checkExitsUser = await _userRepository.CheckByUserNameAndEmail(entity.UserName ?? "");
            if (checkExitsUser == null)
            {
                entity.HashPassword = AuthenHelpers.HashPassword(entity.UserName + entity.Password);
                var rsInsert = await _userRepository.Add(entity);
                if (Convert.ToInt32(rsInsert) > 0)
                {
                    var roleModel = new RoleUser()
                    {
                        RoleID = 2,
                        UserID = entity.UserID,
                    };
                    var resRole = await _roleUserRepository.Add(roleModel);
                    if (Convert.ToInt32(resRole) > 0)
                    {
                        return new ServiceResponse(true, "Tạo thành công");
                    }
                    return new ServiceResponse(false, "Có lỗi xảy ra");
                }
            }
            return new ServiceResponse(false, "Tài khoản đã tồn tại");
        }
    
        public async override Task<ServiceResponse> GetById(object ID)
        {
            try
            {
                var data = await base.GetById(_UserID);
                return new ServiceResponse(true, "lay du lieu thanh cong", data);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
        public async Task<ServiceResponse> InsertUserExam(UserBlockRequestModel data)
        {
            var lstExamBlock = await _examTestRepository.GetExamTestsByBlockID(data.BlockID);
            var dicData = lstExamBlock.GroupBy(x => x.ExamTestID).ToDictionary(k => k.Key, g => g.ToList());
            var lstUserExam = new List<UserExam>();
            var lstUserBlock = new List<UserBlock>();
            foreach (var item in data.LstUser)
            {
                lstUserBlock.Add(new UserBlock() { UserID = item, BlockID = data.BlockID });
            }
            var dataInsertUserBlock = lstUserBlock.Cast<BaseModel>().ToList();
            _ = await _userRepository.MultiInsert(dataInsertUserBlock, false);
            Random rnd = new Random();
            foreach (var item in dicData)
            {
                var lengh = item.Value.Count();
                var index = rnd.Next(1,lengh) - 1;
                foreach (var userID in data.LstUser)
                {
                    lstUserExam.Add(new UserExam() { UserID = userID, ExamCode = item.Value[index].ExamTestCode });
                }
            }
            var lstUserExamInsert = lstUserExam.Cast<BaseModel>().ToList();
            _ = await _userRepository.MultiInsert(lstUserExamInsert, false);
            return new ServiceResponse()
            {
                Success = true,
            };
        }
        //public async Task<ServiceResponse> ResetPassword(string newPassword)
        //{
        //    var hashPassWord = AuthenHelpers.HashPassword(_UserName + newPassword);
        //    var userEntity = new User()
        //    {
        //        HashPassword = hashPassWord,
        //    };
        //    var condition = $"UserID = '{_UserID}'";
        //    var resUpdate = await _userRepository.UpdateCustomColumn(userEntity, new List<string>() { "HashPassword" }, condition);
        //    if (resUpdate)
        //    {
        //        return new ServiceResponse(true, "Mật khẩu đã thay đổi");
        //    }
        //    return new ServiceResponse(false, "Có lỗi xảy ra");
        //}
    }
}
