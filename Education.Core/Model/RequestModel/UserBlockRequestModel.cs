using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.RequestModel
{
    public class UserBlockRequestModel
    {
        public List<Guid> LstUser { get; set; }
        public int BlockID { get; set; }
    }
}
