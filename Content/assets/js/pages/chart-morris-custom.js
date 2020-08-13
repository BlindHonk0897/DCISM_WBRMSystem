'use strict';
$(document).ready(function () {
    setTimeout(function () {

        // [ line-smooth-chart ] start
        Morris.Line({
            element: 'morris-line-smooth-chart',
            data: [{
                y: '2006',
                a: 100,
                b: 90
            },
            {
                y: '2007',
                a: 75,
                b: 65
            },
            {
                y: '2008',
                a: 50,
                b: 40
            },
            {
                y: '2009',
                a: 75,
                b: 65
            },
            {
                y: '2010',
                a: 50,
                b: 40
            },
            {
                y: '2011',
                a: 75,
                b: 65
            },
            {
                y: '2012',
                a: 100,
                b: 90
            }
            ],
            xkey: 'y',
            redraw: true,
            resize: true,
            ykeys: ['a', 'b'],
            hideHover: 'auto',
            responsive: true,
            labels: ['Series A', 'Series B'],
            lineColors: ['#1de9b6', '#A389D4']
        });
        // [ line-smooth-chart ] end


    }, 700);
});
