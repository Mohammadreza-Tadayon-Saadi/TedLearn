//////////////////////////////////
const form = document.getElementById("form");
const userName = document.getElementById("userName");
const phoneNumber = document.getElementById("phoneNumber");
const password = document.getElementById("password");
const rePassword = document.getElementById("rePassword");

userName.addEventListener("keyup", function () {
    checkUserName();
});
phoneNumber.addEventListener("keyup", function () {
    checkPhoneNumber();
});
password.addEventListener("keyup", function () {
    checkPassword();
});
rePassword.addEventListener("keyup", function () {
    checkRePassword();
});

form.addEventListener('submit', (e) => {
    if (checkUserName() === true && checkPhoneNumber() === true &&
        checkPassword() === true && checkRePassword() === true) {
        e.stopPropagation();
    } else {
        e.preventDefault();
    }
});

// Vlidate
function checkUserName() {
    const userNameValue = userName.value.trim();
    const userNameValueLength = userNameValue.length;
    const maxChars = userName.getAttribute("data-max-chars");
    const minChars = userName.getAttribute("data-min-chars");

    if (userNameValue === '') {
        setError(userName, 'لطفا نام کاربری را وارد کنید.');
        return false;
    } else if (userNameValueLength < minChars) {
        setError(userName, 'نام کاربری باید بیشتر از ' + minChars + ' کارکتر باشد.');
        return false;
    } else if (userNameValueLength > maxChars) {
        setError(userName, 'نام کاربری نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + userNameValueLength + ')');
        return false;
    }
    else {
        setSuccess(userName);
        return true;
    }
}

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

function checkPassword() {
    const passwordValue = password.value.trim();
    const passwordValueLength = passwordValue.length;
    const maxChars = password.getAttribute("data-max-chars");
    const minChars = password.getAttribute("data-min-chars");

    if (passwordValue === '') {
        setError(password, 'لطفا رمز عبور را وارد کنید.');
        return false;
    } else if (passwordValueLength < minChars) {
        setError(password, 'رمز عبور باید بیشتر از ' + minChars + ' کارکتر باشد.');
        return false;
    } else if (passwordValueLength > maxChars) {
        setError(password, 'رمز عبور نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + passwordValueLength + ')');
        return false;
    } else if (!isPassword(passwordValue)) {
        setError(password, "رمز عبور وارد شده حداقل باید دارای عدد و حرف باشد.")
        return false;
    }
    else {
        setSuccess(password);
        return true;
    }
}

function checkRePassword() {
    const passwordValue = password.value.trim();
    const rePasswordValue = rePassword.value.trim();


    if (passwordValue !== rePasswordValue) {
        setError(rePassword, 'با رمز عبور وارد شده همخوانی ندارد.');
        return false;
    }
    else {
        setSuccess(rePassword);
        return true;
    }
}



function setError(input, message) {
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2 error';
}

function setSuccess(input) {
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.classList.add('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2';
}

const passwordPattern = /(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])/;
function isPassword(password) {
    return passwordPattern.test(password);
}

const mobilePattern = /^09[0|1|2|3|9][0-9]{8}$/;
function isPhone(phoneNumber) {
    return mobilePattern.test(phoneNumber);
}

// End Vlidate
