function imdbSearch(imdbid) {
    if (imdbid.trim() != "") {
        window.location.href = '?imdb=' + encodeURI(imdbid);
    }
}

function fillForm() {
    document.getElementById('movie_title').value = decodeURI(document.getElementById('titleRaw').innerHTML).replace(/&amp;/g, '&');
    document.getElementById('movie_description').value = decodeURI(document.getElementById('descriptionRaw').innerHTML).replace(/&amp;/g, '&');
    document.getElementById('movie_runtime').value = document.getElementById('runtimeRaw').innerHTML;
    document.getElementById('movie_release_date').value = document.getElementById('releaseDateRaw').innerHTML;
    setSelectedValue(document.getElementById("genre_id"), document.getElementById('genreRaw').innerHTML);
    document.getElementById('imdb_id').value = document.getElementById('imdbIdRaw').innerHTML;
    document.getElementById('imdb_poster').value = document.getElementById('imdbPosterRaw').innerHTML;
}

function setSelectedValue(selectObj, valueToSet) {
    for (var i = 0; i < selectObj.options.length; i++) {
        if (selectObj.options[i].text == valueToSet) {
            selectObj.options[i].selected = true;
            return;
        }
    }
}

enableHost = function (val) {
    var btnHost = document.getElementById("host");

    if (val.checked == true) {
        btnHost.disabled = false;
    }
    else {
        btnHost.disabled = true;
    }
}