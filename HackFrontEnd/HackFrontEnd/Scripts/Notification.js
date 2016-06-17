$(document).ready(function() {
    function sendAlerts() {
        $.post(document._appPath + "notification/confirmed", function(data) {
            alert(data);
        });
    }

    $('#ConfirmButton').click(function() {
        sendAlerts();
    });
});