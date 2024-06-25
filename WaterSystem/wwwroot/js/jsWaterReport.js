$(function () {
    $('#btnDownloadFilePDF').click(function () {
        var contenidoDiv = document.getElementById('dvReport');
        var opciones = {
            margin: 0.2,
            filename: 'ReciboAguaPotable.pdf',
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: { scale: 10 },
            jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
        };

        html2pdf()
            .from(contenidoDiv)
            .set(opciones)
            .save();
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
   
    
