using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("analysis_content", "AnalyticalData")]
    public class AnalysisContent:BaseModel
    {
        public int AnalysisContentID { get; set; }
        public string SubjectCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Point { get; set; }
        public bool? IsMain { get; set; }
        public bool? IsSystem { get; set; }
    }
}
