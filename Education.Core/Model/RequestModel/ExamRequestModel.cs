using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.RequestModel
{
    public class ExamRequestModel
    {
        public ExamTest Exam { get; set; }
        public List<Question> Questions { get; set; }
    }
}
