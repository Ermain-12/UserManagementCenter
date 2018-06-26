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
                $("firstTab").html(response);
            }
        }

        if (form.attr('enctype') === 'multipart/form-data') {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processType"] = false;
        }
        $.ajax();
    }
    return false;
}
