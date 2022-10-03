
const servicoFinanceiro = {
    IdServicoFinanceiro: 0,
    IdContaBancaria: 0,
    IdPeriodoCobranca: 0,
    DescricaoServico: "",
    DiaVencimento: 0,
    ValorCobranca: 0.00,
    QuantidadeParcelas: 0
};

$(document).ready(function () {
    loading.bloquear();

    setTimeout(function () {

        carregarServicosFinanceiro()
            .catch(reason => {
                console.log(reason);
                exibirAlert.mensagemErroAjax(reason);
            })
            .finally(() => {
                loading.desbloquear();
            });
    }, 500);
});


function carregarServicosFinanceiro() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/ServicoFinanceiro/ObterVarios",
            accepts: "application/json"
        }).done(jqXhr => {
            try {
                const { servicosFinanceiro, total, contasBancarias, periodosCobranca } = jqXhr;

                dropdownList.carregarDropDownList("#contaBancariaDropDownList", contasBancarias, "(Selecione conta)");
                dropdownList.carregarDropDownList("#periodoCobrancaDropDownList", periodosCobranca, "(Selecione período cobrança)");

                if (total <= 0) {
                    $("#servicoFinanceiro_tabela_container").css("display", "none");
                    $("#servicoFinanceiro_tabela_container_vazio").css("display", "block");
                    resolve();
                } else {
                    $("#servicoFinanceiro_tabela_container").css("display", "block");
                    $("#servicoFinanceiro_tabela_container_vazio").css("display", "none");
                }

                const table = $("#servicoFinanceiro_table > tbody");

                $("#totalSpan").text(total);

                table.empty();

                servicosFinanceiro.map(sf => {
                    const tr = `
<tr>
    <td>${sf.DescricaoServico}</td>
    <td>${sf.PeriodoCobrancaDescricao}</td>
    <td>${sf.ContaBancariaDescricao}</td>
    <td>${sf.DiaVencimento}</td>
    <td>${sf.ValorCobranca}</td>
    <td>${sf.QuantidadeParcelas}</td>
    <td>
        <span onclick="editarServicoFinanceiroClick(${sf.IdServicoFinanceiro})" style="cursor:pointer">
            <i class="fa fa-pencil"></i>
        </span>
        &nbsp;&nbsp;
        <span  onclick="deletarServicoFinanceiroClick(${sf.IdServicoFinanceiro})" style="cursor:pointer">
            <i class="fa fa-trash"></i>
        </span>
    </td>
</tr>
                `;

                    table.append(tr);
                });

                resolve();
            } catch (err) {
                reject(err);
            }
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}
function novoServicoFinanceiroClick() {
    $("#contaBancariaDropDownList option[value=0]").prop("selected", true);
    $("#periodoCobrancaDropDownList option[value=0]").prop("selected", true);
    $("#descricaoTextBox").val("");
    $("#diaVencimentoTextBox").val("");
    $("#valorCobrancaTextBox").val("");
    $("#quantidadeParcelasTextBox").val("");
    $("#idServicoFinanceiroHidden").val("");

    $("#servicoFinanceiroModal").modal("show");
}

function salvarServicoFinanceiroClick() {

    loading.bloquear();

    servicoFinanceiro.IdServicoFinanceiro = numericoUtil.transformarTextBoxInteiro("#idServicoFinanceiroHidden");
    servicoFinanceiro.IdContaBancaria = numericoUtil.transformarDropDownListNumerico("#contaBancariaDropDownList");
    servicoFinanceiro.IdPeriodoCobranca = numericoUtil.transformarDropDownListNumerico("#periodoCobrancaDropDownList");
    servicoFinanceiro.DescricaoServico = $("#descricaoTextBox").val();
    servicoFinanceiro.DiaVencimento = numericoUtil.transformarTextBoxInteiro("#diaVencimentoTextBox");
    servicoFinanceiro.ValorCobranca = numericoUtil.transformarTextBoxDecimal("#valorCobrancaTextBox");
    servicoFinanceiro.QuantidadeParcelas = numericoUtil.transformarTextBoxInteiro("#quantidadeParcelasTextBox");
    let url = "";

    if (servicoFinanceiro.IdServicoFinanceiro <= 0) {
        url = "/ServicoFinanceiro/Criar";
    } else {
        url = "/ServicoFinanceiro/Editar";
    }

    setTimeout(() => {

        $.ajax({
            type: "POST",
            url,
            data: JSON.stringify(servicoFinanceiro),
            contentType: "application/json",
            complete: () => {
                loading.desbloquear();
            }
        }).done(jqXhr => {
            carregarServicosFinanceiro()
                .then(() => {
                    exibirAlert.mensagemSucesso(jqXhr.mensagem);
                })
                .catch(reason => {
                    exibirAlert.mensagemErroAjax(reason);
                });
        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        });

    }, 500);

}

function editarServicoFinanceiroClick(idServicoFinanceiro) {
    loading.bloquear();

    setTimeout(function () {
        $.ajax({
            type: "GET",
            url: "/ServicoFinanceiro/ObterPorId?id=" + idServicoFinanceiro,
            accepts: "application/json",
            complete: () => {
                loading.desbloquear();
            }
        }).done(jqXhr => {
            const { servicoFinanceiro } = jqXhr;
            $(`#contaBancariaDropDownList option[value=${servicoFinanceiro.IdContaBancaria}]`).prop("selected", true);
            $(`#periodoCobrancaDropDownList option[value=${servicoFinanceiro.IdPeriodoCobranca}]`).prop("selected", true);
            $("#descricaoTextBox").val(servicoFinanceiro.DescricaoServico);
            $("#diaVencimentoTextBox").val(servicoFinanceiro.DiaVencimento);
            $("#valorCobrancaTextBox").val(servicoFinanceiro.ValorCobranca);
            $("#quantidadeParcelasTextBox").val(servicoFinanceiro.QuantidadeParcelas);
            $("#idServicoFinanceiroHidden").val(servicoFinanceiro.IdServicoFinanceiro);

            $("#servicoFinanceiroModal").modal("show");

        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        });

    }, 500)
}

function deletarServicoFinanceiroClick(idServicoFinanceiro) {
    exibirAlert.confirmar("Excluir serviço financeiro", function () {
        loading.bloquear();
        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: "/ServicoFinanceiro/Deletar?id=" + idServicoFinanceiro,
                accepts: "application/json",
                complete: function () {
                    loading.desbloquear();
                }
            }).done( async function (jqXhr) {
                exibirAlert.mensagemSucesso(jqXhr.mensagem);
                try {
                    await carregarServicosFinanceiro();
                } catch (err) {
                    exibirAlert.mensagemErroAjax(err);
                }
            }).fail(function (jqXhr) {
                exibirAlert.mensagemErroAjax(jqXhr);
            });
        }, 500);
    });
}