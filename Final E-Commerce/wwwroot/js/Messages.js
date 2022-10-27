/*"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl(`/messagesHub`, {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets}).build();

$(function () {
    connection.start().then(function () {
        InvokeProducts();
    }).catch(function (err) {
        return console.error(err.toString());
    });
});


function InvokeProducts() {
    connection.invoke("SendMessages").catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceivedMessages", function (messages) {
    BindProductsToGrid(messages);
});



function BindProductsToGrid(messages) {
    $('#newmessages').empty();
    var a;
    
    $.each(messages, function (index, message) {
        a = $('<a/>');
        let audio = document.getElementById("messageaudio");
        audio.play();
        a.html('')
        //$('#newmessages').append();
        console.log(message, index)    

        
        //console.log(product.id);
    });
    
}


*/