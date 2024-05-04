/**     Module     */
function NewModule() {
    $(".swal-overlay").remove();
    $(".hfIdModule").val(0);
    $(".txtTituloM").val("");
    $(".txtDescripcionM").val("");
    $(".txtIconoM").val("");
    $(".txtControladorM").val("");
    $(".txtAccionM").val("");
    $(".txtOrdenM").val("");
    $("#dvMdlNewModule").modal();
    $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
}

function updateModule() {
    var Params = {
        id: $(".hfIdModule:last").val(),
        IdApp: $("#hfIdAppEdit:last").val(),
        Titulo: $(".txtTituloM:last").val(),
        Descripcion: $(".txtDescripcionM:last").val(),
        Icono: $(".txtIconoM:last").val(),
        Controlador: $(".txtControladorM:last").val(),
        Accion: $(".txtAccionM:last").val(),
        Orden: $(".txtOrdenM:last").val(),
    };
    saveChangesModule(Params);
}

function saveModule() {
    var Params = {
        id: $(".hfIdModule").val(),
        IdApp: $("#hfIdAppEdit").val(),
        Titulo: $(".txtTituloM").val(),
        Descripcion: $(".txtDescripcionM").val(),
        Icono: $(".txtIconoM").val(),
        Controlador: $(".txtControladorM").val(),
        Accion: $(".txtAccionM").val(),
        Orden: $(".txtOrdenM").val(),
    };
    saveChangesModule(Params);
}

function saveChangesModule(Params) {
    $.ajax({
        type: "GET",
        url: '/Module/saveModule',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                createTableModules(data.data);
                onDllPerfilesChanged();
                $(".dvCloseModalNewModal").click();
                swal("Éxito", "El registro ha sido guardado con éxito.", "info")
            }
            else {

            }

        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}

function createTableModules(lista) {
    var theadTitle = ["#", "Nombre", "Descripción", "Estatus", "Orden", "<i class='fas fa-cog fa-1x'></i>"];

    createTable("table_SecondId", theadTitle, lista, "#dvTabla", 1);
    // onDllPerfilesChanged();
}

function execfunctionModule(typeInstr) {
    switch (typeInstr) {
        case 1: //Bloquea o desbloquea Module
            var Params = {
                IdModule: $("#hfIdModuleBlock").val(),
                Status: $("#hfStatusModuleBlock").val(),
                IdApp: $("#hfIdAppEdit").val(),
            };
            var url = "/Module/blockUnblockModule";
            var btnClose = "#btnCloseblockModule";
            execDeleteBloqOrUnblockModule(Params, url, btnClose);
            break;
        case 2: //Elimina Module
            var Params = {
                IdModule: $("#hfIdModuleDelete").val(),
                IdApp: $("#hfIdAppEdit").val(),
            };
            var url = "/Module/DeleteModule";
            var btnClose = "#btnCloseDeleteModule";
            execDeleteBloqOrUnblockModule(Params, url, btnClose);
            break;
        default:
    }
}

function execDeleteBloqOrUnblockModule(Params, url, btnClose) {

    $.ajax({
        type: "GET",
        url: url,
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                console.log(data.data);
                createTableModules(data.data);
                onDllPerfilesChanged();
                $(btnClose).click();
                swal("Éxito", "El registro ha sido guardado con éxito.", "info")
            }
            else {

            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

}

function getDetailModule(IdModule, IdApp) {
    $.ajax({
        type: "GET",
        url: "/Module/getDetailModule",
        data: { IdModule: IdModule, IdApp: IdApp },
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {

            if (!$.isEmptyObject(data)) {
                $.each(data, function (key, entidad) {
                    $(".txtTituloM").val(entidad.titulo);
                    $(".txtDescripcionM").val(entidad.descripcion);
                    $(".txtIconoM").val(entidad.i_class);
                    $(".txtControladorM").val(entidad.controlador);
                    $(".txtAccionM").val(entidad.accion);
                    $(".txtOrdenM").val(entidad.orden);
                    $("#lblEstatusMod").val(entidad.activo ? "Activo" : "Inactivo");
                    $("#lblFechaAltaMod").val(entidad.fechaAlta);
                    $("#lblUsuarioAltaMod").val(entidad.nombreUsuarioAlta);
                    $("#lblFechaModMod").val(entidad.fechaModificacion);
                    $("#lblUsuarioModMod").val(entidad.nombreUsuarioMod);
                    $("#lblAplicacionMod").val(entidad.aplicacion);
                });
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}