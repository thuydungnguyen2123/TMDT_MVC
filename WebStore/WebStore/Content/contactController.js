var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var name = $('#contact_name').val();
            var mobile = $('#contact_phone').val();
            var email = $('#contact_mail').val();
            var content = $('#contact_textarea').val();
            $.ajax({
                url: '/MainHome/Send',
                type: 'POST',
                dataType: 'json',
                data: {
                    name: name,
                    mobile: mobile,
                    email: email,
                    content: content
                },
                success: function (res) {
                    if (res.status == true) {
                        alert("Gửi Feedback thành công!");
                        $('contact_name').val('');
                        $('contact_phone').val('');
                        $('contact_mail').val('');
                        $('contact_textarea').val('');
                    }
                }
            });
        });
    }
}
contact.init();