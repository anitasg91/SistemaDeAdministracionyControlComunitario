let cont = 0;
$(document).ready(function () {
    /*let IdManzana = localStorage.getItem('IdManzana');
    if (IdManzana != "null" && IdManzana != null) {
        $("#ddlIDManzana").val(IdManzana);
    }
    configureInitialSearch();*/
});
$("#ddlManzana").on("change", function () {
    var id = $(this).val();
    $("#IdManzana").val(id);
    localStorage.setItem('IdManzana', id);
    busqueda.table();
});

$("#ddlMes").on("change", function () {
    var id = $(this).val();
    $("#IdPeriodo").val(id);
    //localStorage.setItem('IdPeriodo', id);
    //busqueda.table();
});

$(document).on("click", "div[id='btnCharge']", function (e) {
    $(".inpEdit").removeClass('hide');
    $(".isUpdate").addClass('hide');
    var btnSave = createSubmitBtn("btnSave", "btn btn-success float-right btnMarginTop", "fas fa-save fa-1x", "Guardar");
    //var btnSave = createClassicBtn("btnSave", "btn btn-success float-right btnMarginTop", "fas fa-save fa-1x", "Guardar");
    var btnCancel = createClassicBtn("btnCancel", "btn btn-danger float-right btnMarginTopDer", "fas fa-window-close fa-1x", "Cancelar");
    $("#dvBtnContainer").empty();
    $("#dvBtnContainer").append("<br/>");
    $("#dvBtnContainer").append(btnSave);
    $("#dvBtnContainer").append(btnCancel);
    $("#ddlManzana").attr("disabled", "disabled");
    $("#ddlMes").attr("disabled", "disabled");
});

$(document).on("click", "div[id='btnCancel']", function (e) {
    $(".inpEdit").addClass('hide');
    $(".isUpdate").removeClass('hide');
    $("#dvBtnContainer").empty();
    var btnCharge = createClassicBtn("btnCharge", "btn btn-success float-right btnMarginTop", "fas fa-plus-square fa-1x", "Cargar Lectura");
    $("#dvBtnContainer").append("<br/>");
    $("#dvBtnContainer").append(btnCharge);
    $("#ddlIDManzana").removeAttr("disabled");
    $("#ddlMes").removeAttr("disabled");
    
    busqueda.table();
});


/*

$(document).on("click", "div[id='btnSave']", function (e) {
    cont = 0;
    $(".ErrorValidation").remove();
    let model = GetMedidorModel();
    GetValidations(model);
});

function GetMedidorModel() {
    let tabla = document.getElementById('table_id');
    let filas = tabla.getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    let datos = [];

    for (let i = 0; i < filas.length; i++) {
        let fila = filas[i];
        let celdas = fila.getElementsByTagName('td');

        let id = parseInt(celdas[0].dataset.id);
        let numero = celdas[1].textContent;
        let idManzana = parseInt($("#ddlManzana").val());
        let lecturaActual = parseFloat($("#txtLecturaActual" + id).val());
        let lecturaAnterior = parseFloat(celdas[2].textContent);
        let periodo = parseInt($("#ddlMes").val());

        if ($("#txtLecturaActual" + id).val() == "") {
            $("#txtLecturaActual" + id).after("<small class='ErrorValidation'>Inserte un valor</small>");
        }

        let objetoFila = {
            Id: id,
            Numero: numero,
            IdManzana: idManzana,
            LecturaActual: lecturaActual,
            LecturaAnterior: lecturaAnterior,
            Periodo: periodo
        };
        datos.push(objetoFila);
    }

    return datos;
}

async function GetValidations(model) {
    let validacion = document.getElementsByClassName('ErrorValidation');
    if (validacion.length === 0) {
        await SaveInfo(model);
    }
}

async function SaveInfo(datos) {
    return new Promise(function (resolve, reject) {
        datos.forEach(function (elemento) {
            SaveReadingOne(elemento);

            if (cont == datos.length) {
                title = "Éxito";
                mensaje = "La lectura ha sido guardada con éxito.";
                type = "success";

                swal(title, mensaje, type);
                busqueda.table();
                resolve();
            }
        });
    });

}

function SaveReadingOne(model) {
    $.ajax({
        type: 'GET',
        url: '/WaterMeterReading/SaveNewReading',
        contentType: 'application/json',
        data: model,
        success: function (response) {
            cont++;
        },
        error: function (error) {
            console.error('Error al guardar datos:', error);
        }
    });
}

function SaveReading(Medidor) {
    $.ajax({
        type: 'get',
        url: '/WaterMeterReading/SaveNewReading', // Ajusta esta URL según tu configuración
        contentType: 'application/json',
        data: Medidor , // Convertir a JSON
        success: function (response) {
            console.log('Datos guardados correctamente:', response);
        },
        error: function (error) {
            console.error('Error al guardar datos:', error);
        }
    });



    //$.ajax({
    //    url: '/WaterMeterReading/SaveNewReading',
    //    type: 'GET',
    //    contentType: false,  // Importante para enviar FormData correctamente
    //    processData: false,  // Importante para enviar FormData correctamente
    //    data: model,
    //    success: function (response) {
    //        title = "Éxito";
    //        mensaje = "El registro ha sido " + accion + " con éxito.";
    //        type = "success";

    //        swal(title, mensaje, type);
    //        busqueda.table();
    //    },
    //    error: function (error) {
    //        console.error('Ocurrió un error:', error);
    //    }
    //});
}
*/


const busqueda = {
    table: function (valueSelected) {
        $.ajax({
            type: "GET",
            url: '/WaterMeterReading/GetTable',
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