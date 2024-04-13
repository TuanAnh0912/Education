using Education.Application.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.DataValidation;
using System.ComponentModel;
using System.Reflection;

namespace Education.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ExamsController : BaseServiceController<ExamTest>
    {
        private IExamTestService _examTestService;
        public ExamsController(IExamTestService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            this.currentType = typeof(ExamTest);
            _examTestService = serviceProvider.GetRequiredService<IExamTestService>();
        }
        [HttpPost("exam-detail")]
        public async Task<ServiceResponse> InsertExam(ExamRequestModel data)
        {
            var res = new ServiceResponse();
            res.Success = await _examTestService.InsertExamDetail(data);
            return res;
        }
        [HttpPost("paging")]
        public async Task<ServiceResponse> GetPaging(PagingRequestModel data)
        {
            var res  = await _examTestService.Getpaging(data);
            return res;
        }
        [HttpGet("shuff-exams")]
        public async Task<ServiceResponse> ShuffleExam([FromQuery]string examCode)
        {
            var res = new ServiceResponse();
            res.Data = await _examTestService.ShuffleExam(examCode);
            return res;
        }
        [HttpGet("all-shuff-exams")]
        public async Task<ServiceResponse> GetAllShuffleExam([FromQuery] int examID)
        {
            var res = new ServiceResponse();
            res.Data = await _examTestService.GetShuffleExam(examID);
            return res;
        }
        [HttpGet("exam-bycode")]
        public async Task<ServiceResponse> GetExamByCode([FromQuery] string examCode)
        {
            var res = new ServiceResponse();

            try
            {
                res.Data = await _examTestService.GetExamsByCode(examCode);
                res.Success = true;
            }
            catch 
            {
                res.Success = false;
            }
            return res;
        }
        //public async Task<ServiceResponse> Get
    }

}