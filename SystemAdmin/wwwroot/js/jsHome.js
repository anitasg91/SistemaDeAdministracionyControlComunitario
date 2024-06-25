$(document).on("click", "i[name='btnLockUser']", function (e) {
    e.preventDefault();
    let idUser = $(this).data('id');
    let estatus = $(this).data('estatus');

    $.ajax({
        type: "GET",
        url: "/Home/GetModalLockUnlockUser",
        data: { IdUsuario: idUser, Estatus: estatus },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#dvModals").empty();
                $("#dvModals").append(data);
                $("#ModalLockUnlockUser").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnDeleteUser']", function (e) {
    e.preventDefault();
    let idUsuario = $(this).data('id');
   // let idUsuario = $(this).data('estatus');
    
    $.ajax({
        type: "GET",
        url: "/Home/GetModalDeleteUser",
        data: { IdUsuario: idUsuario },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#dvModals").empty();
                $("#dvModals").append(data);
                $("#ModalDeleteUser").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

    e.stopImmediatePropagation();
});

$(document).on("click", "div[id='btnLockUserYes']", function (e) {
    e.preventDefault();
    let idUser = $("#hfIdUserLock").val();
    let estatus = $("#hfEstatusUser").val();
    let motivo = $("#txtMotivo").val();
    if (motivo != "") {
        SaveChangeStatusUser("#btnLockUserNo", idUser, estatus, motivo);
    }
    else {
        $("#txtMotivo").addClass("requerido");
    }
    e.stopImmediatePropagation();
});

$(document).on("click", "div[id='btnDeleteUserYes']", function (e) {
    e.preventDefault();
    let idUser = $("#hfIdUserDelete").val();
    let estatus = $("#hfEstatusUserrDelete").val();
    let motivo = $("#txtMotivo").val();
    if (motivo != "") {
        SaveChangeStatusUser("#btnDeleteUserNo", idUser, estatus, motivo);
    }
    else {
        $("#txtMotivo").addClass("requerido");
    }
    e.stopImmediatePropagation();
});

$(document).on("click", "i[name='btnApproveMoveUser']", function (e) {
    e.preventDefault();
    let idUser = $(this).data('id');
    let estatus = $(this).data('estatus');

    $.ajax({
        type: "GET",
        url: "/Home/GetModalApproveMoveUser",
        data: { IdUsuario: idUser, Estatus: estatus },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $("#dvModals").empty();
                $("#dvModals").append(data);
                $("#ModalApproveMoveUser").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });

    e.stopImmediatePropagation();
});
$(document).on("click", "div[id='btnApproveMoveYes']", function (e) {
    e.preventDefault();
    let idUser = $("#hfIdUserLock").val();
    let estatus = $("#hfEstatusUser").val();
    SaveChangeStatusUser("#btnApproveMoveNo", idUser, estatus, null);
    e.stopImmediatePropagation();
});

function SaveChangeStatusUser(btnClick,idUser, estatus, motivo) {
    $.ajax({
        type: "GET",
        url: "/Home/SaveBlockUnblockUser",
        data: { IdUsuario: idUser, Estatus: estatus, Motivo: motivo },
        success: function (data) {
            if (data.success) {
                window.location.reload();
                swal("Éxito", data.message, "info");
            } else {
                swal("ERROR", data.message, "error");
            }
           $(btnClick).click();
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            swal("ERROR", xhr + ", " + textStatus + ", " + errorThrown, "error");
        }
    });
}


$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

$("#txtPassw").on("change", function () {
    $("#dvConfirmPassw").removeClass("hide");
});

function configInfoNewUser() {
    $(".validar").remove();
    var TabSelect = $("#home").is(":visible") ? 1 : 2;

    var validate = true;
    switch (TabSelect) {
        case 2:
            $(".text-danger").empty();
            var controles = [".Nombre", ".APaterno", ".AMaterno", ".FechaNacimiento"];
            validate = validatePrimerTab(controles);

            if (validate) {
                $("#aWaterMeter").click();
            }
            else {
                $("#aInfoGral").click();
            }
            break;
    }
    if ($('.SendEmail').is(':checked') && $('.Email').val() == "") {
        $('.Email').after("<label class='validar'>El campo es obligatorio</label>");
        validate = false;
    }
    if ($('.SendEmail').is(':checked') && !$('.CreateUser').is(':checked')) {
        $('.dvCreateUser').append("<br class='validar'/><label class='validar'>Es obligatorio crear sesión</label>");
        validate = false;
    }

    return validate;
}

function validatePrimerTab(controles) {
    var cont = 0;
    $.each(controles, function (key, ctrl) {
        if ($(ctrl).val() == "") {
            $(ctrl).after("<label class='validar'>El campo es obligatorio</label>");
            cont++;
        }
    });

    return cont == 0;
}

function AddNMedidor() {
    $(".validar").remove();
    var Numero = $(".NoMedidor").val();
    if (Numero != "")
        $.ajax({
            type: "GET",
            url: "/Home/GetWaterMeter",
            data: "Numero=" + Numero,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (!$.isEmptyObject(data.data)) {
                    if (data.data.idTitular != null) {
                        clearFields();
                        swal("Error", "El Medidor se encuentra ligado a otro usuario, favor de desvincularlo y volver a intentarlo.", "error");
                    }
                    else {
                        let datos = getDataTable(data.data.numero);
                        datos.push({ No: data.data.id, Lectura_Actual: data.data.lecturaActual, Lectura_Anterior: data.data.lecturaAnterior, Número: data.data.numero, "": "" });
                        createTableMedidor(datos);
                        clearFields();
                    }
                }
                else {
                    var lAct = $(".LecturaActual").val();
                    var lAnt = $(".LecturaAnterior").val();

                    if (lAct != "" && lAnt != "") {
                        if (Number(lAnt) > Number(lAct))
                            $(".LecturaAnterior").after("<label class='validar'>No puede ser mayor que la lectura actual.</label>");
                        else {
                            let datos = getDataTable(Numero);
                            datos.push({ No: 0, Lectura_Actual: lAct, Lectura_Anterior: lAnt, Número: Numero, "": "" });
                            createTableMedidor(datos);
                            clearFields();
                        }
                    }
                    else {
                        if (lAct == "")
                            $(".LecturaActual").after("<label class='validar'>El campo es obligatorio.</label>");
                        if (lAnt == "")
                            $(".LecturaAnterior").after("<label class='validar'>El campo es obligatorio.</label>");
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
            }
        });
    else
        $(".NoMedidor").after("<label class='validar'>El campo es obligatorio.</label>");


}

function clearFields() {
    $(".NoMedidor").val("");
    $(".LecturaActual").val("");
    $(".LecturaAnterior").val("");
}

function createTableMedidor(lista) {
    var theadTitle = ["No", "Número", "Lectura_Actual", "Lectura_Anterior", "<i class='fas fa-cog fa-1x'></i>"];
    createTable("table_SecondId", theadTitle, lista, "#tableMedidor", 4);
}

function getDataTable(num) {
    let atributos = [];
    document.querySelectorAll('#table_SecondId thead tr th').forEach(elemento => {
        atributos.push(elemento.innerText);
    });
    let datos = [];
    document.querySelectorAll('#table_SecondId tbody tr').forEach(fila => {
        var cont = 0;
        let dato = {};
        atributos.forEach(campo => { dato[campo] = ''; });
        fila.querySelectorAll('td').forEach((elemento, n) => {
            let input = elemento.querySelector('input');
            if (input !== null) {
                dato[atributos[n]] = input.value;
                dato.id = input.id;
                if (input.name.split('.')[1] == "Numero")
                    cont = dato[atributos[n]] == num ? cont + 1 : cont;
            } else {
                dato[atributos[n]] = elemento.innerText;
            }
        });
        if (cont == 0)
            datos.push(dato);
    });
    return datos;
}

function deleteRow(num) {
    let datos = getDataTable(num);
    createTableMedidor(datos);
}