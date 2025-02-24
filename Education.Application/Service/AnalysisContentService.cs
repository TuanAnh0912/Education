using Education.Application.Helpers;
using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using Education.Core.Repositories;
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


        public async override Task<ServiceResponse> Add(AnalysisContent entity)
        {
            if (entity is null)
            {
                return new ServiceResponse(false, "Dữ liệu trống");
            }
            var rsInsert = await _analysisContentRepository.Add(entity);
            if (Convert.ToInt32(rsInsert) > 0)
            { 
                return new ServiceResponse(true, "Thành công");
            }
            else
            {
                return new ServiceResponse(false, "Có lỗi xảy ra");
            }
        }

        public async override Task<ServiceResponse> Update(AnalysisContent entity)
        {
            if (entity is null)
            {
                return new ServiceResponse(false, "Dữ liệu trống");
            }
            var resUpdate = await _analysisContentRepository.Update(entity);
            if (!resUpdate)
            {
                return new ServiceResponse(false, "cập nhật dữ liệu thất bại");
            }
            return new ServiceResponse(true, "cập nhật dữ liệu thành công");
        }

        public async override Task<ServiceResponse> Delete(object id)
        {
            var resDelete = await _analysisContentRepository.Delete(id);
            if (!resDelete)
            {
                return new ServiceResponse(false, "Xóa dữ liệu thất bại");
            }
            return new ServiceResponse(true, "Xóa dữ liệu thành công");
        }
    }
}
