

$(document).ready(function () {
    usuario_dom_recuperarUsuarios("", "");
})

//#region ajax

function usuario_ajax_ObterVariosUsuarios(nomeCompleto, email) {

    const usuarioFiltro = {
        nomeCompleto: nomeCompleto,
        email: email
    }

    const parametos = new URLSearchParams(usuarioFiltro).toString();

    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Usuario/ObterVarios?" + parametos,
            accepts: "application/json"
        }).done(jqxhr => {
            usuario_dom_carregarTabela(jqxhr);
            resolve();
        }).fail(jqXhr => {
            reject(jqXhr);
        })
    });
}

function usuario_ajax_criar(usuario) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Usuario/Criar",
            contentType: "application/json",
            accepts: "application/json",
            data: JSON.stringify(usuario)
        }).done(jqXhr => {
            resolve(jqXhr);
        }).fail(jqXhr => {
            reject(jqXhr);
        })
    });
}
function usuario_ajax_obterUsuarioPorid(idUsuario) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Usuario/ObterUsuarioPorId?idUsuario=" + idUsuario,
            accepts: "application/json"
        }).done(jqXhr => {
            resolve(jqXhr);
        }).fail(jqXhr => {
            reject(jqXhr);
        });
    });
}

function usuario_ajax_editar(usuario) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Usuario/Editar",
            contentType: "application/json",
            accepts: "application/json",
            data: JSON.stringify(usuario)
        }).done(jqXhr => {
            resolve(jqXhr);
        }).fail(jqXhr => {
            reject(jqXhr);
        })
    });
}

function usuario_ajax_deletar(idUsuario) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "/Usuario/Deletar?idUsuario=" + idUsuario,
            accepts: "application/json"
        })
            .done(jqXhr => {
                resolve(jqXhr);
            })
            .fail(jqXhr => {
                reject(jqXhr);
            })
    })
}
//#endregion


function usuario_dom_recuperarUsuarios(nomeCompleto, email) {
    loading.bloquear();

    setTimeout(async function () {
        try {
            await usuario_ajax_ObterVariosUsuarios(nomeCompleto, email);
        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);
}
function usuario_dom_carregarTabela(jqXhr) {
    const { usuarios } = jqXhr;

    const { total } = jqXhr;

    const table = $("#usuario_tabela > tbody");

    table.empty();

    if (total <= 0) {
        $("#usuario_tabelaContainer").css("display", "block");
        resolve();
        return;
    }

    usuarios.map(function (usuario) {

        let cor = "color:green";

        if (usuario.Desabilitado) {
            cor = "color:red";
        }

        const editarButton = $("<span style='cursor:pointer'><i class='fa fa-pencil'></i></span>")
            .unbind("click")
            .on("click", function () {
                usuario_editar_button_click(usuario.IdUsuario);
            });

        const excluirButton = $("<span style='cursor:pointer'><i class='fa fa-trash'></i></span>")
            .unbind("click")
            .on("click", function () {
                usuario_deletar_button_click(usuario.IdUsuario);
            });

        const tr = $(`
            <tr>
                <td>${usuario.NomeCompleto}</td>
                <td>${usuario.Email}</td>
                <td>
                    <i class="fa fa-check" style="${cor}"></i>
                </td>
            </tr>
        `);
        tr.append(
            $("<td></td>")
                .append(editarButton)
                .append("&nbsp;&nbsp;")
                .append(excluirButton)
        );

        table.append(tr);
    });


    $("#usuario_total").text(total);

    $("#usuario_tabelaContainer").css("display", "block");
}


function abrirModal(idUsuario, nomeCompleto, email, senha, desabilitado) {
    $("#usuario_modal_idUsuario").val(idUsuario);
    $("#usuario_modal_nome").val(nomeCompleto);
    $("#usuario_modal_email").val(email);
    $("#usuario_modal_senha").val(senha);
    $("#usuario_modal_habilitado").prop("checked", !desabilitado);
    Modal.limparMensagens();
    $("#usuario_modal").modal("show");
}

function usuario_novo_button_click() {
    abrirModal("", "", "", "", true);
}

function usuario_modal_salvar_button_click() {
    const usuario = {
        IdUsuario: 0,
        NomeCompleto: "",
        Email: "",
        Senha: "",
        Desabilitado: false
    };

    usuario.NomeCompleto = $("#usuario_modal_nome").val();
    usuario.Email = $("#usuario_modal_email").val();
    usuario.Senha = $("#usuario_modal_senha").val();
    usuario.Desabilitado = !$("#usuario_modal_habilitado").prop("checked");

    const idUsuarioNumero = parseInt($("#usuario_modal_idUsuario").val());

    if (!isNaN(idUsuarioNumero)) {
        usuario.IdUsuario = idUsuarioNumero;
    }


    loading.bloquear();
    setTimeout(async function () {
        try {
            let mensagem = "";

            if (usuario.IdUsuario == 0) {
                const { mensagemSucesso } = await usuario_ajax_criar(usuario);
                mensagem = mensagemSucesso;
            } else {
                const { mensagemSucesso } = await usuario_ajax_editar(usuario);
                mensagem = mensagemSucesso;
            }

            await usuario_ajax_ObterVariosUsuarios("", "");

            Modal.exibirMensagemSucesso(mensagem);

        } catch (err) {
            if (err.responseJSON)
                Modal.exibirMensagemErro(err.responseJSON.erros);
            else
                exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);

}

function usuario_editar_button_click(idUsuario) {
    loading.bloquear();
    setTimeout(async function () {
        try {
            const { usuario } = await usuario_ajax_obterUsuarioPorid(idUsuario);

            abrirModal(usuario.IdUsuario,
                usuario.NomeCompleto,
                usuario.Email,
                usuario.Senha,
                usuario.Desabilitado);

        } catch (err) {
            exibirAlert.mensagemErroAjax(err);
        } finally {
            loading.desbloquear();
        }
    }, 500);
}

async function usuario_deletar_button_click(idUsuario) {
    exibirAlert.confirmar("Excluir Usuário", function () {
        loading.bloquear();

        setTimeout(async function () {
            try {
                await usuario_ajax_deletar(idUsuario);
                await usuario_ajax_ObterVariosUsuarios("", "");
                exibirAlert.mensagemSucesso("Usuário excluído com sucesso!");
            } catch (err) {
                exibirAlert.mensagemErroAjax(jqXhr)
            } finally {
                loading.desbloquear();
            }
        }, 500);
    });
}

function usuario_filtro_button_click() {
    let nomeCompleto = $("#usuario_filtro_nome").val();

    let email = $("#usuario_filtro_email").val();

    usuario_dom_recuperarUsuarios(nomeCompleto, email);
}

function usuario_filtro_limparButton_click() {
    $("#usuario_filtro_nome").val("");
    $("#usuario_filtro_email").val("");
    usuario_dom_recuperarUsuarios("", "");
}