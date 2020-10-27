//above code for highcharts



$(function () {
    $.ajax({
        url: '/Client/MyDeal/Chart',
        dataType: "json",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        async: false,
        processData: false,
        cache: false,
        delay: 15,
        success: function (data) {
            // alert(data);
            var series = [];
            for (var i in data) {
                var serie = new Array(data[i].name, data[i].data);
                series.push(serie);
                //series.push({ name: data[i].label, data: data[i].y });

            }

            DrawPieChart(data);
        },
        error: function (xhr) {

        }
    });
});
function DrawPieChart(series) {
    var serie = [],
     len = series.length,
     i = 0;

    for (i; i < len; i++) {
        serie.push({
            name: [series[i].name],
            data: [series[i].data]
        });
    }

    var chart = $('#chartContainer').highcharts({
        chart: {
            type: 'bar',

            width: 690

        },
        colors: [
        '#37b6bd',
        '#050505',
        '#36387b',
        '#f8f8f8',
        '#FF6600',
        '#666633',
        '#700c62',
        '#9B4AE9',
        '#FF2134'
        ],
        credits: {
            enabled: false
        },

        exporting: {
            enabled: false
        },
        yAxis: {
            min: 0,
            max: 100,
            title: {
                text: 'Percentage %'
            }
        },
        tooltip: {
            formatter: function () {
                return this.series.name;
            }
        },
        xAxis: {
            min: 0,
            gridLineWidth: 0,
            labels: { enabled: false },
            lineColor: 'transparent',
        },
        title: {
            text: 'Timeline'
        },

        legend: {
            reversed: true
        },
        plotOptions: {
            series: {
                stacking: 'normal',
                showInLegend: false,
            },
            dataLabels: {

                format: '150 / 600',
                enabled: true,
                align: 'right',
                style: {
                    color: 'white',
                    textOutline: false,
                }
            }
        },


        series: serie


    });
    $(window).resize(function () {
        var chart = $('#chartContainer').highcharts();
        var w = $('#chartContainer').closest(".wrapper").width()
        var h = $('#chartContainer').closest(".wrapper").height()
        chart.setSize(
            h, w * (3 / 4), false
        );
    });

}


// swipebox
(function ($) {

    $('.swipebox').swipebox({
        useCSS: true,
        useSVG: true,
        initialIndexOnArray: 0,
        hideCloseButtonOnMobile: false,
        removeBarsOnMobile: true,
        hideBarsDelay: 3000,
        videoMaxWidth: 1140,
        beforeOpen: function () { },
        afterOpen: null,
        afterClose: function () { },
        loopAtEnd: false
    });



})(jQuery);
$(document).ready(function () {
    $('.swipebox-video').swipebox();
});

$(document).ready(function () {
    // Tooltip only Text
    $('.masterTooltip').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        $(this).data('tipText', title).removeAttr('title');
        $('<p class="tooltip1"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');
    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.tooltip1').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 20; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.tooltip1')
            .css({ top: mousey, left: mousex })
    });
});
