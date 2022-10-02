const contaBancaria = {
    IdContaBancaria: 0,
    NumeroAgencia: "",
    NumeroConta: "",
    NumeroBanco: 0,
    NomeBanco: "",
    Cnpj: ""
};

$(document).ready(function () {

    loading.bloquear();

    setTimeout(async function () {
        try {
            await carregarContasBancarias();
        } catch (err) {
            console.log(err);
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);
});


function carregarContasBancarias() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            accepts: "application/json",
            url: "/ContaBancaria/ObterVarios"
        }).done(jqXhr => {

            try {
                const contas = jqXhr.contasBancarias;

                const total = jqXhr.total;

                $("#totalSpan").text(total);

                if (total <= 0) {
                    $("#conta_tabela_container").css("display", "none");
                    $("#conta_tabela_container_vazio").css("display", "block");
                    resolve();
                    return;
                }

                const table = $("#conta_table > tbody");

                table.empty();

                contas.map(c => {
                    const tr = $(`
                    <tr>
                        <td>${c.NumeroConta}</td>
                        <td>${c.NumeroAgencia}</td>
                        <td>${c.NumeroBanco}</td>
                        <td>${c.NomeBanco}</td>
                        <td>${c.Cnpj}</td>
                        <td>
                            <span style="cursor:pointer" onclick="editarContaBancariaClick(${c.IdContaBancaria})">
                                <i class="fa fa-pencil"></i>
                            </span>
                            &nbsp;&nbsp;
                            <span style="cursor:pointer" onclick="excluirContaBancariaClick(${c.IdContaBancaria})">
                                <li class="fa fa-trash"></li>
                            </span>
                        </td>
                    </tr>
                `);
                    table.append(tr);
                });

                $("#conta_tabela_container").css("display", "block");

                $("#conta_tabela_container_vazio").css("display", "none");

                resolve();

            } catch (err) {
                reject(err);
            }

        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}

function salvarContaBancaria() {
    loading.bloquear();

    contaBancaria.NumeroConta = $("#numeroContaTextBox").val();
    contaBancaria.NumeroAgencia = $("#numeroAgenciaTextBox").val();
    contaBancaria.NumeroBanco = parseInt($("#numeroBancoTextBox").val());
    contaBancaria.NomeBanco = $("#nomeBancoTextBox").val();
    contaBancaria.Cnpj = $("#cnpjTextBox").val();
    let idContaHidden = parseInt($("#idContaHidden").val());
    let url = "";

    if (!isNaN(idContaHidden)) {
        contaBancaria.IdContaBancaria = idContaHidden;
        url = "/ContaBancaria/Editar";
    } else {
        url = "/ContaBancaria/Criar";
    }

    setTimeout(function () {
        $.ajax({
            url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(contaBancaria)
        }).done(async jqXhr => {

            try {
                await carregarContasBancarias();
                exibirAlert.mensagemSucesso(jqXhr.mensagem);
            } catch (err) {
                exibirAlert.mensagemErro(err);
            } finally {
                loading.desbloquear();
            }

        }).fail(jqXHr => {
            exibirAlert.mensagemErroAjax(jqXHr);
            loading.desbloquear();
        })

    }, 500);
}

function novoContaBancariaClick() {
    $("#idContaHidden").val("");
    $("#numeroContaTextBox").val("");
    $("#numeroAgenciaTextBox").val("");
    $("#numeroBancoTextBox").val("");
    $("#nomeBancoTextBox").val("");
    $("#cnpjTextBox").val("");

    $("#contaBancariaModal").modal("show");
}

function editarContaBancariaClick(idConta) {
    loading.bloquear();
    setTimeout(function () {
        $.ajax({
            type: "GET",
            accepts: "application/json",
            url: "/ContaBancaria/ObterPorId?id=" + idConta,
            complete: () => {
                loading.desbloquear();
            }
        }).done(jqXhr => {

            let { contaBancariaRetorno } = jqXhr;
            $("#idContaHidden").val(contaBancariaRetorno.IdContaBancaria);
            $("#numeroContaTextBox").val(contaBancariaRetorno.NumeroConta);
            $("#numeroAgenciaTextBox").val(contaBancariaRetorno.NumeroAgencia);
            $("#numeroBancoTextBox").val(contaBancariaRetorno.NumeroBanco);
            $("#nomeBancoTextBox").val(contaBancariaRetorno.NomeBanco);
            $("#cnpjTextBox").val(contaBancariaRetorno.Cnpj);
            $("#contaBancariaModal").modal("show");

        }).fail(jqXhr => {
            exibirAlert.mensagemErroAjax(jqXhr);
        });

    }, 500);
}

function excluirContaBancariaClick(idConta) {
    exibirAlert.confirmar("Excluir conta bancária", function () {
        loading.bloquear();

        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: "/ContaBancaria/Deletar?id=" + idConta,
                accepts: "application/json",
                complete: () => {
                    loading.desbloquear();
                }
            }).done(async jqXhr => {
                try {
                    let { mensagem } = jqXhr;
                    exibirAlert.mensagemSucesso(mensagem);
                    await carregarContasBancarias();
                } catch (err) {
                    exibirAlert.mensagemErro(err);
                }
            }).fail(jqXhr => {
                exibirAlert.mensagemErroAjax(jqXhr);
            });

        }, 500);
    });
}