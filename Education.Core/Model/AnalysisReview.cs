using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("analysis_review", "AnalyticalData")]
    public class AnalysisReview: BaseModel
    {
        public int ID { get; set; }
        public string SubjectCode { get; set; }
        public string AnalysisTypeCode { get; set; }
        public string AnalysisFomula { get; set; }
        public string AnalysisReivew { get; set; }
    }
}
