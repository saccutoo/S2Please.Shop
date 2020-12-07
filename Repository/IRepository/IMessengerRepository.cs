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
    public partial interface IMessengerRepository
    {
        //save message
        ResultModel SaveMessenger(ChatModel model);

        //Get 3 messenger new 
        ResultModel GetTop3MessengerNew(ParamType paramType, bool isCheckPermission = true);

        //Get messenger by session id 
        ResultModel GetMessengerBySessionId(string sessionId, bool isUpdate = true, bool isCheckPermission = false);

        //Get messenger IS MAIN
        ResultModel GetMessengerIsMain(bool isCheckPermission = false);
    }
}