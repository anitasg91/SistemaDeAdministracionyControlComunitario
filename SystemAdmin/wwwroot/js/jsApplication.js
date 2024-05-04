
function newApplication() {
    $(".primerModal").removeClass("primerModal");
    $(".txtDescripcion").val("");
    $(".txtIcono").val("");
    $(".txtAccion").val("");
    $(".txtControlador").val("");
    $(".txtDominio").val("");
    $('#dvMdlNewApp').modal();
}

function editApplication(IdApp) {
    $("#aInfoGral").click();
    $("#hfIdAppEdit").val(IdApp);
    //$("#hfIdAppMod").val(IdApp);
    $('#dvMdlEditApp').modal();
    // $('#bodyModalApplication').empty();
    $(".primerModal").removeClass("primerModal");
    getDetailApplication(IdApp);

}

function getDetailApplication(IdApp) {
    $.ajax({
        type: "GET",
        url: "/Application/getDetailApplication",
        data: "IdApp=" + IdApp,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {

            if (!$.isEmptyObject(data)) {
                $.each(data, function (key, entidad) {
                    $(".txtDescripcion").val(entidad.descripcion);
                    $(".txtIcono").val(entidad.icono);
                    $(".txtAccion").val(entidad.accion);
                    $(".txtControlador").val(entidad.controlador);
                    $(".txtDominio").val(entidad.dominio);
                    $("#lblEstatus").val(entidad.activo ? "Activo" : "Inactivo");
                    $("#lblFechaAlta").val(entidad.fechaAlta);
                    $("#lblUsuarioAlta").val(entidad.nombreUsuarioAlta);
                    $("#lblFechaMod").val(entidad.fechaModificacion);
                    $("#lblUsuarioMod").val(entidad.nombreUsuarioMod);

                    createTableModules(entidad.modules);
                    createTableProfiles(entidad.profiles);
                    configureTabPermission();
                    createTablePermission(entidad.permission);

                    // $("#aModules").addClass('disabled');


                });
            }
            else {
                //$("#dvGeneraQR").show();

                //var lblrojo = '<label class="lblRojoSmall">No se encontraron resultados para: ' + cveAlm + ', verifique la información.</label>';
                //$("#txtResponsable1").after(lblrojo);

            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}

function createTableProfiles(lista) {
    var theadTitle = ["#", "Nombre", "Estatus", "<i class='fas fa-cog fa-1x'></i>"];
    if (lista.length == 0)
        $("#aPermission").addClass('disabled');
    else
        $("#aPermission").removeClass('disabled');
    createTable("table_ThirdId", theadTitle, lista, "#dvTablaProfileMod", 2);
    // configureTabPermission();
}