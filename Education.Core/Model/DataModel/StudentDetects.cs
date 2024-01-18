using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class StudentDetects
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string ExamsCode { get; set; }
        public List<string> LstResult { get; set; }
    }
}
