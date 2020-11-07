using Hrm.Common.Helpers;
using Repository.Model;
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
    public partial class SystemRepository : CommonRepository, ISystemRepository
    {
        //Get MultiLanguage
        public ResultModel GetMultiLanguage()
        {
            return ListProcedure<MultiLanguageModel>(new MultiLanguageModel(), "MultiLanguage_Get_MultiLanguage", new List<Param>(), true);
        }
    }
}
