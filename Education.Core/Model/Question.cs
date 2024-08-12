using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("question")]
    public class Question:BaseModel
    {
        [Key]
        public Guid QuestionID { get; set; }
        public int ExamTestID { get; set; }
        public int QuestionSortOrder { get; set; }
        public bool IsTrue { get; set; }
        public string SubAnalysisCode { get; set; }
        public string MainAnalysisCode { get; set; }
        public string Image { get; set; }
        public string QuestionContent { get; set; }
    }
}
