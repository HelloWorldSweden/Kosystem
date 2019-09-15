"use strict";

/**
 * @typedef {object} RoomData
 * @property {string} userName
 * @property {string} userId
 * @property {string} roomName
 * @property {string} roomId
 */

const slideFadeHide = (elem) => {
    const fade = { opacity: 0, transition: 'opacity 0.5s' };
    elem.css(fade).slideUp();
}
const slideFadeShow = (elem) => {
    const fade = { opacity: 1, transition: 'opacity 0.5s' };
    elem.css(fade).slideDown();
}

/**
 * @param {RoomData} data
 */
function roomShow(data) {
    $('#loginForm input, #loginForm button').attr('disabled', 'disabled');
    $('#roomName').text(data.roomName);

    slideFadeHide($('#loginDiv'));
    slideFadeShow($('#roomDiv'));

    $('.container-background').animate({
        'background-position-x': '100%',
        'background-position-y': '50%',
    }, 600, "swing");
}

function roomHide() {
    $('#loginForm input, #loginForm button').removeAttr('disabled');

    slideFadeShow($('#loginDiv'));
    slideFadeHide($('#roomDiv'));

    $('.container-background').animate({
        'background-position-x': '0%',
        'background-position-y': '50%',
    }, 600, "swing");
}