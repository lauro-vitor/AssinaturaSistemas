$(document).ready(async function () {

    loading.bloquear();

    setTimeout(async function () {
        try {
            await ajaxSistemas.obterListaSistemasViewModel();
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);
});

const ajaxSistemas = {
    obterListaSistemasViewModel: function () {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: "GET",
                accepts: "application/json",
                url: "/Sistema/ObterListaSistemasViewModel",
            }).done(jqXhr => {
                dropdownList.carregarDropDownList("#sistema_filtro_cliente", jqXhr.clientes, "(Selecione um cliente)");
                dropdownList.carregarDropDownList("#sistema_filtro_tipoSistema", jqXhr.tiposSistema, "(Selecione um tipo sistema)");
                domSistemas.exibirTabela(jqXhr.sistemas, jqXhr.total);
                resolve();
            }).fail(jqXhr => {
                reject(jqXhr);
            });
        })
    },

    obterSistemas: function () {

        const filtro = {
            idCliente: $("#sistema_filtro_cliente option:selected").val(),
            idTipoSistema: $("#sistema_filtro_tipoSistema option:selected").val(),
            dominioProvisorio: $("#sistema_filtro_dominioProvisorio").val(),
            dominio: $("#sistema_filtro_dominio").val(),
            ativo: null,
            periodoInicialDataInicio: $("#sistema_filtro_periodoInicial_dataInicio").val(),
            periodoFinalDataInicio: $("#sistema_filtro_periodoFinal_dataInicio").val(),
            periodoInicialDataCancelamento: $("#sistema_filtro_periodoInicial_dataCancelamento").val(),
            periodoFinalDataCancelamento: $("#sistema_filtro_periodoFinal_dataCancelamento").val()
        };

        const parametros = new URLSearchParams(filtro);

        return new Promise(function (resolve, reject) {

            $.ajax({
                type: "GET",
                url: "/Sistema/ObterVarios?" + parametros,
                accepts: "application/json"
            }).done(jqXhr => {
                domSistemas.exibirTabela(jqXhr.sistemas, jqXhr.total);
                resolve();
            }).fail(jqXhr => {
                reject(jqXhr);
            });
        });
    },

    deletarSistema: function (id) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: "/Sistema/Deletar?id=" + id,
            }).done(jqXhr => {
                exibirAlert.mensagemSucesso("Sistema excluído com sucesso");
                resolve();
            }).fail(jqXhr => {
                reject(jqXhr);
            })
        });
    }

}


const domSistemas = {
    exibirTabela: function (sistemas, total) {
        const table = $("#sistema_tabela > tbody");

        table.empty();

        if (total <= 0) {
            $("#sistema_tabela_wrapper").css("display", "none");
            $("#sistema_tablela_wrapper_vazio").css("display", "block");
            resolve();
            return;
        }

        $("#sistema_tabela_wrapper").css("display", "block");
        $("#sistema_tablela_wrapper_vazio").css("display", "none");
        $("#totalSpan").text(total);

        sistemas.map(s => {

            let corSistemaAtivo = s.Ativo ? "green" : "red";
            const tr = $(`
            <tr>
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
                    <span style="cursor:pointer;" onclick="editarSistemaClick('${s.IdSistema}');">
                        <i class="fa fa-pencil"></i>
                    </span>
                        &nbsp;&nbsp;
                    <span style="cursor:pointer;" onclick="deletarSistemaClick('${s.IdSistema}')">
                        <i class="fa fa-trash"></i>
                    </span>
                </td>
            </tr>`);

            table.append(tr);
        });
    },


}

function editarSistemaClick(idSistema) {
    window.open("/Sistema/manter?idSistema=" + idSistema, "_blank");
};

function deletarSistemaClick(idSistema) {
    exibirAlert.confirmar("Excluir sistema", function () {

        loading.bloquear();

        setTimeout(async function () {
            try {
                await ajaxSistemas.deletarSistema(idSistema);
                await ajaxSistemas.obterSistemas();
            } catch (err) {
                exibirAlert.mensagemErroAjax(err);
            } finally {
                loading.desbloquear();
            }
        }, 500);

    });

};

function filtrarSistemasClick() {
    loading.bloquear();

    setTimeout(async function () {
        try {
            await ajaxSistemas.obterSistemas();
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
            await ajaxSistemas.obterSistemas();
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }

    }, 500);
};