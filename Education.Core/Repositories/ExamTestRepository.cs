using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Repositories
{
    public class ExamTestRepository : GenericRepositories<ExamTest>, IExamTestRepository
    {
        public ExamTestRepository(IDbContext<ExamTest> dbContext) : base(dbContext)
        {
        }
        public async Task<List<ExamsDetailDto>> GetExamsdetailByExamCodes(List<string> examCodes)
        {
            var param = new Dictionary<string, object>()
            {
                {"v_lstExamCodes",string.Join(',',examCodes) },
            };
            var res = await _dbContext.QueryUsingStore<ExamsDetailDto>(param, "Proc_GetExamDetails");
            return res.ToList();
        }
    }
}
