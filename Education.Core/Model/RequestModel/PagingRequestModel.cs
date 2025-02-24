using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.RequestModel
{
    public class PagingRequestModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ValueWhere { get; set; }

        public Dictionary<string, object> CustomParam { get; set; }
    }
}
