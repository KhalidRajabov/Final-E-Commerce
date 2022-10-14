"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl(`/dashboardHub`, {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
}).build();

$(function () {
    connection.start().then(function () {
        InvokeProducts();
    }).catch(function (err) {
        return console.error(err.toString());
    });
});


function InvokeProducts() {
    connection.invoke("SendPendings").catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceivedPending", function (products) {
    BindProductsToGrid(products);
});

function BindProductsToGrid(products) {
    $('#user-table tbody').empty();

    var tr;

    $.each(products, function (index, product) {
        tr = $('<tr/>');
        tr.append(`<td>${index + 1}</td>`);
        tr.append(`<td><a class='text-dark' href='home/detail/${product.id}'>${product.name}</a></td>`);
        tr.append(`<td>${product.price}</td>`);
        tr.append(`<td>${product.sold}</td>`);
        tr.append(`<td>${product.profit}</td>`);
        tr.append(`<td>${product.count}</td>`);
        $('#user-table').append(tr);
        console.log(product.id)
    });

}