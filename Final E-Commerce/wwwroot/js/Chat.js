(function ($) {

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


    connection.on("ReceiveMessage", function (user, message) {
        var li = document.createElement("li");
        li.classList.add("col-12", "mt-1", "d-flex", "flex-column", "text-start")
        var div = document.createElement("div");
        div.classList.add("d-flex", "flex-column", "bg-secondary", "col-8", "text-info", "p-2", "align-self-start")
        var span = document.createElement("span");
        span.innerText = message;
        div.append(span);
        li.append(div);
        document.getElementById("messagesList").prepend(li);
        let audio = document.getElementById("incomingMsg");
        audio.play();
        console.log("receive message method")
    });



    connection.start().then(function () {
        connection.invoke("GetConnectionId");
    }).catch(function (err) {
        return console.error(err.toString());
    });

document.getElementById("sendToUser").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var noMessage = document.getElementById("zeroMessage");
    var receiverConnectionId = document.getElementById("receiverId").value;
    var message = document.getElementById("messageInput");
    var li = document.createElement("li");
    li.classList.add("col-12", "mt-1", "d-flex", "flex-column", "text-end")
    var div = document.createElement("div");
    div.classList.add("d-flex", "flex-column", "bg-success", "col-8", "text-light", "p-2", "align-self-end")
    var span = document.createElement("span");
    span.innerText = message.value;
    div.append(span);
    li.append(div);
    document.getElementById("messagesList").prepend(li);
    connection.invoke("SendToUser", user, receiverConnectionId, message.value).catch(function (err) {
        return console.error(err.toString());
    });
    let audio = document.getElementById("outMsg");
    audio.play();
    event.preventDefault();
    
    if (receiverConnectionId.length > 0 && message.value.length>0) {
        let noMessage = document.getElementById("zeroMessage");
        if (noMessage != null || noMessage != undefined) {
            noMessage.remove();
        }
        event.preventDefault();
        axios.post("http://rammkhalid-001-site1.itempurl.com/messages/send?text=" + message.value + "&receiverId=" + receiverConnectionId)
            .then(function (response) {
                console.log(response)
            })
            .catch(function (error) {
                console.log("error", error);
            })
    }
    else if (message.value.length<=0) {
        alert("Message can not be empty");
    }

    if (noMessage!=null) {
        noMessage.remove();
    }
    message.value = "";
    
});
})(jQuery);