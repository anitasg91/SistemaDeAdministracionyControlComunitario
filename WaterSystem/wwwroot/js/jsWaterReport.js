$(function () {
    $('#btnDownloadFilePDF').click(function () {
        var options = {
            pagesplit: true
        };
        var pdf = new jsPDF('p', 'pt', 'a2');
        pdf.addHTML($("#dvReport"), 5, 5, options, function () {
            pdf.save('informe.pdf');
        });

    });
});

function showMdlViewReport(id) {
    $("#dvTableDesglose #tableDesglose tbody").empty();
    $("#dvBarras").empty();

    var Params = {
        Id: id,
    };
    $.ajax({
        type: "GET",
        url: '/WaterReport/GetPaymentVoucherJson',
        data: Params,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data.data !== null) {
                var x = data.data;
                $("#lblFechaExp").text(x.fecha);
                $("#lblFolio").text(x.folio);
                $("#lblPeriodo").text(x.periodo);
                $("#lblTitular").text(x.titular);
                $("#lblClave").text(x.claveTitular);
                $("#lblDireccion").text(x.direccionMedidor);
                $("#lblTotalPagar").text("$" + x.totalPagado);
                $("#lblTotalPagarAdeudo").text("$" + x.totalPagarAdeudo);
                $("#lblLectAnterior").text(x.lecturaAnt + "m³");
                $("#lblLectActual").text(x.lecturaAct + "m³");
                $("#lblTotalUsado").text(x.totalImUsado + "m³");
                $("#lblMedidor").text(x.noMedidor);
                
           
               var tdEnc = "";
                $.each(x.desglose, function (index, value) {
                    tdEnc += '<tr><td>' + value.concepto + '</td><td>' + (value.metrosUsados + "m³") + '</td><td>' + ("$" + value.totalPagar) + '</td></tr>';
                });
                $("#dvTableDesglose #tableDesglose tbody").append(tdEnc);

                var divBarra = "";
                $.each(x.grafica, function (index, value) {
                    divBarra += '<div class="bar" style="--bar-value:' + value.totalMUsados + '%;" data-name="' + value.fecha + '"></div>';
                });

                $("#dvBarras").append(divBarra);


                

                $('#dvMdlViewDetail').modal();

            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!' + xhr + ", " + textStatus + ", " + errorThrown);
        }
    });
}
   
    
