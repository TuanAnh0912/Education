using Education.Application.Interface;
using Education.Application.Service.Base;
using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class QuestionService : BaseService<Question>, IQuestionService
    {
        public QuestionService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
