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
    $('#roomName').text(roomName);
}

function roomHide() {

}