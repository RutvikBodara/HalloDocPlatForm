const togglerPswd = document.querySelector("#togglePswd");
const pswd = document.querySelector("#floatingPassword2");
togglerPswd.addEventListener("click", function (e) {
    const changeType = pswd.getAttribute("type") === "password" ? "text" : "password";
    pswd.setAttribute("type", changeType);
    this.classList.toggle("bi-eye");
});