using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("rulesort_exam_question")]
    public class RuleSortExamQuestion: BaseModel
    {
        [Key]
        public int ID { get; set; }
        public int ExamTestID { get; set; }
        public int OriginSortOrder { get; set; }
        public int ShuffleOrder { get; set; }
        public Guid OriginQuestionID { get; set; }
        public string ExamCode { get; set; }
    }
}
