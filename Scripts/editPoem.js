$(document).ready(function () {
    var body = $('#Body');
    var imageField = $('#save-image');
    var audioField = $('#save-audio');
    var previewHtml = body.val();
    $('#preview-body-text').html(previewHtml);
    $('#preview-title-text').empty();
    $('#preview-title-text').append($('#Title').val());
    var imageEdit = false;
    var audioEdit = false;
    $('#Audio').val($('Audio').val());

    var audioContent = '<div class="audio"><audio controls><source id="audio-edit-preview" src="" type="audio/mp4" codec="mp4"/></audio>';

    var encodedValue = encodeURI($('#image-partial').attr('src'));
    $('#Image').val(encodedValue);

    var image = $('#Image').val(); 
    $('#modal-image-preview').html("<img id='image-edit-preview' />");
    $('#image-edit-preview').attr('src', 'http://az806808.vo.msecnd.net/images/' + image);
    var audio = $('#Audio').val();
    $('#modal-audio-preview').html(audioContent);
    $('#audio-edit-preview').attr('src', 'http://az806808.vo.msecnd.net/audio/' + audio);

    $("#Audio").hide();

    body.on('blur', function () {
        body.val(body.val().replace(new RegExp('\r?\n', 'g'), '<br />'));
        var previewHtml = body.val();
        $('#preview-body-text').html(previewHtml);
        $('#preview-title-text').empty();
        $('#preview-title-text').append($('#Title').val());
    });

    $('#edit-image-toggle')
        .click(function() {
            if (!imageEdit) {
                $('#edit-image-preview').hide();
                $('#image-form').show();
                $('#edit-image-toggle').text("Cancel Image Edit");
                imageEdit = true;
            } else {
                $('#edit-image-preview').show();
                $('#image-form').hide();
                $('#edit-image-toggle').text("Edit Image");
                imageEdit = false;
            }
        });


    $('#edit-audio-toggle')
        .click(function () {
            if (!audioEdit) {
                $('#edit-audio-preview').hide();
                $('#audio-form').show();
                $('#edit-audio-toggle').text("Cancel Audio Edit");
                audioEdit = true;
            } else {
                $('#edit-audio-preview').show();
                $('#audio-form').hide();
                $('#edit-audio-toggle').text("Edit Audio");
                audioEdit = false;
            }
        });

    $('#Title').on('blur', function () {
        $('#preview-title-text').empty();
        $('#preview-title-text').append($('#Title').val());
    });

    imageField.click(function saveImage() {
        var file = $("#image-data")[0].files[0];
        var reader = new FileReader();
        var result = reader.readAsDataURL(file);

        reader.onloadend = function () {
            var imageB64 = reader.result;
            var imageName = "image-name";

            $.ajax({
                url: '/Poems/SaveImage',
                contentType: "application/json",
                method: 'POST',
                data: JSON.stringify({ imageData: imageB64, name: file.name }),
                success: function (response) {
                    $("#modal-image-preview").html('');
                    $("#image-preview").html(response);
                    $("#modal-image-preview").html(response);
                    $("#Body").focus();
                    var encodedValue2 = encodeURI($('#image-partial').attr('src'));
                    $('#Image').val(encodedValue2);
                },
                error: function (response) {
                    alert('Error: ' + response.statusText);
                }
            });
        }
    });

    audioField.click(function () {
        var file = $("#audio-data")[0].files[0];
        var reader = new FileReader();
        reader.readAsDataURL(file);

        reader.onloadend = function () {
            var audioB64 = reader.result;
            
            $.ajax({
                url: '/Poems/SaveAudioFile',
                contentType: "application/json",
                method: 'POST',
                data: JSON.stringify({ imageData: audioB64, name: file.name }),
                success: function (response) {
                    $("#modal-audio-preview").html('');
                    $("#audio-preview").html(response);
                    $("#modal-audio-preview").html(response);
                    $("#Body").focus();
                    var encodedValue2 = encodeURI($('#audio-partial').attr('src'));
                    $('#Audio').val(encodedValue2);

                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
    });

});