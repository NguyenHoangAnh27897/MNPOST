function showModel(id) {
    $('#' + id).modal('show');
}

function hideModel(id) {
    $('#' + id).modal('hide');
}


function submitForm(id) {
    $("#loader").css("display", "block");
    $('#' + id).submit();
}

function showLoader(isShow) {

    if (isShow) {
        $("#loader").css("display", "block");
    } else {
        $("#loader").css("display", "none");
    }
}

function fsubmit(msg) {
    if (!confirm(msg)) {
        return false;
    } else {
        showLoader(true);
    }
}

function fsubmit(msg,id) {
    if (!confirm(msg)) {
        return false;
    } else {
        showLoader(true);
        hideModel(id);
    }
}