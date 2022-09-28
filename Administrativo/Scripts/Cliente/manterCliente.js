const clienteModel = {
    IdCliente: 0,
    IdEstado: 0,
    IdPais: 0,
    NomeEmpresa: "",
    Endereco: "",
    CodigoPostal: "",
    Observacao: "",
    DataCadastro: "",
    UltimaAtualizacao: null,
    Ativo: true,
};

$(document).ready(async function () {
    const idCliente = new URLSearchParams(window.location.search).get(
        "idCliente"
    );
    if (idCliente) {
        loading.bloquear();

        setTimeout(async function () {
            try {
                await carregarClienteViewModel(idCliente);
                await contato_ajax_obterVarios();
            } catch (err) {
                if (err.responseJSON != null) {
                    exibirAlert.mensagemErro(err.responseJSON.mensagem);
                } else {
                    exibirAlert.mensagemErro(err.responseText);
                }
            } finally {
                loading.desbloquear();
            }
        }, 500);
    } else {
        esconderListaDeContatos();
        carregarPais();
        $("input[name='clienteAtivo'][value=1]").prop("checked", true);
    }
});

//#region AJAX
function carregarPais() {
    loading.bloquear();

    $.ajax({
        type: "GET",
        accepts: "application/json",
        url: "/Pais/ObterPaises",
        success: function (jqXhr) {
            dropdownList.carregarDropDownList(
                "#paisDropDownList",
                jqXhr.paises,
                "(Selecione um País)"
            );

            $("#paisDropDownList").on("change", function () {
                const idPais = $("#paisDropDownList option:selected").val();
                carregarEstadosPais(idPais);
            });
        },
        error: function (jqXhr) {
            exibirAlert.mensagemErro(jqXhr.responseJSON.mensagem);
        },
        complete: function () {
            loading.desbloquear();
        },
    });
}

function carregarEstadosPais(idPais) {
    if (idPais != "0") {
        loading.bloquear();
        setTimeout(function () {
            $.ajax({
                url: "/Estado/ObterEstadosDoPais?idPais=" + idPais,
                type: "GET",
                accepts: "application/json",
                success: function (jqXhr) {
                    dropdownList.carregarDropDownList(
                        "#estadoDropDownList",
                        jqXhr.estados,
                        "(Selecione um Estado)"
                    );
                    $("#estadoDropDownList").removeAttr("disabled");
                },
                error: function (jqXhr) {
                    if (jqXhr.responseJSON)
                        exibirAlert.mensagemErro(jqXhr.responseJSON.mensagem);
                    else exibirAlert.mensagemErro(jqXhr.responseText);
                },
                complete: function () {
                    loading.desbloquear();
                },
            });
        }, 1000);
    } else {
        $("#estadoDropDownList").empty();

        $("#estadoDropDownList").attr("disabled", "disabled");
    }
}

function salvarCliente() {
    try {
        loading.bloquear();

        const cliente = clienteModel;

        cliente.NomeEmpresa = $("#nomeEmpresa").val();
        cliente.DataCadastro = $("#dataCadastro").val();
        cliente.IdPais = parseInt($("#paisDropDownList option:selected").val());
        cliente.IdEstado = parseInt($("#estadoDropDownList option:selected").val());
        cliente.CodigoPostal = $("#codigoPostal").val();
        cliente.Endereco = $("#endereco").val();
        cliente.Observacao = $("#observacao").val();

        if ($("input[name='clienteAtivo'][value=1]").prop("checked")) {
            cliente.Ativo = true;
        } else if ($("input[name='clienteAtivo'][value=0]").prop("checked")) {
            cliente.Ativo = false;
        }

        if ($("#idCliente")) {
            cliente.IdCliente = parseInt($("#idCliente").val());
        }

        $.ajax({
            type: "POST",
            url: "/Cliente/SalvarCliente",
            contentType: "application/json",
            data: JSON.stringify(cliente),
            success: function (jqXhr) {
                const clienteResponse = jqXhr.cliente;
                $("#idCliente").val(clienteResponse.IdCliente);
                exibirListaDeContatos();
                esconderMensagensErro();
                exibirAlert.mensagemSucesso(jqXhr.mensagem);
            },
            error: function (jqXhr) {
                if (jqXhr.status === 400) {
                    exibirMensagensErro(jqXhr.responseJSON.mensagens);
                } else if (jqXhr.responseJSON && jqXhr.responseJSON.mensagem != null) {
                    exibirAlert.mensagemErro(jqXhr.responseJSON.mensagem);
                } else {
                    exibirAlert.mensagemErro(jqXhr.responseText);
                }
                console.log(jqXhr);
            },
            complete: function () {
                loading.desbloquear();
            },
        });
    } catch (err) {
        exibirAlert.mensagemErro(err);
        loading.desbloquear();
    }
}
function carregarClienteViewModel(idCliente) {

    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Cliente/ObterClienteViewModel?idCliente=" + idCliente,
            type: "GET",
            accepts: "application/json",
            success: function (jqXhr) {
                try {
                    dropdownList.carregarDropDownList(
                        "#paisDropDownList",
                        jqXhr.paises,
                        "(Selecione um País)"
                    );

                    dropdownList.carregarDropDownList(
                        "#estadoDropDownList",
                        jqXhr.estados,
                        "(Selecione um Estado)"
                    );

                    $("#paisDropDownList").on("change", function () {
                        const idPais = $("#paisDropDownList option:selected").val();
                        carregarEstadosPais(idPais);
                    });

                    preencherFormularioCliente(jqXhr.cliente);

                    exibirListaDeContatos();

                    resolve();

                } catch (err) {
                    reject(err);
                }
            },
            error: function (jqXhr) {
                reject(jqXhr);
            },

        });
    });

}
//#endregion

function preencherFormularioCliente(cliente) {
    $("#idCliente").val(cliente.IdCliente);
    $("#nomeEmpresa").val(cliente.NomeEmpresa);
    $("#dataCadastro").val(cliente.DataCadastro);
    if (cliente.Ativo) {
        $("input[name='clienteAtivo'][value='1']").prop("checked", true);
    } else {
        $("input[name='clienteAtivo'][value='0']").prop("checked", true);
    }
    $(`#paisDropDownList option[value='${cliente.IdPais}']`).prop(
        "selected",
        true
    );
    $(`#estadoDropDownList option[value='${cliente.IdEstado}']`).prop(
        "selected",
        true
    );
    $("#estadoDropDownList").removeAttr("disabled");
    $("#codigoPostal").val(cliente.CodigoPostal);
    $("#endereco").val(cliente.Endereco);
    $("#observacao").val(cliente.Observacao);
}

function exibirMensagensErro(mensagens) {
    $("#mensagensErro").empty();

    const lista = $("<ul></ul>");

    mensagens.map((mensagem) => {
        lista.append($("<li></li>").text(mensagem));
    });

    $("#mensagensErro").append($(lista));

    $("#mensagensErro").css("visibility", "visible");
}
function esconderMensagensErro() {
    $("#mensagensErro").css("visibility", "hidden");

    $("#mensagensErro").empty();
}

//#region  CONTATOS
function exibirListaDeContatos() {
    $("#tabContatos").css("visibility", "visible");
    $("#liTabContatos").css("visibility", "visible");
}
function esconderListaDeContatos() {
    $("#tabContatos").css("visibility", "hidden");
    $("#liTabContatos").css("visibility", "hidden");
}


//#region CONTATO_AJAX
function contato_ajax_obterVarios() {
    const contatoFiltro = {
        nomeCompleto: "",
        email: "",
        celular: "",
        telefone: "",
        page: 1,
        pageSize: 10
    };

    contatoFiltro.nomeCompleto = $("#contato_filtro_nome").val();
    contatoFiltro.email = $("#contato_filtro_email").val();
    contatoFiltro.celular = $("#contao_filtro_celular").val();
    contatoFiltro.telefone = $("#contato_filtro_telfone").val();

    const parametros = new URLSearchParams(contatoFiltro).toString();

    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Contato/ObterVarios?" + parametros,
            accepts: "application/json"
        }).done(jqXhr => {
            try {
                contato_carregarTabela(jqXhr);
                resolve();
            } catch (err) {
                reject(err);
            }

        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}

function contato_ajax_obterPorId(idContato) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Contato/ObterPorId?idContato=" + idContato,
            accepts: "application/json"
        }).done(jqXhr => {
            resolve(jqXhr);
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}
function contato_ajax_editar(contato) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Contato/Editar",
            data: JSON.stringify(contato),
            accepts: "application/json",
            contentType: "application/json",
        }).done(jqXhr => {
            resolve(jqXhr);
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}
function contato_ajax_criar(contato) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Contato/Criar",
            data: JSON.stringify(contato),
            accepts: "application/json",
            contentType: "application/json",
        }).done(jqXhr => {
            resolve(jqXhr);
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}
function contato_ajax_deletar(idContato) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Contato/Deletar?idContato=" + idContato,
            accepts: "application/json"
        }).done(jqXhr => {
            resolve(jqXhr)
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    })
}
//#endregion

//#region CONTATO_DOM
function contato_carregarTabela(resposta) {

    const contatos = resposta.dados;
    const total = resposta.total;

    if (total <= 0) {
        $("#contato_mensagemVazio").css("display", "block");
        return;
    }

    $("#contato_total_registros").text(total);

    const table = $("#contatosTable > tbody");

    table.empty();

    contatos.map(contato => {

        const botaoEditar = $("<span style='cursor:pointer;'> <i class='fa fa-pencil'></i></span>")
            .unbind("click")
            .on("click", function () {
                contato_editarClick(contato.IdContato);
            });

        const botaoDeletar = $("<span style='cursor:pointer;'> <i class='fa fa-trash'></i><span>")
            .unbind("click")
            .on("click", function () {
                contato_excluirClick(contato.IdContato);
            });

        const tdOpcoes =
            $("<td></td>")
                .append(botaoEditar)
                .append("&nbsp;&nbsp;")
                .append(botaoDeletar);

        let trHtml =
            `<tr>
                <td>${contato.NomeCompleto}</td>
                <td>${contato.Email}</td>
                <td>${contato.Celular}</td>
                <td>${contato.Telefone}</td>
            </tr>`;
        table.append($(trHtml).append(tdOpcoes));
    });

    $("#contato_tabelaContainer").css("display", "block");
}

async function contato_editarClick(idContato) {
    loading.bloquear();
    contato_modal_limparMensagensErro();
    contato_modal_limparMensagensSucesso();

    setTimeout(async function () {
        try {
            let resultado = await contato_ajax_obterPorId(idContato);

            const contato = resultado.dados;

            $("#contato_modal_idContato").val(contato.IdContato);
            $("#contato_modal_idCliente").val(contato.IdCliente);
            $("#contato_modal_nome").val(contato.NomeCompleto);
            $("#contato_modal_email").val(contato.Email);
            $("#contato_modal_celular").val(contato.Celular);
            $("#contato_modal_telefone").val(contato.Telefone);
            $("#contato_modal_senha").val(contato.Senha);
            $("#contatoModal").modal("show");

        } catch (err) {
            exibirAlert.mensagemErro("Ocorreu um erro");
            console.log(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);
}
function contato_excluirClick(idContato) {

    exibirAlert.confirmar("Excluir contato", function () {
        loading.bloquear();
        setTimeout(function () {
            contato_ajax_deletar(idContato)
                .then(async result => {
                    await contato_ajax_obterVarios();
                    exibirAlert.mensagemSucesso("Contato excluído com sucesso!")
                })
                .catch(reason => {
                    exibirMensagemErro(reason);
                })
                .finally(() => {
                    loading.desbloquear();
                });
        }, 500);
    })
}

function contato_novoButtonClick() {

    contato_modal_limparMensagensErro();
    contato_modal_limparMensagensSucesso();

    $("#contato_modal_idContato").val("");
    $("#contato_modal_idCliente").val($('#idCliente').val());
    $("#contato_modal_nome").val("");
    $("#contato_modal_email").val("");
    $("#contato_modal_celular").val("");
    $("#contato_modal_telefone").val("");
    $("#contato_modal_senha").val("");

    $("#contatoModal").modal("show");

}

function contato_modal_salvarButtonClick() {

    loading.bloquear();

    const contato = {
        IdContato: 0,
        IdCliente: 0,
        NomeCompleto: "",
        Email: "",
        Celular: "",
        Telefone: "",
        Senha: ""
    };

    if (!isNaN(parseInt($("#contato_modal_idContato").val()))) {
        contato.IdContato = parseInt($("#contato_modal_idContato").val());
    }

    contato.IdCliente = parseInt($("#contato_modal_idCliente").val());
    contato.NomeCompleto = $("#contato_modal_nome").val();
    contato.Email = $("#contato_modal_email").val();
    contato.Celular = $("#contato_modal_celular").val();
    contato.Telefone = $("#contato_modal_telefone").val();
    contato.Senha = $("#contato_modal_senha").val();

    setTimeout(async function () {
        try {
            let resultado = {};

            if (contato.IdContato > 0) {
                resultado = await contato_ajax_editar(contato);

            } else {
                resultado = await contato_ajax_criar(contato);
            }

            await contato_ajax_obterVarios();

            $("#contato_modal_mensagemSucesso")
                .text(resultado.mensagemSucesso)
                .css("display", "block");

            contato_modal_limparMensagensErro();

        } catch (err) {
            contato_modal_exibirMensagensErro(err);
        } finally {
            loading.desbloquear();
        }

    }, 500);
}


function contato_modal_exibirMensagensErro(err) {

    contato_modal_limparMensagensErro();

    const { erros } = err.responseJSON;

    if (erros) {
        const ul = $("<ul></ul>");

        erros.map(erro => {
            ul.append(`<li>${erro}</li>`)
        });

        $("#contato_modal_mensagemErro").append(ul);

        $("#contato_modal_mensagemErro").css("display", "block");

        contato_modal_limparMensagensSucesso();
    }
}
function contato_modal_limparMensagensErro() {
    $("#contato_modal_mensagemErro").empty();
    $("#contato_modal_mensagemErro").css("display", "none");
}
function contato_modal_limparMensagensSucesso() {
    $("#contato_modal_mensagemSucesso").empty();
    $("#contato_modal_mensagemSucesso").css("display", "none");
}

function contato_limparFiltroClick() {
    loading.bloquear();

    $("#contato_filtro_nome").val("");
    $("#contato_filtro_email").val("");
    $("#contao_filtro_celular").val("");
    $("#contato_filtro_telfone").val("");

    setTimeout(function () {
        contato_ajax_obterVarios()
            .then(result => {
            })
            .catch(reasonn => {
                exibirMensagemErro(reason);
            })
            .finally(() => {
                loading.desbloquear();
            })
    }, 500);
}
function contato_filtroClick() {
    loading.bloquear();

    setTimeout(function () {
        contato_ajax_obterVarios()
            .then(result => {

            })
            .catch(reason => {
                exibirMensagemErro(reason);
            })
            .finally(() => {
                loading.desbloquear();
            })
    }, 500);
}
function exibirMensagemErro(reason) {
    let mensagem = "Ocorreu algum erro"
    if (reason.responseJSON) {
        mensagem = reason.responseJSON.erros.join(", ");
    }

    exibirAlert.mensagemErro(mensagem);
}
//#endregion CONTATO_DOM


//#endregion
