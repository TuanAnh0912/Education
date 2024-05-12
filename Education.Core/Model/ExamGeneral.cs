using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("exam_general","Exam")]
    public class ExamGeneral:BaseModel
    {
        [Key]
        public int ExamGeneralID { get; set; }
        public string Name { get; set; }
        public int BlockID { get; set; }
    }
}
