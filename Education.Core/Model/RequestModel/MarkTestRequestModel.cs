using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.RequestModel
{
    public class MarkTestRequestModel
    {
        public string ExamCode { get; set; }
        public Guid UserID { get; set; }
        public List<QuestionDetail> QuestionDetails { get; set; }
    }
    public class QuestionDetail
    {
        public int Order { get; set; }
        public List<int> Results { get; set; }
        public string SubAnalysCode { get; set; }
        public string MainAnalysCode { get; set; }
        public string MainAnalysName { get; set; }
        public double SubAnalysPoint { get; set; }
    }
}
