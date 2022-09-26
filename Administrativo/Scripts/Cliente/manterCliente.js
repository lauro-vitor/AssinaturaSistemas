
const clienteModel = {
    IdCliente: 0,
    IdEstado: 0,
    IdPais: 0,
    NomeEmpresa: '',
    Endereco: '',
    CodigoPostal: '',
    Observacao: '',
    DataCadastro: '',
    UltimaAtualizacao: null,
    Ativo: true,
};


$(document).ready(function () {
    const idCliente = new URLSearchParams(window.location.search).get('idCliente');
    if (idCliente) {
        carregarClienteViewModel(idCliente);
    } else {
        carregarPais();
        $("input[name='clienteAtivo'][value=1]").prop('checked', true);
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

            dropdownList.carregarDropDownList('#paisDropDownList', jqXhr.paises, '(Selecione um País)');

            $('#paisDropDownList').on("change", function () {
                const idPais = $('#paisDropDownList option:selected').val();
                carregarEstadosPais(idPais);
            });
        },
        error: function (jqXhr) {
            exibirAlert.mensagemErro(jqXhr.responseJSON.mensagem);
        },
        complete: function () {
            loading.desbloquear();
        }
    })

}

function carregarEstadosPais(idPais) {

    if (idPais != "0") {
        loading.bloquear();
        setTimeout(function () {
            $.ajax({
                url: '/Estado/ObterEstadosDoPais?idPais=' + idPais,
                type: "GET",
                accepts: "application/json",
                success: function (jqXhr) {
                    dropdownList.carregarDropDownList('#estadoDropDownList', jqXhr.estados, '(Selecione um Estado)');
                    $('#estadoDropDownList').removeAttr('disabled');
                },
                error: function (jqXhr) {

                    if (jqXhr.responseJSON)
                        exibirAlert.mensagemErro(jqXhr.responseJSON.mensagem);
                    else
                        exibirAlert.mensagemErro(jqXhr.responseText);
                },
                complete: function () {
                    loading.desbloquear();
                }
            });
        }, 1000);
    } else {
        $('#estadoDropDownList').empty();

        $('#estadoDropDownList').attr('disabled', 'disabled');
    }

}

function salvarCliente() {
    try {
        loading.bloquear();

        const cliente = clienteModel;

        cliente.NomeEmpresa = $('#nomeEmpresa').val();
        cliente.DataCadastro = $('#dataCadastro').val();
        cliente.IdPais = parseInt($('#paisDropDownList option:selected').val());
        cliente.IdEstado = parseInt($('#estadoDropDownList option:selected').val());
        cliente.CodigoPostal = $('#codigoPostal').val();
        cliente.Endereco = $('#endereco').val();
        cliente.Observacao = $('#observacao').val();

        if ($("input[name='clienteAtivo'][value=1]").prop('checked')) {
            cliente.Ativo = true;
        } else if ($("input[name='clienteAtivo'][value=0]").prop('checked')) {
            cliente.Ativo = false;
        }

        if ($('#idCliente')) {
            cliente.IdCliente = parseInt($('#idCliente').val());
        }

        $.ajax({
            type: "POST",
            url: "/Cliente/SalvarCliente",
            contentType: "application/json",
            data: JSON.stringify(cliente),
            success: function (jqXhr) {

                const clienteResponse = jqXhr.cliente;
                $('#idCliente').val(clienteResponse.IdCliente);
                exibirAlert.mensagemSucesso(jqXhr.mensagem);
                esconderMensagensErro();
            },
            error: function (jqXhr) {
                if (jqXhr.status === 400) {
                    exibirMensagensErro(jqXhr.responseJSON.mensagens);

                } else if (jqXhr.responseJSON && (jqXhr.responseJSON.mensagem != null)) {
                    exibirAlert.mensagemErro(jqXhr.responseJSON.mensagem);
                } else {
                    exibirAlert.mensagemErro(jqXhr.responseText);
                }
                console.log(jqXhr);
            },
            complete: function () {
                loading.desbloquear();
            }
        });

    } catch (err) {
        exibirAlert.mensagemErro(err);
        loading.desbloquear();
    }
}
function carregarClienteViewModel(idCliente) {
    loading.bloquear();
    setTimeout(function () {
        $.ajax({
            url: "/Cliente/ObterClienteViewModel?idCliente=" + idCliente,
            type: "GET",
            accepts: "application/json",
            success: function (jqXhr) {
                try {
                    dropdownList.carregarDropDownList("#paisDropDownList", jqXhr.paises, "(Selecione um País)");

                    dropdownList.carregarDropDownList("#estadoDropDownList", jqXhr.estados, "(Selecione um Estado)");

                    $('#paisDropDownList').on("change", function () {
                        const idPais = $('#paisDropDownList option:selected').val();
                        carregarEstadosPais(idPais);
                    });

                    preencherFormularioCliente(jqXhr.cliente);

                } catch (err) {
                    exibirAlert.mensagemErro(err);
                }
            },
            error: function (jqXhr) {
                if (jqXhr.responseJSON != null) {
                    exibirAlert.mensagemErro(jqXhr.responseJSON.mensagem);
                } else {
                    exibirAlert.mensagemErro(jqXhr.responseText);
                }
            },
            complete: function () {
                loading.desbloquear();
            }
        })
    }, 1000);
}
//#endregion



function preencherFormularioCliente(cliente) {
    $('#idCliente').val(cliente.IdCliente);
    $('#nomeEmpresa').val(cliente.NomeEmpresa);
    $('#dataCadastro').val(cliente.DataCadastro);
    if (cliente.Ativo) {
        $("input[name='clienteAtivo'][value='1']").prop("checked", true);
    } else {
        $("input[name='clienteAtivo'][value='0']").prop("checked", true);
    }
    $(`#paisDropDownList option[value='${cliente.IdPais}']`).prop("selected", true);
    $(`#estadoDropDownList option[value='${cliente.IdEstado}']`).prop("selected", true);
    $('#estadoDropDownList').removeAttr("disabled");
    $('#codigoPostal').val(cliente.CodigoPostal);
    $('#endereco').val(cliente.Endereco);
    $('#observacao').val(cliente.Observacao);

}

function exibirMensagensErro(mensagens) {

    $('#mensagensErro').empty();

    const lista = $("<ul></ul>");

    mensagens.map(mensagem => {
        lista.append($("<li></li>").text(mensagem));
    });

    $('#mensagensErro').append($(lista));

    $('#mensagensErro').css("visibility", "visible");
}
function esconderMensagensErro() {
    $('#mensagensErro').css("visibility", "hidden");

    $('#mensagensErro').empty();
}


