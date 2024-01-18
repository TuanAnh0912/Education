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
    public class QuestionRepository : GenericRepositories<Question>, IQuestionRepository
    {
        public QuestionRepository(IDbContext<Question> dbContext) : base(dbContext)
        {
        }
    }
}
