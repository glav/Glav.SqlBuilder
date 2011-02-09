using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Helpers
{
    public static class ExceptionExtensions
    {
        public static string GetFullExceptionMessages(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception innerEx = ex;
            int cnt=0;
            while (innerEx != null)
            {
                cnt++;
                if (cnt > 1)
                    sb.AppendFormat(". ");
                sb.AppendFormat("{0}: {1}", cnt, innerEx.Message);
                innerEx = innerEx.InnerException;
            }
            return sb.ToString();
        }
    }
}
