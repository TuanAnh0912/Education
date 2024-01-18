using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class ExamsDetailDto:ExamTest
    {
        public int QuestionID { get; set; }
        public int ExamTestID { get; set; }
        public int SortOrder { get; set; }
        public string Result { get; set; }
        public int SubAnalysysID { get; set; }
        public int MainAnalysysID { get; set; }
    }
}
