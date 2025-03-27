"use strict";

document.addEventListener("DOMContentLoaded", function () {
    // Tạo connection SignalR chung
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalRServer")
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Xử lý cho trang MyAppointments
    if (window.location.pathname.toLowerCase().includes("/appointments/myappointments")) {
        connection.on("ReceiveDeletedItem", function () {
            location.reload(); // Reload trang mượt mà hơn
        });
    }

    // Xử lý cho trang MyMedicalHistory
    if (window.location.pathname.toLowerCase().includes("/medicalhistories/mymedicalhistory")) {
        connection.on("ReceiveMedicalHistoryUpdate", function () {
            // Thay vì reload toàn trang, ta sẽ cập nhật DOM realtime
            fetchMedicalHistories();
        });
    }

    // Bắt đầu kết nối
    async function startConnection() {
        try {
            await connection.start();
            console.log("SignalR Connected");
        } catch (err) {
            console.error("SignalR Connection Error: ", err);
            setTimeout(startConnection, 5000); // Thử lại sau 5s nếu lỗi
        }
    }

    // Xử lý khi mất kết nối
    connection.onclose(async () => {
        await startConnection();
    });

    // Hàm lấy dữ liệu lịch sử y tế mới
    async function fetchMedicalHistories() {
        try {
            const response = await fetch('/MedicalHistories/MyMedicalHistory', {
                method: 'GET',
                headers: {
                    'Accept': 'text/html'
                }
            });
            const data = await response.text();
            // Cập nhật bảng mà không reload trang
            const parser = new DOMParser();
            const doc = parser.parseFromString(data, 'text/html');
            const newTable = doc.querySelector('.table');
            if (newTable) {
                document.querySelector('.table').outerHTML = newTable.outerHTML;
            }
        } catch (error) {
            console.error("Error fetching medical histories:", error);
        }
    }

    // Khởi động kết nối
    startConnection();
});