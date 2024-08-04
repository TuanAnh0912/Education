﻿using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Education.Application.Service
{
    public class ExamGeneralService : BaseService<ExamGeneral>, IExamGeneralService
    {
        private IExamGeneralRepository _examGeneralRepository;
        public ExamGeneralService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _examGeneralRepository = serviceProvider.GetRequiredService<IExamGeneralRepository>();
        }
        public async Task<ServiceResponse> InsertsExamTestGeneral(ExamGeneralRequestModel data)
        {
            var res = new ServiceResponse();
            var examGenerals = new List<ExamGeneral>();
            examGenerals.Add(new ExamGeneral() { BlockID = data.BlockID, Name = data.Name });
            var lstDataInsert = examGenerals.Cast<BaseModel>().ToList();
            var resInsertExamGeneral = await _examGeneralRepository.MultiInsert(lstDataInsert, true);
            if (Convert.ToInt32(resInsertExamGeneral) > 0)
            {
                var id = Convert.ToInt32(resInsertExamGeneral);
                var examTestGenerals = new List<ExamTestGeneral>();
                data.LstTestID.ForEach(x => examTestGenerals.Add(
                    new ExamTestGeneral() { ExamGeneralID = id, ExamTestID = x.ID }));
                var lstUserIdByBlockID = await _examGeneralRepository.GetLstUserIDByBlockID(data.BlockID);

                var listDataUserExam = new List<UserExam>();
                foreach (var userID in lstUserIdByBlockID)
                {
                    var userExam = new UserExam();
                    userExam.UserID = userID;
                    data.LstTestID.ForEach(x => userExam.ExamCode = x.Code);
                    listDataUserExam.Add(userExam);
                }
                
                //Insert đề vào kỳ thi
                var dataInsert = examTestGenerals.Cast<BaseModel>().ToList();
                var resInsert = await _examGeneralRepository.MultiInsert(dataInsert, false);

                //Insert đề thi vào các học sinh đã phân khối trước đó
                var dataInserUserExam = listDataUserExam.Cast<BaseModel>().ToList();
                var resInsertUserExam = await _examGeneralRepository.MultiInsert(dataInserUserExam, false);

                res.Success = Convert.ToBoolean(resInsert) && Convert.ToBoolean(resInsertUserExam);
            }
            return res;
        }

        public async Task<ServiceResponse> Paging(PagingRequestModel pagingRequest)
        {
            var pagingResponse = await _examGeneralRepository.Paging(pagingRequest);
            var res = new ServiceResponse()
            {
                Data = pagingResponse,
                Success = true
            };
            return res;
        }

        public async Task<ServiceResponse> PagingByUser(PagingRequestModel pagingRequest)
        {
            var pagingResponse = await _examGeneralRepository.PagingByUser(pagingRequest, _UserID.ToString());
            var res = new ServiceResponse()
            {
                Data = pagingResponse,
                Success = true
            };
            return res;
        }
    }
}
