$(document).ready(function () {
    $(document).ready(function () {
        const input = document.querySelector(".mobileman");

        const input1Script=window.intlTelInput(input, {
            utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
            separateDialCode: true,
            preferredCountries: ["in"],
        });
        $(".mobileman").on("keyup", function () {
            if (input1Script.isValidNumber() == false) {
                $("#mobileAspValidation").html("Phonenumber is not valid");
            }
            else {
                $("#mobileAspValidation").html("");
            }
        });
    });
  

    $("#SomeoneSubmit").click(function () {
        $('.SomeOneReq').on('submit', function (e) {
            e.preventDefault();

            var formData = new FormData($(this)[0]);

            $.ajax({

                url: '/PatientDashboard/SomeoneRequestSubmit',
                method: "POST",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response) {
                        toastr.success("request submitted")
                        $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable')
                    }
                    else {
                        toastr.error("enter valid credential")
                    }
                }

            })
        })
    })
})