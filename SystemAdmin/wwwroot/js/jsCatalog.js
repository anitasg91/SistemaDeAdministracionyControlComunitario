function showModalbyCatalog() {
    var IdCatalog = $("#ddlCatalogo").val();
    $("#IDCatalog").val(IdCatalog);

    switch (IdCatalog) {
        case "1":
            $("#dvAplicacion").hide();
            $(".profile").hide();
            $("#exampleModalLabel").html("<i class='fas fa-search-location fa-md'></i> Nueva Manzana");
            $("#dvMdlNeProfile").modal();
            break;
        case "2":
            $("#dvAplicacion").hide();
            $(".profile").hide();
            $("#exampleModalLabel").html("<i class='fas fa-calendar fa-md'></i> Nuevo Mes");
            $("#dvMdlNeProfile").modal();
            break;
        case "3":
            $("#dvAplicacion").show();
            $(".profile").show();
            $("#exampleModalLabel").html("<i class='fas fa-store fa-md'></i> Nuevo Perfil");
            $("#dvMdlNeProfile").modal();
            break;
        case "4":
            $("#lblTitleWaterMeterNE").html("<i class='fas fa-clock fa-lg'></i> Nuevo medidor");

            $("#dvAplicacion").hide();
            $(".profile").hide();
            $(".dvAddWaterMeter").hide();
            $(".NoMedidor").val("");
            $(".LecturaAnterior").val("");
            $(".LecturaActual").val("");
            $(".validar").remove();
            $("#dllIdManzana").val(1);

            $("#dvMdlWaterMeter").modal();
            //createDll("#dllIdManzana", "/Catalog/GetCatalogJson");
            break;
        default:
    }
}

function configureActionsWaterMeter(IdWaterMeter, icon, title, ModifyTypeWaterMeter) {
    var titleLowercase = title.toLowerCase();
    $("#hfIdWaterMeterAction").val(IdWaterMeter);
    $("#hfIdModifyTypeWaterMeterAction").val(ModifyTypeWaterMeter);
    $("#dvMsjWaterMeterAction").show();
    $("#dvSearchUser").hide();
    $("#hfIdTitularNew").val(0);
    
    switch (ModifyTypeWaterMeter) {
        case 1:
            $("#lblTittleWaterMeterAction").html("<i class='fas " + icon + " fa-lg'></i> " + title + " usuario");
            if (titleLowercase == "asociar") {
                $("#dvMsjWaterMeterAction").hide();
                $("#dvSearchUser").show();
                $("#hfIdModifyTypeWaterMeterAction").val(6);
                $("#txtUserName").val("");
                $(".validar").remove();
            }
            else
                $("#lblMsjWaterMeterAction").text("¿Esta seguro que desea " + titleLowercase + " el usuario? Los recibos de agua ya no saldrán a su nombre.");

            $("#dvMdlWaterMeterAction").modal();
            break;
        case 2:
            $("#lblTittleWaterMeterAction").html("<i class='fas " + icon + " fa-lg'></i> " + title + " medidor");
            $("#lblMsjWaterMeterAction").text("¿Esta seguro que desea " + titleLowercase +" el medidor permanentemente? Ya no podrán generar movimientos pero su historial se seguirá mostrando.");
            $("#dvMdlWaterMeterAction").modal();
            break;
        case 3:
            $("#lblTittleWaterMeterAction").html("<i class='fas " + icon + " fa-lg'></i> " + title + " medidor");
            $("#lblMsjWaterMeterAction").text("¿Esta seguro que desea " + titleLowercase +" el medidor? ");
            $("#dvMdlWaterMeterAction").modal();
            break;
        case 4:
            $("#lblTittleWaterMeterAction").html("<i class='fas fa-trash fa-lg'></i> Eliminar medidor");
            $("#lblMsjWaterMeterAction").text("¿Esta seguro que desea eliminar el medidor? No podrá generar movimientos, ni generar un historial.");
            $("#dvMdlWaterMeterAction").modal();
            break;
    }
}
function configureEditWaterMeter(IdWaterMeter, Numero, LAnterior, LActual, IdManzana, ModifyTypeWaterMeter) {

    $("#lblTitleWaterMeterNE").html("<i class='fas fa-clock fa-lg'></i> Editar medidor");

            $(".validar").remove();
            $(".NoMedidor").val(Numero);
            $(".LecturaAnterior").val(LAnterior);
            $(".LecturaActual").val(LActual);
            $('#hfIdWaterMeter').val(IdWaterMeter);

            $("#dvAplicacion").hide();
            $(".profile").hide();
            $(".dvAddWaterMeter").hide();
            $("#dvMdlWaterMeter").modal();

            $("#dllIdManzana").val(IdManzana);
           // $('#dllIdManzana').change();
           // $('#dllIdManzana').selectpicker('render');

}

function showMdlDeleteCatalog(Id) {
    var IDCatalog = $("#ddlCatalogo").val();
    $("#hfIdDelete").val(Id);
    switch (IDCatalog) {
        case "1":
            $("#lblTittleDeleteCat").html("<i class='fas fa-trash fa-lg'></i> Eliminar Manzana");
            $("#lblMsjDeleteCat").text("Si elimina la Manzana, éste y todos sus componentes se verán afectados. ¿Esta seguro de eliminar la Manzana?");
            break;
        case "2":
            $("#lblTittleDeleteCat").html("<i class='fas fa-trash fa-lg'></i> Eliminar Mes");
            $("#lblMsjDeleteCat").text("Si elimina el Mes, éste y todos sus componentes se verán afectados. ¿Esta seguro de eliminar el Mes?");
            break;
        default:
    }
    $("#dvMdlDeleteCatalog").modal();
}

function DeleteCatalog() {

    var Params = {
        IDCatalog: $("#ddlCatalogo").val(),
        Id: $("#hfIdDelete").val()
    };
    $.ajax({
        type: "GET",
        url: '/Catalog/DeleteCatalog',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                if (data.data[0].id == 0)
                    swal("Error", data.data[0].descripcion, "error")
                else {
                    createTableGral(data.data);
                    swal("Éxito", "El registro ha sido guardado con éxito.", "info");
                }
            }
            else {

            }

        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
    $("#hfIdDelete").val("0");
    $("#btnCloseDeleteCatalog").click();
}

function createTableGral(lista) {
    var IDCatalog = Number($("#ddlCatalogo").val());
    var theadTitle = ["#", "Descripción", "<i class='fas fa-cog fa-1x'></i>"];
    createTableCatalog("table_id", theadTitle, lista, "#dvTablaCatalog", IDCatalog);
}

function createTableProfiles(lista) {
    var theadTitle = ["#", "Nombre", "Estatus", "<i class='fas fa-cog fa-1x'></i>"];
    createTable("table_id", theadTitle, lista, "#dvTablaCatalog", 2);
}

//function configureTabPermission() { }
function ModifyWaterMeterEdit(IdWaterMeter, ModifyTypeWaterMeter) {
    $('#hfIdWaterMeterAction').val(IdWaterMeter);
    $('#hfIdModifyTypeWaterMeterAction').val(ModifyTypeWaterMeter);
    ModifyWaterMeter();
}
function ModifyWaterMeter() {
    var IdWaterMeter = $('#hfIdWaterMeterAction').val();
    var ModifyTypeWaterMeter = parseInt($('#hfIdModifyTypeWaterMeterAction').val());
    var accion = "guardado";
    var url = "/Catalog/ModifyWaterMeter";
    var btnClose = "#btnCloseWaterMeterAction";
    var Params = {};
    var execController = true;

    switch (ModifyTypeWaterMeter) {
        case 1:

               Params = {
                    IdWaterMeter: IdWaterMeter,
                    ModifyTypeWaterMeter: ModifyTypeWaterMeter
                };
                accion = "desasociado";

           // execModifyWaterMeter(Params, url, btnClose, accion);
            break;
        case 2:
            Params = {
                IdWaterMeter: IdWaterMeter,
                ModifyTypeWaterMeter: ModifyTypeWaterMeter
            };
            accion = "dado de baja";
            break;
        case 3:
            Params = {
                IdWaterMeter: IdWaterMeter,
                ModifyTypeWaterMeter: ModifyTypeWaterMeter
            };
            accion = "desactivado";
            break;
        case 4:
            break;
        case 5:
            if (validateEmptyFieldsWaterMeter()) {
                Params = {
                    IdWaterMeter: IdWaterMeter,
                    ModifyTypeWaterMeter: ModifyTypeWaterMeter,
                    NoMedidor: $(".NoMedidor").val(),
                    LecturaAnterior: $(".LecturaAnterior").val(),
                    LecturaActual: $(".LecturaActual").val(),
                    IdManzana: $("#dllIdManzana").val(),
                };
                btnClose = "#btnCancelWaterMeter";
                //  execModifyWaterMeter(Params, url, btnClose, accion);
            }
            else {
                execController = false;
            }
            break;
        case 6:
            $(".validar").remove();
            var idTitular = parseInt($("#hfIdTitularNew").val());
            if (idTitular > 0) {
                Params = {
                    IdWaterMeter: IdWaterMeter,
                    ModifyTypeWaterMeter: ModifyTypeWaterMeter,
                    IdTitular: idTitular
                };
                accion = "asociado";
            }
            else {
                $("#txtUserName").after("<label class='validar'> Seleccione al nuevo titular.</label>");
                execController = false;
            }
          //  execModifyWaterMeter(Params, url, btnClose, accion);

            break;
        default:
    }
    if (execController)
    execModifyWaterMeter(Params, url, btnClose, accion);

}
function validateEmptyFieldsWaterMeter() {
    $(".validar").remove();

    var Numero = $(".NoMedidor").val();
    var lAct = $(".LecturaActual").val();
    var lAnt = $(".LecturaAnterior").val();
    var error = 0;

    var ObligatoryField = "<label class='validar'>El campo es obligatorio.</label>";

    if (Numero == "") {
        $(".NoMedidor").after(ObligatoryField);
        error++;
    }
    if (Number(lAnt) > Number(lAct)) {
        $(".LecturaAnterior").after("<label class='validar'>No puede ser mayor que la lectura actual.</label>");
        error++;
    }
    if (lAct == "") {
        $(".LecturaActual").after(ObligatoryField);
        error++;
    }
    if (lAnt == "") {
        $(".LecturaAnterior").after(ObligatoryField);
        error++;
    }
    return error == 0;
}
function execModifyWaterMeter(Params, url, btnClose, accion) {

    $.ajax({
        type: "GET",
        url: url,
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                var title = "", mensaje = "", type = "";
                if (data.data[0].id == 0) {
                    title = "Error";
                    mensaje = data.data[0].detalle;
                    type = "error";
                }
                else {

                    var theadTitle = ["No", "Número", "L_Actual", "L_Anterior", "Titular", "Ubicacion", "Estatus", "<i class='fas fa-cog fa-1x'></i>"];
                    createTable("table_id", theadTitle, data.data, "#dvTablaCatalog", 7);

                    title = "Éxito";
                    mensaje = "El registro ha sido " + accion +" con éxito.";
                    type = "success";
                }
                $(btnClose).click();
                swal(title, mensaje, type);
            }
            else {

            }
        },
        error: function (xhr, textStatus, errorThrown) {
            $(btnClose).click();
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

}

function txtUserNameOnkeyup() {
    $(".searchAutom").remove();
    var strNombre = $("#txtUserName").val();
    if (strNombre != null && strNombre != "" && strNombre != " ") {
        var Params = {
            name: strNombre
        };
        var url = "/Catalog/getUserByParameter";
        txtOnkeyup(Params, url, "#dvSearchUser");
    }
}
function selectTitular(IdTitularNew, nameTitular) {
    $("#hfIdTitularNew").val(IdTitularNew);
    $("#txtUserName").val(nameTitular);
    $(".searchAutom").remove(); 
}
