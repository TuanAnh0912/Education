using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class ExamResultDto
    {
        public int QuestionOrder { get; set; }
        public int OrderAnswer { get; set; }
        public bool IsTrue { get; set; }
    }
}
