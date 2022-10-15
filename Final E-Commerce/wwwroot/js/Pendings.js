"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl(`/pendingsHub`, {
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
    $('#pending-table tbody').empty();
    var tr;
    console.log('binding works');
    $.each(products, function (index, product) {
        tr = $('<tr/>');
        tr.append(`<td>${index + 1}</td>`);
        tr.append(`<td><a class='text-dark col-8' href='https://localhost:44393/Admin/product/detail/${product.id}'>${product.name}</a></td>`);

        /*tr.append(`<td>
            <a class="btn btn-danger declineProduct fs-5" data-id='${product.id}' >x</a>
            <a class="btn btn-warning fs-5" href="https://localhost:44393/Admin/product/detail/${product.id}"><i class="mdi mdi-information-outline"></i></a>
            <a class="btn btn-success confirmProduct fs-5" data-id='${product.id}' ><i class="text-light mdi mdi-check"></i></a>
                </td>`);
        */

        let td = document.createElement("td");
        let icon = document.createElement("i");
        let link = document.createElement("a");
        link.classList.add("btn", "btn-success", "confirmProduct", "fs-5")
        link.setAttribute("data-id", product.id);
        icon.classList.add("text-light", "mdi", "mdi-check");
        link.append(icon);
        td.append(link);
        tr.append(td);
        $('#pending-table').append(tr);
    });
}
let confirm = document.querySelectorAll(".confirmProduct")
confirm.forEach(add =>

    add.addEventListener("click", function () {
        alert("click works")
        var productUrl = `https://localhost:44393/admin/product`
        let id = $(this).attr("data-id");
        $.ajax({
            url: `${productUrl}/confirm?id=` + id,
            method: "get",
            success: function (res) {
                alert(res.status)
            },
            error: function (err) {
                console.log("error ", err)
            }
        })
    })
);



var postBoxes = document.querySelectorAll('.confirmProduct')
postBoxes.forEach(function (postBox) {

    postBox.addEventListener('click', function () {
        alert("clicked");
    })
});