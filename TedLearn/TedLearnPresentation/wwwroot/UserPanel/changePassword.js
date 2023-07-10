        const form = document.getElementById("form");
        const password = document.getElementById("Password");
        const newPassword = document.getElementById("NewPassword");
        const reNewPassword = document.getElementById("ReNewPassword");

        password.addEventListener("keyup", function () {
            checkPassword();
        });
        newPassword.addEventListener("keyup", function () {
            checkNewPassword();
        });
        reNewPassword.addEventListener("keyup", function () {
            checkReNewPassword();
        });

        form.addEventListener('submit', (e) => {
            if (checkPassword() === true && checkNewPassword() === true && checkReNewPassword() === true) {
                e.stopPropagation();
            } else {
                e.preventDefault();
            }
        });

        // Vlidate

        function checkPassword() {
            const passwordValue = password.value.trim();

            if (passwordValue === '') {
                setError(password, 'لطفا رمز عبور فعلی را وارد کنید.');
                return false;
            } 
            else {
                setSuccess(password);
                return true;
            }
        }

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

