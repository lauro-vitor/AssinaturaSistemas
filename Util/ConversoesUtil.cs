using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class ConversoesUtil
    {
        public static DateTime ToDateTimePtBr(DateTime datetime)
        {
            CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

            return Convert.ToDateTime(datetime, new CultureInfo("pt-BR"));
        }
    }
}
