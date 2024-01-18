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
    public class AnalysisHeaderRepository : GenericRepositories<AnalysisHeader>, IAnalysisHeaderRepository
    {
        public AnalysisHeaderRepository(IDbContext<AnalysisHeader> dbContext) : base(dbContext)
        {
        }
    }
}
