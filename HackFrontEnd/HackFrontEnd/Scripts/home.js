$(document).ready(function() {
    function sendAlerts() {
        $.post(document._appPath + "home/confirmed", function(data) {
            alert(data);
        });
    }

    $('#ConfirmButton').click(function() {
        sendAlerts();
    });
});