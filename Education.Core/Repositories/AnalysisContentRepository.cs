using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
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
        public async Task<PagingResponse> GetPagingAnalysisContent(PagingRequestModel pagingRequest)
        {
            var dataPaging = new PagingResponse();
            var offSet = (pagingRequest.PageIndex - 1) * pagingRequest.PageSize;
            var sql = new StringBuilder("select * from analysis_content ");
            var sqlCount  = new StringBuilder("select count(1) from analysis_content ");
            var paramQuery = new Dictionary<string, object>();
            var paramCount = new Dictionary<string, object>();
            paramQuery.Add("@pageSize", pagingRequest.PageSize);
            paramQuery.Add("@offSet", offSet);
            if (!string.IsNullOrEmpty(pagingRequest.ValueWhere))
            {
                sql.Append(" where Name like @stringWhere");
                sqlCount.Append(" where Name like @stringWhere;");
                paramCount.Add("@stringWhere", $"%{pagingRequest.ValueWhere}%");
                paramQuery.Add("@stringWhere", $"%{pagingRequest.ValueWhere}%");
            }
            sql.Append(" limit @pageSize offset @offSet;");
            var res = await _dbContext.QueryUsingStore<object>(paramQuery, sql.ToString(), commandType: CommandType.Text);
            var resCount = await _dbContext.ExecuteScalarUsingStore(paramCount, sqlCount.ToString(), commandType: CommandType.Text);
            dataPaging.PageData = res.ToList();
            dataPaging.PageSize = int.Parse(resCount.ToString() ?? "0");
            return dataPaging;
        }
    }
}
