﻿using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
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
        /// <summary>
        /// Trộn đề
        /// </summary>
        /// <param name="examTestCode"></param>
        /// <returns></returns>
        Task<bool> ShuffleExam(string examTestCode);
        Task<ServiceResponse> GetShuffleExam(string examID);
        Task<ExamRequestModel> GetExamsByCode(string examCode);
        Task<ServiceResponse> Getpaging(PagingRequestModel data);
        Task<ServiceResponse> InsertUserExam(List<UserExam> data);
        Task<ServiceResponse> GetResultExam(MarkTestRequestModel data);
        Task<ServiceResponse> GetDataExamDoingDetail(string examCode);
        /// <summary>
        /// Lấy danh sách bài thi của học sinh
        /// </summary>
        Task<ServiceResponse> ExamsByUser(PagingRequestModel pagingRequest);
        //Task<ServiceResponse> ExamsByUser();
        /// <summary>
        /// Lấy Thông tin đánh giá sau khi chấm điểm
        /// </summary>
        /// <param name="examCode"></param>
        /// <returns></returns>
        Task<ServiceResponse> GetDetailAnalys(string examCode);
    }
}
