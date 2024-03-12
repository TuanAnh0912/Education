using Education.Core.Model;
using Education.Core.Model.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IExamTestService:IBaseService<ExamTest>
    {
        Task<bool> InsertExamDetail(ExamRequestModel data);
    }
}
