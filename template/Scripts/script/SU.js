
    document.addEventListener("DOMContentLoaded", function () {
        const form = document.getElementById("signup-form");

        form.addEventListener("submit", function (event) {
            event.preventDefault(); 


            const username = document.getElementById("username").value.trim();
            const email = document.getElementById("email").value.trim();
            const password = document.getElementById("password").value;
            const confirmPassword = document.getElementById("confirm-password").value;

            // Lấy các element hiển thị lỗi
            const usernameError = document.getElementById("username-error");
            const emailError = document.getElementById("email-error");
            const passwordError = document.getElementById("password-error");
            const confirmError = document.getElementById("confirm-error");

            let isValid = true;

            // 1. Kiểm tra Username (ít nhất 3 ký tự)
            if (username.length < 3) {
                usernameError.style.display = "block";
                isValid = false;
            } else {
                usernameError.style.display = "none";
            }

            // 2. Kiểm tra định dạng Email đơn giản
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(email)) {
                emailError.style.display = "block";
                isValid = false;
            } else {
                emailError.style.display = "none";
            }

            // 3. Kiểm tra Password (ít nhất 6 ký tự)
            if (password.length < 6) {
                passwordError.style.display = "block";
                isValid = false;
            } else {
                passwordError.style.display = "none";
            }

            // 4. Kiểm tra Confirm Password khớp nhau
            if (password !== confirmPassword) {
                confirmError.style.display = "block";
                isValid = false;
            } else {
                confirmError.style.display = "none";
            }

            // Nếu tất cả hợp lệ, chuyển hướng về menu chính
            if (isValid) {

                window.location.href = "mainmenu"; 
            }
        });
    });
