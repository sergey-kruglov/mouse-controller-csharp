// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/mouseControllerHub").build();

//Disable the send button until connection is established.
var touchZone = document.getElementById("touch-zone");
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

let clientX, clientY;

touchZone.addEventListener('click', (e) => {
    connection.invoke("Click").catch(function (err) {
        return console.error(err.toString());
    });
})

touchZone.addEventListener('touchstart', async (e) => {
    clientX = e.touches['0'].pageX;
    clientY = e.touches['0'].pageY;
})

touchZone.addEventListener("touchmove", function (event) {
    const newX = event.touches['0'].pageX
    const newY = event.touches['0'].pageY

    const xDiff = (clientX - newX) / 1.2;
    const yDiff = (clientY - newY) / 1.2;

    clientX = newX
    clientY = newY

    connection.invoke("TouchMove", -Math.ceil(xDiff), -Math.ceil(yDiff)).catch(function (err) {
        return console.error(err.toString());
    });
});