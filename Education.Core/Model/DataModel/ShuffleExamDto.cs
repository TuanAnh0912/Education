using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class ShuffleExamDto
    {
        public int ID { get; set; }
        public int ExamTestID { get; set; }
        public int ShuffleOrder { get; set; }
        public string ExamCode { get; set; }
        public int SortOrder { get; set; }
        public bool IsTrue  { get; set; }
        public string Image { get; set; }
        public string QuestionContent { get; set; }
        public string AnswerContent { get; set; }
        public int ShuffleOrderAnswer { get; set; }
        public Guid OriginQuestionID { get; set; }
    }
}
