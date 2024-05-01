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


$('.myform').on('submit', function (e) {
    e.preventDefault();
    //$(".myform").validate()
    //if ($(".myform").valid() == false) {
    //    return false
    //}
    var formData = new FormData(); // Create FormData object
    // Serialize form data and append it to FormData
    formData.append('Notes', $('#exampleFormControlTextarea09').val());
    formData.append('Firstname', $('#floatingInputz').val());
    formData.append('Lastname', $('#floatingInputx').val());
    formData.append('Date', $('#floatingInputc').val());
    formData.append('Email', $('#PatEmailv').val());
    formData.append('Phonenumber', $('.mobileman').val());
    formData.append('Street', $('#floatingInput2m').val());
    formData.append('City', $('#floatingInput3l').val());
    formData.append('regid', $('.FetchRegion').val());
    formData.append('Zipcode', $('#floatingInput5j').val());
    formData.append('RoomSuite', $('#floatingInput6h').val());
    // Append file input
    var fileInput = $('#formFileLgg')[0].files[0];
    formData.append('uploadFile', fileInput);
    $.ajax({
        url: '/PatientDashboard/MeRequestSubmit',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response) {
                $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable');
                toastr.success("Request Submitted Successfully");
            } else {
                toastr.error("Something went Wrong");
            }
            // Handle success response
        }
    });

});