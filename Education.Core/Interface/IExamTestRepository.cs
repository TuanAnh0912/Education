﻿using Education.Core.Model;
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
        Task<List<CorrectQuestionDto>> GetDetailCorrectQuestion(string examCode, string lstCorrectQuestion);
        Task<bool> InsertExamDetail(ExamRequestModel data);
        Task<bool> InsertShuffleExam(List<RuleSortExamQuestion> ruleSortExamQuestions, List<RuleSortQuestionAnswer> ruleSortQuestionAnswers);
        Task<List<ShuffleExamDto>> GetShuffleExam(int ExamTestID);
        Task<PagingResponse> GetPaging(int pageSize, int pageIndex, string stringWhere = "");
        Task<List<ExamTest>> GetExamTestsByBlockID(int BlockID);
    }
}
