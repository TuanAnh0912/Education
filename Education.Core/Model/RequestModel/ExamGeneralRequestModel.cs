using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.RequestModel
{
    public class ExamGeneralRequestModel
    {
        public string Name { get; set; }
        public int BlockID { get; set; }
        public List<int> LstTestID { get; set; }
    }
}
