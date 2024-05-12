using Education.Application.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml.DataValidation;
using System.ComponentModel;
using System.Reflection;

namespace Education.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
   // [Authorize]
    public class ExamGeneralController : BaseServiceController<ExamGeneral>
    {
        private IExamGeneralService _examGeneralService;
        public ExamGeneralController(IExamGeneralService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            this.currentType = typeof(ExamGeneral);
            _examGeneralService = serviceProvider.GetRequiredService<IExamGeneralService>();
        }
        [HttpPost("exam-test-general")]
        public async Task<ServiceResponse> InsertExamTestGeneral(ExamGeneralRequestModel data)
        {
            return  await _examGeneralService.InsertsExamTestGeneral(data);
        }
    }

}