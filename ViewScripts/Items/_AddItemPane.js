
$(function () {


    $('#UploadItems').click(function () {
        Swal.fire({
            title: 'Choose file to Upload',
            html: '<form id="FormUpload" enctype="multipart/form-data" method="post" action="/Items/Upload"><div class="form-group">' +
                '<input id="input-field" size="10" type="file" class="form-control" />' +
                '<input type="submit" id="Import" value="Import" hidden />' +
                '</div></form>',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success',
            confirmButtonText: 'Upload',
            cancelButtonClass: 'btn btn-danger',
            cancelButtonText: 'Cancel',
            buttonsStyling: false
        }).then(function (result) {
            if (result.value) {
                if ($('#input-field').val()) {
                
                    if (window.FormData !== undefined) {

                        var fileUpload = $("#input-field").get(0);
                        var files = fileUpload.files;

                        // Create FormData object  
                        var fileData = new FormData();

                        // Looping over all files and add it to FormData object  
                        for (var i = 0; i < files.length; i++) {
                            fileData.append(files[i].name, files[i]);
                        }
                        // Adding one more key to FormData object  
                        fileData.append('username', 'Manas');

                        // Show Progress HERE ===
                        $("#Modal-Blur").click();
                        $("body").css("pointer-events", "none");
                        $.ajax({
                            url: '/Items/UploadFiles',
                            type: "POST",
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data  
                            data: fileData,
                            success: function (result) {
                               // alert(JSON.stringify(result));
                                if (result.Message == "Success") {                                  
                                    $("#Modal-Close").click();
                                    $('#Upload-progress').click();
                                   
                                } else {
                                    $("#Modal-Close").click();
                                    $("body").css("pointer-events", "auto");
                                    Swal.fire({
                                        type: 'error',
                                        html: result.Message,
                                        confirmButtonClass: 'btn btn-danger',
                                        buttonsStyling: false,
                                        timer: 2000

                                    });
                                }
                            },
                            error: function (err) {
                                alert(err.statusText);
                            }
                        });
                    } else {
                        Swal.fire({
                            type: 'error',
                            html: 'FormData is not supported.',
                            confirmButtonClass: 'btn btn-danger',
                            buttonsStyling: false,
                            timer: 2000

                        });
                        
                    }

                } else {
                    Swal.fire({
                        type: 'error',
                        html: 'No file Choosen',
                        confirmButtonClass: 'btn btn-danger',
                        buttonsStyling: false,
                        timer: 2000

                    });
                }

            } else {
                Swal.fire({
                    type: 'error',
                    html: 'Cancelled',
                    confirmButtonClass: 'btn btn-danger',
                    buttonsStyling: false,
                    timer: 2000

                });
            }
        })
      
    })

    // ========== SHOW PROGRESS AT RIGHT CORNER =========== //
    $('#Upload-progress').on('click', function () {
        var cur_value = 1, progress;
        var loader = new PNotify({
            title: "Finalizing Uploads",
            text: '<div class="progress progress-striped active" style="margin:0">\
            <div class="progress-bar bg-success" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0">\
            <span class="sr-only">0%</span>\
            </div>\
            </div>',
            addclass: 'bg-primary',
            icon: 'icon-spinner4 spinner',
            hide: false,
            buttons: {
                closer: false,
                sticker: false
            },
            history: {
                history: false
            },
            before_open: function (PNotify) {
                progress = PNotify.get().find("div.progress-bar");
                progress.width(cur_value + "%").attr("aria-valuenow", cur_value).find("span").html(cur_value + "%");
                var timer = setInterval(function () {
                    if (cur_value >= 100) {
                        window.clearInterval(timer);
                        loader.remove();
                        Swal.fire({
                            type: 'success',
                            html: 'Successfully Loaded.',
                            confirmButtonClass: 'btn btn-success',
                            buttonsStyling: false,
                            timer: 1000
                        });
                        $("body").css("pointer-events", "auto");
                        return;
                    }
                    cur_value += 5;
                    progress.width(cur_value + "%").attr("aria-valuenow", cur_value).find("span").html(cur_value + "%");
                }, 65);
            }
        });
    });

    //========== DOWNLOAD FORMAT ===========//
    $('#DownloadFormat').click(function () {
        location.href = "/Items/DownloadFormat";
    })
})