"use strict";

$(function () {
    var $form = $('#loginForm');

    $form.find('.roomBtn').on('click', function (event) {
        event.preventDefault();

        // this = clicked button
        handleRoomButtonClicked(this);
    });

});

function handleRoomButtonClicked(button) {
    var $btn = $(button);
    var $form = $('#loginForm');

    if ($form.valid()) {
        var data = $form.serialize();
        // append button value to form data
        data += "&" + $btn.attr('name') + "=" + $btn.val();
        var url = $form.data('ajax');

        performRegisterUserRequest(url, data);
    } else {
        console.log("Better fix those validations.");
    }
}

function performRegisterUserRequest(url, formData) {
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
    }).done(function (response) {
        if (response.success) {
            onLoginSuccess(response.data);
        } else {
            onLoginFailed();
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        onLoginFailed(errorThrown);
    });
}

function onLoginFailed(reason) {
    alert("Failed to login to room. Reason:\n" + reason);
    console.error("Unexpected error in login. Reason:", reason);
}

function onLoginSuccess(data) {
    var userId = data.userId;
    var roomId = data.roomId;
    var roomName = data.roomName;
    console.log("Logged in with user id:", userId, "\nIn room:", roomName);

    roomShow(data);

    createCookie("kosystem-userId", userId);
    createCookie("kosystem-roomId", roomId);
}
