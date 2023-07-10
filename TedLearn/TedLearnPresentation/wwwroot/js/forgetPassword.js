const form = document.getElementById("form");
        const phoneNumber = document.getElementById("phoneNumber");

        phoneNumber.addEventListener("keyup", function () {
            checkPhoneNumber();
        });

        form.addEventListener('submit', (e) => {
            if (checkPhoneNumber() === true) {
                e.stopPropagation();
            } else {
                e.preventDefault();
            }
        });

        // Vlidate

        function checkPhoneNumber() {
            const phoneNumberValue = phoneNumber.value.trim();
            if (phoneNumberValue === '') {
                setError(phoneNumber, 'لطفا شماره تلفن را وارد کنید.');
                return false;
            }
            else if (!isPhone(phoneNumberValue)) {
                setError(phoneNumber, 'شماره تلفن وارد شده صحیح نیست.');
                return false;
            }
            else {
                setSuccess(phoneNumber);
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
        
        const mobilePattern = /^09[0|1|2|3][0-9]{8}$/;
        function isPhone(phoneNumber) {
            return mobilePattern.test(phoneNumber);
        }
        // End Vlidate