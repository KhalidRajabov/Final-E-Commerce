(function ($) {

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    connection.invoke("GetConnectionId").then(function (id) {
        document.getElementById("connectionId").innerText = id;
    });
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

/*document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});*/

document.getElementById("sendToUser").addEventListener("click", function (event) {
    var receiverConnectionId = document.getElementById("receiverId").value;
    var message = document.getElementById("messageInput").value;
    if (receiverConnectionId != null && message != null) {
        connection.invoke("SendToUser", user, receiverConnectionId, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        axios.post("http://rammkhalid-001-site1.itempurl.com/messages/message?text=" + message + "&receiverId=" + receiverConnectionId)
            .then(function (response) {

                console.log("create method response: ", response)
            })
            .catch(function (error) {
                console.log(error);
            })

    }
    else if (receiverConnectionId != null && message == null) {
        alert("Message can not be empty");
    }
    else {
        alert("Receiver could not be found or some other error");
    }
});
})(jQuery);