/**     Common     */
function createTable(IDTabla, theadTitle, lista, IdDivContendor, IdTipoTable) {
    $("#" + IDTabla + "_wrapper").remove();
    var templateTabla = '<table id="' + IDTabla + '"><thead><tr>' + createFila(theadTitle, true, IdTipoTable)
        + '</tr></thead><tbody>' + createFila(lista, false, IdTipoTable) + '</tbody></table>';
    $(IdDivContendor).append(templateTabla);

    $("#" + IDTabla).dataTable().api();
}

function createFila(theadTitle, EsEncabezado, IdTipoTable) {
    var tdEnc = "";
    var cont = 0;
    $.each(theadTitle, function (index, value) {
        if (EsEncabezado) {
            tdEnc += '<th scope="col">' + value + '</th>';
        }
        else {
            var actions = '<td  class="dt-center ajustes"><i onclick="editComponent(' + IdTipoTable + "," + value.id + ');" style="color:dodgerblue; cursor:pointer;" class="fas fa-edit fa-lg"></i>' +
                '<i onclick="blockUnblockComponent(' + IdTipoTable + ", " + value.id + ',' + (value.activo ? 1 : 0) + ');" style="color:gray; cursor:pointer;" class="' + value.action + ' fa-lg"></i>' +
                '<i onclick="deleteComponent(' + IdTipoTable + ", " + value.id + ');" style="color:red; cursor:pointer;" class="fas fa-trash fa-lg"></i></td>';
            switch (IdTipoTable) {
                case 1:
                    tdEnc += '<tr><td>' + value.id + '</td><td>' + value.titulo + '</td><td>' + value.descripcion + '</td><td class=" dt-center" style="color:' + value.color + '"><i class="' + value.estatus + ' fa-lg"></i></td><td>' + value.orden + '</td>' + actions + '</tr>';
                    break;
                case 2:
                    tdEnc += '<tr><td>' + value.id + '</td><td>' + value.detalle + '</td><td class=" dt-center" style="color:' + value.color + '"><i class="' + value.estatus + ' fa-lg"></i></td>' + actions + '</tr>';
                    break;
                case 3:
                    tdEnc += '<tr><td class="ajustes"><input type="hidden" name="permission[' + cont + '].Id" value="' + value.id + '"/>' + value.id + '</td>'
                        + '<td><input type="hidden" name="permission[' + cont + '].IdModulo" value="' + value.idModulo + '"/> ' + value.modulo + '</td>'
                        + '<td><input type="hidden" name="permission[' + cont + '].IdPerfil" value="' + value.idPerfil + '"/> ' + value.perfil + '</td>'
                        //+ '<td><i class="' + value.aplicacion + ' fa-lg"></i></td>'
                        + '<td>' + value.descripcion + '</i></td>'
                        + '<td class="dt-center"><div class="row">'
                        + '<div class="col-4"><input type = "checkbox" class="dt-center" name="permission[' + cont + '].strAlta" ' + value.chkAlta + '> Alta </div>'
                        + '<div class="col-4"><input type="checkbox" class="dt-center" name="permission[' + cont + '].strModificacion" ' + value.chkModificacion + '> Modificación </div>'
                        + '<div class="col-4"><input type="checkbox" class="dt-center" name="permission[' + cont + '].strConsulta" ' + value.chkConsulta + '> Consulta </div>'
                        + '</div></td></tr>';

                    cont++;
                    break;
                case 4:
                    if (value.No != "Ningún dato disponible en esta tabla" && value.No != "No data available in table") {
                        var actionsOp = '<td class="dt-center ajustes">'
                            + '<i onclick="deleteRow(' + value.Número +');"style="color:red; cursor:pointer;" class="fas fa-trash fa-lg"></i>'
                            + '</td>';
                        tdEnc += '<tr><td class="dt-center ajustes"><input style="width:100%;" class="readOnly" readonly name="Medidor[' + cont + '].Id" value="' + value.No + '"/></td>'
                            + '<td class="dt-center"><input class="readOnly" readonly name="Medidor[' + cont + '].Numero" value="' + value.Número + '"/></td>'
                            + '<td class="dt-center"><input class="readOnly" readonly name="Medidor[' + cont + '].LecturaActual" value="' + value.Lectura_Actual + '"/></td>'
                            + '<td class="dt-center"><input class="readOnly" readonly name="Medidor[' + cont + '].LecturaAnterior" value="' + value.Lectura_Anterior + '"/></td >'
                            + actionsOp + '</tr>';
                        cont++;
                    }
                    break;
                case 5:
                    var actionsAppAllow = '<td  class="dt-center ajustes">'
                        + '<i onclick="saveAsignacionAplicacion(' + value.id + ');" style="color:red; cursor:pointer;" class="fas fa-trash fa-lg"></i></td>';
                    tdEnc += '<tr><td class="ajustes">' + value.id + '</td><td>' + value.nombreApp + '</td><td class="ajustes dt-center" style="color:' + value.color + '"><i class="' + value.estatus + ' fa-lg"></i></td>' + actionsAppAllow + '</tr>';
                    break;
                case 6:
                    if (value.id != "Ningún dato disponible en esta tabla" && value.id != "No data available in table") {
                        var actionsOp = '<td class="dt-center ajustes">'
                            + '<i onclick="deleteRow(' + value.numero + ');"style="color:red; cursor:pointer;" class="fas fa-trash fa-lg"></i>'
                            + '</td>';
                        tdEnc += '<tr><td class="dt-center ajustes"><input style="width:100%;" class="readOnly" readonly name="Medidor[' + cont + '].Id" value="' + value.id + '"/></td>'
                            + '<td class="dt-center"><input class="readOnly" readonly name="Medidor[' + cont + '].Numero" value="' + value.numero + '"/></td>'
                            + '<td class="dt-center"><input class="readOnly" readonly name="Medidor[' + cont + '].LecturaActual" value="' + value.lecturaActual + '"/></td>'
                            + '<td class="dt-center"><input class="readOnly" readonly name="Medidor[' + cont + '].LecturaAnterior" value="' + value.lecturaAnterior + '"/></td >'
                            + actionsOp + '</tr>';
                        cont++;
                    }
                    break;
                case 7:
                    if (value.id != "Ningún dato disponible en esta tabla" && value.id != "No data available in table") {
                        var actionsOp = '<td style="with:9% !important;" class="dt-center">'
                            + '<i title="Editar" onclick="configureEditWaterMeter(' + value.id + ',' + value.numero + ',' + value.lecturaAnterior + ',' + value.lecturaActual + ',' + value.idManzana + ',5);'
                            +' "style="color:dodgerblue; cursor:pointer; margin-right: 10px;" class="fas fa-edit fa-lg"></i>'
                            + '<i title="' + value.titleAssocciate + '" onclick="configureActionsWaterMeter(' + value.id + ',\'' + value.assocciate + '\',\'' + value.titleAssocciate + '\',1); "style="color:' + value.colorAssocciate +'; cursor:pointer; margin-right: 10px;" class="fas ' + value.assocciate + ' fa-lg"></i>'
                            + '<i title="' + value.titleDeactivate + '" onclick="configureActionsWaterMeter(' + value.id + ',\'' + value.deactivate + '\',\'' + value.titleDeactivate + '\',3); "style="color:' + value.colorDeactivate +'; cursor:pointer; margin-right: 10px;" class="fas ' + value.deactivate + ' fa-lg"></i>'
                            + '<i title="' + value.titleUnsubscribe + '" onclick="configureActionsWaterMeter(' + value.id + ',\'fa-power-off\',\'' + value.titleUnsubscribe + '\',2); "style="color:' + value.colorUnsubscribe +'; cursor:pointer; margin-right: 10px;" class="fas fa-power-off fa-lg"></i>'
                           /* + '<i title="Eliminar" onclick="configureActionsWaterMeter(' + value.id + ',null,null,4); "style="color:orange; cursor:pointer;" class="fas fa-trash fa-lg"></i>'*/
                            + '</td>';
                        tdEnc += '<tr><td class="dt-center ajustes"><input style="width:100%;" class="readOnly" readonly name="Medidor[' + cont + '].Id" value="' + value.id + '"/></td>'
                            + '<td class="dt-center">' + value.numero + '</td>'
                            + '<td class="dt-center">' + value.lecturaAnterior + '</td>'
                            + '<td class="dt-center">' + value.lecturaActual + '</td>'
                            + '<td class="dt-center">' + value.nombreTitular + '</td>'
                            + '<td class="dt-center">' + value.ubicacion + '</td>'
                            + '<td class="dt-center" style="color:' + value.color + '"><i class="' + value.estatus + ' fa-lg"></i></td>'
                            + (value.fechaBaja == null ? actionsOp : '<td class="dt-center" style="color:red;"> MEDIDOR DADO DE BAJA </td>') + '</tr>';
                        cont++;
                    }
                    break;
                default:
            }

        }
    });

    return tdEnc;
}

function execAction(TypeAction, IdApp, status) {
    $("#hfIdApp").val(IdApp);
    $("#hfTypeAction").val(TypeAction);
    $("#hfTypeStatus").val(status);
    switch (TypeAction) {
        case 1:
            $("#lblTitle").html("<i class='fas fa-ban fa-lg'></i> Desactivar Aplicación");
            $("#lblMensaje").text("Si desactiva la aplicación, éste y todos sus complementos ya no podrán utilizarse en el sistema. ¿Desea desactivar la aplicación?");
            break;
        case 2:
            $("#lblTitle").html("<i class='fas fa-trash fa-lg'></i> Borrar Aplicación");
            $("#lblMensaje").text("Si elimina la aplicación, éste y todos sus componentes serán removidos. ¿Esta seguro de eliminar la aplicación?");
            break;
        default:
    }

    $('#dvMdlActions').modal();
}

function editComponent(TypeCatalogo, Id) {
    $(".swal-overlay").remove();
    switch (TypeCatalogo) {
        case 1: //Para Modulo
            $("#lblTitle").html("<i class='fas fa-edit fa-lg'></i> Editar Modulo");
            $(".hfIdModule").val(Id);
            var IdApp = $("#hfIdAppEdit").val();
            getDetailModule(Id, IdApp);
            $('#dvMdlEditModule').modal();
            $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
            break;
        case 2: //Para Perfil
            $("#hfIdProfile").val(Id);
            getDetailProfile(Id);
            $('#dvMdlEditProfile').modal();
            $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
            $("#txtDetalleProfileEdit").focus();
            break;
        default:
    }
}

function blockUnblockComponent(TypeCatalogo, Id, status) {
    $(".swal-overlay").remove();
    var mensaje = titulo = "";

    switch (TypeCatalogo) {
        case 1: //Para Módulo
            if (status) {
                titulo = "<i class='fas fa-ban fa-lg'></i> Bloquear Módulo";
                mensaje = "Si desactiva el Módulo, éste y todos sus complementos ya no podrán utilizarse en el sistema. ¿Desea desactivar el Módulo?";
            }
            else {
                titulo = "<i class='fas fa-ban fa-lg'></i> Activar Módulo";
                mensaje = "Si activa el Módulo, éste y todos sus complementos quedarán expuestos en el sistema. ¿Desea activar el Módulo?";
            }
            $('#hfIdModuleBlock').val(Id);
            $('#hfStatusModuleBlock').val(status);
            $('#lblTitleBlockModule').html(titulo);
            $('#lblMensajeBlockModule').text(mensaje);
            $('#dvMdlblockUnblockModule').modal();
            $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
            break;
        case 2: //Para Perfil
            if (status) {
                titulo = "<i class='fas fa-ban fa-lg'></i> Bloquear perfil";
                mensaje = "Si desactiva el perfil, éste y todos sus complementos ya no podrán utilizarse en el sistema. Así como los usuarios que tenga vinculados, no podrán tener acceso al sistema. ¿Desea desactivar el pefil?";
            }
            else {
                titulo = "<i class='fas fa-ban fa-lg'></i> Activar perfil";
                mensaje = "Si activa el perfil, éste y todos sus complementos quedarán expuestos en el sistema. ¿Desea activar el pefil?";
            }
            $('#hfIdProfileBlock').val(Id);
            $('#hfStatusProfileBlock').val(status);
            $('#lblTitleBlockProfile').html(titulo);
            $('#lblMensajeBlockProfile').text(mensaje);
            $('#dvMdlblockUnblockProfile').modal();
            $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
            break;
        default:
    }

}

function deleteComponent(TypeCatalogo, Id) {
    $(".swal-overlay").remove();
    switch (TypeCatalogo) {
        case 1: //Para Modulo
            $('#hfIdModuleDelete').val(Id);
            $('#dvMdlDeleteModule').modal();
            $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
            break;
        case 2: //Para Perfil
            $('#hfIdProfileDelete').val(Id);
            $('#dvMdlDeleteProfile').modal();
            $("body .modal-backdrop:nth-child(3n)").addClass("primerModal");
            break;
        default:
    }
}

function createDropDownList(Lista, ddlName, typeId) {
    $(ddlName).empty();
    var option = "";
    $.each(Lista, function (key, entidad) {
        switch (typeId) {
            case 1:
                option += '<option value="' + entidad.idAplicacion + '">' + entidad.nombreApp + '</option>';
                break;
            case 2:
                option += '<option value="' + entidad.id + '">' + entidad.descripcion + '</option>';
                break;
            default:
        }
    });
    $(ddlName).append(option);
}

/**     Common Catalog     */
function createTableCatalog(IDTabla, theadTitle, lista, IdDivContendor, IdTipoTable) {
    $("#" + IDTabla + "_wrapper").remove();
    var templateTabla = '<table id="' + IDTabla + '"><thead><tr>' + createFilaCatalog(theadTitle, true, IdTipoTable)
        + '</tr></thead><tbody>' + createFilaCatalog(lista, false, IdTipoTable) + '</tbody></table>';
    $(IdDivContendor).append(templateTabla);

    $("#" + IDTabla).dataTable().api();
}

function createFilaCatalog(theadTitle, EsEncabezado, IdTipoTable) {
    var tdEnc = "";

    $.each(theadTitle, function (index, value) {
        if (EsEncabezado) {
            tdEnc += '<th scope="col">' + value + '</th>';
        }
        else {
            var actions = '<td  class="dt-center ajustes">'
                + '<i onclick="showMdlEditCatalog(' + value.id + ',"' + value.descripcion + '");" style="color:dodgerblue; cursor:pointer;" class="fas fa-edit fa-lg"></i>'
                + '<i onclick="showMdlDeleteCatalog(' + value.id + ');" style="color:red; cursor:pointer;" class="fas fa-trash fa-lg"></i></td>';

            switch (IdTipoTable) {
                case 1:
                    tdEnc += '<tr><td class="dt-center ajustes">' + value.id + '</td><td>' + value.descripcion + '</td>' + actions + '</tr>';
                    break;
                case 2:
                    tdEnc += '<tr><td class="dt-center ajustes">' + value.id + '</td><td>' + value.descripcion + '</td>' + actions + '</tr>';
                    break;
            }
        }
    });
    return tdEnc;
}

function ddlCatalogoOnChange() {

    var IdCat = Number($("#ddlCatalogo").val());

    switch (IdCat) {
        case 1://Si es Manzana
        case 2://Si es Mes
            $("#dvAplicacion").addClass("hide");
            getCat();
            break;
        case 3://Si es perfil
            $("#dvAplicacion").removeClass("hide");
            getProf();
            break;
        case 4://Si es Medidor
            $("#dvAplicacion").addClass("hide");
            createDll("#dllIdManzana", "/Catalog/GetCatalogJson");
            getWaterMeterList();
            break;
        default:
            $("#dvAplicacion").addClass("hide");
            break;
    }

}

function getCat() {
    var Params = {
        IDCatalog: $("#ddlCatalogo").val(),
    };
    $.ajax({
        type: "GET",
        url: '/Catalog/GetCatalogJson',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                createTableGral(data.data);
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}

function getProf() {
    var idApp = $("#ddlIDAplicacion").val();
    $("#hfIdAppEdit").val(idApp);
    $("#hfIDApp").val(idApp);
    var Params = {
        IdApp: idApp,
    };
    $.ajax({
        type: "GET",
        url: '/Catalog/getProfilesWithStats',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                createTableProfiles(data.data);

            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}
function createDll(dllName, url) {
    var Params = {
        IDCatalog: 1
    };
    $.ajax({
        type: "GET",
        url: url,
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                createDropDownList(data.data, dllName, 2);
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}

function getWaterMeterList() {
    var Params = {
        IDCatalog: $("#ddlCatalogo").val(),
    };
    $.ajax({
        type: "GET",
        url: '/Catalog/GetWaterMeterJson',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                var theadTitle = ["No", "Número", "L_Anterior", "L_Actual", "Titular", "Ubicacion", "Estatus", "<i class='fas fa-cog fa-1x'></i>"];
                createTable("table_id", theadTitle, data.data, "#dvTablaCatalog", 7);
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}

function txtOnkeyup(Params, url, contentName) {
    var divContent = '<div class="form-control searchAutom" style="max-height: 250px !important; height: auto; overflow-y: scroll;">{}</div>';
    var option = "";
    $.ajax({
        type: "GET",
        url: url,
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            $(".searchAutom").remove();
            if (data.data !== null) {
                $.each(data.data, function (key, entidad) {
                    option += '<div style="cursor: pointer;" onclick="selectTitular(' + entidad.id + ',\'' + entidad.nombreCompleto + '\')">' + entidad.nombreCompleto + '</div>';
                });
                var contentFinal = divContent.replace('{}', option);
                $(contentName).append(contentFinal);
            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}