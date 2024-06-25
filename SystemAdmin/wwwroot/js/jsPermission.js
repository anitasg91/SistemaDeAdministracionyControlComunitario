/**     Permission     */
$(document).on("click", "tr[name='SelectUser']", function (e) {
    e.preventDefault();
    $(".trFila").removeClass("selectUser");
    $(".trFila td i").addClass("oculto");
    var id = $(this).data('id');
    $("#icon_" + id).removeClass("oculto");
    $(this).addClass("selectUser");
    localStorage.setItem('IdUsuario', id);
    BloquearChecksYboton(false);
    GetConfigurationPartialView(id);
    e.stopImmediatePropagation();
});

$(document).on("click", "a[class='paginate_button ']", function (e) {
    e.preventDefault();
    EmptyConfiguration();
    e.stopImmediatePropagation();
});

$(document).on("click", "a[class='paginate_button previous']", function (e) {
    e.preventDefault();
    EmptyConfiguration();
    e.stopImmediatePropagation();
});

$(document).on("click", "a[class='paginate_button next']", function (e) {
    e.preventDefault();
    EmptyConfiguration();
    e.stopImmediatePropagation();
});

$(document).on("click", "input[id='miCheckRole_0']", function (e) {
    let checkbox = $(this)[0];
    var checkboxes = document.querySelectorAll('#ulRolesList li .checkbox');
    checkboxes.forEach(function (cb) {
        if (cb.id !== checkbox.id) {
            cb.checked = checkbox.checked;
        }
    });
    HabilitarBotonGuardarConfig();
});

$(document).on("click", "input[id='miCheckBlock_0']", function (e) {
    let checkbox = $(this)[0];
    var checkboxes = document.querySelectorAll('#ulBlockList li .checkbox');
    checkboxes.forEach(function (cb) {
        if (cb.id !== checkbox.id) {
            cb.checked = checkbox.checked;
        }
    });
    HabilitarBotonGuardarConfig();
});

$(document).on("click", "input[class='form-check-input checkbox']", function (e) {
    HabilitarBotonGuardarConfig();
});

$(document).on("click", "input[id='btnSaveConfig']", function (e) {
    let arrRole = [];
    let arrBlock = [];
    var checkboxes = document.querySelectorAll('#ulRolesList li .checkbox');
    checkboxes.forEach(function (cb) {
        if (cb.id !== "miCheckRole_0" && cb.checked) {
            arrRole.push(cb.value);
        }
    });

    var checkboxes = document.querySelectorAll('#ulBlockList li .checkbox');
    checkboxes.forEach(function (cb) {
        if (cb.id !== "miCheckBlock_0" && cb.checked) {
            arrBlock.push(cb.value);
        }
    });
    
    let IdUsuario = localStorage.getItem('IdUsuario');
    let IdAplicacion = $("#hfIdAppEdit").val();
    var model = {
        IdUsuario: IdUsuario,
        IdAplicacion: IdAplicacion,
        Roles: arrRole,
        Block: arrBlock
    };

    var modelo = JSON.stringify(model);
    saveRolesAndBlocks(modelo);
});

$(".filters").on("change", function (e) {
    e.preventDefault();
    busqueda.table();
});

let filter = () => {
    return {
        IdApp: $("#hfIdAppEdit").val()
    }
};

const busqueda = {
    table: function () {

        $.ajax({
            type: "GET",
            url: "/Permission/GetPermissionsByApp",
            data: filter(),
           // contentType: "application/json; charset=utf-8",
           // dataType: 'json',
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $("#PermissionsList").empty();
                    $("#PermissionsList").append(data);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });


    }
}

function GetConfigurationPartialView(IdUser) {
    let IdAplicacion = $("#hfIdAppEdit").val();

    $.ajax({
        type: "GET",
        url: "/Permission/GetPermissionsListByUser",
        data: { IdUser: IdUser, IdApp: IdAplicacion },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#PermissionsList").empty();
                $("#PermissionsList").append(data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}
function saveRolesAndBlocks(modelo) {
    $.ajax({
        url: '/Permission/SaveConfigPermission',
        contentType: 'application/json',
        data: { model: modelo },
        success: function (response) {
            if (response.success) {
                swal("Éxito", "El registro ha sido guardado con éxito.", "info")
            } else {
                swal("ERROR", response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("ERROR", 'Error de solicitud: ' + error, "error");
        }
    });
    
}

function EmptyConfiguration() {
    $(".trFila").removeClass("selectUser");
    $(".trFila td i").addClass("oculto");
    localStorage.removeItem('IdUsuario');
    BloquearChecksYboton(true);
}

function BloquearChecksYboton(bloquear){
    var checkboxes = document.querySelectorAll('.ulConfigPermissions li .checkbox');
    checkboxes.forEach(function (cb) {
        cb.checked = false;
        $("#" + cb.id).prop('disabled', bloquear);
    });

    if (bloquear) {
        $("#btnSaveConfig").prop('disabled', bloquear);
    }
}

function HabilitarBotonGuardarConfig() {
    let IdUsuario = localStorage.getItem('IdUsuario');
    if (IdUsuario != null) {
        $("#btnSaveConfig").prop('disabled', false);
    }
}

function createTablePermission(lista) {
    var theadTitle = ["#", "Módulo", "Perfil", "Aplicación", "Permisos"];

    createTable("table_FourthId", theadTitle, lista, "#dvTablaPermissionMod", 3);
}

function configureTabPermission() {
    var Param = "IdApp=" + $("#hfIdAppEdit").val();
    // createDdlPermission("#ddlModulo", Param,"/Catalog/getModules",1);
    createDdlPermission("#ddlPerfil", Param, "/Catalog/getProfiles", 2);
}

function createDdlPermission(ddl, Param, url, tipeDdl) {
    $(ddl).empty();

    $.ajax({
        type: "GET",
        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                var option = "";
                $.each(data.data, function (key, entidad) {
                    option += '<option value="' + entidad.id + '">' + (tipeDdl == 1 ? entidad.titulo : entidad.detalle) + '</option>';
                });
                $(ddl).append(option);
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}

function NewPermission() {
    var IDApp = $("#hfIdAppEdit").val();

    $("#dvMdlNewModule").modal();
    $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
}

function onDllPerfilesChanged() {
    var Param = {
        IdPerfil: $("#ddlPerfil").val(),
        IdApp: $("#hfIdAppEdit").val()
    };

    $.ajax({
        type: "GET",
        url: "/Catalog/getPermission",
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                createTablePermission(data.data);
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });


}
