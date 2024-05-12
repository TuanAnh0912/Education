using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
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
            data.Exam.ExamTestCode = RandomString(5);
            return await _examTestRepository.InsertExamDetail(data);
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
                    MainAnalysysID = item.Value.FirstOrDefault()?.MainAnalysysID ?? 0,
                    SubAnalysysID = item.Value.FirstOrDefault()?.SubAnalysysID ?? 0,
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
            var resInsert =  await InsertShuffleRule(originExam);
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
                    MainAnalysysID = item.Value.FirstOrDefault()?.MainAnalysysID ?? 0,
                    SubAnalysysID = item.Value.FirstOrDefault()?.SubAnalysysID ?? 0,
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
            for (int i = 0; i < 3; i++)
            {
                var shuffleCode = $"{data.Exam.ExamTestCode}{i}";
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
            var dbtran = await  _examTestRepository.InsertShuffleExam(newShuffleQuestion, newShuffleAnswer);
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
        public async Task<List<ExamRequestModel>> GetShuffleExam(int examID)
        {
            var dataShuffle = await _examTestRepository.GetShuffleExam(examID);
            var datasExamDic = dataShuffle.GroupBy(x => x.ExamCode).ToDictionary(k => k.Key, g => g.ToList());
            var res = new List<ExamRequestModel>();
            foreach (var dataExamDic in datasExamDic)
            {
                var exam = new ExamRequestModel();
                exam.Exam.ExamTestCode = dataExamDic.Key;
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
                res.Add(exam);
            }
            return res;
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
    }
}
