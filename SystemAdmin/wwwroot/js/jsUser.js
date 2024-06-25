function newUser() {
    $("#liHistorial").css("display", "none");
    $("#Historial").css("display", "none");
    $('#dvMdlNewUser').modal();
    $("#aInfoGral").click();
    $("#hfIdUserEdit").val("0");
    $("#exampleModalLabel").html('<i class="fas fa-user fa-lg"></i> Nuevo Usuario');
    $(".primerModal").removeClass("primerModal");

    $(".Nombre").val("");
    $(".APaterno").val("");
    $(".AMaterno").val("");
    $(".Telefono").val("");
    $(".Email").val("");
    $(".FechaNacimiento").val("");

    $(".imgUser").attr('src', '/images/defaultImgProfile.jpg');

    $(".SendEmailUser").show();
    $(".dvCreateUser").show();

    createTableMedidor(null);
}

function editUser(Id, IdUser) {
    $('#dvMdlNewUser').modal();
    $("#aInfoGral").click();
    $("#hfIdUserEdit").val(IdUser);
    $("#exampleModalLabel").html('<i class="fas fa-user fa-lg"></i> Editar Usuario');
   // $('#dvMdlEditApp').modal();
    $(".primerModal").removeClass("primerModal");
    getDetailUser(IdUser);

}
function getDetailUser(IdUser) {
    $.ajax({
        type: "GET",
        url: "/Home/getDetailUserByIdUser",
        data: "IdUser=" + IdUser,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {

            if (!$.isEmptyObject(data)) {
                $.each(data, function (key, entidad) {
                    $(".Nombre").val(entidad.nombre);
                    $(".APaterno").val(entidad.aPaterno);
                    $(".AMaterno").val(entidad.aMaterno);
                    $(".Telefono").val(entidad.telefono);
                    $(".Email").val(entidad.email);
                    $(".FechaNacimiento").val(entidad.fechaNac);
                    $(".ddlManzana").val(entidad.idManzana);
                    $(".ddlPerfil ").val(entidad.idPerfil);
                    $(".ddlSexo ").val(entidad.sexo == false? 0: 1);
                    

                    if (entidad.imgToBase64string != null)
                        $(".imgUser").attr('src', 'data:image/jpeg;base64,' + entidad.imgToBase64string);
                    else
                        $(".imgUser").attr('src', '/images/defaultImgProfile.jpg');


                    if (entidad.usuario == "") {
                        $(".SendEmailUser").show();
                        $(".dvCreateUser").show();
                    }
                    else {
                        $(".SendEmailUser").hide();
                        $(".dvCreateUser").hide();
                    }

                    if (entidad.idEstatus === 1 || entidad.idEstatus === 2 || entidad.idEstatus === 3) {
                       
                        $("#liHistorial").css("display", "none");
                        $("#Historial").css("display", "none");
                    }
                    else {
                        let hasApprovalSubdelegate = entidad.hasApprovalSubdelegate ? "Aprobado por el subdelegado" : "";
                        let hasApprovalJudge = entidad.hasApprovalJudge ? "Aprobado por el juez" : "";
                        let hasApprovalTreasurer = entidad.hasApprovalTreasurer ? "Aprobado por el tesorero" : "";
                       
                        let html = `<div class="row">
                                        <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-xs-12 EstatusMovimento">
                                            <label>Estatus</label> <br />
                                            <label>${entidad.nombreEstatus}</label> <br />
                                        </div>
                                        <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-xs-12 EstatusMovimento">
                                            <label>Aprobaciones:</label> <br />
                                            <label>${entidad.approvalSubdelegate}</label>
                                            <label>${entidad.approvalJudge}</label>
                                            <label>${entidad.approvalTreasurer}</label>
                                        </div>
                                    </div>`;
                        $("#Historial").html(html);
                    }

                    createTableWaterMeter(entidad.medidor);
                   /* createTableProfiles(entidad.profiles);
                    configureTabPermission();
                    createTablePermission(entidad.permission);
                    */
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

function createTableWaterMeter(lista) {
   // var theadTitle = ["No", "Número", "Lectura_Actual", "Lectura_Anterior", "<i class='fas fa-cog fa-1x'></i>"];
    let canDeleteMed = ([1, 5].some(perm => _permissionCurrentPage.includes(perm)));
    var theadTitle = ["No", "Número", "Lectura_Actual", "Lectura_Anterior"];
    if (canDeleteMed) {
        theadTitle.push("<i class='fas fa-cog fa-1x'></i>");
    }


    createTable("table_SecondId", theadTitle, lista, "#tableMedidor", 6);
    // onDllPerfilesChanged();
}
