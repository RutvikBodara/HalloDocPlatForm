function darkmode() {
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        localStorage.setItem("PageTheme", "light");
        document.documentElement.setAttribute('data-bs-theme', 'light')
    }
    else {
        localStorage.setItem("PageTheme", "dark");
        document.documentElement.setAttribute('data-bs-theme', 'dark')
    }
}

if (localStorage.getItem("PageTheme") === "light") {
    document.documentElement.setAttribute('data-bs-theme', 'light')
}
else {
    document.documentElement.setAttribute('data-bs-theme', 'dark')
}
//$(".mobile").intlTelInput({
//    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/8.4.6/js/utils.js"
//});
