using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("role_user")]
    public class RoleUser:BaseModel
    {
        [Key]
        public int RoleUserID { get; set; }
        public Guid UserID { get; set; }
        public int RoleID { get; set; }
    }
}
