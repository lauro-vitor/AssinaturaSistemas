using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicoFinanceiroBLL 
    {
        public List<string> ValidarServicoFinanceiro(ServicoFinanceiro servicoFinanceiro)
        {
            var erros = new List<string>();

            if (servicoFinanceiro.IdContaBancaria <= 0)
                erros.Add("Conta bancária é obrigatório");

            if (servicoFinanceiro.IdPeriodoCobranca <= 0)
                erros.Add("Período Cobrança é obrigatório");

            if (string.IsNullOrEmpty(servicoFinanceiro.DescricaoServico))
                erros.Add("Descrição do serviço financeiro é obrigatório");

            if (servicoFinanceiro.DiaVencimento <= 0 && servicoFinanceiro.DiaVencimento > 30)
                erros.Add("Dia do vencimento inválido, seu valor deve estar entre 1 e 30");

            if (servicoFinanceiro.ValorCobranca <= 0)
                erros.Add("Valor de cobrança é obrigatório");

            if (servicoFinanceiro.QuantidadeParcelas <= 0)
                erros.Add("Quantidade de parcelas é obrigatório");

            return erros;
        }
    }
}
