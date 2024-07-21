using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.DataModel
{
    public class UserExamDto
    {
        public string FullName { get; set; }
        public double Point { get; set; }
        public string ExamCode { get; set; }
        public string ResultJson { get; set; }
    }
}
