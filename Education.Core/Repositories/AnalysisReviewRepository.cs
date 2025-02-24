using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
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
    public class AnalysisReviewRepository : GenericRepositories<AnalysisReview>, IAnalysisReviewRepository
    {
        public AnalysisReviewRepository(IDbContext<AnalysisReview> dbContext) : base(dbContext)
        {
        }
        public async Task<PagingResponse> GetPagingAnalysisReview(PagingRequestModel pagingRequest)
        {
            var dataPaging = new PagingResponse();
            var offSet = (pagingRequest.PageIndex - 1) * pagingRequest.PageSize;

            var sql = new StringBuilder("select * from analysis_review ");
            var sqlCount = new StringBuilder("select count(1) from analysis_review ");

            var paramQuery = new Dictionary<string, object>();
            var paramCount = new Dictionary<string, object>();
            paramQuery.Add("@pageSize", pagingRequest.PageSize);
            paramQuery.Add("@offSet", offSet);
            // 1. Search
            //if (!string.IsNullOrEmpty(pagingRequest.ValueWhere))
            //{
            //    sql.Append(" where Name like @stringWhere");
            //    sqlCount.Append(" where Name like @stringWhere;");
            //    paramCount.Add("@stringWhere", $"%{pagingRequest.ValueWhere}%");
            //    paramQuery.Add("@stringWhere", $"%{pagingRequest.ValueWhere}%");
            //}

            // 2. CustomParam
            //var customParam = pagingRequest.CustomParam;
            //if (customParam.TryGetValue("IsMain", out object isMainObj))
            //{
            //    var whereCustom = string.Empty;
            //    bool isMain = Convert.ToBoolean(isMainObj.ToString());
            //    sql.Append(" {WhereCustom}");
            //    sqlCount.Append(" {WhereCustom}");
            //    if (isMain is true)
            //    {
            //        whereCustom = "WHERE IsMain is TRUE";
            //    }
            //    else
            //    {
            //        whereCustom = "WHERE IsMain is not TRUE";
            //    }
            //    sql = sql.Replace("{WhereCustom}", whereCustom);
            //    sqlCount = sqlCount.Replace("{WhereCustom}", whereCustom);
            //}

            sql.Append(" limit @pageSize offset @offSet;");
            var res = await _dbContext.QueryUsingStore<object>(paramQuery, sql.ToString(), commandType: CommandType.Text);
            var resCount = await _dbContext.ExecuteScalarUsingStore(paramCount, sqlCount.ToString(), commandType: CommandType.Text);
            dataPaging.PageData = res.ToList();
            dataPaging.PageSize = int.Parse(resCount.ToString() ?? "0");
            return dataPaging;
        }
    }
}
