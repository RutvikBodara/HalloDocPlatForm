$(document).ready(function () {
    const inputval = document.querySelector(".pat_mobile");
    window.intlTelInput(inputval, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js"
    });

    $("#EncounterForm input").attr("disabled", true);
    $("#EncounterSave,#EncounterCancel").hide();

    $(document).on("click", '#EncounterEdit', function (e) {
        e.preventDefault();
        $("#EncounterForm input").removeAttr("disabled", true);
        $("#EncounterSave,#EncounterCancel").show();
        $("#EncounterEdit").hide();
        $(window).scrollTop(0);
    });
})

$("#EncounterCancel").click(function (e) {
    e.preventDefault();
    $("#EncounterForm input").attr("disabled", true);
    $("#EncounterSave,#EncounterCancel").hide();
    $("#EncounterEdit").show();
    $(window).scrollTop(0);
});
