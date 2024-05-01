

$(document).ready(function () {

    const input = document.querySelector(".mobileman");

    const input1Script = window.intlTelInput(input, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
        separateDialCode: true,
        preferredCountries: ["in"],

    });
    const input2 = document.querySelector(".mobileman2");

    const input2Script = window.intlTelInput(input2, {
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
    $(".mobileman2").on("keyup", function () {
        if (input2Script.isValidNumber() == false) {
            $("#mobileBusinesValidation").html("Phonenumber is not valid");
        }
        else {
            $("#mobileBusinesValidation").html("");
        }
    });

    //if (localStorage.getItem("profileEdit") == "admin") {
    //    $("#ProfileAdminBtn").attr("hidden", true);
    //    $.ajax({
    //        url: '/AdminPartials/regionCheckbox',
    //        method: 'POST',
    //        success: function (response) {
    //            $(".regionCheckbox").empty();
    //            response.forEach(function (region) {
    //                $(".regionCheckbox").append('<div class="form-check"> <input class="regioncheck" id="' + region.regionid + '" type = "checkbox" value = "' + region.regionid + '" Checked disabled>  <label class="form-check-label" for="' + region.regionid + '">' + region.region.name + '   </label > </div> ');
    //            });
    //        }
    //    });
    //}
    //else if (localStorage.getItem("profileEdit") == "Provider") {
    //    //add physician regions from here
    //}

    $("#AdminAspForm input,#AdminAspForm select").attr("disabled", true);
    $("#AdminBusinessForm input,#AdminBusinessForm select").attr("disabled", true);
    $("#ProviderProfileForm input,#ProviderProfileForm select,#ProviderProfileForm button ,#ProviderProfileForm textarea").attr("disabled", true);
    $(".submitProfileAll").removeAttr("disabled", true);

    // $("#AdminInfoForm input").attr("disabled", true);
    $("#AdminAspFormSubmitbtn, #SubmitBusinessInfobtn,#AdminAspFormCancelbtn ,#CancelBusinessInfobtn, #SubmitProviderProfilebtn , #CancelProviderProfilebtn").hide();

    $(document).on('click', '#AdminAspFormEditbtn, #EditBusinessInfobtn , #EditProviderProfilebtn', function (event) {
        event.preventDefault();

        if ($(this).attr("id") == "EditBusinessInfobtn") {
            $("#EditBusinessInfobtn").hide();
            $("#SubmitBusinessInfobtn,#CancelBusinessInfobtn").show();
            $("#AdminBusinessForm input,#AdminBusinessForm select").removeAttr("disabled", true);
        }
        else if (($(this).attr("id") == "AdminAspFormEditbtn")) {
            $("#AdminAspFormEditbtn").hide();
            $("#AdminAspFormSubmitbtn, #AdminAspFormCancelbtn").show();
            $("#AdminAspForm input,#AdminAspForm select").removeAttr("disabled", true);
        }
        else {
            $("#EditProviderProfilebtn").hide();
            $("#SubmitProviderProfilebtn, #CancelProviderProfilebtn").show();
            $("#ProviderProfileForm input,#ProviderProfileForm select,#ProviderProfileForm button,#ProviderProfileForm textarea").removeAttr("disabled", true);
        }
        $("#AdminPassword").attr("disabled", true)

    });

    $("#AdminAspFormCancelbtn, #CancelBusinessInfobtn,#CancelProviderProfilebtn").click(function (event) {
        event.preventDefault();

        if ($(this).attr("id") == "CancelBusinessInfobtn") {
            $("#AdminBusinessForm input,#AdminBusinessForm select").attr("disabled", true);
            $("#SubmitBusinessInfobtn ,#CancelBusinessInfobtn").hide();
            $("#EditBusinessInfobtn").show();
        }
        else if (($(this).attr("id") == "AdminAspFormCancelbtn")) {
            $("#AdminAspForm input,#AdminAspForm select").attr("disabled", true);
            $("#AdminAspFormSubmitbtn, #AdminAspFormCancelbtn").hide();
            $("#AdminAspFormEditbtn").show();
        }
        else {
            $("#ProviderProfileForm input,#ProviderProfileForm select,#ProviderProfileForm button,#ProviderProfileForm textarea").attr("disabled", true);
            $(".submitProfileAll").removeAttr("disabled", true);
            $("#SubmitProviderProfilebtn, #CancelProviderProfilebtn").hide();
            $("#EditProviderProfilebtn").show();
        }
    });

    $("#AdminPassword").on("keyup", function () {
        var regularExpression = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;
        var status = regularExpression.test($("#AdminPassword").val());
        if (!status) {
            $(".AdminpasswordValidation").html("Password must contains one upper letter , one lower case, minimum  length , one unique icon and one number ")
        }
        else {
            $(".AdminpasswordValidation").html("");
        }

    });

    $("#ResetAdminPass").click(function (e) {

        e.preventDefault();
        if ($("#ResetAdminPass").text() == "Save Changes") {
            //reset Password
            var regularExpression = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;
            var status = regularExpression.test($("#AdminPassword").val());
            if (status) {
                $.ajax({
                    url: '/AdminPartials/ResetAdminPass',
                    method: 'POST',
                    data: { password: $("#AdminPassword").val() },
                    success: function (response) {
                        if (response) {
                            toastr.success("password changed");
                        }
                        else {
                            toastr.error("Something Went Wrong!")
                        }

                        $("#ResetAdminPass").text("Reset Password");
                        $("#AdminPassword").val("");
                        $("#AdminPassword").attr("disabled", true);
                    }
                });
            }
        }
        else {
            $("#AdminPassword").removeAttr("disabled", true);
            $("#ResetAdminPass").html("Save Changes");
        }
    });

    $("#AdminAspForm , #AdminBusinessForm , #ProviderProfileForm").on("submit", function (e) {
        e.preventDefault();
        if ($("#AdminMail").val() != $("#AdminConfirmMail").val()) {
            $("#confirmMailValidation").html("Email Must Match")
            return false;
        }
        $('#AdminAspForm,#AdminBusinessForm,#ProviderProfileForm').validate();
/*        console.log($(".iti__selected-dial-code").text())*/

        if ($('#AdminAspForm,#AdminBusinessForm,#ProviderProfileForm').valid()) {
            var datas = null;
            var urlLink = '';
            //$.validator.unobtrusive.parse($(this));
            var formtype = $(this).attr("id")
            if (formtype == "AdminAspForm") {
                datas = new FormData($("#AdminAspForm")[0]);
                //for ids
                var ids = [];
                //datas.append("AdminData", $("#AdminAspForm")[0]);
                if (input1Script.isValidNumber() == false) {
                    return false;
                }
                $('input[type=checkbox]:checked').each(function () {
                    datas.append("RegionList", $(this).attr("id"))
                });
                datas.append("CountryCode", $(".iti__selected-dial-code").text());
                //datas.append("RegionList", JSON.stringify(ids));
            }
            else if (formtype == "AdminBusinessForm") {
                //datas.append("AdminData", $("#AdminAspForm")[0]);
                if (input2Script.isValidNumber() == false) {
                    return false;
                }
                datas = new FormData($("#AdminBusinessForm")[0]);
                datas.append("CountryCode", $(".iti__selected-dial-code").text());
                //datas = $("#AdminBusinessForm").serialize();
            }
            else if (formtype == "ProviderProfileForm") {
                //data = new FormData($('#ProviderProfileForm')[0].files[0]);
                datas = new FormData($("#ProviderProfileForm")[0]); // Create FormData object
                // Serialize form data and append it to FormData
                // Append file input
                var fileInput = $('#formFilePhoto')[0].files[0];
                var fileInput2 = $('#formFilesignature')[0].files[0];
                //datas.append('Businessname', $("#BusinessName").val());
                //datas.append('Businesswebsite', $("#Businesswebsite").val());
                //datas.append('Adminnotes', $("#Adminnotes").val());
                datas.append('filePhoto', fileInput);
                datas.append('fileSign', fileInput2);
                datas.append('submitfor', "ProviderProfileForm");
            }
            var adminid = $("#adminId").val();

            if (localStorage.getItem("profileEdit") == "admin") {
                urlLink = '/AdminPartials/AdminAspFormSubmit'
            }
            else if (localStorage.getItem("profileEdit") == "custadmin") {
                urlLink = '/AdminPartials/AdminAspFormSubmit?custadminid='+adminid
            }
            //else if (formtype == "ProviderProfileForm") {
            //    urlLink = '/AdminPartials/SubProviderProfile'
            //}
            else if (localStorage.getItem("profileEdit") == "Provider") {
                urlLink = '/AdminPartials/ProviderAspFormSubmit'
            }
            if (formtype == "ProviderProfileForm") {
                $.ajax({
                    url: urlLink,
                    method: 'POST',
                    data: datas,
                    processData: false,
                    contentType: false,
                    success: function (res) {
                        if (res) {
                            toastr.success("Data Changed");
                        }
                        else {
                            toastr.error("something went wrong!");
                        }
                        if (localStorage.getItem("profileEdit") == "admin") {
                            $("#AdminProfileEdit").click();
                        }
                        else if (localStorage.getItem("profileEdit") == "Provider") {
                            var phyid = localStorage.getItem("phyid")
                            EditPhysicianProfile(phyid)
                        }
                    }
                });
            }
            else {

                $.ajax({
                    url: urlLink,
                    method: 'POST',
                    data: datas,
                    processData: false,
                    contentType: false,
                    success: function (res) {
                        if (res) {
                            toastr.success("Data Changed");
                        }
                        else {
                            toastr.error("something went wrong!");
                        }
                        if (localStorage.getItem("profileEdit") == "admin") {
                            $("#AdminProfileEdit").click();
                        }
                        else if (localStorage.getItem("profileEdit") == "custadmin") {
                            //ADD NAVIGATION HERE
                            EditAdmin(adminid)

                        }
                        else if (localStorage.getItem("profileEdit") == "Provider") {
                            var phyid = localStorage.getItem("phyid")
                            EditPhysicianProfile(phyid)
                        }
                    }
                });
            }
        }
    });
});