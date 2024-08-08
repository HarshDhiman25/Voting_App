"use strict";

// Establish a connection with SignalR
var connection = new signalR.HubConnectionBuilder().withUrl("/voteHub").build();

// Start the connection and handle any errors
connection.start().catch(function (err) {
    return console.error(err.toString());
});

// Handle the SignalR message to update the Vote Index page
connection.on("ReceiveVoteUpdate", function () {
    // Refresh or update the Vote Index page
    // You can use a method to fetch updated votes or just reload the page
    location.reload(); // Reloads the page to reflect the latest votes
});
