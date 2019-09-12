"use strict";

/**
 * @typedef {object} RoomData
 * @property {string} userName
 * @property {string} userId
 * @property {string} roomName
 * @property {string} roomId
 */

/**
 * @param {RoomData} data
 */
function roomShow(data) {
    $('#loginForm input, #loginForm button').attr('disabled', 'disabled');
    $('#roomDiv').slideDown();
    $('#loginDiv').slideUp();
    $('#roomName').text(roomName);

    $('.container-background').animate({
        'background-position-x': '100%',
        'background-position-y': '50%',
    }, 1200, "swing");
}

function roomHide() {
    $('#loginForm input, #loginForm button').removeAttr('disabled');
    $('#roomDiv').slideUp();
    $('#loginDiv').slideDown();

    $('.container-background').animate({
        'background-position-x': '0%',
        'background-position-y': '50%',
    }, 1200, "swing");
}