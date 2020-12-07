$(function () {
    var messenger = $.connection.messengerHub;
    messenger.client.addNewMessageToPage = function (customerName, email, phone, userCustomerId, html, chats, sessionId) {
        var customerNameView = $('#messenger-name').val();
        var emailView = $('#messenger-email').val();
        var userCustomerIdView = $('#messenger-id').val();
        var sessionId = $('#session-id').val();
        if (sessionId == sessionId) {
            $('#chat-content').append(html);
            $('#chat-content').show();
            $('#chat-publisher').show();
            $('#information-chat').hide();
            $.ajax({
                type: "POST",
                url: "/Base/SetCokkieChat",
                data: {
                    chats: chats
                },
                dataType: "json",
                success: function (response) {

                },
            });
        }
       
    };

    $.connection.hub.start().done(function () {
        $('#start-chat').click(function () {
            $("#form-information-chat label[name*='message-error-']").text("");
            $.ajax({
                type: "POST",
                url: "/Base/StartChat",
                data: $('#form-information-chat').serializeArray(),
                dataType: "json",
                success: function (response) {
                    if (response.Invalid) {
                        var validations = response.Result;
                        renderError(validations, "form-information-chat");
                    }
                    else if (!response.Invalid) {
                        var customerName = $('#messenger-name').val();
                        var email = $('#messenger-email').val();
                        var phone = $('#messenger-phone').val();
                        var userCustomerId = $('#messenger-id').val();
                        var sessionId = $('#session-id').val();
                        messenger.server.startChat(customerName, email, phone, userCustomerId, " ", sessionId);
                    }
                },
                error: function (req, status, error) {
                    alert("Error try again");
                }
            });
        })
    });
})