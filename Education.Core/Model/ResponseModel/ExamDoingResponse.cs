using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.ResponseModel
{
    public class ExamDoingResponse
    {
        public Guid ID { get; set; }
        public string QuestionContent { get; set; }
        public bool IsMultiAnswer { get; set; }
        public string SubAnalysName { get; set; }
        public string SubAnalysCode { get; set; }
        public string MainAnalysName { get; set; }
        public string MainAnalysCode { get; set; }
        public double SubAnalysPoint { get; set; }
        public List<AnswerDoing> Answers { get; set; }
    }
    public class AnswerDoing
    {
        public string AnswerContent { get; set; }
        public int AnswerSortOrder { get; set; }
    }
}
