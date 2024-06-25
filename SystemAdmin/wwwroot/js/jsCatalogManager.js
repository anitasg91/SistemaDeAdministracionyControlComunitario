let catalogsEquals = [1, 2];

$(document).on("click", "div[id='btnNuevo']", function (e) {
    let IdCatalog = $("#ddlCatalogManager").val();
    switch (IdCatalog) {
        case "1":
        case "2":
            let title = IdCatalog == 1 ? "a manzana" : "o mes";
            $("#hfId").val("0");
            $("#txtNombre").val("");
            $("#lblTitle").html(`<i class='fas fa-calendar fa-md'></i> Nuev${title}`);
            $("#txtNombre").removeClass("inputDetail");
            $("#txtNombre").removeAttr("disabled");
            $("#ModalCatalogo").modal();
            break;
        case "3":
            $("#hfId").val("0");
            $("#txtNombre").val("");
            $("#txtDescripcion").val("");
            $("#dvEstatus").addClass("oculto");
            $("#dvFecha").addClass("oculto");
            $("#txtNombre").removeClass("inputDetail");
            $("#txtDescripcion").removeClass("inputDetail");
            $("#txtNombre").removeAttr("disabled");
            $("#txtDescripcion").removeAttr("disabled");


            $("#ModalPermissions").modal();
            break;
        default:
    }
});

$(document).on("click", "i[name='btnEdit']", function (e) {
    let btnClick = $(this);
    let Id = btnClick.attr("data-id");
    GetConfigureModalByCatalogSelected(Id);
    e.stopImmediatePropagation();
});

$(document).on("click", "div[id='btnSave']", function (e) {
    SaveInfo();
    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnDelete']", function (e) {
    var IDCatalog = $("#ddlCatalogManager").val();
    let btnClick = $(this);
    let id = btnClick.attr("data-id");
    $.ajax({
        type: "GET",
        url: `/CatalogManager/GetDeletePartialView`,
        data: { IdCatalogType: IDCatalog, IdDelete: id },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#dvModalCatalog").empty();
                $("#dvModalCatalog").append(data);
                $("#ModalDelete").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
    e.stopImmediatePropagation();
});

$(document).on("click", "div[name='DeleteYes']", function (e) {
    var IDCatalog = Number($("#ddlCatalogManager").val());
    var id = $("#hfIdDelete").val();

    $.ajax({
        type: "GET",
        url: `/CatalogManager/DeleteCatalog`,
        data: { IDCatalog: IDCatalog, IdDelete: id },
        success: function (data) {
            if (data.data[0].id == 0) {
                $("#DeleteNo").click();
                swal("Error", data.data[0].descripcion, "error");
            }
            else {
                $("#DeleteNo").click();
                swal("Éxito", "El registro ha sido guardado con éxito.", "info");
            }
                busqueda.table(IDCatalog);
        },
        error: function (xhr, textStatus, errorThrown) {
            reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnLock']", function (e) {
    let btnClick = $(this);
    let Id = btnClick.attr("data-id");
    let activar = btnClick.attr("data-active") == "True";
    let titulo = activar ? "<i class='fas fa-ban fa-lg'></i> Bloquear permiso" : "<i class='fas fa-unlock fa-lg'></i> Activar permiso";
    let mensaje = activar ? "Si desactiva el permiso, puede que éste y sus páginas relacionadas ya no funcionen de manera óptima. Se pueden perder ciertos privilegios en los usuarios. ¿Desea desactivar el permiso?" :
        "Si activa el permiso, éste y sus páginas relacionadas quedarán expuestos en el sistema. Podría asignar privilegios no deseados a usuarios no deseados. ¿Desea activar el permiso?";

    $("#hfActivo").val(activar ? "1":"0");
    $('#hfIdLock').val(Id);
    $('#lblTitleLock').html(titulo);
    $('#lblDescripcionLock').html(mensaje);
    $("#ModalLock").modal();
    e.stopImmediatePropagation();
});

$(document).on("click", "div[name='btnLockSi']", function (e) {
    var IDCatalog = Number($("#ddlCatalogManager").val());
    var id = $("#hfIdLock").val();
    var activo = $("#hfActivo").val();
    
    $.ajax({
        type: "GET",
        url: `/CatalogManager/ActivateCatalog`,
        data: { IdLock: id, Activar: activo },
        success: function (data) {
            if (data.data.id == 0) {
                swal("Error", data.data[0].descripcion, "error");
            }
            else {
                swal("Éxito", "El estatus del permiso ha sido actualizado con éxito.", "info");
            }
            $("#btnLockNo").click();
            busqueda.table(IDCatalog);
        },
        error: function (xhr, textStatus, errorThrown) {
            reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
});

const GetModel = function () {
    let IdCatalog = Number($("#ddlCatalogManager").val());
    let isMM = catalogsEquals.includes(IdCatalog);
    if (isMM) {
        return {
            Id: $("#hfId").val(),
            Descripcion: $("#txtNombre").val(),
            //TypeCatalog: IdCatalog,
        };
    }
    else {
        return {
            Id: Number($("#hfId").val()),
            Nombre: $("#txtNombre").val(),
            Descripcion: $("#txtDescripcion").val(),
            Activo: $("#chkEstatus").prop('checked'),
           // TypeCatalog: IdCatalog,
        };
    }
   
}

$(".filters").on("change", function (e) {
    let valueSelected = Number($(this).val());
    busqueda.table(valueSelected);
    if (
        (valueSelected == 1 && [1, 34].some(perm => _permissionCurrentPage.includes(perm)))
        || (valueSelected == 2 && [1, 37].some(perm => _permissionCurrentPage.includes(perm)))
        || (valueSelected == 3 && [1, 40].some(perm => _permissionCurrentPage.includes(perm)))
    ){
        $("#btnNuevo").removeClass("oculto");
    }
    else {
        $("#btnNuevo").addClass("oculto");
    }


});

const busqueda = {
    table: function (valueSelected) {
        let isMM = catalogsEquals.includes(valueSelected);
        let ActionName = isMM ? "GetCatalogPartialView" : "GetPermissionPartialView";
        $.ajax({
            type: "GET",
            url: `/CatalogManager/${ActionName}`,
            data: isMM ? { IdCatalogType: valueSelected } : null,
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $("#dvTablaCatalog").empty();
                    $("#dvTablaCatalog").append(data);
                    $("#table_id").dataTable().api();

                    ConfigureModalByCatalogSelected(valueSelected, false, null);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    }
}

async function GetConfigureModalByCatalogSelected(id) {
    let valueSelected = Number($("#ddlCatalogManager").val());
    await ConfigureModalByCatalogSelected(valueSelected, true, id);
    switch (valueSelected) {
        case 1:
        case 2:
            let editStr = valueSelected == 1 ? "manzana" : "mes";
            $("#lblTitle").html(`<i class='fas fa-calendar fa-md'></i> Editar ${editStr}`);
            $("#ModalCatalogo").modal();
            break;
        case 3:
            $("#lblTitle").html("<i class='fas fa-calendar fa-md'></i> Editar permiso");
            $("#dvEstatus").removeClass("oculto");
            $("#dvFecha").removeClass("oculto");
            $("#ModalPermissions").modal();
            break;
        default:
    }
}

async function ConfigureModalByCatalogSelected(valueSelected, editarMod, id) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: `/CatalogManager/GetNewPartialView`,
            data: { IdCatalogType: valueSelected, EditMod: editarMod, Id: id },
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $("#dvModalCatalog").empty();
                    $("#dvModalCatalog").append(data);
                    $(".modal-backdrop.fade").remove();
                }
                resolve();
            },
            error: function (xhr, textStatus, errorThrown) {
                reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    });
}

async function SaveInfo() {
    return new Promise(function (resolve, reject) {
       // let model = GetModel();
        let IdCatalog = Number($("#ddlCatalogManager").val());
        let model = JSON.stringify(GetModel());
        $.ajax({
            type: "GET",
            url: `/CatalogManager/SaveInfoCatalogJson`,
            data: { model: model, IdCatalogType: IdCatalog },
            success: function (data) {
                $("#dvTablaCatalog").empty();
                $("#dvTablaCatalog").append(data);
                $("#table_id").dataTable().api();

                swal("Éxito", "El registro ha sido guardado con éxito.", "info");
                $("#btnCancel").click();

                resolve();
            },
            error: function (xhr, textStatus, errorThrown) {
                reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
                swal("ERROR", xhr + ", " + textStatus + ", " + errorThrown, "error");
            }
        });
    });
}











