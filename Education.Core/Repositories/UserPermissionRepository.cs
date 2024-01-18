using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Education.Core.Repositories
{
    public class UserPermissionRepository : GenericRepositories<UserPermission>, IUserPermissionRepository
    {
        public UserPermissionRepository(IDbContext<UserPermission> dbContext) : base(dbContext)
        {
        }
        public async Task<List<UserRolesDto>> GetRolePermisstionsByUserID(Guid userID)
        {
            var sql = "SELECT sp.SystemPermissionName,GROUP_CONCAT(p.PermisstionCode) as Roles from system_permission sp JOIN permission p ON p.PermissionID = sp.PermissionID JOIN user_permission up ON up.SystemPermissionName = sp.SystemPermissionName WHERE (up.TotalBit & p.Bit > 0) AND up.UserID = @UserID GROUP by sp.SystemPermissionName";
            var param = new Dictionary<string, object>()
            {
                {"@UserID",userID}
            };
            var res = await _dbContext.QueryUsingStore<UserRolesDto>(param, sql, commandType: System.Data.CommandType.Text);
            return res.ToList();
        }
    }
}
