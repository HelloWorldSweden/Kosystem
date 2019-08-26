// main.js

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/kohub").build();

connection.on("JoinRoomAck", function (foo) {
    console.log("JoinRoomAck:", foo);
});

connection.start().then(function () {
    console.log("Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});

$('#loginForm .roomBtn').on('click', function (event) {
    event.preventDefault();

    var $btn = $(this);
    var $form = $('#loginForm');
    if ($form.valid()) {
        var data = $form.serialize();
        data += "&" + $btn.attr('name') + "=" + $btn.val();

        $.ajax({
            type: 'POST',
            url: $form.data('ajax'),
            data: data,
        }).done(function (response) {
            if (response.success) {
                var userId = response.data.userId;
                var roomName = response.data.roomName;
                console.log("User id:", userId);

                $('#loginForm input, #loginForm button').attr('disabled', 'disabled');
                $('#roomDiv').slideDown();
                $('#roomName').text(roomName);

                createCookie("kosystem-userId", userId);
            } else {
                console.error("Unexpected in login response", response);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error("Error during login", errorThrown, "Response:", jqXHR.responseJSON || jqXHR.responseText);
        });
    } else {
        console.log("Better fix those validations.");
    }
});

function createCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0)
            return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}