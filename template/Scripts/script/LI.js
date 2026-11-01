document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("login-form");
    const emailInput = document.getElementById("email");
    const passwordInput = document.getElementById("password");
    const emailError = document.getElementById("email-error");
    const passwordError = document.getElementById("password-error");

    form.addEventListener("submit", function (e) {
        e.preventDefault();

        let isValid = true;

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(emailInput.value.trim())) {
            emailError.style.display = "block";
            isValid = false;
        } else {
            emailError.style.display = "none";
        }

        if (passwordInput.value.trim() === "") {
            passwordError.style.display = "block";
            isValid = false;
        } else {
            passwordError.style.display = "none";
        }

       
        if (isValid) {
            window.location.href = "mainmenu";
        }
    });
});