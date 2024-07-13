//$(function () {
//    $('#btnDownloadFilePDF').click(function () {
//        var options = {
//            pagesplit: true
//        };
//        var pdf = new jsPDF('p', 'pt', 'a2');
//        pdf.addHTML($("#dvReport"), 5, 5, options, function () {
//            pdf.save('informe.pdf');
//        });

//    });
//});
$(document).ready(function () {
    /*let IdManzana = localStorage.getItem('IdManzana');
    if (IdManzana != "null" && IdManzana != null) {
        $("#ddlIDManzana").val(IdManzana);
    }
    configureInitialSearch();*/
});

$("#ddlManzana").on("change", function () {
    var id = $(this).val();
    $("#hfIdManzana").val(id);
    localStorage.setItem('IdManzana', id);
    busqueda.table();
});

$(document).on("click", "i[name='btnAsociar']", function (e) {
    let btnClick = $(this);
    let id = btnClick.attr("data-id");
    let type = btnClick.attr("data-type");
    //let activar = btnClick.attr("data-active") == "True";

    var parameter = {
        IdWaterMeter: id,
        option: type
    };
    GetModalAction(parameter);
    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnActivar']", function (e) {
    let btnClick = $(this);
    let id = btnClick.attr("data-id");
    let type = btnClick.attr("data-type");
    //let activar = btnClick.attr("data-active") == "True";

    var parameter = {
        IdWaterMeter: id,
        option: type
    };
    GetModalAction(parameter);
    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnDarBaja']", function (e) {
    let btnClick = $(this);
    let id = btnClick.attr("data-id");
    let type = btnClick.attr("data-type");
    //let activar = btnClick.attr("data-active") == "True";

    var parameter = {
        IdWaterMeter: id,
        option: type
    };
    GetModalAction(parameter);
    e.stopImmediatePropagation();
});

$(document).on("click", "td[name='btnMedidorBaja']", function (e) {
    let btnClick = $(this);
    let isAdmin = btnClick.attr("data-isAdmin") == "True";
    if (isAdmin) {
        let id = btnClick.attr("data-id");

        var parameter = {
            IdWaterMeter: id,
            option: 8
        };
        GetModalAction(parameter);
    }
   
    e.stopImmediatePropagation();
});

//var Params = {
//    idManzana: localStorage.getItem('IdManzana'),
//    AllSearch: true
//};

const busqueda = {
    table: function (valueSelected) {
        $.ajax({
            type: "GET",
            url: '/WaterMEter/GetTable',
            data: { idManzana: $("#ddlManzana").val() },
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $("#dvTabla").empty();
                    $("#dvTabla").append(data);
                    $("#table_id").dataTable().api();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    }
}

function GetModalAction(parameter) {
    $.ajax({
        type: "GET",
        url: '/WaterMEter/GetModalAction',
        data: parameter,
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#dvModal").empty();
                $("#dvModal").append(data);
                $("#dvMdlWaterMeterAction").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            reject('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}
/*
function configureNewReading() {
    var id = $("#ddlIDManzana").val();
    var Params = {
        idManzana: id,
    };
    $.ajax({
        type: "GET",
        url: '/WaterMEter/GetWaterMeterJson',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                theadTitle = ["IdMedidor", "Manzana", "Medidor", "Titular", "Anterior", "Actual"];
                createTableCatalog("table_id", theadTitle, data.data, "#dvTablaCatalog", 3);
              
                $("#dvBtnContainer").empty();

               // var btnSave = createBasicBtn("btnSave", "btn btn-success float-right btnMarginTop", "btnSaveFunction();", "fas fa-save fa-1x", "Guardar");
                var btnSave = createSubmitBtn("btnSave", "btn btn-success float-right btnMarginTop", "fas fa-save fa-1x", "Guardar");
                var btnCancel = createBasicBtn("btnCancel", "btn btn-danger float-right btnMarginTopDer", "btnCancelFunction();", "fas fa-window-close fa-1x", "Cancelar");
                
                $("#dvBtnContainer").empty();
                $("#dvBtnContainer").append(btnSave);
                $("#dvBtnContainer").append(btnCancel);
                $("#ddlIDManzana").attr("disabled", "disabled");
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}

function btnCancelFunction() {
    $("#dvBtnContainer").empty();

    //configureInitialSearch();

    var btnCharge = createBasicBtn("btnCharge", "btn btn-info float-right btnMarginTop", "configureNewReading();", "fas fa-plus-square fa-1x", "Cargar Lectura");
    $("#dvBtnContainer").append(btnCharge);
    $("#ddlIDManzana").removeAttr("disabled");

}
*/
//function configureInitialSearch() {
//    var id = $("#ddlIDManzana").val();
//    $("#hfIdManzana").val(id);
//    localStorage.setItem('IdManzana', id);
    
//    var Params = {
//        idManzana: id,
//        AllSearch: true
//    };
//    $.ajax({
//        type: "GET",
//        url: '/WaterMEter/GetWaterMeterJson',
//        data: Params,
//        contentType: "application/json; charset=utf-8",
//        dataType: 'json',
//        success: function (data) {
//            if (data.data !== null) {
//                $("#btnCharge").remove();
//                $("#btnCreate").remove();
//                $("#btnEdit").remove();

//                var theadTitle = ["Número", "L_Actual", "L_Anterior", "Titular", "Ubicacion", "Estatus", "Última Lectura", "<i class='fas fa-cog fa-1x'></i>"];
//                createTable("table_id", theadTitle, data.data, "#dvTablaCatalog", 7);


//                var fecha = new Date(data.data[0].fechaLectura).getMonth();
//                var fechaActual = (new Date).getMonth();
//                if (fecha === fechaActual) {
//                    //var btnCreate = createSubmitBtn("btnCreate", "btn btn-info float-right btnMarginTopDer", "fas fa-save fa-1x", "Crear Comprobante");
//                    var btnEdit = createBasicBtn("btnEdit", "btn btn-success float-right btnMarginTop", "configureEditReading();", "fas fa-edit fa-1x", "Editar Lectura");
//                    $("#dvBtnContainer").append(btnEdit);
//                    var btnCreate = createBasicBtn("btnCreate", "btn btn-info float-right btnMarginTopDer", "configureCreateVoucher();", "fas fa-plus-square fa-1x", "Crear Comprobante");
//                    $("#dvBtnContainer").append(btnCreate);
//                }
//                else {
//                    var btnCreate = createBasicBtn("btnCharge", "btn btn-success float-right btnMarginTop", "configureNewReading();", "fas fa-plus-square fa-1x", "Cargar Lectura");
//                    $("#dvBtnContainer").append(btnCreate);
//                }
//            }
//            else {
//            }
//        },
//        error: function (xhr, textStatus, errorThrown) {
//            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
//        }
//    });
//}
/*function configureEditReading() {
    var id = $("#ddlIDManzana").val();
    var Params = {
        idManzana: id,
    };
    $.ajax({
        type: "GET",
        url: '/WaterMEter/GetWaterMeterJson',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                theadTitle = ["IdMedidor", "Manzana", "Medidor", "Titular", "Anterior", "Actual"];
                createTableCatalog("table_id", theadTitle, data.data, "#dvTablaCatalog", 4);
                $("#dvBtnContainer").empty();

                var btnSave = createSubmitBtn("btnSave", "btn btn-success float-right btnMarginTop", "fas fa-save fa-1x", "Guardar");
                var btnCancel = createBasicBtn("btnCancel", "btn btn-danger float-right btnMarginTopDer", "btnCancelFunction();", "fas fa-window-close fa-1x", "Cancelar");

                $("#dvBtnContainer").empty();
                $("#dvBtnContainer").append(btnSave);
                $("#dvBtnContainer").append(btnCancel);
                $("#ddlIDManzana").attr("disabled", "disabled");
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}*/