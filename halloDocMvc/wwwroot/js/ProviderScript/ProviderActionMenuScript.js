
$(document).ready(function () {
    var RequestState = "NewRequests";
    if (localStorage.getItem("RequestState") != null) {
        var reqType = localStorage.getItem("RequestState");
        showsvg(reqType);

        localStorage.setItem("MainState", "AdminPartials");
        $(".table-container").load('/ProviderAccount/' + reqType);
    }
    else {
        showsvg('NewRequests');
    }
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    }
    function showPosition(position) {
        var latitude = position.coords.latitude;
        var longtitude = position.coords.longitude;
        $.ajax({
            url: '/ProviderAccount/UpdateLocation',
            data: { latitude: latitude, longtitude: longtitude },
            success: function (response) { }
        });
    }
    //let input1 = document.querySelector(".SendLinkMobile2");
    //window.intlTelInput(input1, {
    //    utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js"
    //});

});
function showsvg(svg) { // request status data load
    if (svg == 'NewRequests') {
        resetDefault();
        $("#floatingInputsearch").val('');
        $(".Admin-side-cards").removeClass("active");
        $(".Admin-side-cards").removeClass("shineAnimation");
        $(".svg").hide();
        $("#onesvg").show();
        $("#NewRequests").addClass("active");
        $("#NewRequests").addClass("shineAnimation");
        $("#StatusPatient").html("(New)")
        $(".table-container").load('/ProviderAccount/NewRequests');
        localStorage.setItem("RequestState", "NewRequests");
        RequestState = "NewRequests";
    }
    else if (svg == 'PendingRequests') {
        $("#floatingInputsearch").val('');
        resetDefault();
        $(".Admin-side-cards").removeClass("active");
        $(".Admin-side-cards").removeClass("shineAnimation");
        $(".svg").hide();
        $("#twosvg").show();
        $("#PendingRequests").addClass("active");
        $("#PendingRequests").addClass("shineAnimation");
        $("#StatusPatient").html("(Pending)")
        localStorage.setItem("RequestState", "PendingRequests");
        $(".table-container").load('/ProviderAccount/PendingRequests');
        RequestState = "PendingRequests";

    } else if (svg == 'ActiveRequests') {
        $("#floatingInputsearch").val('');
        resetDefault();
        $(".Admin-side-cards").removeClass("active");
        $(".Admin-side-cards").removeClass("shineAnimation");
        $(".svg").hide();
        $("#threesvg").show();
        $("#ActiveRequests").addClass("active");
        $("#ActiveRequests").addClass("shineAnimation");
        $("#StatusPatient").html("(Active)")
        localStorage.setItem("RequestState", "ActiveRequests");
        $(".table-container").load('/ProviderAccount/ActiveRequests');
        RequestState = "ActiveRequests";
    }
    else if (svg == 'ConcludeRequests') {
        $("#floatingInputsearch").val('');
        resetDefault();
        $(".Admin-side-cards").removeClass("active");
        $(".Admin-side-cards").removeClass("shineAnimation");
        $(".svg").hide();
        $("#foursvg").show();
        $("#ConcludeRequests").addClass("active");
        $("#ConcludeRequests").addClass("shineAnimation");
        $("#StatusPatient").html("(Conclude)")
        localStorage.setItem("RequestState", "ConcludeRequests");
        $(".table-container").load('/ProviderAccount/ConcludeRequests');
        RequestState = "ConcludeRequests";
    }
}
function resetDefault() {
    $.ajax({
        url: '/ProviderAccount/LoadPhysicianRegion',
        method: 'POST',
        success: function (response) {
            $('#RegionList').empty();
            $('#RegionList').append('<option value="-1" Selected>Select Region</option>')
            response.forEach(function (regionname) {
                $('#RegionList').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
            })
        }
    });
}
var LastVisited = null;
var reqtype = null;
function lastPage() {
    let lastpage = 999999;
    var CurrentPage = localStorage.getItem("RequestState");
    $(".table-container").load("/ProviderAccount/" + CurrentPage + "?PageNumber=" + lastpage + "&regid=" + $("#RegionList").val() + '&search=' + $("#floatingInputsearch").val() + "&reqType=" + reqtype);
};
function FirstPage() {
    var CurrentPage = localStorage.getItem("RequestState");
    $(".table-container").load("/ProviderAccount/" + CurrentPage + "?regid=" + $("#RegionList").val() + '&search=' + $("#floatingInputsearch").val() + "&reqType=" + reqtype)
}
function Previous() {
    var CurrentPage = localStorage.getItem("RequestState");
    if ($("#PageNumber").val() > 1) {
        var pageNumbner = +$("#PageNumber").val() - 1;
        $(".table-container").load("/ProviderAccount/" + CurrentPage + "?PageNumber=" + pageNumbner + "&regid=" + $("#RegionList").val() + '&search=' + $("#floatingInputsearch").val() + "&reqType=" + reqtype);
    }
    else {
        return true;
    }
}
function Next(maxpage) {
    var CurrentPage = localStorage.getItem("RequestState");
    if ($("#PageNumber").val() != maxpage) {
        var pageNumbner = +$("#PageNumber").val() + 1;
        $(".table-container").load("/ProviderAccount/" + CurrentPage + "?PageNumber=" + pageNumbner + "&search=" + $("#floatingInputsearch").val() + "&regid=" + $("#RegionList").val() + "&reqType=" + reqtype);
    }
    else {
        return true;
    }
}
function PatientDataFetch() {
    $(".FilterBtn").css("border", "none");
    $("#PatientDataFetch").css("border", "solid 1px");
    reqtype = 2;
    var SearchValue = $("#floatingInputsearch").val();
    $(".table-container").load("/ProviderAccount/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
function AllBtn() {
    $(".FilterBtn").css("border", "none");
    $("#AllBtn").css("border", "solid 1px");
    $("#floatingInputsearch").val('');
    resetDefault();
    reqtype = null;
    $(".table-container").load('/ProviderAccount/' + RequestState);
}

function FamilyDataFetch() {
    $(".FilterBtn").css("border", "none");
    $("#FamilyDataFetch").css("border", "solid 1px");
    reqtype = 3;
    var SearchValue = $("#floatingInputsearch").val();
    $(".table-container").load("/ProviderAccount/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
function BusinessDataFetch() {
    $(".FilterBtn").css("border", "none");
    $("#BusinessDataFetch").css("border", "solid 1px");
    reqtype = 1;
    var SearchValue = $("#floatingInputsearch").val();
    $(".table-container").load("/ProviderAccount/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
function ConciergeDataFetch() {
    $(".FilterBtn").css("border", "none");
    $("#ConciergeDataFetch").css("border", "solid 1px");
    reqtype = 4;
    var SearchValue = $("#floatingInputsearch").val();
    $(".table-container").load("/ProviderAccount/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
$('#RegionList').on("change", function () {
    var regid = $(this).val();
    if (regid == -1) {
        regid = null;
    }
    var SearchValue = $("#floatingInputsearch").val();
    $(".table-container").load('/ProviderAccount/' + RequestState + '?regid=' + regid + '&search=' + SearchValue);
});
//search_request_wise
$('#floatingInputsearch').on("keyup", function () {
    var SearchValue = $(this).val();
    $(".table-container").load('/ProviderAccount/' + RequestState + '?search=' + SearchValue + "&regid=" + $("#RegionList").val());
});

function viewCases(id) {
    LastVisited = "ViewCase"
    $(".Provider-data-container").load('/ProviderAccount/ViewNewCases?requestid=' + id, function () {
    });
}
function AcceptCase(reqid) {
    $.ajax({
        url: '/ProviderAccount/AcceptCaseByProvider',
        data: { RequestId: reqid },
        method: 'POST',
        success: function (response) {
            if (response) {
                toastr.success("Request Accepted")
            }
            else {
                toastr.success("Something Went Wrong")
            }
            $("#ProviderDash").click();
        }
    })
}
function ShowViewNotes(id) {
    $(".Provider-data-container").load('/ProviderAccount/ShowViewNotes?requestid=' + id);
}
function SendAgreement(RequestId, RequestType, Phonenumber, Email) {
    //reqtype
    if (RequestType == 1) {
        $(".RequestType").html(`<svg height="15" width="15" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                            <circle fill="#e26277" r="45" cx="50" cy="50" stroke="grey" stroke-width="3" />
                        </svg>
                        <span class="ms-2">Business</span>`);
    }
    else if (RequestType == 2) {
        $(".RequestType").html(`  <svg height="15" width="15" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                    <circle fill="#198754" r = "45" cx = "50" cy = "50" stroke = "grey" stroke - width="3" />
                    </svg>
                        <span class="ms-2">Patient</span>`);
    }
    else if (RequestType == 3) {
        $(".RequestType").html(` <svg height="15" width="15" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                            <circle fill="#ed9124" r="45" cx="50" cy="50" stroke="grey" stroke-width="3" />
                        </svg>
                        <span class="ms-2">Family</span>`);
    }
    else if (RequestType == 4) {
        $(".RequestType").html(`<svg height="15" width="15" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                            <circle fill="#00bafa" r="45" cx="50" cy="50" stroke="grey" stroke-width="3" />
                        </svg>
                        <span class="ms-2">Concierge</span>`);
    }

    $('#Send_Agr_PhoneNumber').val(Phonenumber);
    let flagVal = null;
    let SendAgrMobile = document.querySelector("#Send_Agr_PhoneNumber");
    flagVal= window.intlTelInput(SendAgrMobile, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
        //preferredCountries: ["in"],
        separateDialCode: true,
    });
    $('#Send_Agr_Email').val(Email);
    $('.Send_Agr_ConfirmBtn').attr("data-requestId", RequestId);
    $('#SendAgreementModel').modal("show");
}

function Send_AgreementConfirm() {
    var reqid = $('.Send_Agr_ConfirmBtn').attr("data-requestId");
    var phonenumber = $('#Send_Agr_PhoneNumber').val();
    var Email = $('#Send_Agr_Email').val();

    var regularExpression = /\S+@\S+\.\S+/;
    var emailtest = regularExpression.test(Email);

    if (!emailtest) {
        $(".Send_Agr_Email_Validation").html("Email is not Valid")
        return false;
    }
    else {
        $(".Send_Agr_Email_Validation").html("")
    }
    $.ajax({
        url: '/ProviderAccount/SendAgreement',
        method: 'POST',
        data: {
            reqid: reqid,
            Email: Email,
            phonenumber: phonenumber
        },
        success: function (response) {

            if (response.responseText != null) {
                toastr.error("Agreement Already Sent Once!");
            }
            else if (response) {
                toastr.success("Agreement Sent");
            }
            else {
                toastr.error("Agreement Not Sent!");
            }
            $(".btn-close").click();
            $(".Provider-data-container").load('/ProviderAccount/ProviderDashBoardMain')
        }
    });
};
function View_Document(req_id, typeDoc) {
    $(".Provider-data-container").load('/ProviderAccount/View_Document?id=' + req_id);
}
function TransferBack(RequestId) {
    $('.SubmitTransferBackBtn').attr("data-requestId", RequestId);
    $('#TransferBack').modal("show");
}
function TransferBackAdmin() {
    var reqid = $('.SubmitTransferBackBtn').attr("data-requestId");
    var TransferNotes = $("#TransferToAdmin").val();
    if (TransferNotes == '') {
        $(".TransferReason").html("Please fill above field")
        return false;
    }
    else {
        $.ajax({
            url: '/ProviderAccount/TransferBackAdmin',
            method: 'POST',
            data: {
                reqid: reqid,
                TransferNotes: TransferNotes
            },
            success: function (response) {
                if (response) {
                    toastr.success("Request Submitted");
                }
                else {
                    toastr.error("Request Not Submitted");
                }
                $(".btnclose").click();
                $(".Provider-data-container").load('/ProviderAccount/ProviderDashBoardMain')
            }
        });
    }

}
function EncounterModel(reqid) {
    $('#SaveEncounter').attr('data-requestid', reqid)
    $('#Encounter_Model').modal("show");
}

var Consult = document.getElementById("#Consult");
var HouseCall = document.getElementById("#HouseCall");
function SaveEncounter() {

    var options = document.querySelector('input[name="options"]:checked').id;
    //set status as a conclude state
    var reqid = $('#SaveEncounter').attr('data-requestid');
    $.ajax({
        url: '/ProviderAccount/SaveEncounterPreferences',
        data: {
            reqid: reqid,
            SelectType: options
        },
        method: 'POST',
        success: function (response) {

            if (response) {
                toastr.success("Encounter Saved!");
            }
            else {
                toastr.error("Encounter Not Saved!");
            }
            $('.EncounterCancel').click();
            $(".Provider-data-container").load('/ProviderAccount/ProviderDashBoardMain')

        }
    });
}
function EncounterForm(reqid) {
    $(".Provider-data-container").load("/ProviderAccount/EncounterForm?reqid=" + reqid);
}

function ConcludeRequest(reqid) {
    $.ajax({
        url: '/ProviderAccount/ConcludeRequest',
        data: { reqid: reqid },
        method: 'POST',
        success: function (response) {
            if (response) {
                toastr.success("Status Changed");
                $(".Provider-data-container").load('/ProviderAccount/ProviderDashBoardMain')
            }
            else {
                toastr.error("Something went wrong");
            }
        }
    });
}
function DownloadEncounterFormPopUp(reqid) {
    $("#ReqestIdKeeper").val(reqid);
    $("#DownloadEncounter").modal("show");
}
function DownloadReport() {
    $(".closeBtn").click();
}
function ConcludeCare(reqid) {
    $(".Provider-data-container").load('/ProviderAccount/ConcludeCareIndex?RequestId=' + reqid);
}

$("#Req_FirstName").on("keyup", function () {
    ($("#Req_FirstName").val() == '') ? $(".firstname_Validation").html("This Field Is Required") : $(".firstname_Validation").html("");
});
$("#Req_LastName").on("keyup", function () {
    ($("#Req_LastName").val() == '') ? $(".lastname_Validation").html("This Field Is Required") : $(".lastname_Validation").html("");
});
var emailtest = false;
$("#Req_Email").on("keyup", function () {
    var formdata = $(this).serialize();
    var regularExpression = /\S+@\S+\.\S+/;
    var emailtest = regularExpression.test($("#Req_Email").val());
    (!emailtest) ? $(".Email_Validation").html("Email is not Valid") : $(".Email_Validation").html("");
});

$("#SendLinkForm").on("submit", function (e) {
    e.preventDefault();

    //var emailtest = regularExpression.test($("#Req_Email").val());
    //if ($("#Req_FirstName").val() == '' || $("#Req_LastName").val() == '') {
    //    ($("#Req_FirstName").val() == '') ? $(".firstname_Validation").html("This Field Is Required") : $(".firstname_Validation").html("");
    //    ($("#Req_LastName").val() == '') ? $(".lastname_Validation").html("This Field Is Required") : $(".lastname_Validation").html("");
    //    (!emailtest) ? $(".Email_Validation").html("Email is not Valid") : $(".Email_Validation").html("");
    //    return false;
    //}
    //if (!emailtest) {
    //    $(".Email_Validation").html("Email is not Valid")
    //    return false;
    //}
    if (emailtest && formdata.firstname != "" && formdata.lastname != "") {
        var formdata = $(this).serialize();
        $.ajax({
            url: '/ProviderAccount/SendLink',
            method: 'POST',
            data: formdata,
            success: function (response) {
                if (response) {
                    toastr.success("Link sent to given user")
                }
                else {
                    toastr.error("something went Wrong")
                }
                $(".closebtn").click();
            }
        });
    }
});
$("#ProviderRequest").click(function () {
    $(".Provider-data-container").load('/ProviderAccount/ProviderPatientRequest');
});

function ViewNotesBack(id) {//view notes back navigation
    if (LastVisited == "ViewCase") {
        $(".Provider-data-container").load('/ProviderAccount/ViewNewCases?requestid=' + id);
    }
    else {
        $(".Provider-data-container").load('/ProviderAccount/ProviderDashBoardMain');
    }
}
$(document).ready(function () {
    $(document).ajaxSend(function () {
        $(".table-container").fadeOut(250);
    });
    $(document).ajaxComplete(function () {
        $(".table-container").fadeIn(250);
    });
});