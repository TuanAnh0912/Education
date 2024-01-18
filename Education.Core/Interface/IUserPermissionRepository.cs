using Education.Core.Model;
using Education.Core.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IUserPermissionRepository:IGenericRepository<UserPermission>
    {
        Task<List<UserRolesDto>> GetRolePermisstionsByUserID(Guid userID);
    }
}
