function voltar() {
    window.history.back();
}

//function salvar() {
//    $("#myForm").submit();
//}

function salvar(controllerName) {
    $.ajax({
        type: "POST",
        url: "/" + controllerName + "/Salvar",
        data: $("#myForm").serialize(),
        success: function (data) {
            sucesso(data);
        },
        error: function (msg) {
            var title = (/<title>(.*?)<\/title>/m).exec(msg.responseText)[1];
            bootbox.alert(title);
        }
    });
}

function load() {
    posLoad();
}

function adicionar(controllerName) {
    window.location.href = "/" + controllerName + "/Adicionar";
}

function alterar(controllerName, field, id) {
    window.location.href = "/" + controllerName + "/Alterar?" + field + "=" + id;
}

function remover(controllerName, id) {
    bootbox.confirm("Deseja remover o registro?", function (confirmed) {
        if (confirmed) {
            $.ajax({
                url: "/" + controllerName + "/Remover",
                data: { id: id },
                type: 'POST',
                success: function (data) {
                    bootbox.alert(data.Message, function () {
                        if (data.Status == 0) {
                            window.location.href = "/" + controllerName + "/Listar";
                        }
                    });
                },
                error: function (msg) {
                    var title = (/<title>(.*?)<\/title>/m).exec(msg.responseText)[1];
                    bootbox.alert(title);
                }
            });
        }
    });
}