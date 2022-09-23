using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class UrlUtil
    {
        public static string MontarParametrosUrl(Dictionary<string,string> parametros)
        {
            string parametrosCriptografado = "";

            using (var conteudo = new FormUrlEncodedContent(parametros))
            {
                string parametrosAuxiliar = conteudo.ReadAsStringAsync().Result;

                parametrosCriptografado = CriptografiaUtil.Encrypt(parametrosAuxiliar);
            }

            return parametrosCriptografado;
        }

        public static Dictionary<string, string> RecuperarParametrosUrl(string queryString)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();

            var queryStringDecript = CriptografiaUtil.Decrypt(queryString);

            string[] parametrosQueryString = queryStringDecript.Split('&');


            for (int j = 0; j < parametrosQueryString.Count(); j++)
            {
                string[] elemento = parametrosQueryString[j].Split('=');

                string key = elemento[0];
                string value = elemento[1];
                parametros.Add(key, value);
            }

            return parametros;
        }
        

    }
}
