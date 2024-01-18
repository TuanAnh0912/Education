using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("student_exams")]
    public class StudenExams:BaseModel
    {
        [Key]
        public int StudentExamsID { get; set; }
        public string ExamTestCode { get; set; }
        public string ResultJson { get; set; }
        public string StudentCode { get; set; }
    }
}
