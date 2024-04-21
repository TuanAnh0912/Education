using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Interface;
using Education.Core.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class UserPermissionService : BaseService<RolePermission>, IUserPermissionService
    {
        private IUserPermissionRepository _userPermissionRepository;
        public UserPermissionService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userPermissionRepository = serviceProvider.GetRequiredService<IUserPermissionRepository>();
        }
        public async Task<bool> CheckRoleAccess(string system, string role)
        {
            var rolesByUser = await _userPermissionRepository.GetRolePermisstionsByUserID(_UserID);
            return rolesByUser.Any(x => x.SystemPermissionName == system && x.Roles.Contains(role));
        }
    }
}
