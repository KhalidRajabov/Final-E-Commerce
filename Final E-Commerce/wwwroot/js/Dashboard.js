"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl(`/dashboardHub`, {
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
    connection.invoke("SendProducts").catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceivedProducts", function (products) {
    BindProductsToGrid(products);
});



function BindProductsToGrid(products) {
    $('#user-table tbody').empty();

    var totalProfit = 0;
    var totalViews = 0;
    var totalSold = 0;
    var tr;
    
    $.each(products, function (index, product) {
        tr = $('<tr/>');
        tr.append(`<td>${index + 1}</td>`);
        tr.append(`<td><a class='text-dark' href='admin/product/detail/${product.id}'>${product.name}</a></td>`);
        tr.append(`<td>${product.price}</td>`);
        tr.append(`<td>${product.sold}</td>`);
        tr.append(`<td>${product.profit}</td>`);
        tr.append(`<td>${product.count}</td>`);
        tr.append(`<td>${product.rating}</td>`);
        tr.append(`<td>${product.views}</td>`);
        $('#user-table').append(tr);
        totalProfit += product.profit;
        totalSold += product.sold;
        totalViews += product.views;


        $('#span1').text("$"+totalProfit);
        $('#span2').text(totalSold);
        $('#span3').text(totalViews);
        console.log(product.id);
    });
    console.log("profit: ", totalProfit);
    console.log("sold: ", totalSold);
    console.log("views: ", totalViews);
}


