using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IUserPermissionService:IBaseService<UserPermission>
    {
        Task<bool> CheckRoleAccess(string system, string role);
    }
}
