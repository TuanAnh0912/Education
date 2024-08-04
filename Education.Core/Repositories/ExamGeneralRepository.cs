using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public async Task<List<Guid>> GetLstUserIDByBlockID(int blockID)
        {
            var sql = "SELECT ub.UserID FROM user_block ub JOIN exam_general eg ON ub.BlockID = eg.BlockID WHERE eg.BlockID = @blockID GROUP by ub.UserID;";
            var param = new Dictionary<string, object>()
            {
                {"@blockID",blockID }
            };
            var data = await _dbContext.QueryUsingStore<Guid>(param, sql, commandType: System.Data.CommandType.Text);
            return data.ToList();
        }
    }
}
