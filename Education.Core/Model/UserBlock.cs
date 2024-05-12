using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("user_block","Exam")]
    public class UserBlock:BaseModel
    {
        public int UserBlockID { get; set; }
        public int BlockID { get; set; }
        public Guid UserID { get; set; }
    }
}
