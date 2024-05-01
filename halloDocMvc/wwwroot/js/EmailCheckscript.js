$(document).ready(function () {
    $('#PatEmail').on("change", function () {

        var emailValue = $(this).val();

        $.ajax({
            url: '/PatientRequests/emailcheck',
            method: 'post',
            data:
            {
                email: emailValue
            },
            success: function (Response) {
                if (Response.emails) {
                    $('#Password').show();
                    $('#ConfirmPassword').show();
                }
                else {
                    $('#Password').hide();
                    $('#ConfirmPassword').hide();
                }
            }
        });
    });

    //check password same 

});
