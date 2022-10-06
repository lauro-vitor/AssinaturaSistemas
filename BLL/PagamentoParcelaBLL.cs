using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PagamentoParcelaBLL
    {
       
       
        public List<string> ValidarPagamentoParcela(PagamentoParcela pagamentoParcela, PagamentoParcela pagamentosDaParcela)
        {
            var erros = new List<string>();

            if (pagamentoParcela.IdParcela <= 0)
                erros.Add("Id Parcela é obrigatório");

            if (pagamentoParcela.IdPagamentoParcela == 0 && pagamentosDaParcela != null)
                erros.Add("Já existe um pagamento lançado para essa parcela");
            

            if (pagamentoParcela.DataPagamento == new DateTime())
                erros.Add("Data de pagamento é obrigatório");

            if (pagamentoParcela.ValorCartaoCredito < 0)
                erros.Add("Valor cartão de crédito não pode ser nagativo");

            if (pagamentoParcela.ValorCartaoDebito < 0)
                erros.Add("Valor cartão de débito não pode ser negativo");

            if (pagamentoParcela.ValorDepositoBancario < 0)
                erros.Add("Valor depósito bancário não pode ser negativo");

            if (pagamentoParcela.ValorCartaoCredito == 0 &&
                pagamentoParcela.ValorCartaoDebito == 0 &&
                pagamentoParcela.ValorDepositoBancario == 0)
                erros.Add("Valor do pagamento é obrigatório, preencha um dos campos valores");

            return erros;
        }
    }
}
