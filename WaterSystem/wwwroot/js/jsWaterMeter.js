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

    configureInitialSearch();

    var btnCharge = createBasicBtn("btnCharge", "btn btn-info float-right btnMarginTop", "configureNewReading();", "fas fa-plus-square fa-1x", "Cargar Lectura");
    $("#dvBtnContainer").append(btnCharge);
    $("#ddlIDManzana").removeAttr("disabled");

}

function configureInitialSearch() {
    var id = $("#ddlIDManzana").val();
    $("#hfIdManzana").val(id);
    
    var Params = {
        idManzana: id,
        AllSearch: true
    };
    $.ajax({
        type: "GET",
        url: '/WaterMEter/GetWaterMeterJson',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {

                var theadTitle = ["Número", "L_Actual", "L_Anterior", "Titular", "Ubicacion", "Estatus", "Última Lectura", "<i class='fas fa-cog fa-1x'></i>"];
                createTable("table_id", theadTitle, data.data, "#dvTablaCatalog", 7);


                var fecha = new Date(data.data[0].fechaLectura).getMonth();
                var fechaActual = (new Date).getMonth();
                if (fecha === fechaActual) {
                    //var btnCreate = createSubmitBtn("btnCreate", "btn btn-info float-right btnMarginTopDer", "fas fa-save fa-1x", "Crear Comprobante");
                    var btnCreate = createBasicBtn("btnCreate", "btn btn-info float-right btnMarginTopDer", "configureCreateVoucher();", "fas fa-plus-square fa-1x", "Crear Comprobante");
                    $("#dvBtnContainer").append(btnCreate);
                }
                else {
                    $("#btnCreate").remove();
                }
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}
    
