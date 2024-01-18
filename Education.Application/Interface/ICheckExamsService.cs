using Education.Core.Model.DataModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface ICheckExamsService
    {
        Task<MemoryStream> ExportData(List<StudentDetects> resultStudent, string studentName);
        Task ImportStudentsResultExams(IFormFile formFile);
    }
}
