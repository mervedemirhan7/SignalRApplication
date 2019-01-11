var Helper = {
    Init: function () {
        $('input').on('keypress', function (e) {
            var code = e.keyCode || e.which;
            if (code == 13) {
                $('#sendmessage').click();
            }
        });
        $.connection.hub.url = "http://localhost:1453/signalr";

        var chat = $.connection.myHub;

        chat.client.addMessage = function (message) {
            var encodedMsg = $('<div />').text(message).html();
            $('#discussion').append('<li><strong>' + encodedMsg + '</strong></li>');
        };
        $('#message').focus();

        $('#sendmessage').click(function () {
            Helper.StartHub();
            if (Helper.IsConnected()) {
                chat.server.send($('#message').val());
                $('#message').val('').focus();
            }
            
        });
        $.connection.hub.error(function (error) {
            $('#sendmessage').attr("disabled", true);
            $.connection.hub.stop();
            console.log('SignalR error: ' + error)
        });

        Helper.StartHub();
        $.connection.hub.disconnected(function () {
            if ($.connection.hub.lastError) {
                $('#sendmessage').attr("disabled", true);
                $.connection.hub.stop();
                console.log("Disconnected. Reason: " + $.connection.hub.lastError.message);
                var result = window.confirm("Program çalışmasını istiyor musunuz?")
                if (result==true) {
                    window.location.href = 'denemePro://';
                }
                else {
                    return;
                }
            }
        });
    },
    StartHub: function () {
        if (!Helper.IsConnected()) {
            $.connection.hub.start().done(function () {
                $('#sendmessage').attr("disabled", false); 
            }).fail(function (err) { $('#sendmessage').attr("disabled", true); });
            
        }
    },
    IsConnected: function () {
        return $.connection.hub.state === $.connection.connectionState.connected;
    }
}