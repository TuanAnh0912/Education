using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                {"v_HashPassword",entity.HashPassword ?? ""},
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

        public async Task<User?> CheckLogin(string username, string password)
        {
            var param = new Dictionary<string, object>
                {
                    {"v_UserName",username },
                    {"v_Password", password }
                };
            var rsCheckLogin = (await _dbContext.QueryUsingStore(param, "Proc_CheckLogin")).FirstOrDefault();
            return rsCheckLogin;
        }
    }
}
