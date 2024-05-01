$(document).ready(function () {
    $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial');
});
$("#VendorSearch").on("keyup", function () {
    $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial?search=' + $("#VendorSearch").val() + '&profId=' + $("#LoadProfession").val(), function (response) {
    });
});
$("#LoadProfession").on("change", function () {
    var value = $(this).val();
    $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial?search=' + $("#VendorSearch").val() + '&profId=' + value, function (response) {
    });
});
$("#CreatePartner").click(function () {
    $(".admin-data-container").load('/AdminPartials/EditVendor', function () {
        $("#SaveBusiness").prop("hidden", false);
        $("#CancelBusiness").prop("hidden", true);
        $("#EditBusiness").prop("hidden", true);
        $("#EditVendor input ,#EditVendor select").attr("disabled", false);
        const input = document.querySelector(".mobileman");

        const InputScript = window.intlTelInput(input, {
            utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
            preferredCountries: ["in"],
            separateDialCode: true,
            //initialCountry: "in"
        });
        $(".mobileman").on("keyup", function () {
            // console.log(input.isValidNumber());
            if (InputScript.isValidNumber() == false) {
                $("#PhonenumberValidation").html("Phonenumber is not valid");
            }
            else {
                $("#PhonenumberValidation").html("");
            }
        });
    });
})

function EditVendor(vendorid) {
    $(".admin-data-container").load('/AdminPartials/EditVendor?vendorid=' + vendorid, function () {
        const input = document.querySelector(".mobileman");
        $("#SaveBusiness").prop("hidden", true);
        $("#CancelBusiness").prop("hidden", true);
        $("#EditBusiness").prop("hidden", false);
        $("#EditVendor input ,#EditVendor select").prop("disabled", true);
        const input2 = document.querySelector(".mobileman");

    const InputScript2 = window.intlTelInput(input2, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
        preferredCountries: ["in"],
        separateDialCode: true,
        //initialCountry: "in"
    });
    $(".mobileman").on("keyup", function () {
        // console.log(input.isValidNumber());
        if (InputScript2.isValidNumber() == false) {
            $("#PhonenumberValidation").html("Phonenumber is not valid");
        }
        else {
            $("#PhonenumberValidation").html("");
        }
    });
    });
};
function DeleteVendor(vendorid) {
    //popup for delete
    $("#DeleteVendor").modal("show");
    $("#DeleteVendorBtn").attr("data-vendorid", vendorid);
}

$("#DeleteVendorBtn").click(function () {
    var vendorid = $("#DeleteVendorBtn").attr("data-vendorid");
    $.ajax({
        url: '/AdminPartials/DeleteBusiness',
        data: { vendorid: vendorid },
        method: 'POST',
        success: function (response) {
            if (response) {
                toastr.success("Business Deleted")
            }
            else {
                toastr.success("Something went wrong")
            }
            $("#DeleteVendor").modal("hide");
            $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial?search=' + $("#VendorSearch").val() + '&profId=' + $("#LoadProfession").val(), function (response) {
            });
        }
    })
});

$("#EditBusiness").click(function (e) {
    e.preventDefault();
    $(this).prop("hidden", true);
    $("#SaveBusiness").prop("hidden", false);
    $("#CancelBusiness").prop("hidden", false);
    $("#EditVendor input ,#EditVendor select").prop("disabled", false);
});
$("#CancelBusiness").click(function (e) {
    e.preventDefault();
    $(".validspans").html("");
    $("#EditBusiness").prop("hidden", false);
    $("#SaveBusiness").prop("hidden", true);
    $("#CancelBusiness").prop("hidden", true);
    $("#EditVendor input ,#EditVendor select").attr("disabled", true);
});
$("#SaveBusiness").click(function () {
    var FinalMobile = $(".iti__selected-dial-code").text() + $(".mobileman").val();
    $(".mobileman").val(FinalMobile)
});

//create and update combine
$("#EditVendor").on("submit", function (e) {
    e.preventDefault();
    var formdata = $("#EditVendor").serialize();
    $("#EditVendor").validate();

    //if (InputScript2.isValidNumber() == false || InputScript.isValidNumber() ==false)  {
    //    return false;
    //    $("#PhonenumberValidation").html("Phonenumber is not valid");
    //}
    if ($("#EditVendor").valid()) {
        $.ajax({
            url: '/AdminPartials/UpdateBusiness',
            data: formdata,
            method: 'POST',
            success: function (response) {
                if (response == 1) {
                    toastr.success("business updated")
                    $("#CancelBusiness").click()
                }
                else if (response == 2) {
                    toastr.success("business Create")
                    $("#Partner").click();
                }
                else if (!response) {
                    toastr.success("Something went wrong")
                }
            }
        })
    }
});

function lastPage() {
    let lastpage = 999999;
    $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial?search=' + $("#VendorSearch").val() + '&profId=' + $("#LoadProfession").val() + '&PageNumber=' + lastpage, function (response) {
    });
};

function FirstPage() {
    var CurrentPage = localStorage.getItem("RequestState");
    $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial?search=' + $("#VendorSearch").val() + '&profId=' + $("#LoadProfession").val(), function (response) {
    });
}
function Previous() {
    var CurrentPage = localStorage.getItem("RequestState");
    if ($("#PageNumber").val() > 1) {
        var pageNumbner = +$("#PageNumber").val() - 1;
        $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial?search=' + $("#VendorSearch").val() + '&profId=' + $("#LoadProfession").val() + '&PageNumber=' + pageNumbner, function (response) {
        });
    }
    else {
        return true;
    }
}
function Next(maxpage) {
    var CurrentPage = localStorage.getItem("RequestState");
    if ($("#PageNumber").val() != maxpage) {
        var pageNumbner = +$("#PageNumber").val() + 1;
        $("#loadPartnerTable").load('/AdminPartials/PartnerTablePartial?search=' + $("#VendorSearch").val() + '&profId=' + $("#LoadProfession").val() + '&PageNumber=' + pageNumbner, function (response) {
        });
    }
    else {
        return true;
    }
}