using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("Block", "Exam")]
    public class Block:BaseModel
    {
        [Key]
        public int BlockID { get; set; }
        public string BlockName { get; set; }
    }
}
