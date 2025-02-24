using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            // 1. Search
            //if (!string.IsNullOrEmpty(pagingRequest.ValueWhere))
            //{
            //    sql.Append(" where Name like @stringWhere");
            //    sqlCount.Append(" where Name like @stringWhere;");
            //    paramCount.Add("@stringWhere", $"%{pagingRequest.ValueWhere}%");
            //    paramQuery.Add("@stringWhere", $"%{pagingRequest.ValueWhere}%");
            //}

            // 2. CustomParam
            var customParam = pagingRequest.CustomParam;
            if (customParam.TryGetValue("IsMain", out object isMainObj))
            {
                var whereCustom = string.Empty;
                bool isMain = Convert.ToBoolean(isMainObj.ToString());
                sql.Append(" {WhereCustom}");
                sqlCount.Append(" {WhereCustom}");
                if (isMain is true)
                {
                    whereCustom = "WHERE IsMain is TRUE";
                }
                else
                {
                    whereCustom = "WHERE IsMain is not TRUE";
                }
                sql = sql.Replace("{WhereCustom}", whereCustom);
                sqlCount = sqlCount.Replace("{WhereCustom}", whereCustom);
            }

            sql.Append(" ORDER BY AnalysisContentID DESC limit @pageSize offset @offSet;");
            var res = await _dbContext.QueryUsingStore<object>(paramQuery, sql.ToString(), commandType: CommandType.Text);
            var resCount = await _dbContext.ExecuteScalarUsingStore(paramCount, sqlCount.ToString(), commandType: CommandType.Text);
            dataPaging.PageData = res.ToList();
            dataPaging.PageSize = int.Parse(resCount.ToString() ?? "0");
            return dataPaging;
        }

        public async override Task<object> Add(AnalysisContent entity)
        {
            var param2 = new Dictionary<string, object>
            {
                {"v_SubjectCode", entity.SubjectCode},
                {"v_Code", entity.Code},
                {"v_Name", entity.Name},
                {"v_Point", entity.Point},
                {"v_IsMain", entity.IsMain}
            };
            
            using (var db = _dbContext.GetDbTransaction())
            {
                var transaction = _dbContext.GetDbTransaction();
                var rsInsert = await _dbContext.ExcuseUsingStore(param2, "Proc_Insert_AnalysisContent", transaction);
                if (rsInsert > 0)
                {
                    transaction.Commit();
                }
                return rsInsert;
            }
        }

        public async override Task<bool> Update(AnalysisContent entity)
        {
            var param2 = new Dictionary<string, object>
            {
                {"v_AnalysisContentID", entity.AnalysisContentID},
                {"v_SubjectCode", entity.SubjectCode},
                {"v_Code", entity.Code},
                {"v_Name", entity.Name},
                {"v_Point", entity.Point},
                {"v_IsMain", entity.IsMain}
            };

            using (var db = _dbContext.GetDbTransaction())
            {
                var transaction = _dbContext.GetDbTransaction();
                var rsUpdate = await _dbContext.ExcuseUsingStore(param2, "Proc_Update_AnalysisContent", transaction);
                if (rsUpdate > 0)
                {
                    transaction.Commit();
                    return true;
                }
                return false;
            }
        }
        public async override Task<bool> Delete(object id)
        {
            var sql = "DELETE FROM analysis_content WHERE AnalysisContentID = @AnaylysisContentID;";
            var paramQuery = new Dictionary<string, object>()
            {
                { "@AnaylysisContentID", id }
            };
            var res = await _dbContext.ExcuseUsingStore(paramQuery, sql, commandType: CommandType.Text);
            if (res > 0)
            {
                return true;
            }
            return false;
        }
    }
}
