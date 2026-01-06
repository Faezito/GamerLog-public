if (window.feather) {
    feather.replace();
}

//// LOADING PLUGIN

// site.js

window.Loading = (function () {

    const defaultConfigs = {
        "overlayBackgroundColor": "#000000",
        "overlayOpacity": 0.6,
        "spinnerIcon": "pacman",
        "spinnerColor": "#FFFFFF",
        "spinnerSize": "2x",
        "overlayIDName": "Carregando",
        "spinnerIDName": "",
        "offsetX": 0,
        "offsetY": 0,
        "containerID": null,
        "lockScroll": true,
        "overlayZIndex": 9998,
        "spinnerZIndex": 9999
    };

    function show(customConfigs = {}) {
        JsLoadingOverlay.show({
            ...defaultConfigs,
            ...customConfigs
        });
    }

    function hide() {
        JsLoadingOverlay.hide();
    }

    return {
        show,
        hide
    };

})();

// Ativa loading para QUALQUER requisição AJAX
$(document).ajaxStart(function () {
    Loading.show();
});

$(document).ajaxStop(function () {
    Loading.hide();
});

//// ENDLOADINGS

$(document).on('click', '.card-game-link', function () {
    if (this.dataset.loading) return;
    this.dataset.loading = 'true';

    Loading.show();

    setTimeout(() => {
        Loading.hide();
    }, 30000);
});


$.ajaxSetup({
    complete: function (xhr) {
        if (xhr.responseJSON?.swal) {
            Swal.fire(xhr.responseJSON.swal).then(() => {
                if (xhr.responseJSON.redirect) {
                    window.location.href = xhr.responseJSON.redirect;
                }
            });
        }
    }
});

function initRatings(context) {

    if (!$.fn.rateYo) {
        console.warn("RateYo não carregado");
        return;
    }

    const scope = context || document;

    $(scope).find(".rating").each(function () {

        const nota = parseFloat($(this).data("nota")) || 0;
        const readonly = $(this).data("readonly") === true;

        $(this).rateYo({
            rating: nota,
            minValue: 0,
            maxValue: 100,
            readOnly: readonly,
            starWidth: "20px",
            ratedFill: "#ffc107"
        });

    });
}

$(function () {
    initRatings();
});




$(document).ready(function () {
    $('.date').mask('00/00/0000');
    $('.time').mask('00:00:00');
    $('.date_time').mask('00/00/0000 00:00:00');
    $('.cep').mask('00000-000');
    $('.phone').mask('0000-0000');
    $('.phone_with_ddd').mask('(00) 0000-0000');
    $('.phone_us').mask('(000) 000-0000');
    $('.mixed').mask('AAA 000-S0S');
    $('.cpf').mask('000.000.000-00', { reverse: true });
    $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
    $('.money').mask('000.000.000.000.000,00', { reverse: true });
    $('.money2').mask("#.##0,00", { reverse: true });
    $('.ip_address').mask('0ZZ.0ZZ.0ZZ.0ZZ', {
        translation: {
            'Z': {
                pattern: /[0-9]/, optional: true
            }
        }
    });
    $('.ip_address').mask('099.099.099.099');
    $('.percent').mask('##0,00%', { reverse: true });
    $('.clear-if-not-match').mask("00/00/0000", { clearIfNotMatch: true });
    $('.placeholder').mask("00/00/0000", { placeholder: "__/__/____" });
    $('.fallback').mask("00r00r0000", {
        translation: {
            'r': {
                pattern: /[\/]/,
                fallback: '/'
            },
            placeholder: "__/__/____"
        }
    });
    $('.selectonfocus').mask("00/00/0000", { selectOnFocus: true });
});