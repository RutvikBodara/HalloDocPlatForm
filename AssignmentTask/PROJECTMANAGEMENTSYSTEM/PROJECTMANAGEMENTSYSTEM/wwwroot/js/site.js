$("#AddProject").on("click", function () {
    $("#PopUpLoader").load('/Project/LoadAddProjectPartial', function () {
        $("#AddProjectPopUp").modal("show");
        //$.ajax({
        //    url: '/Project/LoadDomain',
        //    method: 'POST',
        //    success: function (response) {
        //        $('.Domain').empty();
        //        //$('#.Domain').append('<option value="-1" Selected>Select Region</option>')
        //        response.forEach(function (item) {
        //            $('.Domain').append('<option value="' + item.name + '">' + item.name + '</option>')
        //        })
        //    }
        //})
    });
});
