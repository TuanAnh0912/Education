﻿using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
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
        public async Task<List<ExamTest>> GetExamsByExamCodes(List<string> examCodes)
        {
            var sql = string.Format("SELECT et.ExamTestID,et.ExamTestCode,et.ExamTestName,et.IsOrigin,et.FileID,et.SubjectCode,et.SubjectName,et.Time,et.EducationTrainName,et.SchoolName FROM exam_test et WHERE et.ExamTestCode IN ({0});", string.Join(",", examCodes.Select(x => $"'{x}'")));
            var res =  await _dbContext.QueryUsingStore<ExamTest>(new Dictionary<string, object>(), sql, commandType: CommandType.Text);
            return res.ToList();
        }
        public async Task<List<ExamTest>> GetExamsByExamIDs(List<int> examIDs)
        {
            var sql = string.Format("SELECT et.ExamTestID,et.ExamTestCode,et.ExamTestName,et.IsOrigin,et.FileID,et.SubjectCode,et.SubjectName,et.Time,et.EducationTrainName,et.SchoolName FROM exam_test et WHERE et.ExamTestID IN ({0});", string.Join(",", examIDs));
            var res =  await _dbContext.QueryUsingStore<ExamTest>(new Dictionary<string, object>(), sql, commandType: CommandType.Text);
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
        public async Task<PagingResponse> ExamsByUser(PagingRequestModel pagingRequest, string userID)
        {
            var res = new PagingResponse();
            var sql = new StringBuilder();
            sql.Append(" (SELECT ue.UserID, req.ExamTestID, ue.ExamCode as SubExamCode, ue.IsTest from user_exam ue JOIN rulesort_exam_question req ON req.ExamCode = ue.ExamCode GROUP BY ue.UserID, req.ExamTestID, ue.ExamCode, ue.IsTest) tmp");
            sql.Append(" JOIN exam_test ut USING(ExamTestID)");
            sql.Append(" WHERE tmp.UserID = @UserID");
            
            var pageIndex = pagingRequest.PageIndex;
            var pageSize = pagingRequest.PageSize;
            var offSet = (pageIndex - 1) * pageSize;
            var param = new Dictionary<string, object>()
            {
                {"@UserID", userID },
                {"@PageSize",pageSize },
                {"@OffSet",offSet },
                {"@StringWhere",$"%{pagingRequest.ValueWhere}%" },
            };
            var data = await _dbContext.QueryUsingStore<object>(param, "SELECT tmp.*, ut.ExamTestCode as MainExamCode, ut.SubjectName, ut.Time FROM " + sql, commandType: CommandType.Text);
            res.PageData = data;
            var total = await _dbContext.QueryUsingStore<int>(param, "SELECT COUNT(1) FROM " + sql, commandType: CommandType.Text);
            res.PageSize = total.FirstOrDefault();
            return res;
        }
        public async Task<bool> InsertExamDetail(ExamRequestModel data)
        {
            var trans = _dbContext.GetDbTransaction();
            var paramExam = new Dictionary<string, object>()
            {
                {"v_ExamTestCode",data.Exam.ExamTestCode},
                {"v_ExamTestName",data.Exam.ExamTestName},
                {"v_SubjectCode",data.Exam.SubjectCode},
                {"v_SubjectName",data.Exam.SubjectName},
                {"v_Time",data.Exam.Time},
                {"v_IsOrigin",data.Exam.IsOrigin},
                {"v_EducationTrainName",data.Exam.EducationTrainName},
                {"v_SchoolName",data.Exam.SchoolName},
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
                questionModel.MainAnalysisCode = item.MainAnalysisCode;
                questionModel.SubAnalysisCode = item.SubAnalysisCode;
                questionModel.Image = item.Image;
                questionModel.ExamTestID = Convert.ToInt32(resInsertExam);
                questionModel.IsMultiResult = item.IsMultiResult;
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
        public async Task<List<ShuffleExamDto>> GetShuffleExam(string ExamTestID)
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
        public async Task<PagingResponse> GetPaging(int pageSize,int pageIndex,string stringWhere = "")
        {
            var dataPaging = new PagingResponse();
            var offSet = (pageIndex - 1) * pageSize;
            var sqlData = " Select * from exam_test where (ExamTestCode like @stringWhere or SubjectName like @stringWhere) ORDER BY CreatedDate limit @pageSize offset @offSet;";
            var param = new Dictionary<string, object>()
            {
               {"@stringWhere",$"%{stringWhere}%" },
                {"@pageSize",pageSize },
                {"@offSet",offSet },
            };
            var res = await _dbContext.QueryUsingStore<ExamTest>(param, sqlData, commandType: CommandType.Text);
            dataPaging.PageData = res.ToList();
            var sqlTotal = " Select count(1)  as Total from exam_test where (ExamTestCode like @stringWhere or SubjectName like @stringWhere);";
            var param2 = new Dictionary<string, object>()
            {
                {"@stringWhere",$"%{stringWhere}%" },
            };
            var resTotal = await _dbContext.QueryUsingStore<int>(param2, sqlTotal, commandType: CommandType.Text);
            dataPaging.PageSize = resTotal.FirstOrDefault();
            return dataPaging;
        }
        public async Task<List<ExamTest>> GetExamTestsByBlockID(int BlockID)
        {
            var sql = $"select etg.ExamTestID,req.ExamCode as ExamTestCode from exam_general eg JOIN exam_test_general etg on eg.ExamGeneralID = etg.ExamGeneralID JOIN rulesort_exam_question req on req.ExamTestID = etg.ExamTestID WHERE eg.BlockID = @blockID GROUP by  etg.ExamTestID,req.ExamCode ;";
            var param = new Dictionary<string, object>()
            {
                {"@blockID",BlockID }
            };
            var res = await _dbContext.QueryUsingStore(param,sql, commandType: CommandType.Text);
            return res.ToList();
        }
        public async Task<List<ExamResultDto>> GetResultByExamCode(string examCode)
        {
            var sql = "Proc_GetResultByExamCode";
            var param = new Dictionary<string, object>()
            {
                {"v_ExamCode",examCode }
            };
            var res = await _dbContext.QueryUsingStore<ExamResultDto>(param, sql);
            return res.ToList();
        }
        public async Task<bool> UpdateUserExam(UserExam dataUpdate)
        {
            var sql = "UPDATE user_exam SET IsTest = @isTest,Point = @point, ResultJson = @resultjson WHERE UserID = @userID AND ExamCode = @examCode";
            var param = new Dictionary<string, object>()
            {
                {"@isTest",true },
                {"@resultjson",dataUpdate.ResultJson },
                {"@userID",dataUpdate.UserID },
                {"@examCode",dataUpdate.ExamCode },
                {"@point",dataUpdate.Point },
            };
            var res = await _dbContext.ExcuseUsingStore(param, sql, commandType: CommandType.Text);
            return res > 0;

        }
        public async Task<List<DataExamDoingDto>> GetDataExamDoing(string examCode,Guid userID)
        {
            var param = new Dictionary<string, object>()
            {
                {"v_ExamCode",examCode },
                {"v_UserID",userID },
            };
            var res = await _dbContext.QueryUsingStore<DataExamDoingDto>(param, "Proc_GetExamDoingByCode");
            return res.ToList();
        }
        public async Task<UserExamDto> GetDetailAnalys(string examCode, Guid userID)
        {
            var param = new Dictionary<string, object>()
            {
                {"@userID",userID },
                {"@examCode",examCode },
            };
            var sql = "select * from user_exam ue JOIN user u on ue.UserID = u.UserID WHERE u.UserID = @userID AND ue.ExamCode = @examCode;";
            var res = await _dbContext.QueryUsingStore<UserExamDto>(param, sql,commandType:CommandType.Text);
            return res.FirstOrDefault();
        }
        public async  Task<List<RuleSortExamQuestion>> GetShuffleExaByIDs(List<int> Ids)
        {
            var sql = "select ExamTestID,ExamCode from rulesort_exam_question WHERE ExamTestID in @IDs GROUP by ExamTestID,ExamCode;";
            var param = new Dictionary<string, object>()
            {
                {"@IDs", Ids}
            };
            var res = await _dbContext.QueryUsingStore<RuleSortExamQuestion>(param, sql, commandType: CommandType.Text);
            return res.ToList();
        }
    }
}
