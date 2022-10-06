const idSistema = new URLSearchParams(window.location.search).get("id");

const pagamentoParcelaRequest = {
    IdPagamentoParcela: 0,
    IdParcela: 0,
    DataPagamento: "",
    ValorDepositoBancario: 0,
    ValorCartaoCredito: 0,
    ValorCartaoDebito: 0
};

const parcelaRequest = {
    IdParcela: 0,
    IdSistema: 0,
    IdServicoFinanceiro: 0,
    IdStatusParcela: 0,
    Numero: 0,
    DataGeracao: "",
    DataVencimento: "",
    DataCancelamento: "",
    Valor: 0,
    Desconto: 0,
    Acrescimo: 0,
    Observacao: ""
}

$(document).ready(function () {
    loading.bloquear();
    setTimeout(async function () {
        try {
            await carregarSistema();
            await carregarParcelas();
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }

    }, 500);
});

function carregarSistema() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            accepts: "application/json",
            url: "/Sistema/ObterVwSistemaPorId?id=" + idSistema
        }).done(function (jqXhr) {
            const { sistema } = jqXhr;

            $("#clienteLiteral").text(sistema.ClienteNomeEmpresa);
            $("#tipoSistemaLiteral").text(sistema.DescricaoTipoSistema);
            $("#dominioLiteral").text(sistema.Dominio);
            $("#dominioProvisorioLiteral").text(sistema.DominioProvisorio);
            $("#dataInicioLiteral").text(formatarData(sistema.DataInicioVM));
            $("#dataCancelamentoLiteral").text(formatarData(sistema.DataCancelamentoVM));

            resolve();
        }).fail(function (jqXhr) {
            reject(jqXhr);
        });
    })
}

function carregarParcelas() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/FichaFinanceira/ObterParcelas?idSistema=" + idSistema,
            type: "GET",
            accepts: "application/json"
        }).done(jqXhr => {
            const { parcelas } = jqXhr;

            const total = parcelas.length;

            $("#totalParcelas").text(total);

            if (total <= 0) {
                $("#parcela_conteudoTabela").css("display", "none");
                $("#parcela_vazioTabela").css("display", "block");
                resolve();
            } else {
                $("#parcela_conteudoTabela").css("display", "block");
                $("#parcela_vazioTabela").css("display", "none");
            }
            const table = $("#percela_tabela > tbody");
            table.empty();
            parcelas.map(parcela => {
                let valorPago = parcela.ValorPago;

                let valorDesconto = parcela.Desconto;

                let valorAcrescimo = parcela.Acrescimo;

                if (valorDesconto <= 0)
                    valorDesconto = "-";

                if (valorAcrescimo <= 0)
                    valorAcrescimo = "-";

                if (valorPago <= 0)
                    valorPago = "-";


                let atributosSpanPagamento = `onclick="pagamentoParcelaClick(${parcela.IdParcela})"  style="cursor:pointer"`;
                let atriburosSpanEditarParcela = `onclick="editarParcelaClick(${parcela.IdParcela})" style="cursor:pointer"`;
                let atributosCancelarParcela = ` onclick="cancelarParcelaClick(${parcela.IdParcela})" style="cursor:pointer" `;
                let atributosExcluirParcela = ` onclick="excluirParcelaClick(${parcela.IdParcela})"  style="cursor:pointer" `;

                let atributosBloqueado = "onclick='return;' style='cursor:not-allowed'";

                if (parcela.ValorPago > 0) //pago
                {
                    atriburosSpanEditarParcela = atributosBloqueado;
                    atributosCancelarParcela = atributosBloqueado;
                    atributosExcluirParcela = atributosBloqueado;
                }
                else if (parcela.IdStatusParcela == 4) //cancelado
                {
                    atributosSpanPagamento = atributosBloqueado;
                    atriburosSpanEditarParcela = atributosBloqueado;
                    atributosCancelarParcela = atributosBloqueado
                }


                const tr = `
<tr>
    <td>${parcela.DescricaoServico}</td>
    <td>${parcela.Numero}</td>
    <td>${parcela.StatusParcelaDescricao}</td>
    <td>${parcela.DataVencimentoVM}</td>
    <td>${valorDesconto}</td>
    <td>${valorAcrescimo}</td>
    <td>${valorPago}</td>
    <td>${parcela.ValorPagar}</td>
    <td>
        <span ${atributosSpanPagamento} title="Pagar">
           <i class="fa-solid fa-money-bill"></i>
        </span>

        &nbsp;

        <span ${atriburosSpanEditarParcela} title="Editar">
            <i class="fa fa-pencil"></i>
        </span>
         &nbsp;
         
        <span ${atributosCancelarParcela} title="Cancelar">
          <i class="fa-solid fa-circle-xmark"></i>
        </span>
         &nbsp;
        <span ${atributosExcluirParcela} title="Excluir" >
           <i class="fa fa-trash"></i>
        </span>
    </td>
</tr>
`;
                table.append(tr);
            });
            resolve();

        }).fail(jqXhr => {
            reject(jqXhr);
        })
    });
}

function concederServicoFinanceiroClick() {

    loading.bloquear();
    let idServicoFinanceiroSelected = $("#servicoFinanceiroDropDownList option:selected").val();

    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/FichaFinanceira/ConcederServicoFinanceiro?idSistema=" + idSistema + "&idServicoFinanceiro=" + idServicoFinanceiroSelected,
            accepts: "application/json",
            complete: () => {
                loading.desbloquear();
            }
        }).done(async jqXhr => {
            try {
                const { mensagem } = jqXhr;
                await carregarParcelas();
                exibirAlert.mensagemSucesso(mensagem);
            } catch (err) {
                exibirAlert.mensagemErroAjax(jqXhr);
            }
        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        })
    }, 500);
}

function abrirModalConcederServicoFinanceiro() {

    loading.bloquear();

    setTimeout(function () {
        $.ajax({
            type: "GET",
            url: "/ServicoFinanceiro/ObterServicoFinanceiroDropDownList",
            accepts: "application/json",
            complete: () => {
                loading.desbloquear();
            }
        }).done(jqXhr => {
            const { servicosFinanceiro } = jqXhr;

            dropdownList.carregarDropDownList("#servicoFinanceiroDropDownList", servicosFinanceiro, "(Selecione um Serviço financeiro)");

            $("#concederServicoModal").modal("show");
        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        });
    }, 500);

}

function editarParcelaClick(idParcela) {
    loading.bloquear();

    setTimeout(function () {
        $.ajax({
            type: "GET",
            url: "/FichaFinanceira/ObterParcelaPorId?id=" + idParcela,
            accepts: "application/json",
            complete: () => {
                loading.desbloquear();
            }
        }).done(jqXhr => {
            const { parcela } = jqXhr;

            $("#idParcelaHiddenField").val(parcela.IdParcela);
            $("#dataVencimentoTextBox").val(parcela.DataVencimentoVM);
            $("#valorTextBox").val(parcela.Valor);
            $("#descontoTextBox").val(parcela.Desconto);
            $("#acrescimoTextBox").val(parcela.Acrescimo);
            $("#observacaoTextBox").val(parcela.Observacao);
            $("#editarParcelaModal").modal("show");

        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        });
    }, 500);
}

function salvarEditarParcelaClick() {

    loading.bloquear();

    parcelaRequest.IdParcela = numericoUtil.transformarTextBoxInteiro("#idParcelaHiddenField");
    parcelaRequest.DataVencimento = $("#dataVencimentoTextBox").val();
    parcelaRequest.Valor = numericoUtil.transformarTextBoxDecimal("#valorTextBox");
    parcelaRequest.Desconto = numericoUtil.transformarTextBoxDecimal("#descontoTextBox");
    parcelaRequest.Acrescimo = numericoUtil.transformarTextBoxDecimal("#acrescimoTextBox");
    parcelaRequest.Observacao = $("#observacaoTextBox").val();

    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "/FichaFinanceira/EditarParcela",
        data: JSON.stringify(parcelaRequest),
        complete: () => {
            loading.desbloquear();
        }
    }).done(async jqXhr => {
        try {
            const { mensagem } = jqXhr;
            await carregarParcelas();
            exibirAlert.mensagemSucesso(mensagem);

        } catch (err) {
            exibirAlert.mensagemErroAjax(jqXhr);
        }

    }).fail(jqXhr => {
        exibirAlert.mensagemErroAjax(jqXhr);
    });
}

function cancelarParcelaClick(idParcela) {

    exibirAlert.confirmar("Deseja cancelar parcela ?", function () {
        loading.bloquear();
        setTimeout(function () {

            $.ajax({
                type: "POST",
                url: "/FichaFinanceira/CancelarParcela?idParcela=" + idParcela,
                accepts: "application/json",
                complete: () => {
                    loading.desbloquear();
                }
            }).done(async jqXhr => {
                const { mensagem } = jqXhr;
                await carregarParcelas();
                exibirAlert.mensagemSucesso(mensagem);
            }).fail(jqXhr => {
                exibirAlert.mensagemErroAjax(jqXhr);
            });

        }, 500);
    })

}

function excluirParcelaClick(idParcela) {
    exibirAlert.confirmar("Deseja excluir parcela? ", function () {

        loading.bloquear();

        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: "/FichaFinanceira/DeletarParcela?idParcela=" + idParcela,
                accepts: "application/json",
                complete: () => {
                    loading.desbloquear();
                }
            }).done(async jqXhr => {
                try {
                    await carregarParcelas();
                    exibirAlert.mensagemSucesso("Parcela excluída com sucesso");
                } catch (err) {
                    exibirAlert.mensagemErroAjax(err);
                }
            }).fail(jqXhr => {
                exibirAlert.mensagemErroAjax(jqXhr);
            });
        }, 500);

    });
}

function pagamentoParcelaClick(idParcela) {

    loading.bloquear();

    setTimeout(function () {
        $.ajax({
            type: "GET",
            url: "/FichaFinanceira/ObterPagamentoParcela?idParcela=" + idParcela,
            accepts: "application/json",
            complete: () => {
                loading.desbloquear();
            }
        }).done(jqXhr => {
            const { pagamentoParcela } = jqXhr;
            $("#pagamentoParcela_IdPagamentoParcela").val(pagamentoParcela.IdPagamentoParcela);
            $("#pagamentoParcela_IdParcela").val(pagamentoParcela.IdParcela);
            $("#pagamentoParcela_DataPagamento").val(pagamentoParcela.DataPagamentoVM);
            $("#pagamentoParcela_ValorDepositoBancario").val(pagamentoParcela.ValorDepositoBancario);
            $("#pagamentoParcela_ValorCartaoCredito").val(pagamentoParcela.ValorCartaoCredito);
            $("#pagamentoParcela_ValorCartaoDebito").val(pagamentoParcela.ValorCartaoDebito);
            $("#pagamentoParcela_Modal").modal("show");
        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        });

    }, 500);
}

function salvarPagamentoParcelaClick() {
    loading.bloquear();

    setTimeout(function () {
        pagamentoParcelaRequest.IdPagamentoParcela = numericoUtil.transformarTextBoxInteiro("#pagamentoParcela_IdPagamentoParcela");
        pagamentoParcelaRequest.IdParcela = numericoUtil.transformarTextBoxInteiro("#pagamentoParcela_IdParcela");
        pagamentoParcelaRequest.DataPagamento = $("#pagamentoParcela_DataPagamento").val();
        pagamentoParcelaRequest.ValorDepositoBancario = numericoUtil.transformarTextBoxDecimal("#pagamentoParcela_ValorDepositoBancario");
        pagamentoParcelaRequest.ValorCartaoCredito = numericoUtil.transformarTextBoxDecimal("#pagamentoParcela_ValorCartaoCredito");
        pagamentoParcelaRequest.ValorCartaoDebito = numericoUtil.transformarTextBoxDecimal("#pagamentoParcela_ValorCartaoDebito");

        $.ajax({
            type: "POST",
            url: "/FichaFinanceira/PagarParcela",
            contentType: "application/json",
            data: JSON.stringify(pagamentoParcelaRequest),
            complete: () => {
                loading.desbloquear();
            }
        }).done(async jqXhr => {
            const { mensagem } = jqXhr;
            await carregarParcelas();
            exibirAlert.mensagemSucesso(mensagem);
        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        });

    }, 500);
}

