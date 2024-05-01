
$(document).ready(function () {
    $("#ViewCaseForm :input,#closeCaseForm :input").attr("disabled", true);
    $("#ViewCaseForm :button, #closeCaseForm :button").removeAttr("disabled", true);
    $("#Submitbtn,#CloseCaseSubmitbtn").hide();
    $(document).on('click', '#Editbtn , #CloseCaseEditbtn', function (event) {
        event.preventDefault();
        $("#Submitbtn,#CloseCaseSubmitbtn").show();
        $("#PatientMobile , #PatientEmail , #closeCaseForm input").removeAttr("disabled", true);
        $("#Editbtn, #CloseCaseEditbtn").hide();
    });

    $('#viewNotes').click(function (event) {
        event.preventDefault();
    });
    $('#Cancelbtn').click(function (event) {
        event.preventDefault();
        $("#ViewCaseForm :input").attr("disabled", true);
        $("#ViewCaseForm :button").removeAttr("disabled", true);
        $("#Editbtn").show();
        $("#Submitbtn").hide();

    });
});

$(document).ready(function () {
    let input1 = document.querySelector(".Patientmobile");

    const input1script = window.intlTelInput(input1, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
        preferredCountries: ["in"],
        separateDialCode: true,
        //initialCountry: "in"
    });
    $(".Patientmobile").on("keyup", function () {
        // console.log(input.isValidNumber());
        if (input1script.isValidNumber() == false) {
            $("#mobilePatientValidation").html("Phonenumber is not valid");
        }
        else {
            $("#mobilePatientValidation").html("");
        }
    });
    //$("#closeCaseForm").on("submit", function (e) {
    //    e.preventDefault();
    //});
    $('.myform,#closeCaseForm').on('submit', function (e) {debugger
        e.preventDefault();
        if ($(this).attr("id") == "closeCaseForm") {
            $("#closeCaseForm").validate();
        }
        else {
            $('.myform').validate();
        }

        //$("#PatientCountryCode").val();
        var formData = $(this).serializeArray();
        formData.push({ name: "PatientCountryCode", value: $(".iti__selected-dial-code").text() });

        if (input1script.isValidNumber() == false) {
            e.preventDefault();
            return false;
        }

        if ($(this).attr("id") == "closeCaseForm") {debugger
            if ($("#closeCaseForm").valid()) {
                $.post('/AdminPartials/EditUserData', formData, function (response) {
                    //show tostr
                    if (response) {
                        $(".admin-data-container").load('/AdminPartials/CloseCase?reqid=' + response);
                        toastr.success("Patient Data Changed!");

                    }
                    else {
                        toastr.error("Error Occured!");
                    }
                });
            }
        }
        else {
            if ($(".myform").valid()) {
                $.post('/AdminPartials/ViewNewCasespost', formData, function (response) {
                    if (response.status) {
                        $(".admin-data-container").load('/AdminPartials/ViewNewCases?requestid=' + response.reqid, function () {
                            $("#AssignBtn").attr("hidden", true);
                            $("#Cancelbtn").attr("hidden", true);

                        });
                        toastr.success("Patient Data Changed!");
                    }
                    else {
                        toastr.error("Error Occured!");
                    }
                });

            }
        }
        if ($(".myform ,#closeCaseForm ").valid()) {

            if (input1script.isValidNumber() == false) {
                e.preventDefault();
                return false;
            }
            else {

                $("#ViewCaseForm :input , #closeCaseForm :input").attr("disabled", true);
                $("#ViewCaseForm :button,#closeCaseForm :button").removeAttr("disabled", true);
                $("#Submitbtn,#CloseCaseSubmitbtn").hide();
                $("#Editbtn , #CloseCaseEditbtn").show();
            }
        }
    });



    $('#CloseCaseCancelbtn').on("click", function (e) {
        e.preventDefault();
        $.ajax({
            url: '/AdminPartials/CloseCasePermennt',
            method: 'POST',
            success: function (response) {
                if (response) {

                    toastr.success("Case Closed Permently!")

                    $(".admin-data-container").load('/AdminPartials/AdminDashBoardMain', function () {
                    });
                }
                else {
                    toastr.error("Something went wrong!")

                }


            }
        });
    });
});
//$('#invoiceGenerate').click(function (e) {

//    e.preventDefault();

//    //load invoice here
//    $("#filekeeper").val($("#closeCaseForm").html());

//    $.ajax({
//        url: '/AdminPartials/invoiceGenerate',
//        method: 'POST',
//        data: {
//            datas: $("#filekeeper").val()
//        },
//        success: function () {

//        }
//    })
//})