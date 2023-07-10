const form = document.getElementById("form");
        const code = document.getElementById("code");

        code.addEventListener("keyup", function () {
            checkCode();
        });

        form.addEventListener('submit', (e) => {
            if (checkCode() === true) {
                e.stopPropagation();
            } else {
                e.preventDefault();
            }
        });

        // Vlidate

        function checkCode() {
            const codeValue = code.value.trim();

            if (codeValue === '') {
                setError(userName, 'لطفا کد را وارد کنید.');
                return false;
            }
            else {
                setSuccess(userName);
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

        // End Vlidate
    