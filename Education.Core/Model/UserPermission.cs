using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("user_permission")]
    public class UserPermission
    {
        [Key]
        public int UserPermissionID { get; set; }
        public Guid UserID { get; set; }
        public int TotalBit { get; set; }
    }
}
