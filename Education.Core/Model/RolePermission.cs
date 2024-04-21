using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("role_permission")]
    public class RolePermission
    {
        [Key]
        public int UserPermissionID { get; set; }
        public int RoleID { get; set; }
        public int TotalBit { get; set; }
        public string SystemPermissionName { get; set; }
    }
}
