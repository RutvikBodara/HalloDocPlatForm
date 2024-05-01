document.getElementById("cancelbtn").addEventListener("click", (event) => {
    event.preventDefault();
});

document.getElementById("editbtn").addEventListener("click", (event) => {
    event.preventDefault();
});

$(document).ready(function () {
    const input = document.querySelector(".mobileman");
    window.intlTelInput(input, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
        preferredCountries: ["in"],
        separateDialCode: true,
        //initialCountry: "in"
    });
});
$("#submitbtn").click(function (e) {
    $("#CountryCode").val($(".iti__selected-dial-code").text());
});
function enableEdit() {
    var inputs = document.querySelectorAll("input");
    var editbtns = document.getElementById("editbtn");
    var SubmitBTN = document.getElementById("submitbtn");
    var CancelBTN = document.getElementById("cancelbtn");

    for (var i = 0; i < inputs.length; i++) {
        inputs[i].removeAttribute("disabled");
    }
    $(".FetchRegion").attr("disabled", false);
    SubmitBTN.removeAttribute("hidden");
    CancelBTN.removeAttribute("hidden");

    editbtns.hidden = true;
}
function disableEdit() {


    $("#ProfileEdit").click();

    var inputs = document.querySelectorAll("input");
    var editbtns = document.getElementById("editbtn");
    var SubmitBTN = document.getElementById("submitbtn");
    var CancelBTN = document.getElementById("cancelbtn");

    $(".FetchRegion").attr("disabled", true);
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].setAttribute("disabled", "true");
    }

    SubmitBTN.hidden = true;
    CancelBTN.hidden = true;
    editbtns.hidden = false;
}