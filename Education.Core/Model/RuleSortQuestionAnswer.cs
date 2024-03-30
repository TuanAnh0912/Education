using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("rulesort_question_answer")]
    public class RuleSortQuestionAnswer:BaseModel
    {
        [Key]
        public int ID { get; set; }
        public Guid QuestionID { get; set; }
        public int OriginSortOrder { get; set; }
        public int ShuffleOrderAnswer { get; set; }
        public int OriginAnswerID { get; set; }
        public string ExamCode { get; set; }
    }
}
