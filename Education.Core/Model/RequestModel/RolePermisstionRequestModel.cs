using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.RequestModel
{
    public class RolePermisstionRequestModel
    {
        public string RoleName { get; set; }
        public List<int> PermissionIDs { get; set; }
    }
}
