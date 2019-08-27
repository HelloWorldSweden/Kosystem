"use strict";

/**
 * @param {{userName:string, userId:string, roomName:string, roomId:string}} data
 */
function roomShow(data) {
    $('#loginForm input, #loginForm button').attr('disabled', 'disabled');
    $('#roomDiv').slideDown();
    $('#roomName').text(roomName);
}

function roomHide() {

}