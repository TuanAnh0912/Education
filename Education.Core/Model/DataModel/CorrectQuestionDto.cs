using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class CorrectQuestionDto
    {
        public string SubContentCode { get; set; }
        public string MainContentCode { get; set; }
        public int SortOrder { get; set; }
        public decimal Point { get; set; }

    }
}
