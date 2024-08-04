﻿using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IExamGeneralRepository:IGenericRepository<ExamGeneral>
    {
        Task<List<Guid>> GetLstUserIDByBlockID(int blockID);
    }
}
