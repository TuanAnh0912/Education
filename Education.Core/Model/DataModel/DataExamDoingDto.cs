using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class DataExamDoingDto
    {
        public string SubAnalysName { get; set; }
        public string SubAnalysCode { get; set; }
        public string MainAnalysName { get; set; }
        public string MainAnalysCode { get; set; }
        public double SubAnalysPoint { get; set; }
        public Guid OriginQuestionID { get; set; }
        public int QuestionSortOrder { get; set; }
        public string QuestionContent { get; set; }
        public string AnswerContent { get; set; }
        public int AnswerSortOrder { get; set; }
        public bool IsMultiResult { get; set; }
    }
}
