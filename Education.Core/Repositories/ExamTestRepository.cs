using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Education.Core.Repositories
{
    public class ExamTestRepository : GenericRepositories<ExamTest>, IExamTestRepository
    {
        public ExamTestRepository(IDbContext<ExamTest> dbContext,IServiceProvider serviceProvider) : base(dbContext)
        {
        }
        public async Task<List<ExamsDetailDto>> GetExamsdetailByExamCodes(List<string> examCodes)
        {
            var param = new Dictionary<string, object>()
            {
                {"v_lstExamCodes",string.Join(',',examCodes) },
            };
            var res = await _dbContext.QueryUsingStore<ExamsDetailDto>(param, "Proc_GetExamDetails");
            return res.ToList();
        }
        public override string GetTableName()
        {
            return "Exam_test";
        }
        public async Task<List<CorrectQuestionDto>> GetDetailCorrectQuestion(string examCode,string lstCorrectQuestion)
        {
            var param = new Dictionary<string, object>()
            {
                {"v_ExamCode",examCode },
                {"v_LstCorrectQuestion", lstCorrectQuestion}
            };
            var res = await _dbContext.QueryUsingStore<CorrectQuestionDto>(param, "Proc_CountPoint");
            return res.ToList();
        }
        public async Task<bool> InsertExamDetail(ExamRequestModel data)
        {
            var trans = _dbContext.GetDbTransaction();
            var paramExam = new Dictionary<string, object>()
            {
                {"v_ExamTestCode",data.Exam.ExamTestCode},
                {"v_IsOrigin",data.Exam.IsOrigin}
            };
            var resInsertExam = await _dbContext.ExcuseUsingStore(paramExam, "Proc_Insert_exam_test",trans);
            if (Convert.ToInt32(resInsertExam) <= 0)
            {
                trans.Rollback();
                return false;
            }
            var lstDataInsert = data.Questions.Cast<BaseModel>();
            var resInsertQuestion = await MultiInsert(lstDataInsert.ToList(),false,trans);
            if (!Convert.ToBoolean(resInsertQuestion))
            {
                trans.Rollback();
                return false;
            }
            trans.Commit();
            return true;
        }
    }
}
