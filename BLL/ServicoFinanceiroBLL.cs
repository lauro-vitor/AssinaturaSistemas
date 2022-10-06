using Entidades;
using Entidades.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicoFinanceiroBLL
    {
        public bool ValidarConcessao(int idServicoFinanceiro, int idSistema, List<VwParcela> parcelasDoSistema)
        {
            return !parcelasDoSistema.Any(p => p.IdServicoFinanceiro == idServicoFinanceiro && p.IdSistema == idSistema);
        }

        public List<Parcela> ConcederServicoFinanceiro(ServicoFinanceiro servicoFinanceiro, int idSistema)
        {
            var parcelas = new List<Parcela>();

            var hoje = DateTime.Now;

            var dataServico = new DateTime();

            if (hoje.Day < servicoFinanceiro.DiaVencimento)
            {
                dataServico = new DateTime(hoje.Year, (hoje.Month + 1), servicoFinanceiro.DiaVencimento);
            }
            else
            {
                dataServico = new DateTime(hoje.Year, (hoje.Month + 1), servicoFinanceiro.DiaVencimento);
            }


            for (int i = 1; i <= servicoFinanceiro.QuantidadeParcelas; i++)
            {

                var novaParcela = new Parcela()
                {
                    Acrescimo = 0.00M,
                    Desconto = 0.00M,
                    DataGeracao = DateTime.Now,
                    DataCancelamento = null,
                    IdServicoFinanceiro = servicoFinanceiro.IdServicoFinanceiro,
                    Valor = servicoFinanceiro.ValorCobranca,
                    IdSistema = idSistema,
                    IdStatusParcela = (int)EnumStatusParcela.Aberto,
                    Observacao = "",
                    DataVencimento = dataServico,
                    Numero = i,
                };


                if (servicoFinanceiro.IdPeriodoCobranca == (int)EnumPeriodoCobranca.Mensal)
                    dataServico = dataServico.AddMonths(1);
                else if (servicoFinanceiro.IdPeriodoCobranca == (int)EnumPeriodoCobranca.Semestral)
                    dataServico = dataServico.AddMonths(6);
                else if (servicoFinanceiro.IdPeriodoCobranca == (int)EnumPeriodoCobranca.Anual)
                    dataServico = dataServico.AddYears(1);


                parcelas.Add(novaParcela);
            }

            return parcelas;
        }
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
