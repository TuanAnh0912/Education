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
    public class AnalysisContentService : BaseService<AnalysisContent>, IAnalysisContentService
    {
        private IAnalysisContentRepository _analysisContentRepository;
        public AnalysisContentService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _analysisContentRepository = serviceProvider.GetRequiredService<IAnalysisContentRepository>();
        }
        public async Task<ServiceResponse> PagingAnalysis(PagingRequestModel pagingRequest)
        {
            var pagingResponse = await _analysisContentRepository.GetPagingAnalysisContent(pagingRequest);
            var res = new ServiceResponse()
            {
                Data = pagingResponse,
                Success = true
            };
            return res;
        }
    }
}
