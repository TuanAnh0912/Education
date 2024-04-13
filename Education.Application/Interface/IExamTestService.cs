using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IExamTestService:IBaseService<ExamTest>
    {
        Task<bool> InsertExamDetail(ExamRequestModel data);
        Task<bool> ShuffleExam(string examTestCode);
        Task<List<ExamRequestModel>> GetShuffleExam(int examID);
        Task<ExamRequestModel> GetExamsByCode(string examCode);
        Task<ServiceResponse> Getpaging(PagingRequestModel data);
    }
}
