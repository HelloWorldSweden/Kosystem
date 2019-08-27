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
