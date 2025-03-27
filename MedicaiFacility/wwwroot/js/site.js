"use strict";

document.addEventListener("DOMContentLoaded", function () {
    // Kiểm tra nếu đang ở trang MyAppointments
    if (window.location.pathname.toLowerCase().includes("/appointments/myappointments")) {
        var connection = new signalR.HubConnectionBuilder()
            .withUrl("/signalRServer")
            .build();

        connection.on("ReceiveDeletedItem", function () {
            // Reload trang khi nhận được sự kiện
            location.href = '/Appointments/MyAppointments';
        });

        connection.start().catch(function (err) {
            return console.error(err.toString());
        });
    }
});