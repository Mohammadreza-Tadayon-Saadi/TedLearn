let options = {
    time: true,
    date: true,
}
jalaliDatepicker.startWatch(options);

const form = document.getElementById("form"); 
const discountCode = document.getElementById("discountCode");
const percent = document.getElementById("percent");
const usableCount = document.getElementById("usableCount");
const startDate = document.getElementById("startDate");
const endDate = document.getElementById("endDate");

discountCode.addEventListener("keyup" , function(){
    checkDiscountCode();
});

startDate.addEventListener("change", function () {
    checkStartDate();
});

endDate.addEventListener("change", function () {
    checkEndDate();
});

percent.addEventListener("keyup", function () {
    checkPercent();
});

usableCount.addEventListener("keyup", function () {
    checkUsableCount();
});

form.addEventListener('submit', (e) => {
    if (checkDiscountCode() === true &&
        checkStartDate() === true && checkEndDate() === true &&
        checkPercent() === true && checkUsableCount() === true) {
        e.stopPropagation();
    }else{
        e.preventDefault();
    }
});

// Validate
function checkDiscountCode(){
    const discountCodeValue = discountCode.value.trim();
    const discountCodeLength = discountCodeValue.length;
    const maxChars = discountCode.getAttribute("data-max-chars");

    if (discountCodeValue === '') {
        setError(discountCode, 'لطفا کد تخفیف را وارد کنید.');
        return false;
    } else if (discountCodeLength > maxChars) {
        setError(discountCode, 'کد تخفیف نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + discountCodeLength + ')');
        return false;
    } else{
        setSuccess(discountCode);
        return true;
    }
}

function checkStartDate() {
    const startDateValue = startDate.value.trim();

    if (startDateValue === '') {
        setError(startDate, 'لطفا تاریخ شروع را وارد کنید.');
        return false;
    } else if (!isDate(startDateValue)) {
        setError(startDate, 'لطفا تاریخ را به درستی وارد کنید.');
        return false;
    } else {
        setSuccess(startDate);
        return true;
    }
}

function checkEndDate() {
    const endDateValue = endDate.value.trim();

    if (endDateValue === '') {
        setError(endDate, 'لطفا تاریخ اتمام را وارد کنید.');
        return false;
    } else if (!isDate(endDateValue)) {
        setError(endDate, 'لطفا تاریخ را به درستی وارد کنید.');
        return false;
    } else {
        setSuccess(endDate);
        return true;
    }
}

function checkPercent() {
    const percentValue = percent.value.trim();

    if (percentValue < 1 || percentValue > 100) {
        setError(percent, 'درصد تخفیف باید بین 1 تا 100 باشد.');
        return false;
    }else {
        setSuccess(percent);
        return true;
    }
}

function checkUsableCount() {
    const usableCountValue = usableCount.value.trim();

    if (usableCountValue < 1 || usableCountValue > 100) {
        setError(usableCount, 'تعداد تخفیف باید بین 1 تا 100 باشد.');
        return false;
    } else {
        setSuccess(usableCount);
        return true;
    }
}

function setError(input, message) {
    var formControl = input.parentElement;
    const span = formControl.querySelector('span.block');
    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2 error';
}

function setSuccess(input) {
    var formControl = input.parentElement;
    const span = formControl.querySelector('span.block');
    span.classList.add('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2';
}

const datePattern = /^(\d{4})\/([0-9]|0[1-9]|1[0-2])\/([0-9]|0[1-9]|[12][0-9]|3[01]) ([01][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$/;
function isDate(date) {
    return datePattern.test(date);
}
// End Validate
