
const loading = {
    bloquear: function () {
        $('body').waitMe({
            effect: 'roundBounce',
            text: 'Carregando ...',
            bg: 'rgba(255, 255, 255, 0.9)',
            color: '#000',
            maxSize: '',
            waitTime: -1,
            textPos: 'vertical',
            fontSize: '',
            source: '',
            onClose: function () { }
        });
    },
    desbloquear: function () {
        $('body').waitMe('hide');
    }
}

const exibirAlert = {
    mensagemErro: function (mensagem) {
        Swal.fire({
            icon: 'error',
            title: 'Oops ...',
            text: mensagem
        });
    },
    mensagemErroAjax: function (jqXhr) {

        let mensagem = "Ocorreu algum erro";

        if (jqXhr.responseJSON) {
            mensagem = jqXhr.responseJSON.erros.join("<br/>");
        } else if (jqXhr) {
            mensagem = jqXhr.responseText;
        }

        Swal.fire({
            icon: 'error',
            title: 'Oops ...',
            html: mensagem
        });
    },
    mensagemSucesso: function (mensagem) {
        Swal.fire({
            icon: 'success',
            title: 'Ok',
            text: mensagem
        });
    },
    confirmar: function (texto, callbackAposConfirmacao) {

        Swal.fire({
            title: 'Tem certeza ?',
            text: texto,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                callbackAposConfirmacao();
            }
        })
    }
}

const dropdownList = {
    carregarDropDownList: function (idDropDownlist, selectListItem, mensagemSelecione) {
        $(idDropDownlist).empty();

        const primeiroOpt = $('<option></option>').val(0).text(mensagemSelecione);

        $(idDropDownlist).append(primeiroOpt);

        selectListItem.map(s => {
            const opt = $('<option></option>')
                .val(s.Value)
                .text(s.Text);

            $(idDropDownlist).append(opt);
        });
    }

}

const Modal = {
    exibirMensagemErro: function (mensagens) {
        $("#modal_mensagem_container").empty();

        const ul = $("<ul></ul>");

        mensagens.map(mensagem => {
            ul.append(`<li>${mensagem}</li>`);
        });

        let alert = $(`<div class='alert alert-danger'></div>`).append(ul);

        $("#modal_mensagem_container")
            .append($("<div class='col-xs-12'></div>").append(alert));

    },

    exibirMensagemSucesso: function (mensagem) {
        $("#modal_mensagem_container").empty();

        let alert = $(`<div class='alert alert-success'></div>`).text(mensagem);

        $("#modal_mensagem_container")
            .append($("<div class='col-xs-12'></div>").append(alert));
    },
    limparMensagens() {
        $("#modal_mensagem_container").empty();
    }
}
