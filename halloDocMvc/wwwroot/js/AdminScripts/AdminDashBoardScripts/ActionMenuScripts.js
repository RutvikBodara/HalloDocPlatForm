
var RequestState = "NewRequests";
$(document).ready(function () {
	if (localStorage.getItem("RequestState") != null) {
		var reqType = localStorage.getItem("RequestState");
		showsvg(reqType);
		//localStorage.setItem("MainState", "AdminPartials");
		localStorage.setItem("BackDoc", "AdminDashBoardMain");
		$(".table-container").load('/AdminPartials/' + reqType);
	}
	else {
		showsvg('NewRequests');
	}
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
		$(".table-container").load('/AdminPartials/NewRequests');
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
		$(".table-container").load('/AdminPartials/PendingRequests');
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
		$(".table-container").load('/AdminPartials/ActiveRequests');
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
		$(".table-container").load('/AdminPartials/ConcludeRequests');
		RequestState = "ConcludeRequests";
	}
	else if (svg == 'CloseRequests') {
		$("#floatingInputsearch").val('');
		resetDefault();
		$(".Admin-side-cards").removeClass("active");
		$(".Admin-side-cards").removeClass("shineAnimation");
		$(".svg").hide();
		$("#fivesvg").show();
		$("#CloseRequests").addClass("active");
		$("#CloseRequests").addClass("shineAnimation");
		$("#StatusPatient").html("(Close)")
		localStorage.setItem("RequestState", "CloseRequests");
		$(".table-container").load('/AdminPartials/CloseRequests');
		RequestState = "CloseRequests";
	}
	else if (svg == 'UnPaidRequests') {
		$("#floatingInputsearch").val('');
		resetDefault();
		$(".Admin-side-cards").removeClass("active");
		$(".Admin-side-cards").removeClass("shineAnimation");
		$(".svg").hide();
		$("#sixsvg").show();
		$("#UnPaidRequests").addClass("active");
		$("#UnPaidRequests").addClass("shineAnimation");
		$("#StatusPatient").html("(Unpaid)")
		localStorage.setItem("RequestState", "UnPaidRequests");
		$(".table-container").load('/AdminPartials/UnPaidRequests');
		RequestState = "UnPaidRequests";
	}
}
//variable for backtrack navigation
var LastVisited = null;
var reqtype = null;
function lastPage() {

	let lastpage = 999999;
	//put condition for max page so not do reload again for last page

	//if ($("#PageNumber").val() 
	var CurrentPage = localStorage.getItem("RequestState");
	$(".table-container").load("/AdminPartials/" + CurrentPage + "?PageNumber=" + lastpage + "&regid=" + $("#RegionList").val() + '&search=' + $("#floatingInputsearch").val() + "&reqType=" + reqtype);
};
function FirstPage() {
	if ($("#PageNumber").val() > 1) {
		var CurrentPage = localStorage.getItem("RequestState");
		$(".table-container").load("/AdminPartials/" + CurrentPage + "?regid=" + $("#RegionList").val() + '&search=' + $("#floatingInputsearch").val() + "&reqType=" + reqtype)
	}
}
function Previous() {
	var CurrentPage = localStorage.getItem("RequestState");
	if ($("#PageNumber").val() > 1) {
		var pageNumbner = +$("#PageNumber").val() - 1;
		$(".table-container").load("/AdminPartials/" + CurrentPage + "?PageNumber=" + pageNumbner + "&regid=" + $("#RegionList").val() + '&search=' + $("#floatingInputsearch").val() + "&reqType=" + reqtype);
	}
	else {
		return true;
	}
}
function Next(maxpage) {
	var CurrentPage = localStorage.getItem("RequestState");
	if ($("#PageNumber").val() != maxpage) {
		var pageNumbner = +$("#PageNumber").val() + 1;
		$(".table-container").load("/AdminPartials/" + CurrentPage + "?PageNumber=" + pageNumbner + "&search=" + $("#floatingInputsearch").val() + "&regid=" + $("#RegionList").val() + "&reqType=" + reqtype);
	}
	else {
		return true;
	}
}
function viewCases(id) {
	LastVisited = "ViewCase"
	//localStorage.setItem("BackDoc","AdminDashBoardMain");
	$(".admin-data-container").load('/AdminPartials/ViewNewCases?requestid=' + id, function () {
		if (RequestState == "NewRequests") {
			$("#Editbtn").attr("hidden", true);
		}
		else {
			$("#AssignBtn").attr("hidden", true);
			$("#Cancelbtn").attr("hidden", true);
		}
	});
}
function resetDefault() {
	$.ajax({
		url: '/AdminPartials/LoadRegion',
		method: 'POST',
		success: function (response) {
			$('#RegionList').empty();
			$('#RegionList').append('<option value="-1" Selected>Select Region</option>')
			response.region.forEach(function (regionname) {
				$('#RegionList').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
			})
		}
	});
}
function ShowViewNotes(id) {
	$(".admin-data-container").load('/AdminPartials/ShowViewNotes?requestid=' + id);
}
function View_Document(req_id, typeDoc) {
	if (typeDoc == 1) {
		localStorage.setItem("BackDoc", "PatientHistoryIndex");
	}
	else if (typeDoc == 2) {
		localStorage.setItem("BackDoc", "AdminDashBoardMain");
	}
	$(".admin-data-container").load('/AdminPartials/View_Document?id=' + req_id);
}

function ViewNotesBack(id) {//view notes back navigation
	if (LastVisited == "ViewCase") {
		$(".admin-data-container").load('/AdminPartials/ViewNewCases?requestid=' + id);
	}
	else {
		$(".admin-data-container").load('/AdminPartials/AdminDashBoardMain');
	}
}
//filters by request type

function PatientDataFetch() {
	$(".FilterBtn").css("border", "none");
	$("#PatientDataFetch").css("border", "solid 1px");
	reqtype = 2;
	var SearchValue = $("#floatingInputsearch").val();
	$(".table-container").load("/AdminPartials/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
function AllBtn() {
	$(".FilterBtn").css("border", "none");
	$("#AllBtn").css("border", "solid 1px");
	$("#floatingInputsearch").val('');
	resetDefault();
	reqtype = null;
	$(".table-container").load('/AdminPartials/' + RequestState);
}

function FamilyDataFetch() {
	$(".FilterBtn").css("border", "none");
	$("#FamilyDataFetch").css("border", "solid 1px");
	reqtype = 3;
	var SearchValue = $("#floatingInputsearch").val();
	$(".table-container").load("/AdminPartials/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
function BusinessDataFetch() {
	$(".FilterBtn").css("border", "none");
	$("#BusinessDataFetch").css("border", "solid 1px");
	reqtype = 1;
	var SearchValue = $("#floatingInputsearch").val();
	$(".table-container").load("/AdminPartials/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
function ConciergeDataFetch() {
	$(".FilterBtn").css("border", "none");
	$("#ConciergeDataFetch").css("border", "solid 1px");
	reqtype = 4;
	var SearchValue = $("#floatingInputsearch").val();
	$(".table-container").load("/AdminPartials/" + RequestState + "?reqType=" + reqtype + "&regid=" + $("#RegionList").val() + '&search=' + SearchValue);
}
function EncounterForm(reqid) {
	$(".admin-data-container").load("/AdminPartials/EncounterForm?reqid=" + reqid);
}

$('#RegionList').on("change", function () {
	var regid = $(this).val();
	if (regid == -1) {
		regid = null;
	}
	var SearchValue = $("#floatingInputsearch").val();
	$(".table-container").load('/AdminPartials/' + RequestState + '?regid=' + regid + '&search=' + SearchValue + "&reqType=" + reqtype);
});
//search_request_wise //use keyup is good option
$('#floatingInputsearch').on("change", function () {
	var SearchValue = $(this).val();
	$(".table-container").load('/AdminPartials/' + RequestState + '?search=' + SearchValue + "&regid=" + $("#RegionList").val() + "&reqType=" + reqtype);
});
function AssignModal(reqid, region, phyid, cases, e) {
	e = e || window.event;
	e.preventDefault();

	if (region.trim() === '') {
		$('.Current_Region').html("Select Region");
	}
	else {
		$('.Current_Region').html(region);
	}
	$('.AssignConfirmBtn').attr("data-requestId", reqid);
	if (cases == "transferCase") {
		localStorage.setItem("assignedphy", phyid);
		$('.AssignDescription').empty();
		$('#headerToggle').empty();
		$('#headerToggle').html('Trasfer Request')
		$('.AssignDescription').html('To transfer this request , search and select another physician');
	}
	$('#AssignModel').modal("show");
}
function CancelModal(reqid, name) {
	$('.UserName').html(name)
	$('.CancelConfirmBtn').attr("data-requestId", reqid);
	$('#CancelModel').modal("show");
};
function CloseCase(reqid) {
	$(".admin-data-container").load('/AdminPartials/CloseCase?reqid=' + reqid);
}
function BlockModel(reqid, name) {
	$('.FirstName').html(name)
	$('.BlockConfirmBtn').attr("data-requestId", reqid);
	$('#BlockModel').modal("show");
}
function ClearCasePopUp(reqid) {
	$('.clearCaseBtn').attr("data-requestId", reqid)
	$('#ClearCasePopUp').modal("show");
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
	$('#Send_Agr_PhoneNumber').val("");
	$('#Send_Agr_PhoneNumber').val(Phonenumber);
	let SendAgrMobile = document.querySelector(".SendAgreementMobile");

	window.intlTelInput(SendAgrMobile, {
		utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
		preferredCountries: ["in"],
		separateDialCode: true,
	});
	$('#Send_Agr_Email').val(Email);
	$('.Send_Agr_ConfirmBtn').attr("data-requestId", RequestId);
	$('#SendAgreementModel').modal("show");
}

//$(".CancelReason").on("change", function () {
//    var reason = $('.CancelReason').val();
//    if (reason == -1) {
//        $(".CancelReasonValidation").html("Please Select Cancel Reason");
//        return false;
//    }
//    else {
//        $(".CancelReasonValidation").html("");
//    }
//});
function CancelCaseSubmit() {
	var notes = $('#CancelNotes').val();
	var reason = $('.CancelReason').val();
	var reqid = $('.CancelConfirmBtn').attr("data-requestId");

	if (reason == -1) {
		$(".CancelReasonValidation").html("Please Select Cancel Reason");
		return false;
	}

	$.ajax({
		url: '/AdminPartials/CancelFormData',
		method: 'POST',
		data: {
			AdditionalNotes: notes,
			reason: reason,
			reqid: reqid
		},
		success: function (response) {
			toastr.success("Case Cleared!")
			$(".btn-close").click();
			$(".admin-data-container").load('/AdminPartials/AdminDashBoardMain')
			$(".CancelReasonValidation").html("");
		}
	});
};
function AssignPhysician() {
	var RegionSelect = $('.RegionSelect').val();
	var PhysicianSelect = $('.PhysicianSelect').val();
	var AdminDescription = $('.AdminDescription').val();

	if (RegionSelect == null || PhysicianSelect == null) {

		(RegionSelect == null) ? $(".RegionValid").html("Please Select Region.") : $(".RegionValid").html("");
		(PhysicianSelect == null) ? $(".PhyValid").html("Please Selct Physician.") : $(".PhyValid").html("")
		return false;
	}
	//if (AdminDescription ) {

	//}
	//else {

	//}3

	var reqid = $('.AssignConfirmBtn').attr("data-requestId");
	var action = 'AssignPhysician';
	var Trans = $('#headerToggle').text();
	if (Trans == "Trasfer Request") {
		action = 'TransferPhysician';
	}

	$.ajax({
		url: '/AdminPartials/AssignPhysician',
		method: 'POST',
		data: {
			RegionSelect: RegionSelect,
			PhysicianSelect: PhysicianSelect,
			AdminDescription: AdminDescription,
			reqid: reqid,
			actionName: action
		},
		success: function (response) {
			toastr.success("Physician Assigned");
			$(".btn-close").click();
			$(".admin-data-container").load('/AdminPartials/AdminDashBoardMain')
			$(".RegionValid").html("")
		}
	});
};
function ClearCase() {

	var reqid = $('.clearCaseBtn').attr("data-requestId");
	$.ajax({
		url: '/AdminPartials/ClearCase',
		method: 'POST',
		data: {
			reqid: reqid
		},
		success: function (response) {
			if (response) {

				toastr.success("Request Cleared");
			}
			else {

				toastr.error("Something went wroung!");
			}
			$(".ClearClose").click();
			$(".admin-data-container").load('/AdminPartials/AdminDashBoardMain')
		}
	});
}
function BlockCase() {
	var notes = $('#BlockNotes').val();
	var reqid = $('.BlockConfirmBtn').attr("data-requestId");

	if (!notes) {
		$(".BlockReqValid").html("please select reason")
		return false;
	}

	$.ajax({
		url: '/AdminPartials/BlockRequest',
		method: 'POST',
		data: {
			BlockNotes: notes,
			reqid: reqid
		},
		success: function (response) {
			toastr.success("Case Blocked!");
			$(".btn-close").click();
			$(".admin-data-container").load('/AdminPartials/AdminDashBoardMain');
		}
	});
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
	//for pattern
	//email

	$.ajax({
		url: '/AdminPartials/SendAgreement',
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
			$(".admin-data-container").load('/AdminPartials/AdminDashBoardMain')
		}
	});
};
function ContactProvider(phyid, phonenumber, email, firstname) {
	$("#ContactProvider").modal("show");
	$(".ProviderSubmit").attr("data-phyid", phyid);
	$(".ProviderSubmit").attr("data-phonenumber", phonenumber);
	$(".ProviderSubmit").attr("data-email", email);
	$(".ProviderSubmit").attr("data-firstname", firstname);
}

var Consult = document.getElementById("#Consult");
var HouseCall = document.getElementById("#HouseCall");
//function SaveEncounter() {

//    var options = document.querySelector('input[name="options"]:checked').id;
//        //set status as a conclude state
//        var reqid = $('#SaveEncounter').attr('data-requestid');
//        $.ajax({
//            url: '/AdminPartials/SaveEncounterPreferences',
//            data: {
//                reqid: reqid,
//                SelectType: options
//            },
//            method: 'POST',
//            success: function (response) {

//                if (response) {
//                    toastr.success("Encounter Saved!");
//                }
//                else {
//                    toastr.error("Encounter Not Saved!");
//                }
//                $('.EncounterCancel').click();
//                $(".admin-data-container").load('/AdminPartials/AdminDashBoardMain')

//            }
//        });
//}
//function EncounterModel(reqid) {
//    $('#SaveEncounter').attr('data-requestid' , reqid)
//    $('#Encounter_Model').modal("show");
//}

//for providers

$("#Export").click(function (e) {
	e.preventDefault();
	var datas = $('#' + RequestState + 'Excel');
	TableToExcel.convert(datas[0], {
		name: `${RequestState}Data.xlsx`,
		sheet: {
			name: `${RequestState}Data`
		}
	});
});

//$("#ExportAll").click(function () {
//    console.log("afjbasc")
//    if (localStorage.getItem("RequestState") == "NewRequests")
//    {
//        $.ajax({
//            url: '/AdminPartials/ExportAllData',
//            data: { RequestType: 1 },
//            method: 'POST',
//            success: function (data)
//            {
//                var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
//                var link = document.createElement('a');
//                link.href = window.URL.createObjectURL(blob);
//                link.download = 'Requests.xlsx';
//                link.click();
//            } 
//        });
//    }
//});

$("#AdminRequest").click(function () {
	$(".admin-data-container").load('/AdminPartials/AdminPatientRequest');
});

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
	emailtest = regularExpression.test($("#Req_Email").val());
	(!emailtest) ? $(".Email_Validation").html("Email is not Valid") : $(".Email_Validation").html("");
});

$("#SendLinkForm").on("submit", function (e) {
	e.preventDefault();
	var formdata = $(this).serialize();
	if (emailtest && formdata.firstname != "" && formdata.lastname != "") {
		$(".closebtn").click();
		$.ajax({
			url: '/AdminPartials/SendLink',
			method: 'POST',
			data: formdata,
			success: function (response) {
				if (response) {
					toastr.success("Link sent to given user")
				}
				else {
					toastr.error("something went Wrong")
				}
			}
		});
	}
	else {
		($("#Req_FirstName").val() == '') ? $(".firstname_Validation").html("This Field Is Required") : $(".firstname_Validation").html("");

		($("#Req_LastName").val() == '') ? $(".lastname_Validation").html("This Field Is Required") : $(".lastname_Validation").html("")
		var regularExpression = /\S+@\S+\.\S+/;
		emailtest = regularExpression.test($("#Req_Email").val());
		(!emailtest) ? $(".Email_Validation").html("Email is not Valid") : $(".Email_Validation").html("");
	}
});

