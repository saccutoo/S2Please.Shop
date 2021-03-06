﻿using Repository.Model;
using Repository.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public partial interface IUserRepository
    {
        //Get all user
        ResultModel GetAllUser();
        //Get all user
        ResultModel GetUserByUserId(long userId);
    }
}
