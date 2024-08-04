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
        public List<ExamInfor> LstTestID { get; set; }
    }
    public class ExamInfor
    {
        public int ID { get; set; }
        public string Code { get; set; }
    }
}
