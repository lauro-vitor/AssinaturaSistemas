using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ParcelaBLL
    {
        public List<string> ValidarEditarParcela(Parcela parcela)
        {
            var erros = new List<string>();

            if (parcela.IdParcela == 0)
                erros.Add("Id parcela é obrigatório");

            if (parcela.DataVencimento == new DateTime())
                erros.Add("Data de vencimento é obrigatório");

            if (parcela.Valor <= 0)
                erros.Add("Valor deve ser maior do que 0");

            if (parcela.Desconto < 0)
                erros.Add("Desconto deve ser maior ou igual a 0");

            if (parcela.Acrescimo < 0)
                erros.Add("Acréscimo deve ser maior ou igual a 0");

            return erros;
        }
    }
}
