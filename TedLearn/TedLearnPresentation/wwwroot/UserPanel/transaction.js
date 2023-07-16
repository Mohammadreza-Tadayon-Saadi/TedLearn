$("#result").load("/UserPanel/GetUserTransactions");

const form = document.getElementById("form");
const amount = document.getElementById("amount");

amount.addEventListener("keyup", function () {
    checkAmount();
});

form.addEventListener('submit', (e) => {
    if (checkAmount() === true) {
        e.stopPropagation();
    } else {
        e.preventDefault();
    }
});

// Vlidate

function checkAmount() {
    const amountValue = amount.value;

    if (amountValue === '') {
        return false;
    } else if (!isAmount(amountValue)) {
        setError(amount, 'مبلغ وارد شده صحیح نیست.');
        return false;
    }else if (amountValue < 1000) {
        setError(amount, 'مبلغ وارد شده نمیتواند کمتر از 1000 تومان باشد.');
        return false;
    } else if (amountValue > 2500000) {
        setError(amount, 'مبلغ وارد شده نمیتواند بیشتر از 2,500,000 تومان باشد.');
        return false;
    }
    else {
        setSuccess(amount);
        return true;
    }
}

function setError(input, message) {
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control text-gray-700 w-full sm:w-3/4 mb-4 sm:mb-0 error';
}

function setSuccess(input) {
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.classList.add('hidden');
    formControl.className = 'form-control text-gray-700 w-full sm:w-3/4 mb-4 sm:mb-0';
}

const amountPattern = /^[1-9]\d*$/;
function isAmount(amount) {
    return amountPattern.test(amount);
}
// End Vlidate


