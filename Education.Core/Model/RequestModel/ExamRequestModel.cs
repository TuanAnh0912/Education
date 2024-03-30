using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.RequestModel
{
    public class ExamRequestModel
    {
        public ExamTest Exam { get; set; } = new ExamTest();
        public List<QuestionAnswers> QuestionAnswers { get; set; } = new List<QuestionAnswers>();
       
    }
    public class QuestionAnswers:Question
    {
        public List<Answer> Answers { get; set; } = new List<Answer>();

    }
}
