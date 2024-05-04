/**     Permission     */
function createTablePermission(lista) {
    // var theadTitle = ["#", "Módulo", "Perfil", "Aplicación","Alta","Modificación","Consulta", "<i class='fas fa-cog fa-1x'></i>"];
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
