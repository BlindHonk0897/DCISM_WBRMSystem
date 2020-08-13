


$("#pills-all-tab").click(function () {
    var itemApi = "/api/Common/Items/Recent";

    $.getJSON(itemApi).done(function (data) {
        $('#Table_Item_Recent').empty();
        var tableAppend = '';
        for (var i = 0; i < data.length -5; i++) {
            var toAppend = '<tr>' +
                '<th scope="row">' + data[i].Room_No + '</th>' +
                '<td>' + data[i].Category + '</td>' +
                '<td>' + data[i].Item_Name + '</td>' +
                '<td>' + data[i].Brand_Model + '</td>' +
                '<td>' + data[i].Date_Acquired + '</td>' +
                '<td>' + data[i].Remarks + '</td>' +
                '</tr >';
            tableAppend += toAppend;
        }
        $('#Table_Item_Recent').append(tableAppend);
    })
})

var iconlist = ['Control Room', 'Functional Unit', 'LB 400', 'LB 443', 'LB 445', 'LB 446', 'LB 447', 'LB 448', 'LC 469', 'LC 483', 'LC 484',
    'LC 485', 'LC 486'
];

for (var i = 0, l = iconlist.length; i < l; i++) {
    $('#icon-wrapper').append(
        '<div class="i-block" data-clipboard-text="feather ' + iconlist[i] + '" data-filter="' + iconlist[i] + '"  data-toggle="tooltip" title="' + iconlist[i] + '">' +
        '<a href="/Items/ListItems/' + iconlist[i] + '">' + iconlist[i] + '</a>' +
        '</div>');
}
$(window).on('load', function () {
    $("#icon-search").on("keyup", function () {
        var g = $(this).val().toLowerCase();
        $(".i-main .i-block").each(function () {
            var t = $(this).attr('data-filter');
            if (t) {
                var s = t.toLowerCase();
            }
            if (s) {
                var n = s.indexOf(g);
                if (n !== -1) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            }
        });
    });
});

/*

$.getJSON(api).done(function (data) {
                $.getJSON(api1).done(function (data1) {
                    var children = [];
                    for (var i = 0; i < data.length; i++) {
                        var subchild = [];
                        for (var x = 0; x < data1.length; x++) {
                            subchild.push({ "text": data1[x].Category_Name, "data": data1[x].Category_Name + "-" + data[i] });
                        }
                        children.push({ "text": data[i], "data": "root", "children": subchild });
                    }
                    $('#evts1').on("changed.jstree", function (e, datatree) {
                        if (datatree.selected.length) {
                            if (datatree.instance.get_node(datatree.selected[0]).data != "root") {
                                var details = datatree.instance.get_node(datatree.selected[0]).data.split('-');
                                var ap = "/api/Common/GetItem/room/" + details[1] + "/category/" + details[0];
                                tableData.destroy();
                                $.getJSON(ap).done(function (dataap) {
                                    var datasetree = [];
                                    for (var i = 0; i < dataap.length; i++) {
                                        var arr = [];
                                        arr.push(dataap[i].Id_Item);
                                        arr.push(dataap[i].Room_No);
                                        arr.push(dataap[i].Category);
                                        arr.push(dataap[i].Item_Name);
                                        arr.push(dataap[i].Brand_Model);
                                        arr.push(dataap[i].Date_Acquired);
                                        arr.push(dataap[i].Remarks);
                                        datasetree.push(arr);
                                    }
                                    tableData = $('#demo-foo-filtering').DataTable({
                                        data: datasetree,
                                        "columns": [
                                            { title: "Id" },
                                            { title: "Room_No" },
                                            { title: "Category" },
                                            { title: "Item_Name" },
                                            { title: "Brand_Model" },
                                            { title: "Date_Acquired" },
                                            { title: "Remarks" }],
                                        "language": {
                                            "emptyTable": "No data. <b>to preview...</b>"
                                        }
                                    });
                                })
                            }

                        }
                    }).jstree({
                        'core': {
                            'multiple': true,
                            'data': [{
                                "text": "FACILITIES",
                                'data': "root",
                                "state": { "opened": true },
                                "children": children
                            }]
                        }
                    });
                })
            })

*/