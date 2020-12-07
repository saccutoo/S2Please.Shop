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
            var audio = new Audio('../SoundNotification/Notification-ring-tone.mp3');
            var playPromise = audio.play();
        }
    };

    messenger.client.reloadContentMessageAdmin = function (sessionId, userId, content) {
        if ($("#session-id").length > 0 && $("#session-id").val() == sessionId) {
            $('#content-all').html(content);
            $('#content-value').val("");
        }
    };


    $.connection.hub.start().done(function () {
        $("#btn-comment-more").click(function () {
            messenger.server.sendMessageToAdmin(false);
        });

        $("#btn-send-message-admin").click(function () {
            var userId = $('#current-user-id').val();
            var content = $('#content-value').val();
            var sessionId = $("#session-id").val()
            if (content != '' && content != null && content != undefined) {
                messenger.server.sendMessageFromAdmin(userId, content, sessionId);
            } 
        });

        $('#content-value').keyup(function (e) {
            if (e.keyCode == 13) {
                var userId = $('#current-user-id').val();
                var content = $('#content-value').val();
                var sessionId = $("#session-id").val()
                if (content != '' && content != null && content != undefined) {
                    messenger.server.sendMessageFromAdmin(userId, content, sessionId);
                }              
            }
        });
    });

    $('.chat_list').click(function () {

        $('.chat_list').removeClass("active_chat");
        $(this).addClass('active_chat');
        var sessionId = $(this)[0].getAttribute('value-id');

        $.ajax(
            {
                type: "POST",
                url: "/ADMIN/Notification/ContentMessenger",
                data: {
                    sessionId: sessionId
                },
                success: function (response) {
                    $('#content-all').html(response.html);
                    $('#total-' + sessionId).text("0");
                    messenger.server.sendMessageToAdmin(false);
                }
            });
    });

})
