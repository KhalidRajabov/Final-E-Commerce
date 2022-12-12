(function ($) {
    //defineing connection
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();



    var element = document.getElementById("messagesList");
    element.scrollTop = element.scrollHeight;

    

    //if other user is typing, we should know that he is typing.
    //we need a global sign in functions to know whether user is still typing or not.
    //we have a variable named typeCount, everytime other user is pressing a key, variable becomes 1
    //after each key pressed, we start counting down to 5 seconds
    //after 5 seconds if no key wa pressed by other user, we make the variable 0
    // if the value of variable is 1, it means other user is typing, so we add "typing" message to the chat
    //if the value is 0, we delete the "typing" message
    var typeCount = 0;
    function clearTypingList() {
        var typeList = document.getElementById("typing");
        typeCount = 0;
        if (typeList != null) {
            typeList.remove();
        }
    }
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

    


    //when the other user is in the same chat page with us, messages he writes directly appears on our messages.
    //user's username and message is sent to us with "ReceiveMessage" through chathub.
    connection.on("ReceiveMessage", function (user, message) {

        //first we delete
        var typing = document.getElementById("typing");
        typing.remove();

        //dessigning the message, adding bootstrap classes and css style

        /*designing message starts*/
        var li = document.createElement("li");
        li.classList.add("col-12", "mt-1", "d-flex", "flex-column", "text-start")
        var div = document.createElement("div");
        div.classList.add("d-flex", "flex-column", "bg-secondary", "col-8", "text-info", "p-2", "align-self-start")
        var span = document.createElement("span");
        span.style.wordBreak = "break-word";
        span.innerText = message;
        var span2 = document.createElement("span");

        //getting current time
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
        //adding message to message list
        document.getElementById("messagesList").appendChild(li);
        /*designing message ends*/



        //playing a sound for incoming message and it only plays if the user is connected to the chat page
        //if user joins later, they will not hear this sound play which is normal.
        let audio = document.getElementById("incomingMsg");
        audio.play();
        console.log("receive message method");

        //bring scroll to the last message
        element.scrollTop = element.scrollHeight;



        //if you are both in the same chat page, we sent other user notification that.......
        //you just read the message he sent you
        var read = true;
        var receiverId = document.getElementById("receiverId").value;
        connection.invoke("Read", user, receiverId, read).catch(function (err) {
            return console.error(err.toString());
        });



        //https://localhost:44393/
        //http://rammkhalid-001-site1.itempurl.com/


        //now we send message to our controller which will save messages, time and relation between users.
        var myUserName = document.getElementById("userInput").value;
        axios.post("https://localhost:44393/messages/MessageRead?username=" + myUserName + "&receiverId=" + receiverId)
            .then(function (response) {
                console.log(response)
            })
            .catch(function (error) {
                console.log("error", error);
            })
    });


    //if you have unread message by a user, when he joins this chat page and you are also in the page, 
    //it means he already read your message and you get notified about it by getting closed eye icon opened
    connection.on("MessageRead", function (user, read) {
        //if the other user is in the same page, it means he is connected to the chat, and sees our message
        if (read) {
            console.log("message read")
            var unreadIcons = document.querySelectorAll(".fa-eye-slash");
            unreadIcons.forEach((i) => {
                //eye closed icon changes to eye open
                i.classList.remove("fa-eye-slash");
                i.classList.add("fa-eye")
            })
        }
    });


    //when connecting the chat page
    connection.start().then(function () {

        //starting a connection and declaring a connection id for the user joined
        connection.invoke("GetConnectionId");

        //our user name is sent to hub from hidden html input in html page
        var user = document.getElementById("userInput").value;

        //we create a variable which means we are in the chat page, so we read any message he sent us 
        //then we will send this sign to other user so that if they are in chat, they will know that we saw their message
        var read = true;
        //we write other user's id in the html page a hidden input, with the id of receiverId
        //we do it from both user's page. one user's id is sent to hub from other user. 
        //so hub understands who sent it, and where it needs to send the data.
        var receiverConnectionId = document.getElementById("receiverId").value;

        //invoking read method from chathub which will let other user know we read their message.
        connection.invoke("Read", user, receiverConnectionId, read).catch(function (err) {
            return console.error(err.toString());
        });
        //in case of error:
    }).catch(function (err) {
        return console.error(err.toString());
    });


    //input our message is written in
    var input = document.getElementById("messageInput");

    //everytime we press a key we send other user a sign that we write something.
    input.addEventListener("keyup", function (event) {

        //we take our username from hidden html input which users can not see.
        var user = document.getElementById("userInput").value;

        //we get other user's id and invoke "Typing" method in hub
        var receiverConnectionId = document.getElementById("receiverId").value;
        var typing = true;
        //when we invoke "Typing", we sent our name, true valued variable and receiver's id to the hub,
        //hub sends our name and variable to the user with Id of receiverId we sent
        //method in up lets other user know we are typing.
        connection.invoke("Typing", user, receiverConnectionId, typing).catch(function (err) {
            return console.error(err.toString());
        });


        //when writing something in message input, we "click" on send button by pressing Enter key
        if (event.keyCode === 13) {
            event.preventDefault();
            document.getElementById("sendToUser").click();
        }
    });


    //when clicking send button or pressing Enter key while typing a message in input, we activate the method below:
document.getElementById("sendToUser").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;

    //this variable is the name of html section which means that there is no message between two users yet.
    //we will delete it when we send our message
    var noMessage = document.getElementById("zeroMessage");


    //getting other user's id from hidden html input element
    var receiverConnectionId = document.getElementById("receiverId").value;

    //our message input
    var message = document.getElementById("messageInput");


    //designing our message to be directly show in our message list


    /*designing message starts*/
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
    //adding message to message list
    document.getElementById("messagesList").appendChild(li);
    /*designing the message ends*/


    //keeps scroll on the last message in the chat
    element.scrollTop = element.scrollHeight;


    //the method for sending other user from hub is invoking
    //we send to the hub our username, our message and other user's Id. Hub sends our message to...
    //the user with the same id we sent to hub and also the user which is connected to the chat page
    //If other user is connected he will directly see, if no, will see when they join, becase...
    //our messages and receiver Id are also send to the controller which will save it to database..
    //so anytime user opens chat page, he will see our messages.
    
    connection.invoke("SendToUser", user, receiverConnectionId, message.value).catch(function (err) {
        return console.error(err.toString());
    });

    //"message sent" sound is being played when we send the message
    let audio = document.getElementById("outMsg");
    audio.play();

    //for us not to be redirected to anywhere.
    event.preventDefault();


    //if we have our receiverId and message in input, we send the message
    if (receiverConnectionId.length > 0 && message.value.length>0) {

        //this variable is an html element which means there has not been any conversation between these two users.
        //we need to delete it because we already started a conversation
        let noMessage = document.getElementById("zeroMessage");
        if (noMessage != null || noMessage != undefined) {
            noMessage.remove();
        }
        


        //https://localhost:44393/
        //http://rammkhalid-001-site1.itempurl.com/


        //now we send message to our controller which will save messages, time and relation between users.
        axios.post("https://localhost:44393/messages/send?text=" + message.value + "&receiverId=" + receiverConnectionId)
            .then(function (response) {
                console.log(response)
            })
            .catch(function (error) {
                console.log("error", error);
            })
    }

    //if we don't have any text in message input, it gives alert,
    else if (message.value.length<=0) {
        alert("Message can not be empty");
    }

    //clearing message input after sending
    message.value = "";

    
});

    
})(jQuery);