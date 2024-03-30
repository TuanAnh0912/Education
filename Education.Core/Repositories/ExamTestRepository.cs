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
        public async Task<bool> InsertShuffleExam(List<RuleSortExamQuestion> ruleSortExamQuestions, List<RuleSortQuestionAnswer> ruleSortQuestionAnswers)
        {
            var tran = _dbContext.GetDbTransaction();
            var lstDataQuestions = ruleSortExamQuestions.Cast<BaseModel>().ToList();
            var resInsertsQuestionShuffle =await MultiInsert(lstDataQuestions, false, tran);
            if (!Convert.ToBoolean(resInsertsQuestionShuffle))
            {
                return false;
            }
            var lstDataAnswer = ruleSortQuestionAnswers.Cast<BaseModel>().ToList();
            var resInsertsAnswerShuffle = await MultiInsert(lstDataAnswer, false, tran);
            if (!Convert.ToBoolean(resInsertsAnswerShuffle))
            {
                tran.Rollback();
                return false;
            }
            tran.Commit();
            return true;
        }
        public async Task<bool> InsertExamDetail(ExamRequestModel data)
        {
            var trans = _dbContext.GetDbTransaction();
            var paramExam = new Dictionary<string, object>()
            {
                {"v_ExamTestCode",data.Exam.ExamTestCode},
                {"v_IsOrigin",data.Exam.IsOrigin}
            };
            var resInsertExam = await _dbContext.ExecuteScalarUsingStore(paramExam, "Proc_Insert_exam_test",trans);
            if (Convert.ToInt32(resInsertExam) <= 0)
            {
                trans.Rollback();
                return false;
            }
            var answerInsert = new List<Answer>();
            var answerQuestion = new List<Question>();
            foreach (var item in data.QuestionAnswers)
            {
                var questionModel = new Question();
                questionModel.QuestionID = Guid.NewGuid();
                questionModel.IsTrue = item.IsTrue;
                questionModel.QuestionSortOrder = item.QuestionSortOrder;
                questionModel.QuestionContent = item.QuestionContent;
                questionModel.MainAnalysysID = item.MainAnalysysID;
                questionModel.SubAnalysysID = item.SubAnalysysID;
                questionModel.Image = item.Image;
                questionModel.ExamTestID = Convert.ToInt32(resInsertExam);
                answerQuestion.Add(questionModel);
                foreach (var answer in item.Answers)
                {
                    answer.QuestionID = questionModel.QuestionID;
                    answerInsert.Add(answer);
                }
            }
            var lstDataInsert = answerQuestion.Cast<BaseModel>();
            var resInsertQuestion = await MultiInsert(lstDataInsert.ToList(),false,trans);
            if (!Convert.ToBoolean(resInsertQuestion))
            {
                trans.Rollback();
                return false;
            }
            var lstDataAnswerInsert = answerInsert.Cast<BaseModel>();
            var resInsertAnswer = await MultiInsert(lstDataAnswerInsert.ToList(), false, trans);
            if (!Convert.ToBoolean(resInsertQuestion))
            {
                trans.Rollback();
                return false;
            }
            trans.Commit();
            return true;
        }
        public async Task<List<ShuffleExamDto>> GetShuffleExam(int ExamTestID)
        {
            var sql = "select req.ExamCode,req.ShuffleOrder,req.OriginQuestionID,q.QuestionContent,rqa.ShuffleOrderAnswer,a.AnswerContent,a.IsTrue  from " +
                "rulesort_exam_question req JOIN question q on req.OriginQuestionID = q.QuestionID " +
                "JOIN rulesort_question_answer rqa ON req.OriginQuestionID = rqa.QuestionID AND req.ExamCode = rqa.ExamCode " +
                "JOIN answer a on a.AnswerID = rqa.OriginAnswerID where req.ExamTestID = @ExamTestID ;";
            var param = new Dictionary<string, object>()
            {
                {"@ExamTestID",ExamTestID }
            };
            var res = await _dbContext.QueryUsingStore<ShuffleExamDto>(param, sql, commandType: System.Data.CommandType.Text);
            return res.ToList();
        }
    }
}
