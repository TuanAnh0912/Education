using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("answer")]
    public class Answer:BaseModel
    {
        public int AnswerID { get; set; }
        public string AnswerContent { get; set; }
        public bool IsTrue { get; set; }
        public Guid QuestionID { get; set; }
        public int AnswerSortOrder { get; set; }
    }
}
