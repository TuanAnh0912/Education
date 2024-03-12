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
    }

}