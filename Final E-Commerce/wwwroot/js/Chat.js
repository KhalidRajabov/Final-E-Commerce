(function ($) {
    //defineing connection
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();



    var element = document.getElementById("messagesList");
    element.scrollTop = element.scrollHeight;

    //if you have unread message by a user, when he joins this chat page and you are also in the page, 
    //it means he already read your message and you get notified about it by getting closed eye icon opened
    connection.on("MessageRead", function (user, read) {
        if (read) {
            console.log("message read")


            var unreadIcons = document.querySelectorAll(".fa-eye-slash");
            unreadIcons.forEach((i) => {
                i.classList.remove("fa-eye-slash");
                i.classList.add("fa-eye")
            })
        }
    });

    var typeCount = 0;
    function clearTypingList() {
        var typeList = document.getElementById("typing");
        typeCount = 0;
        if (typeList != null) {
            typeList.remove();
        }
    }
    //if other user is typing, we should know that he is typing.
    connection.on("MessageTyping", function (user, typing) {
        typeCount += 1;
        console.log(typeCount)
        if (typeCount==1) {
            //add "typing" to somewhere
            var li = document.createElement("li");
            
            li.classList.add("col-12", "mt-1", "d-flex", "flex-column", "text-start")
            var div = document.createElement("div");
            div.classList.add("d-flex", "flex-column", "bg-secondary", "col-8", "text-info", "p-2", "align-self-start");
            let span = document.createElement("span");
            span.innerText = user + " typing...";
            div.appendChild(span);
            div.style.borderRadius = "10px";
            li.setAttribute("id", "typing");

            li.appendChild(div);
            document.getElementById("messagesList").appendChild(li);
            console.log(user, " typing");
            element.scrollTop = element.scrollHeight;

            setInterval(clearTypingList, 5000);
        }
    });

    


    connection.on("ReceiveMessage", function (user, message) {
        var li = document.createElement("li");
        li.classList.add("col-12", "mt-1", "d-flex", "flex-column", "text-start")
        var div = document.createElement("div");
        div.classList.add("d-flex", "flex-column", "bg-secondary", "col-8", "text-info", "p-2", "align-self-start")
        var span = document.createElement("span");
        span.style.wordBreak = "break-word";
        span.innerText = message;
        var span2 = document.createElement("span");
        var m = new Date();
        var dateString =
            m.getUTCFullYear() + "/" +
            ("0" + (m.getUTCMonth() + 1)).slice(-2) + "/" +
            ("0" + m.getUTCDate()).slice(-2) + " " +
            ("0" + m.getHours()).slice(-2) + ":" +
            ("0" + m.getUTCMinutes()).slice(-2) + ":" +
            ("0" + m.getUTCSeconds()).slice(-2);
        span2.innerText = dateString;
        span2.style.fontSize = "70%";
        div.append(span);
        div.append(span2);
        div.style.borderRadius = "10px";
        li.append(div);
        document.getElementById("messagesList").appendChild(li);
        let audio = document.getElementById("incomingMsg");
        audio.play();
        console.log("receive message method");
        element.scrollTop = element.scrollHeight;



        //if you are both in the same chat page, we sent other user notification that you just read
        //the message he sent you
        var read = true;
        var receiverConnectionId = document.getElementById("receiverId").value;
        connection.invoke("Read", user, receiverConnectionId, read).catch(function (err) {
            return console.error(err.toString());
        });
    });



    //when connecting the chat page
    connection.start().then(function () {


        connection.invoke("GetConnectionId");
        var user = document.getElementById("userInput").value;
        var read = true;
        var receiverConnectionId = document.getElementById("receiverId").value;
        connection.invoke("Read", user, receiverConnectionId, read).catch(function (err) {
            return console.error(err.toString());
        });
    }).catch(function (err) {
        return console.error(err.toString());
    });

    var input = document.getElementById("messageInput");
    input.addEventListener("keyup", function (event) {
        var user = document.getElementById("userInput").value;
        var receiverConnectionId = document.getElementById("receiverId").value;
        var typing = true;
        connection.invoke("Typing", user, receiverConnectionId, typing).catch(function (err) {
            return console.error(err.toString());
        });

        if (event.keyCode === 13) {
            event.preventDefault();
            document.getElementById("sendToUser").click();
        }
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
    span.style.wordBreak = "break-word";
    span.innerText = message.value;
    span.style.marginRight = "15px";
    var span2 = document.createElement("span");
    var m = new Date();
    var dateString =
        m.getUTCFullYear() + "/" +
        ("0" + (m.getUTCMonth() + 1)).slice(-2) + "/" +
        ("0" + m.getUTCDate()).slice(-2) + " " +
        ("0" + m.getHours()).slice(-2) + ":" +
        ("0" + m.getUTCMinutes()).slice(-2) + ":" +
        ("0" + m.getUTCSeconds()).slice(-2);
    span2.innerText = dateString;
    span2.style.fontSize = "70%";
    var div2 = document.createElement("div");
    div.append(span);
    div2.append(span2);
    div2.classList.add("col-12");
    var unreadIcon = document.createElement("i");
    unreadIcon.classList.add("fa-solid", "fa-eye-slash");
    div2.appendChild(unreadIcon);
    div.appendChild(div2);
    div.style.borderRadius = "10px";
    li.append(div);
    
    document.getElementById("messagesList").appendChild(li);
    element.scrollTop = element.scrollHeight;
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
        //https://localhost:44393/
        //http://rammkhalid-001-site1.itempurl.com/
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