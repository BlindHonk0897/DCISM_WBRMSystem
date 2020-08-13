'use strict';
$(window).on('load', function () {
    $('#html').jstree();
    $('#data').jstree({
        'core': {
            'data': [{
                "text": "Root node",
                "children": [{ "text": "Child node 1" }, { "text": "Child node 2" }]
            }]
        }
    });
    $('#frmt').jstree({
        'core': {
            'data': [{
                "text": "Root node",
                "state": { "opened": true },
                "children": [{
                    "text": "Child node 1",
                    "state": { "selected": true },
                    "icon": "jstree-file"
                }, {
                        "text": "Child node 2",
                        "state": { "disabled": true }
                    }]
            }]
        }
    });
    $('#clbk').jstree({
        'core': {
            'data': function (node, cb) {
                if (node.id === "#") {
                    cb([{
                        "text": "Root",
                        "id": "1",
                        "children": true
                    }]);
                } else {
                    cb(["Child"]);
                }
            }
        }
    });
    $('#ajax').jstree({
        'core': {
            'data': {
                "url": "../assets/plugins/jstree/json/root.json",
                "dataType": "json"
            }
        }
    });
    $('#lazy').jstree({
        'core': {
            'data': {
                "url": "../assets/plugins/jstree/json/lazy.json",
                "data": function (node) {
                    return { "id": node.id };
                }
            }
        }
    });

    $('#evts_button').on("click", function () {
        var instance = $('#evts').jstree(true);
        instance.deselect_all();
        instance.select_node('1');
    });
    $('#evts').on("changed.jstree", function (e, data) {
        if (data.selected.length) {
            alert('The selected node is: ' + data.instance.get_node(data.selected[0]).text);
        }
    }).jstree({
        'core': {
            'multiple': false,
            'data': [{
                    "text": "LB 446",
                    "children": [
                        { "text": "Child node 1", "id": 1 },
                        { "text": "Child node 2" }
                    ]
                }]
        }
    });

    $('#evts').on("changed.jstree", function (e, data) {
        if (data.selected.length) {
            alert('The selected node is: ' + data.instance.get_node(data.selected[0]).text);
        }
    }).jstree({
        'core': {
            'multiple': true,
            'data': [{
                "text": "FACILITIES/ROOMS",
                "state": { "opened": true },
                "children": [
                    { "text": "LB 445", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 446", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 447", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 448", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 449", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 450", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 451", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 452", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 453", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 454", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 455", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 456", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 457", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 458", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] },
                    { "text": "LB 459", "children": [{ "text": "MOUSE" }, { "text": "KEYBOARD" }, { "text": "CPU" }, { "text": "MONITOR" }, { "text": "EXTRAS" }] }
                ]
            }]
        }
    });
   
});