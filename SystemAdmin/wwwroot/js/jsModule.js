/**     Module     */
$(document).on("click", "div[id='btnAddPermissions']", function (e) {
    let IdPermission = Number($("#ddlPermissionPage").val());
    let IdPage = Number($(".hfIdModule").val());
    SavePermisoPagina(IdPermission, IdPage);
});

$(document).on("click", "i[name='btnDeleteP']", function (e) {
    let btnClick = $(this);
    let IdPermissionPage = btnClick.attr("data-idpp");
    let IdPage = Number($(".hfIdModule").val());
    RemovePermisoPagina(IdPermissionPage, IdPage);
});

$(function () {

});
$(document).on("click", "input[id='HasChild']", function (e) {
    let checkbox = $(this)[0];
    if (checkbox.checked) {
        $(".divFather").removeClass("oculto");
        let IdApp = $("#hfIdAppEdit:last").val();
        GetPagesFather(IdApp);
    }
    else {
        $(".divFather").addClass("oculto");
    }
});

async function GetPagesFather(IdApp) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: '/Module/GetPagesFather',
            data: { IdApp: IdApp },
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.data !== null) {
                    createDropDownList(data.data, ".ddlIDPadre", 3);
                    resolve();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    });
}

/*function GetPagesFather(IdApp) {
    $.ajax({
        type: "GET",
        url: '/Module/GetPagesFather',
        data: { IdApp: IdApp },
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                createDropDownList(data.data, ".ddlIDPadre", 3);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}*/

function NewModule() {
    $(".swal-overlay").remove();
    $(".hfIdModule").val(0);
    $(".txtTituloM").val("");
    $(".txtDescripcionM").val("");
    $(".txtIconoM").val("");
    $(".txtControladorM").val("");
    $(".txtAccionM").val("");
    $(".txtOrdenM").val("");
    $(".ddlIDPadre").val(0);
    $(".HasChild").checked = false;
    $("#dvMdlNewModule").modal();
    $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
}

function updateModule() {
    let check = $(".HasChild:last").prop('checked');
    var Params = {
        id: $(".hfIdModule:last").val(),
        IdApp: $("#hfIdAppEdit:last").val(),
        Titulo: $(".txtTituloM:last").val(),
        Descripcion: $(".txtDescripcionM:last").val(),
        Icono: $(".txtIconoM:last").val(),
        Controlador: $(".txtControladorM:last").val(),
        Accion: $(".txtAccionM:last").val(),
        Orden: $(".txtOrdenM:last").val(),
        IdPadre: check ? $(".ddlIDPadre:last").val() : 0,
    };
    saveChangesModule(Params);
}

function saveModule() {
    let checkbox = $(".HasChild")[0];
    var Params = {
        id: $(".hfIdModule").val(),
        IdApp: $("#hfIdAppEdit").val(),
        Titulo: $(".txtTituloM").val(),
        Descripcion: $(".txtDescripcionM").val(),
        Icono: $(".txtIconoM").val(),
        Controlador: $(".txtControladorM").val(),
        Accion: $(".txtAccionM").val(),
        Orden: $(".txtOrdenM").val(),
        IdPadre: checkbox.checked ? $(".ddlIDPadre").val() : 0,
        //IdPadre: $(".ddlIDPadre").val(),
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
    var theadTitle = ["#", "Nombre", "Descripción", "Estatus", "Orden"];
    if (PermisosParaTabPaginas.some(perm => _permissionCurrentPage.includes(perm))) {
        theadTitle.push("<i class='fas fa-cog fa-1x'></i>");
    }

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

async function getDetailModule(IdModule, IdApp) {
    await GetPagesFather(IdApp);
    await getConfigPagesEditMode(IdModule, IdApp);
}

async function getConfigPagesEditMode(IdModule,IdApp) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Module/getDetailModule",
            data: { IdModule: IdModule, IdApp: IdApp },
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (!$.isEmptyObject(data.data)) {
                    let entidad = data.data;
                    // $.each(data.data, function (key, entidad) {
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
                    if (entidad.idPadre != null) {
                        let idPadre = entidad.idPadre;
                        $(".HasChild:last").prop('checked', true);
                        $(".divFather:last").removeClass("oculto");
                        $(".ddlIDPadre:last").val(entidad.idPadre);
                    }
                    else {
                        $(".HasChild:last").prop('checked', false);
                        $(".divFather:last").addClass("oculto");
                        $(".ddlIDPadre:last").prop('value', 0);
                    }
                    // });
                }
                configTablePermission(data.permissionMissing, data.permissionAsigned);
                /*if (!$.isEmptyObject(data.permissionMissing)) {
                    var option = "";
                    $.each(data.permissionMissing, function (key, entidad) {
                        option += '<option value="' + entidad.id + '">' + entidad.nombre + '</option>';
                    });
                    $("#ddlPermissionPage").empty();
                    $("#ddlPermissionPage").append(option);
                    $("#dvAsignP").removeClass("oculto");
                }
                else {
                    $("#dvAsignP").addClass("oculto");
                }

                var theadTitle = ["#", "Nombre", "Descripción", "<i class='fas fa-cog fa-1x'></i>"];
                createTable("table_ThirdId", theadTitle, data.permissionAsigned, "#dvTablePermissionByPage", 9);*/
                resolve();
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    });
}

function configTablePermission(permissionMissing, permissionAsigned) {
    if (!$.isEmptyObject(permissionMissing)) {
        var option = "";
        let cont = 1;
        $.each(permissionMissing, function (key, entidad) {
            option += '<option title="' + entidad.descripcion + '" value="' + entidad.id + '">' + cont + "-" + entidad.nombre + '</option>';
            cont++;
        });
        $("#ddlPermissionPage").empty();
        $("#ddlPermissionPage").append(option);
        $("#dvAsignP").removeClass("oculto");
    }
    else {
        $("#dvAsignP").addClass("oculto");
    }
   
    var theadTitle = ["#", "Nombre", "Descripción", ];
    if ([1, 27].some(perm => _permissionCurrentPage.includes(perm))) {//Permiso para mostrar ajutes columna de los permisos
        theadTitle.push("<i class='fas fa-cog fa-1x'></i>");
    }

    createTable("table_FourthId", theadTitle, permissionAsigned, "#dvTablePermissionByPage", 9);
}

async function SavePermisoPagina(IdPermission,IdPage) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/Module/SavePermisoPagina',
            data: { IdPermiso: IdPermission, IdPagina: IdPage },
            success: function (data) {
               configTablePermission(data.permissionMissing, data.permissionAsigned);
               resolve();
            },
            error: function (xhr, textStatus, errorThrown) {
                reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    });
}

async function RemovePermisoPagina(IdPermissionPage, IdPage) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/Module/DeletePermissionPagina',
            data: { IdPermisoPagina: IdPermissionPage, IdPagina: IdPage },
            success: function (data) {
                if (data.success) {
                    configTablePermission(data.permissionMissing, data.permissionAsigned);
                }
                else {
                    swal("ERROR", data.message, "error");
                }
                resolve();
            },
            error: function (xhr, textStatus, errorThrown) {
                reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
                swal("ERROR", xhr + ", " + textStatus + ", " + errorThrown, "error");
            }
        });
    });
}