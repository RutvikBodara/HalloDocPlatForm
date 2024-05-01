function DeletedAll() {
    let count = 0;
    let checkboxes = document.querySelectorAll(".checkbtn");
    var filesna = document.querySelectorAll(".filesna");
    var flag = false;
    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            flag = true;
            DeleteSeperateFile(filesna[count].innerHTML);
        }
        count++;
    });
    if (!flag) {
        toastr.error("Please Select Some Of file");
    }
}
//console.log("update file")
//function SendMail(email) {
//    console.log(email)
//}

$('.UploadFile').on('submit', function (e) {
    e.preventDefault();
    var formData = new FormData($(this)[0]);
    $.ajax({
        url: '/AdminPartials/Upload_View_Document',
        data: formData,
        method: 'POST',
        processData: false,
        contentType: false,
        success: function (req_id) {
            toastr.success("File Uploaded Successfully");
            $(".admin-data-container").load('/AdminPartials/View_Document?id=' + req_id);
        }
    });
});


function DeleteSeperateFile(fileName) {
    $.ajax({
        url: '/AdminPartials/DeleteSeperateFile',
        data: { fileName: fileName },
        method: 'POST',
        success: function (req_id) {
            if (!req_id) {
                toastr.error("File Not Deleted");
                $(".admin-data-container").load('/AdminPartials/View_Document?id=' + req_id);
            }
            else {
                toastr.success("File Deleted Successfully");
                $(".admin-data-container").load('/AdminPartials/View_Document?id=' + req_id);
            }
        }
    });
}
