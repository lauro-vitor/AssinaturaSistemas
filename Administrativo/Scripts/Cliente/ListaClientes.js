

$(document).ready(async function () {
    loading.bloquear();

    setTimeout(async function () {
        try {
            await carregarPaises();
            await carregarClientes();
        } catch (err) {
            if (err.responseJSON)
                exibirAlert.mensagemErro(err.responseJSON.mensagem);
            else
                exibirAlert.mensagemErro(err.responseText);
        } finally {
            loading.desbloquear();
        }
    }, 500);

});

function carregarClientes() {

    return new Promise(function (resolve, reject) {
        let idPaisSelected = 0;

        if (!isNaN($('#paisDropDownList option:selected').val())) {
            idPaisSelected = parseInt($('#paisDropDownList option:selected').val());
        }
        let idEstadoSelected = 0;

        if (!isNaN($('#estadoDropDownList  option:selected').val())) {
            idEstadoSelected = parseInt($('#estadoDropDownList option:selected').val());
        }

        const clientesViewModelFiltro = {
            nomeEmpresa: $('#nomeEmpresa').val(),
            idPais: idPaisSelected,
            idEstado: idEstadoSelected,
            codigoPostal: $('#codigoPostal').val(),
            endereco: $('#endereco').val(),
            dataCadastroInicial: $('#dataCadastroInicial').val(),
            dataCadastroFinal: $('#dataCadastroFinal').val(),
            ativo: null
        };

        const parametros = new URLSearchParams(clientesViewModelFiltro).toString();

        $("#pagination").pagination({
            dataSource: "/Cliente/ObterListaDeClientesViewModel?" + parametros,
            locator: "clientes",
            totalNumberLocator: function (response) {
                $("#totalSpan").text(response.total);
                return response.total;
            },
            pageSize: 10,
            callback: function (data, pagination) {
                carregarTabelaClientes(data);
                resolve();
            },
            formatAjaxError: function (jqXHR, textStatus, errorThrown) {
                reject(jqXHR);
            },
        })
    });
}


function carregarPaises() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Pais/ObterPaises",
            accepts: "application/json",
            success: function (jqXhr) {
                dropdownList.carregarDropDownList("#paisDropDownList", jqXhr.paises, "(Selecione um País)");
                $("#paisDropDownList").on("change", function () {
                    let idPais = $("#paisDropDownList option:selected").val();
                    carregarEstadosPais(idPais);
                });

                resolve();
            },
            error: function (jqXhr) {
                reject(jqXhr);
            }
        })
    });
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
        }, 500);
    } else {
        $('#estadoDropDownList').empty();

        $('#estadoDropDownList').attr('disabled', 'disabled');
    }
}

function deletarCliente(idCliente) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Cliente/DeletarCliente",
            contentType: "application/json",
            data: JSON.stringify({ idCliente }),
            success: function (jqXhr) {
                resolve();
            },
            error: function (jqXhr) {
                reject(jqXhr);
            },
        });
    });
}

function carregarTabelaClientes(clientes) {
    const table = $('#clientesTable > tbody');

    table.empty();

    clientes.map(c => {
        const elementoAlterar = $("<span style='cursor:pointer'></span>")
            .append($("<i class='fa fa-pencil'></i>"))
            .unbind("click")
            .on("click", function () {
                window.open('/Cliente/Manter?idCliente=' + c.IdCliente, '_blank');

            });

        const elementoExcluir = $("<span style='cursor:pointer'></span>")
            .append($("<i class='fa fa-trash'></i>"))
            .unbind("click")
            .on("click", function () {
                exibirAlert.confirmar('Esta ação irá excluir o cliente', function () {
                    loading.bloquear();

                    setTimeout( async function () {
                        try {
                            await deletarCliente(c.IdCliente);
                            await carregarClientes();
                            exibirAlert.mensagemSucesso("Cliente excluído com sucesso!");
                        } catch (err) {
                            if (err.responseJSON)
                                exibirAlert.mensagemErro(err.responseJSON.mensagem);
                            else
                                exibirAlert.mensagemErro(err.responseText);
                        } finally {
                            loading.desbloquear();
                        }
                    }, 500);
                });
            });

        const td = $("<td></td>")
            .append(elementoAlterar)
            .append("&nbsp;&nbsp;")
            .append(elementoExcluir);

        let iconAtivo = "";

        if (c.Ativo) {
            iconAtivo = "<i class='fa-solid fa-check' style='color:green;'></i>"
        } else {
            iconAtivo = "<i class='fa-solid fa-check' style='color:red;'></i>";
        }

        const tr = $(`
            <tr>
                <td>${c.NomeEmpresa}</td>
                <td>${c.NomePais}</td>
                <td>${c.NomeEstado}</td>
                <td>${c.CodigoPostal}</td>
                <td>${c.Endereco}</td>
                <td>${c.DataCadastro}</td>
                <td>${c.UltimaAtualizacao}</td>
                <td>${iconAtivo}</td>
            </tr>`);

        tr.append(td);
        table.append(tr);

    });
}



function filtrar() {
    loading.bloquear();

    setTimeout(async function () {
        try {
            await carregarClientes();
        } catch (err) {
            if (err.responseJSON)
                exibirAlert.mensagemErro(err.responseJSON.mensagem);
            else
                exibirAlert.mensagemErro(err.responseText);
        } finally {
            loading.desbloquear();
        }
    }, 500);
};
function limparFiltro() {
    $("#paisDropDownList option[value='0']").attr("selected", "selected");
    $('#nomeEmpresa').val("");
    $('#codigoPostal').val("");
    $('#endereco').val("");
    $('#dataCadastroInicial').val("");
    $('#dataCadastroFinal').val("");

    $("#estadoDropDownList option[value='0']").attr("selected", "selected");
    $("#estadoDropDownList").empty();
    $("#estadoDropDownList").attr("disabled", true);

    loading.bloquear();

    setTimeout(async function () {
        try {
            await carregarPaises();
            await carregarClientes();
        } catch (err) {
            if (err.responseJSON)
                exibirAlert.mensagemErro(err.responseJSON.mensagem);
            else
                exibirAlert.mensagemErro(err.responseText);
        } finally {
            loading.desbloquear();
        }
    }, 500);
}



