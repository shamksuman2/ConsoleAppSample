"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:7071/api/").build();
var connection = new signalR.HubConnectionBuilder().withUrl("https://functionappeventhubsignalrfoodtemp.azurewebsites.net/api/").build();
//Disable send button until connection is established


connection.on("Transaction", function (objs) {
    console.error("received...");
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //var encodedMsg = message;// user + " says " + msg;
    //var li = document.createElement("li");
    //li.textContent = encodedMsg;

    for (var i = 0; i < 15; i++) {
        $('#txt' + i).html(objs.MsgsBody[i].Temperature);
    }
    
    //for (var i in cards) {
    //    document.getElementById("11").innerText(item.Id);
    //    document.getElementById("12").innerText(item.FleetRoute);
    //    document.getElementById("13").innerText(item.DriverName);
    //    document.getElementById("14").innerText(item.VehicleId);
    //}
});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});