//#region MODELS
const subscriptionModel = {
    Email: "",
    PriceId: ""
}

const confirmPaymentModel = {
    Email: "",
    Company: "",
    FullName: "",
    Phone: "",
    PriceId: "",
    PaymentIntent: {}
}


let stripe;

//#endregion

//#region AJAX
function ajaxCarregarPlanos() {
    $.ajax({
        type: "GET",
        url: "/Subscription/ObtemServicosFinanceiroStripe",
        accepts: "application/json"
    }).done(jqXhr => {
        const { servicosFinanceiro } = jqXhr;

        $("#plansContainer").empty();

        const divWrapper = $("<div></div>").addClass("row").css("margin-top", "20px");

        servicosFinanceiro.map(sf => {
            const planoItem = `
                <div class="col-xs-12 col-sm-4">
                    <div id="plano_item_${sf.IdServicoFinanceiro}"   class="panel panel-default" >
                        <input  type="hidden" value="${sf.StripePriceId}" />
                        <div class="panel-body text-center">
                            <h4>${sf.DescricaoServico}</h4>
                            <h6 style="margin:20px 0;">U$ ${sf.ValorCobranca.toFixed(2)}</h6>
                            <p>
                               <button  onclick="selecionarPlanoItem(${sf.IdServicoFinanceiro})" class="btn btn-success">Select</button>
                            </p>
                        </div>
                    </div>
                </div>
            `;

            divWrapper.append(planoItem);
        });

        $("#plansContainer").append(divWrapper);
    }).fail(jqXhr => {
        const { erros } = jqXhr.responseJSON;
        exibirMensagensDeErro(erros);
    });
}

function ajaxFazerAssinatura() {



    subscriptionModel.Email = $("#emailTextBox").val();
    subscriptionModel.PriceId = $(".selected > input[type=hidden]").val();

    loading.bloquear();

    $.ajax({
        url:"/Subscription/Subscribe",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(subscriptionModel),
        complete: () => {
            loading.desbloquear();
        }
    }).done(jqXhr => {
        const { subscritpionId, paymentIntentClientSecret, publishableKey } = jqXhr;

        inicializarPagamentoStripe(publishableKey, paymentIntentClientSecret);

        limparMensagemErro();
        $("#passo2").css("display", "none");
        $("#passo3").css("display", "block");
    }).fail(jqXhr => {
        const { erros } = jqXhr.responseJSON;
        exibirMensagensDeErro(erros);
    });
}
//#endregion


//#region EVENTOS_CLICK
function clickAvancarPasso1() {

    let erros = validarPasso1();

    if (erros.length > 0) {
        exibirMensagensDeErro(erros);
        return;
    }

    limparMensagemErro();
    $("#passo1").css("display", "none");
    $("#passo2").css("display", "block");
}
function clickVoltarPasso1() {
    $("#passo1").css("display", "block");
    $("#passo2").css("display", "none");
}

function clickAvancarPasso2() {
    ajaxFazerAssinatura();
}

function clickVoltarPasso2() {
    $("#passo3").css("display", "none");
    $("#passo2").css("display", "block");
}

//#endregion


//#region DOM
function selecionarPlanoItem(idServicoFinanceiro) {
    $('.selected').removeClass('selected');
    $(`#plano_item_${idServicoFinanceiro}`).addClass("selected");
}

function exibirMensagensDeErro(mensagens) {
    const listaErros = $("<ul></ul>");

    mensagens.map(mensagem => {
        const errosItem = `<li>${mensagem}</li>`;
        listaErros.append(errosItem);
    });

    $("#erroContainer").empty();

    $("#erroContainer")
        .append(listaErros)
        .css("display", "block");
}
function limparMensagemErro() {
    $("#erroContainer")
        .css("display", "none")
        .empty();
}

//#endregion

//#region VALIDACAO
function validarPasso1() {
    let erros = [];
    let email = $("#emailTextBox").val();
    let phone = $("#phoneTextBox").val();

    if (!$('#companyTextBox').val())
        erros.push("Company is mandatory field");

    if (!email)
        erros.push("E-mail is mandatory field")
    else if (!validarEmail(email))
        erros.push("E-mail is invalid");


    if (!$("#fullNameTextBox").val())
        erros.push("Full Name is mandatory field");

    if (!phone)
        erros.push("Phone is mandatory field");
    else if (!(/^[0-9]{1,}$/.exec(phone)))
        erros.push("Phone must be numeric");

    return erros;
}

const validarEmail = (email) => {
    return String(email)
        .toLowerCase()
        .match(
            /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        );
};
//#endregion



$(document).ready(function () {
    ajaxCarregarPlanos();
    $("#passo1").css("display", "block");
});


//#region PAGAMENTO_CARTAO_CREDITO

const setMessage = (message) => {
    const messageDiv = document.querySelector('#messages');
    messageDiv.innerHTML += "<br>" + message;
}


const inicializarPagamentoStripe = (publishableKey, paymentIntentClientSecret) => {
    'use strict';

    stripe = Stripe(publishableKey);
    
    var elements = stripe.elements({
        fonts: [
            {
                cssSrc: 'https://fonts.googleapis.com/css?family=Source+Code+Pro',
            },
        ],
        // Stripe's examples are localized to specific languages, but if
        // you wish to have Elements automatically detect your user's locale,
        // use `locale: 'auto'` instead.
        locale: window.__exampleLocale
    });

    var inputs = document.querySelectorAll('.cell.example.example2 .input');
    Array.prototype.forEach.call(inputs, function (input) {
        input.addEventListener('focus', function () {
            input.classList.add('focused');
        });
        input.addEventListener('blur', function () {
            input.classList.remove('focused');
        });
        input.addEventListener('keyup', function () {
            if (input.value.length === 0) {
                input.classList.add('empty');
            } else {
                input.classList.remove('empty');
            }
        });
    });

    var elementStyles = {
        base: {
            color: '#32325D',
            fontWeight: 500,
            fontFamily: 'Source Code Pro, Consolas, Menlo, monospace',
            fontSize: '16px',
            fontSmoothing: 'antialiased',

            '::placeholder': {
                color: '#CFD7DF',
            },
            ':-webkit-autofill': {
                color: '#e39f48',
            },
        },
        invalid: {
            color: '#E25950',

            '::placeholder': {
                color: '#FFCCA5',
            },
        },
    };

    var elementClasses = {
        focus: 'focused',
        empty: 'empty',
        invalid: 'invalid',
    };

    var cardNumber = elements.create('cardNumber', {
        style: elementStyles,
        classes: elementClasses,
    });
    cardNumber.mount('#example2-card-number');

    var cardExpiry = elements.create('cardExpiry', {
        style: elementStyles,
        classes: elementClasses,
    });
    cardExpiry.mount('#example2-card-expiry');

    var cardCvc = elements.create('cardCvc', {
        style: elementStyles,
        classes: elementClasses,
    });

    cardCvc.mount('#example2-card-cvc');

    registerElements([cardNumber, cardExpiry, cardCvc], 'example2', paymentIntentClientSecret);

}

function registerElements(elements, exampleName, paymentIntentClientSecret) {
    var formClass = '.' + exampleName;
    var example = document.querySelector(formClass);

    var form = example.querySelector('form');
    var resetButton = example.querySelector('a.reset');
    var error = form.querySelector('.error');
    var errorMessage = error.querySelector('.message');

    function enableInputs() {
        Array.prototype.forEach.call(
            form.querySelectorAll(
                "input[type='text'], input[type='email'], input[type='tel']"
            ),
            function (input) {
                input.removeAttribute('disabled');
            }
        );
    }

    function disableInputs() {
        Array.prototype.forEach.call(
            form.querySelectorAll(
                "input[type='text'], input[type='email'], input[type='tel']"
            ),
            function (input) {
                input.setAttribute('disabled', 'true');
            }
        );
    }

    function triggerBrowserValidation() {
        // The only way to trigger HTML5 form validation UI is to fake a user submit
        // event.
        var submit = document.createElement('input');
        submit.type = 'submit';
        submit.style.display = 'none';
        form.appendChild(submit);
        submit.click();
        submit.remove();
    }

    // Listen for errors from each Element, and show error messages in the UI.
    var savedErrors = {};
    elements.forEach(function (element, idx) {
        element.on('change', function (event) {
            if (event.error) {
                error.classList.add('visible');
                savedErrors[idx] = event.error.message;
                errorMessage.innerText = event.error.message;
            } else {
                savedErrors[idx] = null;

                // Loop over the saved errors and find the first one, if any.
                var nextError = Object.keys(savedErrors)
                    .sort()
                    .reduce(function (maybeFoundError, key) {
                        return maybeFoundError || savedErrors[key];
                    }, null);

                if (nextError) {
                    // Now that they've fixed the current error, show another one.
                    errorMessage.innerText = nextError;
                } else {
                    // The user fixed the last error; no more errors.
                    error.classList.remove('visible');
                }
            }
        });
    });

    // Listen on the form's 'submit' handler...
    form.addEventListener('submit', function (e) {
        e.preventDefault();

        // Trigger HTML5 validation UI on the form if any of the inputs fail
        // validation.
        var plainInputsValid = true;
        Array.prototype.forEach.call(form.querySelectorAll('input'), function (
            input
        ) {
            if (input.checkValidity && !input.checkValidity()) {
                plainInputsValid = false;
                return;
            }
        });

        if (!plainInputsValid) {
            triggerBrowserValidation();
            return;
        }

        const mensagemErroTexto = document.querySelector('.error > .message');
        mensagemErroTexto.innerHTML = "";

        // Show a loading screen...
        loading.bloquear();

        // Disable all inputs.
        disableInputs();

        stripe.confirmCardPayment(paymentIntentClientSecret, {
            payment_method: {
                card: elements[2],
                billing_details: {
                    name: subscriptionModel.FullName,
                },
            }
        }).then((result) => {
            if (result.error) {
                mensagemErroTexto.innerHTML = `Payment failed: ${result.error.message}`;
            } else {

                mensagemErroTexto.innerHTML = "";

                const { paymentIntent } = result;
                confirmPaymentModel.Email =  $("#emailTextBox").val();
                confirmPaymentModel.Company = $("#companyTextBox").val();
                confirmPaymentModel.FullName = $("#fullNameTextBox").val();
                confirmPaymentModel.Phone = $("#phoneTextBox").val();
                confirmPaymentModel.PriceId = $(".selected > input[type=hidden]").val();
                confirmPaymentModel.PaymentIntent = paymentIntent;

                $.ajax({
                    type: "POST",
                    url: "/Subscription/ConfirmPaymentStripe",
                    contentType: "application/json",
                    data: JSON.stringify(confirmPaymentModel),
                    complete: () => {
                        loading.desbloquear();
                    }
                }).done(jqXhr => {
                    $("#confirmacaoPagamento").css("display", "block");
                    $("#passo3").css("display", "none");
                }).fail(jqXhr => {
                    const { erros } = jqXhr.responseJSON;
                    exibirMensagensDeErro(erros);
                    $("#confirmacaoEstorno").css("display", "block");
                    $("#passo3").css("display", "none");
                });

            }

        }).catch(reason => {
            mensagemErroTexto.innerHTML = `Payment failed: ${reason}`;
        });
    });

}

//#endregion 




