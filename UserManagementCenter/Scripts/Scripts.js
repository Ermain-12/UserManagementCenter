$(function () {
    $("#loaderBody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderBody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderBody").addClass('hide');
    });
});


function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        };
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function UserManagementPost()
{
    $.validator.uobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.success) {
                    $("firstTab").html(html);
                    refreshAndNewTab($(form).attr('reset-Url'), true);
                    // Implement Success message
                    $.notify(response.message, "success");

                    if (typeof activateJQueryTable !== 'undefined' && $.isFunction(activateJQueryTable))
                        activateJQueryTable();
                }
                else {
                    // TODO: Implement Error Message
                    $.notify(response.message, "error");
                }
            }
        };

        if (form.attr('enctype') === 'multipart/form-data') {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processType"] = false;
        }
        $.ajax();
    }
    return false;
}


function refreshAndNewTab(resetURL, showViewTab)
{
    $.ajax({
        type: 'GET',
        url: resetURL,
        success: function (response) {
            $("#secondTab").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Add New');
            if (showViewTab) {
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
            }
        }
    });
}

function Edit(Url) {
    $.ajax({
        type: 'GET',
        url: Url,
        success: function (response) {
            $("#seconTab").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Edit');
            $('ul.nav.nav-tabs a:eq(1)').tab('show');

        }
    });
}

function Delete(Url) {
    if (confirm('Confirm the removal of this record?') === true)
    {
        $.ajax({
            type: 'POST',
            url: Url,
            success: function (response) {

                if (response.success) {
                    $("#firstTab").html(response.html);
                    $.notify(response.message, "warn");
                    if (typeof activateJQueryTable !== 'undefined' && $.isFunction(activateJQueryTable))
                        activateJQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }
                $("#seconTab").html(response);
                $('ul.nav.nav-tabs a:eq(1)').html('Edit');
                $('ul.nav.nav-tabs a:eq(1)').tab('show');
            }
        });
    }
}
