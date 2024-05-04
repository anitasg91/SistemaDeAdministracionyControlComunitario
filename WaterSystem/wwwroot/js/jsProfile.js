/**     Perfil     */
function NewProfile() {
    var descripcion = $("#txtNombreProfile").val() == '' ? $("#txtDetalleProfileEdit").val() : $("#txtNombreProfile").val();

    var Params = {
        IdProfile: $("#hfIdProfile").val(),
        IdApp: $("#hfIdAppEdit").val(),
        Descripcion: descripcion,
    };

    $.ajax({
        type: "GET",
        url: '/Catalog/saveProfile',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                createTableProfiles(data.data);
                $("#hfIdProfile").val(0);
                $("#btnCloseEditProfile").click();
                configureTabPermission();
                swal("Éxito", "El registro ha sido guardado con éxito.", "info")
            }
            else {

            }

        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
    $("#txtNombreProfile").val("");
    $("#hfIdProfile").val("");
    onDllPerfilesChanged();
}

function getDetailProfile(IdPerfil) {
    $.ajax({
        type: "GET",
        url: "/Catalog/getDetailProfile",
        data: "IdPerfil=" + IdPerfil,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {

            if (!$.isEmptyObject(data)) {
                $.each(data, function (key, entidad) {
                    $("#txtDetalleProfileEdit").val(entidad.detalle);
                    $("#lblEstatusP").val(entidad.activo ? "Activo" : "Inactivo");
                    $("#lblFechaAltaP").val(entidad.fechaAlta);
                    $("#lblUsuarioAltaP").val(entidad.nombreUsuarioAlta);
                    $("#lblFechaP").val(entidad.fechaModificacion);
                    $("#lblUsuarioP").val(entidad.nombreUsuarioMod);
                    $("#lblAplicacionP").val(entidad.aplicacion);
                    if (entidad.appSinAcceso.length > 0) {
                        $(".dvAddApps").show();
                        createDropDownList(entidad.appSinAcceso, '#ddlIDAplicacionEditProf',1);
                    }
                    else
                        $(".dvAddApps").hide();
                    createTableAppAllow(entidad.asignacionAplicacion);
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

function execfunctionProfile(typeInstr) {
    switch (typeInstr) {
        case 1: //Bloquea o desbloquea Perfil
            var Params = {
                IdProfile: $("#hfIdProfileBlock").val(),
                Status: $("#hfStatusProfileBlock").val(),
                IdApp: $("#hfIdAppEdit").val(),
            };
            var url = "/Catalog/blockUnblockProfile";
            var btnClose = "#btnCloseblockProfile";
            execDeleteBloqOrUnblockProfile(Params, url, btnClose);
            break;
        case 2: //Elimina perfil
            var Params = {
                IdProfile: $("#hfIdProfileDelete").val(),
                IdApp: $("#hfIdAppEdit").val(),
            };
            var url = "/Catalog/DeleteProfile";
            var btnClose = "#btnCloseDeleteProfile";
            execDeleteBloqOrUnblockProfile(Params, url, btnClose);
            break;
        default:
    }
}

function execDeleteBloqOrUnblockProfile(Params, url, btnClose) {

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
                    createTableProfiles(data.data);
                    configureTabPermission();
                    title = "Éxito";
                    mensaje = "El registro ha sido eliminado con éxito.";
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

function createTableAppAllow(lista) {
    var theadTitle = ["#", "Aplicación","Estatus", "<i class='fas fa-cog fa-1x'></i>"];
    createTable("table_FifthId", theadTitle, lista, "#dvTablaAppPerm", 5);
}

function saveAsignacionAplicacion(id) {
    var IdApp = $("#ddlIDAplicacionEditProf").val();
    var IdProfile = $("#hfIdProfile").val();

    var Params = {
        Id: id,
        IdProfile: IdProfile,
        IdApp: IdApp
    };
    $.ajax({
        type: "GET",
        url: '/Permission/saveAsignacionAplicacion',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            var titulo = "Éxito", mensaje = "El registro ha sido guardado con éxito.", type ="success";
            if (data.data !== null) {
                if (data.data[0].id == 0) {
                    titulo = "Error"; mensaje = data.data[0].detalle; type = "error";
                }
                else
                getDetailProfile(IdProfile);
                //reateDropDownList(entidad.appSinAcceso, '#ddlIDAplicacionEditProf');
                //createTableAppAllow(entidad.asignacionAplicacion);
            swal(titulo, mensaje, type)
            }
            else {

            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });



}
