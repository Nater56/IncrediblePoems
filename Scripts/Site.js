$(document).ready(function () {
    var children = $('.category-links').children();
    var poemImageTag = $('#poem1-img');



    if (poemImageTag.attr('src') === 'http://az806808.vo.msecnd.net/images/undefined' || poemImageTag.attr('src') === 'http://az806808.vo.msecnd.net/images/') {
            poemImageTag.hide();
        } else {
            poemImageTag.show();
        }



    children.each(function () {
        if($(this).text().toLowerCase().indexOf('limericks') > -1){
            $(this).parent().parent().addClass('hidden');
        }
    });

   
    $('#limerick-filter').click(function () {
        children.each(function () {
            if ($(this).text().toLowerCase().indexOf('limericks') > -1) {
                $(this).parent().parent().removeClass('hidden');
            }
            else{
                $(this).parent().parent().addClass('hidden');
            }
        });
    });

    $('#poem-filter').click(function () {
        children.each(function () {
            if ($(this).text().toLowerCase().indexOf('limericks') > -1) {
                $(this).parent().parent().addClass('hidden');
            }
            else{
                $(this).parent().parent().removeClass('hidden');
            }
        });
    });
});

var ShowPoemById = function (id) {

    var poemTitle = $('#' + id + 't').html();
    var poemBody = $('#' + id + 'b').html();
    var poemAuthor = $('#' + id + 'a').html();
    var poemAudio = $('#' + id).html();
    poemBody += poemAuthor; 
    
    var poemImage = $('#' + id + 'i').html();
    $('#cat-poem-title').html(poemTitle);
    $('#cat-poem-body').html(poemBody);

    if (poemImage === 'undefined' || poemImage === '') {
        $('#poem1-img').hide();
    } else {
        
        $('#poem1-img').attr('src', 'http://az806808.vo.msecnd.net/images/' + poemImage);
        $('#poem1-img').show();
    }
    if (poemAudio === 'undefined' || poemAudio === '')
        $('#audio-poem-body').hide();
    else {
        $('#audio-poem-body').html(poemAudio);
        $('#audio-poem-body').show();
    }
}