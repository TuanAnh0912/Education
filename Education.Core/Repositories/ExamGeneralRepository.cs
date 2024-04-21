using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Repositories
{
    public class ExamGeneralRepository : GenericRepositories<ExamGeneral>, IExamGeneralRepository
    {
        public ExamGeneralRepository(IDbContext<ExamGeneral> dbContext) : base(dbContext)
        {
        }
    }
}
