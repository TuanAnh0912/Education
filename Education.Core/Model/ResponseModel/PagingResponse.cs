using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.ResponseModel
{
    public class PagingResponse
    {
        public object PageData { get; set; }
        public int PageSize { get; set; }
    }
}
