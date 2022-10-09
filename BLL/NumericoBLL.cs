using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class NumericoBLL
    {
        public static bool EhNumerico(string valor)
        {
            var regexEhNumerico = new Regex("^[0-9]+$");

            return regexEhNumerico.IsMatch(valor);
        }
    }
}
