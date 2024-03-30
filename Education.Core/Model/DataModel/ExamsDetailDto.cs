using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class ExamsDetailDto:ExamTest
    {
        public Guid QuestionID { get; set; }
        public int QuestionSortOrder { get; set; }
        public bool IsTrue { get; set; }
        public int SubAnalysysID { get; set; }
        public int MainAnalysysID { get; set; }
        public string Image { get; set; }
        public int AnswerID { get; set; }
        public int AnswerSortOrder { get; set; }
        public string AnswerContent { get; set; }
    }
}
