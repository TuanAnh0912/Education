using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class StudentExamsExcelDto
    {
        public string StudentCode { get; set; }
        public string ExamTestCode { get; set; }
        public string Name { get; set; }
        public List<DetailQuestion> DetailQuestions { get; set; }
    }
    public class DetailQuestion
    {
        public int QuestionOrder { get; set; }
        public string Result { get; set; }
    }
}
