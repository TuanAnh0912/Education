using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("exam_test_general")]
    public class ExamTestGeneral:BaseModel
    {
        [Key]
        public int ExamTestGeneralID { get; set; }
        public int ExamTestID { get; set; }
        public int ExamGeneralID { get; set; }
    }
}
