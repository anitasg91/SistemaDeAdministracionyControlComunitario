function validar() {

    campo = document.getElementById('iniciosesion');
    valido = document.getElementById('iniciosesionspan');
    expRegex = /^[0-9]+$/;
    emailRegex = /^[a-zA-Z0-9\._-]+@[a-zA-Z0-9-]{2,}[.][a-zA-Z]{2,4}$/;

    if (emailRegex.test(campo.value) == true || expRegex.test(campo.value) == true) {

    } else {
            valido.innerText = "No es un dato Válido";
    }   
};


    //var current = 0;
    //var imagenes = new Array();

    //    $(document).ready(function () {
    //        var numImages = 6;
    //        if (numImages <= 3) {
    //    $('.right-arrow').css('display', 'none');
    //            $('.left-arrow').css('display', 'none');
    //        }

    //        $('.left-arrow').on('click', function () {
    //            if (current > 0) {
    //    current = current - 1;
    //            } else {
    //    current = numImages - 3;
    //            }

    //            $(".carrusel").animate({"left": -($('#product_' + current).position().left) }, 600);

    //            return false;
    //        });

    //        $('.left-arrow').on('hover', function () {
    //    $(this).css('opacity', '0.5');
    //        }, function () {
    //    $(this).css('opacity', '1');
    //        });

    //        $('.right-arrow').on('hover', function () {
    //    $(this).css('opacity', '0.5');
    //        }, function () {
    //    $(this).css('opacity', '1');
    //        });

    //        $('.right-arrow').on('click', function () {
    //            if (numImages > current + 3) {
    //    current = current + 1;
    //            } else {
    //    current = 0;
    //            }

    //            $(".carrusel").animate({"left": -($('#product_' + current).position().left) }, 600);

    //            return false;
    //        });
    //    });
