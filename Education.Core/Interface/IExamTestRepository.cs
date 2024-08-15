using Education.Core.Model;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
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
        Task<List<ExamTest>> GetExamsByExamCodes(List<string> examCodes);
        Task<List<ExamTest>> GetExamsByExamIDs(List<int> ids);
        Task<List<CorrectQuestionDto>> GetDetailCorrectQuestion(string examCode, string lstCorrectQuestion);
        Task<bool> InsertExamDetail(ExamRequestModel data);
        Task<bool> InsertShuffleExam(List<RuleSortExamQuestion> ruleSortExamQuestions, List<RuleSortQuestionAnswer> ruleSortQuestionAnswers);
        Task<List<ShuffleExamDto>> GetShuffleExam(string ExamTestID);
        Task<PagingResponse> GetPaging(int pageSize, int pageIndex, string stringWhere = "");
        Task<List<ExamTest>> GetExamTestsByBlockID(int BlockID);
        Task<List<ExamResultDto>> GetResultByExamCode(string examCode);
        Task<bool> UpdateUserExam(UserExam dataUpdate);
        Task<PagingResponse> ExamsByUser(PagingRequestModel pagingRequest, string userID);
        Task<UserExamDto> GetDetailAnalys(string examCode, Guid userID);
        Task<List<RuleSortExamQuestion>> GetShuffleExaByIDs(List<int> Ids);
        Task<List<DataExamDoingDto>> GetDataExamDoing(string examCode, Guid userID);
    }
}
