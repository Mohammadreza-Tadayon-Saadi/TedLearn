const form = document.getElementById("form");
        const newPassword = document.getElementById("NewPassword");
        const togglePassword = document.querySelector(".toggle-password");

        const reNewPassword = document.getElementById("ReNewPassword");
        const toggleRePassword = document.querySelector(".toggle-repassword");

        newPassword.addEventListener("keyup", function () {
            checkNewPassword();
        });
        reNewPassword.addEventListener("keyup", function () {
            checkReNewPassword();
        });

        form.addEventListener('submit', (e) => {
            if (checkNewPassword() === true && checkReNewPassword() === true) {
                e.stopPropagation();
            } else {
                e.preventDefault();
            }
        });

        // Vlidate


        function checkNewPassword() {
            const passwordValue = newPassword.value.trim();
            const passwordValueLength = passwordValue.length;
            const maxChars = newPassword.getAttribute("data-max-chars");
            const minChars = newPassword.getAttribute("data-min-chars");

            if (passwordValue === '') {
                setError(newPassword, 'لطفا رمز عبور جدید را وارد کنید.');
                return false;
            } else if (passwordValueLength < minChars) {
                setError(newPassword, 'رمز عبور باید بیشتر از ' + minChars + ' کارکتر باشد.');
                return false;
            } else if (passwordValueLength > maxChars) {
                setError(newPassword, 'رمز عبور نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + passwordValueLength + ')');
                return false;
            } else if (!isPassword(passwordValue)) {
                setError(newPassword, "رمز عبور وارد شده حداقل باید دارای عدد و حرف باشد.")
                return false;
            }
            else {
                setSuccess(newPassword);
                return true;
            }
        }


        function checkReNewPassword() {
            const passwordValue = newPassword.value.trim();
            const rePasswordValue = reNewPassword.value.trim();


            if (passwordValue !== rePasswordValue) {
                setError(reNewPassword, 'با رمز عبور وارد شده همخوانی ندارد.');
                return false;
            }
            else {
                setSuccess(reNewPassword);
                return true;
            }
        }

        function setError(input, message) {
            const formControl = input.parentElement;
            const span = formControl.querySelector('span.block');

            span.innerHTML = message;
            span.classList.remove('hidden');
            formControl.className = 'form-control flex flex-wrap items-center w-full relative error';
        }

        function setSuccess(input) {
            const formControl = input.parentElement;
            const span = formControl.querySelector('span.block');

            span.classList.add('hidden');
            formControl.className = 'form-control flex flex-wrap items-center w-full relative';
        }

        const passwordPattern = /(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])/;
        function isPassword(password) {
            return passwordPattern.test(password);
        }
        // End Vlidate


        togglePassword.addEventListener("click", function () {
            if (newPassword.type === 'password') {
                newPassword.setAttribute('type', 'text');
                togglePassword.classList.add("hide");
            } else {
                newPassword.setAttribute('type', 'password');
                togglePassword.classList.remove("hide");
            }
        });

        toggleRePassword.addEventListener("click", function () {
            if (reNewPassword.type === 'password') {
                reNewPassword.setAttribute('type', 'text');
                toggleRePassword.classList.add("hide");
            } else {
                reNewPassword.setAttribute('type', 'password');
                toggleRePassword.classList.remove("hide");
            }
        });