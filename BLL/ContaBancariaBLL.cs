using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class ContaBancariaBLL
    {
        public List<string>  ValidarContaBancaria(ContaBancaria contaBancaria)
        {
            var erros = new List<string>();

            var regexEhNumerico = new Regex("^[0-9]+$");

            if (string.IsNullOrEmpty(contaBancaria.NumeroAgencia))
                erros.Add("Número da Agência é obrigatório");

            if (!string.IsNullOrEmpty(contaBancaria.NumeroAgencia) && !regexEhNumerico.IsMatch(contaBancaria.NumeroAgencia))
                erros.Add("Agência deve ser numérico");

            if (string.IsNullOrEmpty(contaBancaria.NumeroConta))
                erros.Add("Número da Conta é obrigatório");

            if (!string.IsNullOrEmpty(contaBancaria.NumeroConta) && !regexEhNumerico.IsMatch(contaBancaria.NumeroConta))
                erros.Add("Número da conta deve ser numérico");

            if (contaBancaria.NumeroBanco <= 0)
                erros.Add("Número do banco é obrigatório");

            if (!string.IsNullOrEmpty(contaBancaria.Cnpj) && !regexEhNumerico.IsMatch(contaBancaria.Cnpj))
                erros.Add("CNPJ deve ser numérico");

            return erros;
        }
    }
}
