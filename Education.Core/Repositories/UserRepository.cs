using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Repositories
{
    public class UserRepository : GenericRepositories<User>, IUserRepository
    {
        public UserRepository(IDbContext<User> dbContext) : base(dbContext)
        {
        }
        public async override Task<object> Add(User entity)
        {
            var param2 = new Dictionary<string, object>
            {
                {"v_UserID", entity.UserID},
                {"v_UserName", entity.UserName ?? ""},
                {"v_FullName", entity.FullName ?? ""},
                {"v_HashPassword", entity.HashPassword ?? ""},
            };
            using (var db = _dbContext.GetDbTransaction())
            {
                var transaction = _dbContext.GetDbTransaction();
                var rsInsert = await _dbContext.ExcuseUsingStore(param2, "Proc_InsertUser", transaction);
                if (rsInsert > 0)
                {
                    transaction.Commit();
                }
                return rsInsert;
            }
        }
        public async Task<User> CheckByUserNameAndEmail(string userName)
        {
            var param1 = new Dictionary<string, object>
            {
                {"v_UserName", userName }

            };
            return (await _dbContext.QueryUsingStore(param1, "Proc_CheckUserNameAndEmail")).FirstOrDefault();
        }

        public async Task<UserDto?> CheckLogin(string username, string password)
        {
            var param = new Dictionary<string, object>
                {
                    {"v_UserName",username },
                    {"v_Password", password }
                };
            var rsCheckLogin = (await _dbContext.QueryUsingStore<UserDto>(param, "Proc_CheckLogin")).FirstOrDefault();
            return rsCheckLogin;
        }
        public async Task<Role> GetRoleUserByID(Guid userID)
        {
            var param = new Dictionary<string, object>
            {
                {"@UserId",userID }
            };
            var sql = "SELECT r.* from role_user as ru JOIN role r ON r.RoleID = ru.RoleID WHERE ru.UserID = @UserId;";
            var res = await _dbContext.QueryUsingStore<Role>(param, sql, commandType: CommandType.Text);
            return res.FirstOrDefault();
        }
        public async Task<PagingResponse> GetPagingUserExamByID(Guid userID,bool isTearcher, int pageSize, int pageIndex)
        {
            var dataPaging = new PagingResponse();
            var offSet = (pageIndex - 1) * pageSize;
            var sql = new StringBuilder ("SELECT u.FullName,ue.Point,ue.ExamCode,ue.ResultJson from user_exam as ue JOIN user as u on u.UserID = ue.UserID WHERE ");
            var param = new Dictionary<string, object>();
            param.Add("@pageSize", pageSize);
            param.Add("@offSet", offSet);
            if (!isTearcher) 
            {
                sql.Append(" u.UserID = @userID and ue.IsTest = TRUE limit @pageSize offset @offSet;");
                param.Add("@userID", userID);
            }
            else
            {
                sql.Append(" ue.IsTest = TRUE limit @pageSize offset @offSet;");
            }
            var res = await _dbContext.QueryUsingStore<UserExamDto>(param,sql.ToString(),commandType:CommandType.Text);
             dataPaging.PageData = res.ToList();
            return dataPaging;
        }
        public async Task<List<UserDto>> GetUserWithoutBlock(int blockID)
        {
            var sql = "SELECT * FROM user u LEFT JOIN user_block ub ON u.UserID = ub.UserID WHERE ub.UserID IS NULL AND ub.UserBlockID = @blockID;";
            var param = new Dictionary<string, object>()
            {
                {"@blockID",blockID}
            };
            var res = await _dbContext.QueryUsingStore<UserDto>(param, sql, commandType: CommandType.Text);
            return res.ToList();
        }
        /// <summary>
        /// Hàm lấy dữ liệu cần thiết sau login
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<object> InitLogin(string userID)
        {
            var sqlGetRole = "SELECT rp.* FROM role_permission rp JOIN role_user ru USING (RoleID) WHERE ru.UserID = @UserID;";
            var rsRole = await _dbContext.QueryUsingStore<RolePermission>(new Dictionary<string, object> { { "@UserID", userID } }, sqlGetRole, commandType: CommandType.Text);
            return new
            {
                Roles = rsRole
            };
        }
    }
}
