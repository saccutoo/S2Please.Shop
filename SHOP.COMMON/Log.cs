using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SHOP.COMMON
{
    public static class Log
    {
        public static Response Writer(string content, string filePath)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    outputFile.WriteLine(content);
                }

                return new Response
                {
                    Message = "Success",
                    Success = true,
                };
            }
            catch (Exception e)
            {

                return new Response
                {
                    Message = e.ToString(),
                    Success = false,
                };
            }

        }
    }
}
