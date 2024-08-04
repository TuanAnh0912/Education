using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IExamGeneralRepository: IGenericRepository<ExamGeneral>
    {
        Task<PagingResponse> Paging(PagingRequestModel pagingRequest);
        Task<PagingResponse> PagingByUser(PagingRequestModel pagingRequest, string userID);
        Task<List<Guid>> GetLstUserIDByBlockID(int blockID);
    }
}
