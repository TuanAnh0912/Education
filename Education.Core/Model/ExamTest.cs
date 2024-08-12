using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("exam_test","Exam")]
    public class ExamTest:BaseModel
    {
        [Key]
        public int ExamTestID { get; set; }
        public string ExamTestCode { get; set; }
        public string ExamTestName { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string EducationTrainName { get; set; }
        public string SchoolName { get; set; }
        public int Time { get; set; }
        public bool IsOrigin { get; set; }
    }
}
