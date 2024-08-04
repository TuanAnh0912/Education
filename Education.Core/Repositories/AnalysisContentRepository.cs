using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.DataModel;
using Education.Core.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Repositories
{
    public class AnalysisContentRepository : GenericRepositories<AnalysisContent>, IAnalysisContentRepository
    {
        public AnalysisContentRepository(IDbContext<AnalysisContent> dbContext) : base(dbContext)
        {
        }
        public async Task<PagingResponse> GetPagingAnalysisContent(int pageSize, int pageIndex)
        {
            var dataPaging = new PagingResponse();
            var offSet = (pageIndex - 1) * pageSize;
            var sql = "select * from analysis_content  limit @pageSize offset @offSet;";
            var param = new Dictionary<string, object>();
            param.Add("@pageSize", pageSize);
            param.Add("@offSet", offSet);
            var res = await _dbContext.QueryUsingStore<UserExamDto>(param, sql.ToString(), commandType: CommandType.Text);
            dataPaging.PageData = res.ToList();
            return dataPaging;
        }
    }
}
