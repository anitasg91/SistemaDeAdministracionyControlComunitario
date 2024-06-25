$(document).on("click", "input[name='chkPages']", function (e) {
    let chkPage = $(this);
    let IdPagina = chkPage.val();
    let isChecked = chkPage.prop('checked');
    //let IdPagina = $(this).data('id');
    if (isChecked) {
        $("#divPermisoByPagina_" + IdPagina).removeClass("oculto");
    }
    else {
        $("#divPermisoByPagina_" + IdPagina).addClass("oculto");
    }
    e.stopImmediatePropagation();
    //e.stopPropagation();
});

$(document).on("click", "div[id='btnCancel']", function (e) {
    e.preventDefault();
    busqueda.table();
    $("#hfIdAppEdit").prop('disabled', false);
    $("#btnNuevo").removeClass('oculto');
    e.stopImmediatePropagation();
});

$(document).on("click", "div[id='btnSave']", function (e) {
    const chkPermisosPaginas = document.querySelectorAll('.chkPermisosPaginas');
    let IdPerfil = $("#hfIdPerfil").val();
    let Nombre = $("#Detalle").val();
    let Estatus = $("#chkEstatus").prop("checked");

    let PermisosPaginasList = [];
    chkPermisosPaginas.forEach(elem => {
        //const estilo = window.getComputedStyle(elem);
        let IdPadre = elem.dataset.idpadre;
        let divContainer = $("#divPermisoByPagina_" + IdPadre);

        if (elem.checked && !divContainer.hasClass('oculto')) {
            let IdPermisoPagina = elem.dataset.id;

            PermisosPaginasList.push({
                IdPermisoPagina: IdPermisoPagina,
                IdPerfil: IdPerfil
            });
        }
    });

    //Aquí guardamos
    const model = {
        IdPerfil: IdPerfil,
        Nombre: Nombre,
        Activo: Estatus,
        PermisoPaginaPerfil: PermisosPaginasList
    };
    let s = compressData(model);
    let strData = compressData(model);
    if (Nombre != null && Nombre != '') {
        SaveDetailProfile(strData);
        $("#btnNuevo").removeClass("oculto");
    }
    else {
        $("#Detalle").addClass("requerido");
    }
});

$(document).on("click", "i[name='Asign']", function (e) {
    e.preventDefault();
    let IdPerfil = $(this).data('id');
    let IdApp = $("#hfIdAppEdit").val();
    $("#btnNuevo").addClass('oculto');
    GetProfilesById(IdApp, IdPerfil);
    $("#hfIdAppEdit").prop('disabled', true);
   
    e.stopImmediatePropagation();
});

$(document).on("click", "div[id='btnNuevo']", function (e) {
    e.preventDefault();
    let IdApp = $("#hfIdAppEdit").val();
    $(this).addClass('oculto');
    $("#hfIdAppEdit").prop('disabled', true);
    GetProfilesById(IdApp, 0);
   
    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnLock']", function (e) {
    e.preventDefault();
    let idPerfil = $(this).data('id');
    let estatus = $(this).data('estatus');

    $.ajax({
        type: "GET",
        url: "/PermisoPaginaPerfil/GetModalLockUnlockProfile",
        data: { IdPerfil: idPerfil, Estatus: estatus },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#dvModals").empty();
                $("#dvModals").append(data);
                $("#ModalLockUnlockProfile").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

    e.stopImmediatePropagation();
});

$(document).on("click", "div[id='btnLockYes']", function (e) {
    e.preventDefault();
    let idPerfil = $("#hfIdPerfilLock").val();
    let estatus = $("#hfEstatusPerfil").val();
    $.ajax({
        type: "GET",
        url: "/PermisoPaginaPerfil/blockUnblockProfile",
        data: { IdProfile: idPerfil, Status: estatus },
        success: function (data) {
            if (data.success) {
                swal("Éxito", data.message, "info");
                busqueda.table();
                $("#btnLockNo").click();
            } else {
                swal("ERROR", data.message, "error");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnDelete']", function (e) {
    e.preventDefault();
    let idPerfil = $(this).data('id');

    $.ajax({
        type: "GET",
        url: "/PermisoPaginaPerfil/GetModalDeleteProfile",
        data: { IdPerfil: idPerfil },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#dvModals").empty();
                $("#dvModals").append(data);
                $("#ModalDeleteProfile").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

    e.stopImmediatePropagation();
});

$(document).on("click", "div[id='btnDeleteYes']", function (e) {
    e.preventDefault();
    let idPerfil = $("#hfIdPerfilDelete").val();
    $.ajax({
        type: "GET",
        url: "/PermisoPaginaPerfil/DeleteProfile",
        data: { IdProfile: idPerfil},
        success: function (data) {
            if (data.success) {
                swal("Éxito", data.message, "info");
                busqueda.table();
                $("#btnDeleteNo").click();
            } else {
                swal("ERROR", data.message, "error");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
    e.stopImmediatePropagation();
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
            url: "/PermisoPaginaPerfil/GetProfilesByApp",
            data: filter(),
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $("#divTable").empty();
                    $("#divTable").append(data);
                    $("#table_id").dataTable().api();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    }
}

function GetProfilesById(IdAplication, IdProfile) {
    $.ajax({
        type: "GET",
        url: "/PermisoPaginaPerfil/GetProfilesById",
        data: { IdApp: IdAplication, IdPerfil: IdProfile },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#divTable").empty();
                $("#divTable").append(data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

}

function SaveDetailProfile(modelo) {
    $.ajax({
        url: '/PermisoPaginaPerfil/SaveDetailProfile',
        contentType: 'application/json',
        data: { model: modelo },
        success: function (response) {
            if (response.success) {
                swal("Éxito", "Los datos han sido guardados con éxito.", "info");
                busqueda.table();
                $("#hfIdAppEdit").prop('disabled', false);
            } else {
                swal("ERROR", response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("ERROR", 'Error de solicitud: ' + error, "error");
        }
    });

}

function compressData(data) {
    var jsonData = JSON.stringify(data);
    var compressedData = pako.gzip(jsonData, { level: 9 }); // 'pako' es una librería para compresión gzip en JavaScript
    var base64Data = btoa(String.fromCharCode.apply(null, compressedData));
    return base64Data;
}

