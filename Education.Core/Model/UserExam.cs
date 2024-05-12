using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("user_exam","Exam")]
    public class UserExam:BaseModel
    {
        public int UserExamID { get; set; }
        public Guid UserID { get; set; }
        public string ExamCode { get; set; }
        public bool IsTest { get; set; } = false;
        public double Point { get; set; } = 0;
        public string ResultJson { get; set; }
        public string Evaluate { get; set; }
    }
}
