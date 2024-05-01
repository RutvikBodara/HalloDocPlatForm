function downloadSelected() {

    let selectedFiles = [];
    let checkboxes = document.querySelectorAll(".checkbtn");
    let count = 0;
    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            selectedFiles.push(count);
        }
        count++;
    });
    selectedFiles.forEach(function (i) {
        document.getElementById(i).click();
    });
};

