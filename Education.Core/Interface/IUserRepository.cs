﻿using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<User?> CheckLogin(string username, string password);
        Task<User> CheckByUserNameAndEmail(string userName);
        //Task<User?> GetUserByRefreshToken(string resfreshToken);
    }
}
