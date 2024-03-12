using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model
{
    [TableEducation("")]
    public class AnalysisHeader
    {
        [Key]
        public int AnalysisHeaderID { get; set; }
        public string Name { get; set; }
    }
}
