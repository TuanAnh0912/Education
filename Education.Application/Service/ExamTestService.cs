﻿using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
using Education.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class ExamTestService : BaseService<ExamTest>, IExamTestService
    {
        private IExamTestRepository _examTestRepository;
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ExamTestService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _examTestRepository = serviceProvider.GetRequiredService<IExamTestRepository>();
        }
        public async Task<bool> InsertExamDetail(ExamRequestModel data)
        {
            data.Exam.ExamTestCode = $"{data.Exam.ExamTestCode}_{RandomString(5)}";
            return await _examTestRepository.InsertExamDetail(data);
        }
        public async Task<ServiceResponse> ExamsByUser(PagingRequestModel pagingRequest)
        {
            var pagingResponse = await _examTestRepository.ExamsByUser(pagingRequest, _UserID.ToString());

            return new ServiceResponse(true, "", data: pagingResponse);
        }
        /// <summary>
        /// Trộn đề
        /// </summary>
        /// <param name="examTestCode"></param>
        /// <returns></returns>
        public async Task<bool> ShuffleExam(string examTestCode)
        {
            var examCodes = new List<string>() { examTestCode };
            var examsDetail = await _examTestRepository.GetExamsdetailByExamCodes(examCodes);
            var dicData = examsDetail.GroupBy(x => x.QuestionID).ToDictionary(k => k.Key, g => g.ToList());
            var originExam = new ExamRequestModel();
            foreach (var item in dicData)
            {
                originExam.Exam = new ExamTest()
                {
                    ExamTestCode = item.Value.FirstOrDefault()?.ExamTestCode ?? "",
                    ExamTestID = item.Value.FirstOrDefault()?.ExamTestID ?? 0
                };
                var lstQuestion = new QuestionAnswers()
                {
                    Image = item.Value.FirstOrDefault()?.Image ?? "",
                    QuestionID = item.Value.FirstOrDefault()?.QuestionID ?? Guid.Empty,
                    MainAnalysisCode = item.Value.FirstOrDefault()?.MainAnalysisCode ?? "",
                    SubAnalysisCode = item.Value.FirstOrDefault()?.SubAnalysisCode ?? "",
                    QuestionSortOrder = item.Value.FirstOrDefault()?.QuestionSortOrder ?? 0,
                    IsTrue = item.Value.FirstOrDefault()?.IsTrue ?? false,
                };
                foreach (var childItem in item.Value)
                {
                    lstQuestion.Answers.Add(new Answer()
                    {
                        AnswerID = childItem.AnswerID,
                        AnswerContent = childItem.AnswerContent,
                        IsTrue = childItem.IsTrue,
                        QuestionID = childItem.QuestionID,
                        AnswerSortOrder = childItem.AnswerSortOrder,
                    });
                }
                originExam.QuestionAnswers.Add(lstQuestion);
            }
            var resInsert = await InsertShuffleRule(originExam);
            return resInsert;
        }
        /// <summary>
        /// Lấy chi tiết đề theo mã
        /// </summary>
        /// <param name="examCode"></param>
        /// <returns></returns>
        public async Task<ExamRequestModel> GetExamsByCode(string examCode)
        {
            var examCodes = new List<string>() { examCode };
            var exam = (await _examTestRepository.GetExamsByExamCodes(examCodes)).First();
            var examsDetail = await _examTestRepository.GetExamsdetailByExamCodes(examCodes);
            var dicData = examsDetail.GroupBy(x => x.QuestionID).ToDictionary(k => k.Key, g => g.ToList());
            var originExam = new ExamRequestModel();
            foreach (var item in dicData)
            {
                originExam.Exam = exam;
                var lstQuestion = new QuestionAnswers()
                {
                    Image = item.Value.FirstOrDefault()?.Image ?? "",
                    QuestionID = item.Value.FirstOrDefault()?.QuestionID ?? Guid.Empty,
                    MainAnalysisCode = item.Value.FirstOrDefault()?.MainAnalysisCode ?? "",
                    SubAnalysisCode = item.Value.FirstOrDefault()?.SubAnalysisCode ?? "",
                    QuestionSortOrder = item.Value.FirstOrDefault()?.QuestionSortOrder ?? 0,
                    IsTrue = item.Value.FirstOrDefault()?.IsTrue ?? false,
                    QuestionContent = item.Value.FirstOrDefault()?.QuestionContent ?? ""
                };
                foreach (var childItem in item.Value)
                {
                    lstQuestion.Answers.Add(new Answer()
                    {
                        AnswerID = childItem.AnswerID,
                        AnswerContent = childItem.AnswerContent,
                        IsTrue = childItem.IsTrue,
                        QuestionID = childItem.QuestionID,
                        AnswerSortOrder = childItem.AnswerSortOrder,
                    });
                }
                originExam.QuestionAnswers.Add(lstQuestion);
            }
            return originExam;
        }
        public async Task<bool> InsertShuffleRule(ExamRequestModel data)
        {
            var newShuffleQuestion = new List<RuleSortExamQuestion>();
            var newShuffleAnswer = new List<RuleSortQuestionAnswer>();
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                int randnum = random.Next(100);
                var shuffleCode = $"{data.Exam.ExamTestCode}_{randnum}";
                var lstSortQuestion = data.QuestionAnswers.Select(x => x.QuestionSortOrder).ToList();
                var listNewOrderQuestion = Shuffle(lstSortQuestion);
                for (int j = 0; j < listNewOrderQuestion.Count; j++)
                {
                    newShuffleQuestion.Add(new RuleSortExamQuestion()
                    {
                        ExamTestID = data.Exam.ExamTestID,
                        OriginSortOrder = data.QuestionAnswers[j].QuestionSortOrder,
                        ShuffleOrder = listNewOrderQuestion[j],
                        ExamCode = shuffleCode,
                        OriginQuestionID = data.QuestionAnswers[j].QuestionID,
                    });
                    var listNewOrderAnswer = Shuffle(data.QuestionAnswers[j].Answers.Select(x => x.AnswerSortOrder).ToList());
                    var dataQuestion = data.QuestionAnswers[j].Answers;
                    for (int k = 0; k < dataQuestion.Count; k++)
                    {
                        newShuffleAnswer.Add(new RuleSortQuestionAnswer()
                        {
                            QuestionID = dataQuestion[k].QuestionID,
                            OriginSortOrder = dataQuestion[k].AnswerSortOrder,
                            ShuffleOrderAnswer = listNewOrderAnswer[k],
                            OriginAnswerID = dataQuestion[k].AnswerID,
                            ExamCode = shuffleCode
                        });
                    }

                }
            }
            var dbtran = await _examTestRepository.InsertShuffleExam(newShuffleQuestion, newShuffleAnswer);
            return true;
        }
        public async Task<ServiceResponse> Getpaging(PagingRequestModel data)
        {
            var resData = await _examTestRepository.GetPaging(data.PageSize, data.PageIndex, data.ValueWhere);
            var res = new ServiceResponse();
            res.Data = resData;
            res.Success = true;
            return res;
        }
        public async Task<ServiceResponse> GetShuffleExam(string examID)
        {
            var examTest = (await _examTestRepository.GetExamsByExamIDs(new List<int> { int.Parse(examID) })).First();
            var dataShuffle = await _examTestRepository.GetShuffleExam(examID);
            var datasExamDic = dataShuffle.GroupBy(x => x.ExamCode).ToDictionary(k => k.Key, g => g.ToList());
            var pagingResponse = new PagingResponse();
            var pageData = new List<ExamRequestModel>();
            foreach (var dataExamDic in datasExamDic)
            {
                var exam = new ExamRequestModel();
                //examTest.ExamTestID = item.Value.FirstOrDefault()?.ExamTestID ?? 0;
                var examTmp = JsonConvert.DeserializeObject<ExamTest>(JsonConvert.SerializeObject(examTest));
                examTmp.ExamTestCode = dataExamDic.Key;
                exam.Exam = examTmp;
                var dataQuestionDic = dataExamDic.Value.ToList().GroupBy(x => x.OriginQuestionID).ToDictionary(k => k.Key, g => g.ToList());
                foreach (var item in dataQuestionDic)
                {
                    var question = new QuestionAnswers();
                    question.QuestionID = item.Value.FirstOrDefault()?.OriginQuestionID ?? Guid.Empty;
                    question.QuestionContent = item.Value.FirstOrDefault()?.QuestionContent ?? "";
                    question.QuestionSortOrder = item.Value.FirstOrDefault()?.ShuffleOrder ?? 1;
                    foreach (var answer in item.Value)
                    {
                        question.Answers.Add(new Answer()
                        {
                            IsTrue = answer.IsTrue,
                            AnswerContent = answer.AnswerContent,
                            AnswerSortOrder = answer.ShuffleOrderAnswer
                        });
                    }
                    exam.QuestionAnswers.Add(question);
                }
                exam.QuestionAnswers = exam.QuestionAnswers.OrderBy(x => x.QuestionSortOrder).ToList();
                exam.QuestionAnswers.ForEach(x => x.Answers = x.Answers.OrderBy(k => k.AnswerSortOrder).ToList());
                pageData.Add(exam);
            }
            pagingResponse.PageData = pageData;
            return new ServiceResponse(true, "", data: pagingResponse);
        }
        public List<int> Shuffle(List<int> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        public async Task<ServiceResponse> InsertUserExam(List<UserExam> data)
        {
            var res = new ServiceResponse();
            var lstUserExam = data.Cast<BaseModel>().ToList();
            var resInsert = await _examTestRepository.MultiInsert(lstUserExam, false);
            res.Success = Convert.ToBoolean(resInsert);
            return res;
        }
        public async Task<ServiceResponse> GetResultExam(MarkTestRequestModel data)
        {

            var resultExamByCode = await _examTestRepository.GetResultByExamCode(data.ExamCode);
            var point = 0m;
            var lstAnalysis = new List<AnalysisQuestionDto>();
            var dataRsByCodeDic = resultExamByCode.GroupBy(x => x.QuestionOrder).ToDictionary(k => k.Key, g => g.ToList());
            var i = 0;
            if (data.QuestionDetails.Any())
            {
                foreach (var item in dataRsByCodeDic)
                {
                    var dataModel = data.QuestionDetails[i];
                    if (dataModel.Results.Any())
                    {
                        var isCorrect = dataModel.Results.All(item.Value.Where(k=>k.IsTrue == true).Select(l => l.OrderAnswer).OrderBy(x => x).Contains);
                        if(isCorrect){
                            point += (10m / dataRsByCodeDic.Count);
                            lstAnalysis.Add(new AnalysisQuestionDto()
                            {
                                point += (10 / dataRsByCodeDic.Count);
                                lstAnalysis.Add(new AnalysisQuestionDto()
                                {
                                    MainAnalysCode = dataModel.MainAnalysCode,
                                    MainAnalysName = dataModel.MainAnalysName,
                                    SubAnalysCode = dataModel.SubAnalysCode,
                                    SubAnalysPoint = dataModel.SubAnalysPoint
                                });
                            }
                        }
                    }
                    i++;
                }
            }
            var dicDataAnalys = lstAnalysis.GroupBy(x => x.MainAnalysCode).ToDictionary(k => k.Key, g => g.ToList());
            var dicCalculateAnalys = new Dictionary<string, object>();
            var comment = new StringBuilder("");
            double totalPoin = 0;
            foreach (var itemAnalys in dicDataAnalys)
            {
                double poinSub = 0;
                var nameMainAnalys = itemAnalys.Value.FirstOrDefault()?.MainAnalysName ?? "";
                foreach (var itemSubAnalys in itemAnalys.Value)
                {
                    poinSub += itemSubAnalys.SubAnalysPoint;
                }
                totalPoin += poinSub;
                if (itemAnalys.Key == "N1")
                {
                    if (poinSub < 3)
                    {
                        comment.AppendLine($"Bạn cần bổ sung kiến thức và tính toán kỹ lưỡng hơn {nameMainAnalys}<br>");
                    }
                    else if (poinSub >= 3 && poinSub < 5)
                    {
                        comment.AppendLine($"Bạn áp dụng tốt kiến thức vào bài kiểm tra {nameMainAnalys}<br>");
                    }
                    else
                    {
                        comment.AppendLine($"Bạn hoàn thành xuất sắc {nameMainAnalys}<br>");
                    }
                }
                else if (itemAnalys.Key == "N2")
                {
                    if (poinSub < 3.5)
                    {
                        comment.AppendLine($"Bạn cần bổ sung kiến thức và tính toán kỹ lưỡng hơn  {nameMainAnalys} <br>");
                    }
                    else if (poinSub >= 3.5 && poinSub < 4)
                    {
                        comment.AppendLine($"Bạn áp dụng tốt kiến thức vào bài kiểm tra, {nameMainAnalys}<br>");
                    }
                    else
                    {
                        comment.AppendLine($"Bạn hoàn thành xuất sắc {nameMainAnalys}<br>");
                    }
                }
                else if (itemAnalys.Key == "N3")
                {
                    if (poinSub < 2)
                    {
                        comment.AppendLine($"Bạn cần bổ sung kiến thức và tính toán kỹ lưỡng hơn {nameMainAnalys}<br>");
                    }
                    else if (poinSub >= 2 && poinSub < 6)
                    {
                        comment.AppendLine($"Bạn áp dụng tốt kiến thức vào bài kiểm tra {nameMainAnalys} <br>");
                    }
                    else
                    {
                        comment.AppendLine($"Bạn hoàn thành xuất sắc {nameMainAnalys} <br>");
                    }
                }
            }
            if (totalPoin < 13.5)
            {
                comment.AppendLine($"Bạn đạt kết quả kém trong bài kiểm tra lần này, bạn cần nỗ lực bổ sung kiến thức để bài kiểm tra tới đạt kết quả tốt.<br>");
            }
            else if (totalPoin >= 13.5 && totalPoin < 16)
            {
                comment.AppendLine("Bạn đã áp dụng tốt những kiến thức trên lớp học vào bài kiểm tra, cần tiếp tục nỗ lực cho bài kiểm tra tiếp theo.<br>");
            }
            else
            {
                comment.AppendLine("Hoàn thành bài Kiểm tra đạt kết quả tốt, tiếp tục phấn đấu <br>");
            }
            var dataUpdate = new UserExam()
            {
                ResultJson = comment.ToString(),
                Point = point,
                UserID = _UserID,
                ExamCode = data.ExamCode,
                IsTest = true
            } 
            ;
            var resInsert = await _examTestRepository.UpdateUserExam(dataUpdate);
            return new ServiceResponse();
        }
        public async Task<ServiceResponse> GetDataExamDoingDetail(string examCode)
        {
            var resData = await _examTestRepository.GetDataExamDoing(examCode,_UserID);
            var dicData = resData.GroupBy(x => x.OriginQuestionID).ToDictionary(k => k.Key, g => g.ToList());
            var data = new List<ExamDoingResponse>();
            foreach (var item in dicData)
            {
                var dataModel = new ExamDoingResponse();
                var dataQuestion = item.Value.FirstOrDefault();
                dataModel.IsMultiAnswer = dataQuestion?.IsMultiResult ?? false;
                dataModel.ID = dataQuestion.OriginQuestionID;
                dataModel.QuestionContent = dataQuestion.QuestionContent;
                dataModel.SubAnalysName = dataQuestion.SubAnalysName;
                dataModel.SubAnalysCode = dataQuestion.SubAnalysCode;
                dataModel.MainAnalysName = dataQuestion.MainAnalysName;
                dataModel.MainAnalysCode = dataQuestion.MainAnalysCode;
                dataModel.SubAnalysPoint = dataQuestion.SubAnalysPoint;
                dataModel.Answers = new List<AnswerDoing>();
                foreach (var ans in item.Value)
                {
                    dataModel.Answers.Add(new AnswerDoing()
                    {
                        AnswerContent = ans.AnswerContent,
                        AnswerSortOrder = ans.AnswerSortOrder,
                    });
                }
                dataModel.Answers = dataModel.Answers.OrderBy(x => x.AnswerSortOrder).ToList();
                data.Add(dataModel);
            }
            return new ServiceResponse()
            {
                Success = true,
                Data = data
            };
        }
        public async Task<ServiceResponse> GetDetailAnalys(string examCode)
        {
            var data = await _examTestRepository.GetDetailAnalys(examCode,_UserID);
            return new ServiceResponse()
            {
                Success = true,
                Data = data
            };
        }
    }
}
