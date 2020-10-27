$(function () {
    $(document).ajaxStart(function () {
        $("#ajaxLoading").css("display", "none");
        $("#ajax-backdrop").css("display", "none");
    });
});
$(document).ready(function () {
    $(".PropertyTax").bind('keyup change click', function (e) {
        var HousePrice = parseFloat($("#Inputs_HousePrice").val());
        var PropertyTax = parseFloat(this.value);
        var TaxPercent = (PropertyTax * 100) / HousePrice;
        $("#Inputs_PropertyTaxPercent").val(TaxPercent.toFixed(2));
    });
    $(".PropertyTaxPercent").bind('keyup change click', function (e) {
        var HousePrice = parseFloat($("#Inputs_HousePrice").val());
        var PropertyTaxPercent = parseFloat(this.value);
        var TaxAmount = Math.round((HousePrice * PropertyTaxPercent) / 100);
        $("#Inputs_PropertyTax").val(TaxAmount);
    });
});
$(document).ready(function () {
    $(".drop-change,:input").bind('keyup change click', function (e) {


        var $form = $("#Mortgage");
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $("#DataGrid");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
        return false;


    });
});
$(document).ready(function () {

    $(".ddl-select2").change(function () {


        var $form = $("#Mortgage");
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $("#DataGrid");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
        return false;


    });
});
$(document).ready(function () {

    $(".Slider").on("change", function () {


        var $form = $("#Mortgage");
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $("#DataGrid");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
        return false;


    });
});

$(document).ready(function () {

    var range = $('.homeprice'),
    value = $('.HomePriceValue');

    value.html(range.attr('value'));

    range.on('input', function () {
        //value.attr('value') = this.value;
        value.val(this.value);
        $('.DownPaymentSlider').attr('max', this.value)
    });
})

$(document).ready(function () {

    var range = $('.DownPaymentSlider'),
    value = $('.DownPayment');

    value.html(range.attr('value'));
    range.on('input', function () {
        //value.attr('value') = this.value;
        value.val(this.value);

        var HousePrice = parseFloat($("#Inputs_HousePrice").val());
        var DownPayment = parseFloat(this.value);
        var DownPercent = parseFloat((DownPayment / HousePrice) * 100)
        
        $('#Inputs_DownPercent').val(DownPercent.toFixed(2) || '');

    });


    $("#Inputs_DownPayment").bind('keyup change click', function (e) {
        var HousePrice = parseFloat($("#Inputs_HousePrice").val());
        var DownPayment = parseFloat(this.value);
        var DownPercent = Math.round(parseFloat((DownPayment / HousePrice) * 100))
        $("#Inputs_DownPercent").val(DownPercent);
        var slider = document.getElementById('DownPaymentSlider');
        slider.value = parseInt(DownPayment)
    });

    $("#Inputs_DownPercent").bind('keyup change click', function (e) {
        var HousePrice = parseFloat($("#Inputs_HousePrice").val());
        var DownPercent = parseFloat(this.value);
        var DownPayment = Math.round(parseFloat((HousePrice * DownPercent) / 100))
        $("#Inputs_DownPayment").val(DownPayment);
        //$('.DownPaymentSlider').attr('value', DownPayment);
        //$('.DownPaymentSlider').attr('value', DownPayment);
        var slider = document.getElementById('DownPaymentSlider');
        slider.value = parseInt(DownPayment)
    });

    $("#Inputs_HousePrice").bind('keyup change click', function (e) {
       

        var HousePrice = parseFloat(this.value);
        var DownPercent = parseFloat($("#Inputs_DownPercent").val());
        var DownPayment = Math.round(parseFloat((HousePrice * DownPercent) / 100))
        $("#Inputs_DownPayment").val(DownPayment);
        var slider = document.getElementById('HomePriceSlider');
        slider.value = parseInt(this.value)
    });
    

})