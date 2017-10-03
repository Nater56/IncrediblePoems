$(document).ready(function () {
    var body = $('#Body');
    var imageField = $('#save-image');

    body.on('blur', function () {
        body.val(body.val().replace(new RegExp('\r?\n', 'g'), '<br />'));
        var previewHtml = body.val();
        $('#preview-body-text').html(previewHtml);
        $('#preview-title-text').empty();
        $('#preview-title-text').append($('#Title').val());

        var encodedValue = encodeURI($('#image-partial').attr('src'));
        $('#Image').val(encodedValue);
    });
   

    $('#Title').on('blur', function () {
        $('#preview-title-text').empty();
        $('#preview-title-text').append($('#Title').val());
    });

    imageField.click(function saveImage(event) {
        var file = $("#image-data")[0].files[0];
        var reader = new FileReader();
        var result = reader.readAsDataURL(file);

        reader.onloadend = function (event) {
            var imageB64 = reader.result;
            var imageName = "image-name";

            $.ajax({
                url: '/Poems/SaveImage',
                contentType:"application/json",
                method: 'POST',
                data: JSON.stringify({ imageData: imageB64, name:file.name }),
                success: function (response) {
                    $("#image-preview").html(response);
                    $("#modal-image-preview").html(response);
                    $("#Body").focus(); 
                },
                error: function (response) {
                    alert('Error: ' + response.statusText);
                }
            });
        }
    });

});