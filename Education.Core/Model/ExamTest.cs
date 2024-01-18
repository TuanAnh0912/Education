using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("exam_test")]
    public class ExamTest
    {
        [Key]
        public int ExamTestID { get; set; }
        public string ExamTestCode { get; set; }
        public bool IsOrigin { get; set; }
    }
}
