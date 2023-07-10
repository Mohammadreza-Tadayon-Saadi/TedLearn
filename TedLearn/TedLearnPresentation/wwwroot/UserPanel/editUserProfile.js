const form = document.getElementById("form");
const firsName = document.getElementById("firstName");
const lastName = document.getElementById("lastName");
const email = document.getElementById("email");
const websiteAddress = document.getElementById("websiteAddress");
const biography = document.getElementById("biography");


firsName.addEventListener("keyup", function () {
    checkFirsName();
});
lastName.addEventListener("keyup", function () {
    checkLastName();
});
email.addEventListener("keyup", function () {
    checkEmail();
});
websiteAddress.addEventListener("keyup", function () {
    checkWebsiteAddress();
});
biography.addEventListener("keyup", function () {
    checkBiography();
});

form.addEventListener('submit', (e) => {
    if (checkFirsName() === true && checkLastName() === true &&
        checkEmail() === true && checkBiography() === true &&
        checkWebsiteAddress() === true) {
        e.stopPropagation();
    } else {
        e.preventDefault();
    }
});

// Vlidate
function checkFirsName() {
    const firsNameValue = firsName.value.trim();
    const firsNameValueLength = firsNameValue.length;
    const maxChars = firsName.getAttribute("data-max-chars");

    if (firsNameValueLength > maxChars) {
        setError(firsName, 'نام نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + firsNameValueLength + ')');
        return false;
    }
    else {
        setSuccess(firsName);
        return true;
    }
}


function checkLastName() {
    const lastNameValue = lastName.value.trim();
    const lastNameValueLength = lastNameValue.length;
    const maxChars = lastName.getAttribute("data-max-chars");

    if (lastNameValueLength > maxChars) {
        setError(lastName, 'نام خانوادگی نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + lastNameValueLength + ')');
        return false;
    }
    else {
        setSuccess(lastName);
        return true;
    }
}


function checkEmail() {
    const emailValue = email.value.trim();
    const emailValueLength = emailValue.length;
    const maxChars = email.getAttribute("data-max-chars");

    if (emailValueLength > maxChars) {
        setError(email, 'ایمیل نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + emailValueLength + ')');
        return false;
    } else if (!isEmail(emailValue)) {
        setError(email, 'ایمیل وارد شده صحیح نیست.');
        return false;
    } 
    else {
        setSuccess(email);
        return true;
    }
}


function checkWebsiteAddress() {
    const websiteAddressValue = websiteAddress.value.trim();
    const websiteAddressLength = websiteAddressValue.length;
    const maxChars = websiteAddress.getAttribute("data-max-chars");

    if (!isWebsite(websiteAddressValue)) {
        setError(websiteAddress, 'آدرس سایت وارد شده صحیح نیست.');
        return false;
    }else if (websiteAddressLength > maxChars) {
        setError(websiteAddress, 'آدرس سایت نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + websiteAddressLength + ')');
        return false;
    }
    else {
        setSuccess(websiteAddress);
        return true;
    }
}


function checkBiography() {
    const biographyValue = biography.value.trim();
    const biographyValueLength = biographyValue.length;
    const maxChars = biography.getAttribute("data-max-chars");

    if (biographyValueLength > maxChars) {
        setError(biography, 'بیوگرافی نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + biographyValueLength + ')');
        return false;
    }
    else {
        setSuccess(biography);
        return true;
    }
}


function setError(input, message) {
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control text-gray-700 error';
}

function setSuccess(input) {
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.classList.add('hidden');
    formControl.className = 'form-control text-gray-700';
}

const emailPattern = /(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])/;
function isEmail(email) {
    if (email === '') return true;
    else {
        return emailPattern.test(email);
    }
}

const websitePattern = /(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})/;
function isWebsite(website) {
    if (website === '') return true;
    return websitePattern.test(website);
}
// End Vlidate