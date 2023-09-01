const form = document.getElementById("form");
const discountCode = document.getElementById("code");

form.addEventListener('submit', (e) => {
    if (checkDiscountCode() === true) {
        e.stopPropagation();
    } else {
        e.preventDefault();
    }
});

function checkDiscountCode() {
    const discountCodeValue = discountCode.value.trim();
    const discountCodeLength = discountCodeValue.length;
    const maxChars = discountCode.getAttribute("data-max-chars");

    if (discountCodeValue === '') {
        setError(discountCode, 'لطفا کد تخفیف را وارد کنید.');
        return false;
    } else if (discountCodeLength > maxChars) {
        setError(discountCode, 'کد تخفیف نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + discountCodeLength + ')');
        return false;
    } else {
        setSuccess(discountCode);
        return true;
    }
}

function setError(input, message) {
    var formControl = input.parentElement;
    const span = document.querySelector('span.codValidate');
    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'flex w-full error';
}

function setSuccess(input) {
    var formControl = input.parentElement;
    const span = formControl.querySelector('span.codValidate');
    span.classList.add('hidden');
    formControl.className = 'flex w-full';
}