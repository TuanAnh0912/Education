using Education.Core.Model;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IAnalysisContentRepository:IGenericRepository<AnalysisContent>
    {
        Task<PagingResponse> GetPagingAnalysisContent(PagingRequestModel pagingRequest);
    }
}
