$(function () {
    var messenger = $.connection.messengerHub;
    messenger.client.sendMessageToAdmin = function (total, titleTotal, html, isSound) {
        if ($("#quantity-total-messenger").length > 0) {
            $("#quantity-total-messenger").text(total);
            $("#quantity-total-messenger-p").text(titleTotal);
        }
        if ($("#content-message").length > 0) {
            $("#content-message").html(html);
        }
        if (isSound) {
            $('#notification-ring-tone')[0].play();
        }
    };

    $.connection.hub.start().done(function () {
       
    });
})