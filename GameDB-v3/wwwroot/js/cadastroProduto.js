$(document).on("change", "select[name^='Ingredientes']", function () {
    let unidade = $(this).find(":selected").data("unidade");
    let span = $(this).closest(".col-12").find(".unidade-span");

    span.text(unidade || "");
});

function somarCustos() {
    let total = 0;

    $("input[name$='.Custo']").each(function () {
        let valor = parseFloat($(this).val()) || 0;
        total += valor;
    });
    return total;
}

function formatBRNumber(num) {
    return Number(num).toFixed(2).replace(".", ",");
}

function atualizarCustoTotal() {
    let total = somarCustos();
    calcularPreco();
    $("#Custo").val(formatBRNumber(total)).trigger("input");
}

// REMOVER ingrediente
$(document).on("click", ".deletarIngrediente", function () {
    $(this).closest(".col-12").remove();
    reindexIngredientes();
});

// REINDEXAR
function reindexIngredientes() {
    $("#ingredientes .col-12").each(function (index) {

        // Reindexa todos os campos dentro do grupo
        $(this).find("input, select").each(function () {

            let name = $(this).attr("name");

            if (name) {
                // Substitui o número dentro dos colchetes [x]
                let newName = name.replace(/\[\d+\]/, "[" + index + "]");
                $(this).attr("name", newName);
            }
        });
    });
    ingredientesIndex = $("#ingredientes .col-12").length;
}

// CALCULAR SUGESTAO DE VENDA

$("#Markup").on('keyup', function () {
    calcularPreco();
});

function calcularPreco() {
    let markup = $("#Markup").val();
    let custo = $("#Custo").val();

    let preco = markup * custo;

    $("#Preco").val(formatBRNumber(preco));
}
