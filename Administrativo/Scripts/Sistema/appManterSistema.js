const Sistema = {
    IdSistema: 0,
    IdTipoSistema: 0,
    IdCliente: 0,
    DominioProvisorio: "",
    Dominio: "",
    Pasta: "",
    BancoDeDados: "",
    Ativo: true,
    DataInicio: "",
    DataCancelamento: null
};

$(document).ready(async function () {
    loading.bloquear();

    setTimeout(async function () {
        try {

            await carregarClientesDropDownList();

            await carregarTipoSistemaDropDownList();

            const idSistema = new URLSearchParams(window.location.search).get("idSistema");

            if (idSistema) {
                await recuperarSistema(idSistema);
            } else {
                $("#ativoCheckBox").prop("checked", true);
            }

        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);
});


function carregarClientesDropDownList() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Cliente/ObterClientesDropDownList",
            accepts: "application/json"
        }).done(jqXhr => {
            dropdownList.carregarDropDownList("#clienteDropDownList", jqXhr.clientes, "(Selecione um cliente)");
            resolve();
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}
function carregarTipoSistemaDropDownList() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Sistema/ObterTiposSistemaDropDownList",
            accepts: "application/json"
        }).done(jqXhr => {
            dropdownList.carregarDropDownList("#tipoSistemaDropDownlist", jqXhr.tiposSistema, "(Selecione um tipo de sistema)");
            resolve();
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}

function carregarSistema(sistema) {
    $("#idSistemaHidden").val(sistema.IdSistema);
    $(`#clienteDropDownList option[value=${sistema.IdCliente}]`).prop("selected", true);
    $(`#tipoSistemaDropDownlist option[value=${sistema.IdTipoSistema}]`).prop("selected", true);
    $("#dominioTextBox").val(sistema.Dominio);
    $("#bancoDeDadosTextBox").val(sistema.BancoDeDados);
    $("#pastaTextBox").val(sistema.Pasta);
    $("#dataInicioTextBox").val(sistema.DataInicioVM);
    $("#dataCancelamentoTextBox").val(sistema.DataCancelamentoVM);
    $("#dominioProvisorioTextBox").val(sistema.DominioProvisorio);

    if (sistema.Ativo) {
        $("#ativoCheckBox").prop("checked", true);
    } else {
        $("#ativoCheckBox").prop("checked", false);
    }

}
function recuperarSistema(idSistema) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Sistema/ObterPorId?id="+idSistema,
            contentType: "application/json"
        }).done(jqXhr => {
            carregarSistema(jqXhr.sistema);
            resolve();
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}
function salvarSistema(sistema) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Sistema/Criar",
            contentType: "application/json",
            data: JSON.stringify(sistema),
        }).done(jqXhr => {
            carregarSistema(jqXhr.sistema);
            exibirAlert.mensagemSucesso(jqXhr.mensagem);
            resolve();
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}
function editarSistema(sistema) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Sistema/Editar",
            contentType: "application/json",
            data: JSON.stringify(sistema),
        }).done(jqXhr => {
            carregarSistema(jqXhr.sistema);
            exibirAlert.mensagemSucesso(jqXhr.mensagem);
            resolve();
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}

function salvarSistemaClick() {

    loading.bloquear();

    let idSistema = parseInt($("#idSistemaHidden").val());

    if (!isNaN(idSistema))
        Sistema.IdSistema = idSistema;

    Sistema.IdCliente = $("#clienteDropDownList option:selected").val();
    Sistema.IdTipoSistema = $("#tipoSistemaDropDownlist option:selected").val();
    Sistema.Dominio = $("#dominioTextBox").val();
    Sistema.Pasta = $("#pastaTextBox").val();
    Sistema.BancoDeDados = $("#bancoDeDadosTextBox").val();
    Sistema.DataInicio = $("#dataInicioTextBox").val();
    Sistema.DataCancelamento = $("#dataCancelamentoTextBox").val();
    Sistema.DominioProvisorio = $("#dominioProvisorioTextBox").val();
    Sistema.Ativo = $("#ativoCheckBox").prop("checked");
  
   
    setTimeout(async function () {
        try {
            if (Sistema.IdSistema <= 0) {
                await salvarSistema(Sistema);
            } else {
                await editarSistema(Sistema);
            }
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);


}