using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class AnalysisReviewService: BaseService<AnalysisReview>, IAnalysisReviewService
    {
        private IAnalysisReviewRepository _analysisReviewRepository;
        public AnalysisReviewService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _analysisReviewRepository = serviceProvider.GetRequiredService<IAnalysisReviewRepository>();
        }
        public async Task<ServiceResponse> PagingAnalysisReview(PagingRequestModel pagingRequest)
        {
            var pagingResponse = await _analysisReviewRepository.GetPagingAnalysisReview(pagingRequest);
            var res = new ServiceResponse()
            {
                Data = pagingResponse,
                Success = true
            };
            return res;
        }
    }
}
