using Education.Application.Interface;
using Education.Application.Service;
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
    public class AnalysisReviewController : BaseServiceController<AnalysisReview>
    {
        private IAnalysisReviewService _analysisReviewService;
        public AnalysisReviewController(IAnalysisReviewService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            this.currentType = typeof(AnalysisReview);
            _analysisReviewService = serviceProvider.GetRequiredService<IAnalysisReviewService>();
        }
        [HttpPost("paging")]
        public async Task<ServiceResponse> Paging(PagingRequestModel pagingRequest)
        {
            return await _analysisReviewService.PagingAnalysisReview(pagingRequest);
        }
    }

}