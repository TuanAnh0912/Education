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
        public string Name { get; set; }
        public int AnalysisHeaderID { get; set; }
        public string Code { get; set; }
        public double Point { get; set; }
    }
}
