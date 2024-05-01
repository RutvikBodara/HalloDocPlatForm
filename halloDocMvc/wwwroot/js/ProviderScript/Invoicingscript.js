$(document).ready(function () {

    var currendate = new Date();
    var firstday = new Date(currendate.getFullYear(), currendate.getMonth(), 1);
    var lastday = new Date(currendate.getFullYear(), currendate.getMonth(), 15);
    var SecondFirstDate = new Date(currendate.getFullYear(), currendate.getMonth(), 16);
    var SecondlastDay = new Date(currendate.getFullYear(), currendate.getMonth() + 1, 0);

    var formatedfirstday = formatDate(firstday);
    var formatedLastday = formatDate(lastday);
    var formatedSecondFirstDate = formatDate(SecondFirstDate);
    var formatedSecondLastday = formatDate(SecondlastDay);
    var x = 100;
    do {
        //lasthalf
        $(".FetchDates").append("<option value='" + formatedSecondFirstDate + "/" + formatedSecondLastday + "'> " + formatedSecondFirstDate + " to " + formatedSecondLastday + "  </option>")
        //firstHalf
        $(".FetchDates").append("<option value='" + formatedfirstday + "/" + formatedLastday + "'> " + formatedfirstday + " to " + formatedLastday + "  </option>")
        x--;
        firstday.setMonth(firstday.getMonth() - 1);
        lastday.setMonth(lastday.getMonth() - 1);
        SecondFirstDate.setMonth(SecondFirstDate.getMonth() - 1);
        SecondlastDay.setDate(0);
        formatedfirstday = formatDate(firstday)
        formatedLastday = formatDate(lastday);
        formatedSecondFirstDate = formatDate(SecondFirstDate);
        formatedSecondLastday = formatDate(SecondlastDay);
    } while (x != 0)
})
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}
$(".FetchDates").on("change", function () {
    $(".LoadExistingTimeSheet").load('/ProviderAccount/FinalizeTimeSheetView?DateScoped=' + $(".FetchDates").val(), function () {
    });
});
$("#FinalizeTimeSheet").on("click", function () {
    $(".Provider-data-container").load('/ProviderAccount/FinalizeTimeSheetView?DateScoped=' + $(".FetchDates").val(), function () {
    });
});
$(".WekklyFormSubmit").on("submit", function (e) {
    e.preventDefault();

    var formdata = new FormData();
    formdata.append("Date", $(".Shift-Dates:first").text());
    $('.Total-hours').each(function () {
        console.log($(this).val())
        //var data = $(this.textContent);
        if ($(this).val() == "") {
            $(this).val("0");
        }
        formdata.append("TotalHoursPost", $(this).val());
    });

    $('.House-call').each(function () {
        console.log($(this).val())
        //var data = $(this.textContent);
        if ($(this).val() == "") {
            $(this).val("0");
        }
        formdata.append("HouseCallPost", $(this).val());
    });
    $('.Phone-call').each(function () {
        console.log($(this).val())
        //var data = $(this.textContent);
        if ($(this).val() == "") {
            $(this).val("0");
        }
        formdata.append("PhoneConsult", $(this).val());
    });
    console.log(formdata);
    $.ajax({
        url: '/ProviderAccount/SubmitWeeklyForm',
        method: 'POST',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (response) {
            if(response)
                toastr.success("Data Changed Sucessfuly");
            else
                toastr.error("something went wrong");
        }
    })
})
