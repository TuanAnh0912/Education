using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.RequestModel;
using Microsoft.Extensions.DependencyInjection;
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
        public ExamTestService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _examTestRepository = serviceProvider.GetRequiredService<IExamTestRepository>();
        }
        public async Task<bool> InsertExamDetail(ExamRequestModel data)
        {
            return await _examTestRepository.InsertExamDetail(data);
        }
    }
}
