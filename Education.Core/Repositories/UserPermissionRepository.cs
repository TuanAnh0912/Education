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
    public class UserPermissionRepository : GenericRepositories<RolePermission>, IUserPermissionRepository
    {
        public UserPermissionRepository(IDbContext<RolePermission> dbContext) : base(dbContext)
        {
        }
        public async Task<List<UserRolesDto>> GetRolePermisstionsByUserID(Guid userID)
        {
            var sql = "SELECT sp.SystemPermissionName,GROUP_CONCAT(DISTINCT p.PermisstionCode) as Roles FROM role_user ru JOIN role_permission rp ON ru.RoleID = rp.RoleID JOIN system_permission sp ON rp.SystemPermissionName = sp.SystemPermissionName JOIN permission p ON p.PermissionID = sp.PermissionID WHERE (rp.TotalBit & p.Bit > 0) AND ru.UserID = @UserID GROUP BY rp.SystemPermissionName;";
            var param = new Dictionary<string, object>()
            {
                {"@UserID",userID}
            };
            var res = await _dbContext.QueryUsingStore<UserRolesDto>(param, sql, commandType: System.Data.CommandType.Text);
            return res.ToList();
        }
    }
}
