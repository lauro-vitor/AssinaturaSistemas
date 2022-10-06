$(document).ready(async function () {

    loading.bloquear();

    setTimeout(async function () {
        try {
            await carregarSistemas();
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);
});


function carregarSistemas() {
    const filtro = {
        idCliente: "0",
        idTipoSistema: "0",
        dominioProvisorio: $("#sistema_filtro_dominioProvisorio").val(),
        dominio: $("#sistema_filtro_dominio").val(),
        ativo: null,
        periodoInicialDataInicio: $("#sistema_filtro_periodoInicial_dataInicio").val(),
        periodoFinalDataInicio: $("#sistema_filtro_periodoFinal_dataInicio").val(),
        periodoInicialDataCancelamento: $("#sistema_filtro_periodoInicial_dataCancelamento").val(),
        periodoFinalDataCancelamento: $("#sistema_filtro_periodoFinal_dataCancelamento").val()
    };

    let idCliente = $("#sistema_filtro_cliente option:selected").val();

    let idTipoSistema = $("#sistema_filtro_tipoSistema option:selected").val();

    if (idCliente)
        filtro.idCliente = idCliente;

    if (idTipoSistema)
        filtro.idTipoSistema = idTipoSistema;

    const parametros = new URLSearchParams(filtro);

    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Sistema/ObterVarios?" + parametros,
            accepts: "application/json"
        }).done(jqXhr => {
            const { sistemas, total, clientes, tiposSistema } = jqXhr;

            dropdownList.carregarDropDownList("#sistema_filtro_cliente", clientes, "(Selecione um cliente)");
            dropdownList.carregarDropDownList("#sistema_filtro_tipoSistema", tiposSistema, "(Selecione um tipo sistema)");

            const table = $("#sistema_tabela > tbody");

            table.empty();

            if (total <= 0) {
                $("#sistema_tabela_wrapper").css("display", "none");
                $("#sistema_tablela_wrapper_vazio").css("display", "block");
                resolve();
            } else {
                $("#sistema_tabela_wrapper").css("display", "block");
                $("#sistema_tablela_wrapper_vazio").css("display", "none");
            }


            $("#totalSpan").text(total);

            sistemas.map(s => {

                let corSistemaAtivo = s.Ativo ? "green" : "red";
                const tr = $(` <tr>
                                    <td>${s.ClienteNomeEmpresa}</td>
                                    <td>${s.TipoSistemaDescricao}</td>
                                    <td>${s.Dominio}</td>
                                    <td>${s.DominioProvisorio}</td>
                                    <td>${s.DataInicio}</td>
                                    <td>${s.DataCancelamento}</td>
                                    <td>
                                        <i class="fa fa-check" style="color:${corSistemaAtivo}"></i>
                                    </td>
                                    <td>
                                        <span style="cursor:pointer" onclick="irParaFicharFinanceira('${s.IdSistema}')">
                                           <i class="fa fa-money-check-dollar"></i>
                                        </spn>
                                    </td>
                                </tr>`);

                table.append(tr);
            });

            resolve();
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}

function irParaFicharFinanceira(id) {
    window.open("/FichaFinanceira/manter?id=" + id, "_self");
}

function filtrarSistemasClick() {
    loading.bloquear();

    setTimeout(async function () {
        try {
            await carregarSistemas();
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }

    }, 500);
};


function limparFiltroSistemasClick() {
    loading.bloquear();

    $("#sistema_filtro_cliente [value=0]").prop("selected", true);
    $("#sistema_filtro_tipoSistema [value=0]").prop("selected", true);
    $("#sistema_filtro_dominioProvisorio").val("");
    $("#sistema_filtro_dominio").val("");
    $("#sistema_filtro_periodoInicial_dataInicio").val("");
    $("#sistema_filtro_periodoFinal_dataInicio").val("");
    $("#sistema_filtro_periodoInicial_dataCancelamento").val("");
    $("#sistema_filtro_periodoFinal_dataCancelamento").val("");

    setTimeout(async function () {
        try {
            await carregarSistemas();
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }

    }, 500);
};