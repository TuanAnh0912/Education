using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IExamGeneralService:IBaseService<ExamGeneral>
    {
        Task<ServiceResponse> InsertsExamTestGeneral(ExamGeneralRequestModel data);
    }
}
