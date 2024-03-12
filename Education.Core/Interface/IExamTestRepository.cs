using Education.Core.Model;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IExamTestRepository:IGenericRepository<ExamTest>
    {
        Task<List<ExamsDetailDto>> GetExamsdetailByExamCodes(List<string> examCodes);
        Task<List<CorrectQuestionDto>> GetDetailCorrectQuestion(string examCode, string lstCorrectQuestion);
        Task<bool> InsertExamDetail(ExamRequestModel data);
    }
}
