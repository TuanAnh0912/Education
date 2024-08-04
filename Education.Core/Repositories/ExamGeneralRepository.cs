using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Education.Core.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public async Task<PagingResponse> Paging(PagingRequestModel pagingRequest)
        {
            var res = new PagingResponse();
            var sql = new StringBuilder();
            sql.Append(" exam_general eg");
            sql.Append(" JOIN block b USING(BlockID)");
            sql.Append(" JOIN user_block ub USING(BlockID)");
            sql.Append(" JOIN exam_test_general etg ON eg.ExamGeneralID = etg.ExamGeneralID");
            sql.Append(" JOIN exam_test et ON etg.ExamTestID = et.ExamTestID");
            //sql.Append(" Fields like @stringWhere ");
            sql.Append(" LIMIT @PageSize OFFSET @OffSet;");

            var pageIndex = pagingRequest.PageIndex;
            var pageSize = pagingRequest.PageSize;
            var offSet = (pageIndex - 1) * pageSize;
            var param = new Dictionary<string, object>()
            {
                {"@PageSize", pageSize },
                {"@OffSet", offSet },
                {"@StringWhere", $"%{pagingRequest.ValueWhere}%" }
            };
            var data = await _dbContext.QueryUsingStore<object>(param, "SELECT eg.ExamGeneralID, eg.Name as ExamGeneralName, b.BlockName, et.* FROM " + sql, commandType: CommandType.Text);
            res.PageData = data;
            var total = await _dbContext.QueryUsingStore<int>(param, "SELECT COUNT(1) FROM " + sql, commandType: CommandType.Text);
            res.PageSize = total.FirstOrDefault();
            return res;
        }

        public async Task<PagingResponse> PagingByUser(PagingRequestModel pagingRequest, string userID)
        {
            var res = new PagingResponse();
            var sql = new StringBuilder();
            sql.Append(" exam_general eg");
            sql.Append(" JOIN block b USING(BlockID)");
            sql.Append(" JOIN user_block ub USING(BlockID)");
            sql.Append(" JOIN exam_test_general etg ON eg.ExamGeneralID = etg.ExamGeneralID");
            sql.Append(" JOIN exam_test et ON etg.ExamTestID = et.ExamTestID");
            sql.Append(" WHERE ub.UserID = @UserID");
            //sql.Append(" Fields like @stringWhere ");
            sql.Append(" LIMIT @PageSize OFFSET @OffSet;");

            var pageIndex = pagingRequest.PageIndex;
            var pageSize = pagingRequest.PageSize;
            var offSet = (pageIndex - 1) * pageSize;
            var param = new Dictionary<string, object>()
            {
                {"@UserID", userID },
                {"@PageSize",pageSize },
                {"@OffSet",offSet },
                {"@StringWhere",$"%{pagingRequest.ValueWhere}%" }
            };
            var data = await _dbContext.QueryUsingStore<object>(param, "SELECT eg.ExamGeneralID, eg.Name as ExamGeneralName, b.BlockName, et.* FROM " + sql, commandType: CommandType.Text);
            res.PageData = data;
            var total = await _dbContext.QueryUsingStore<int>(param, "SELECT COUNT(1) FROM " + sql, commandType: CommandType.Text);
            res.PageSize = total.FirstOrDefault();
            return res;
        }
    }
}
