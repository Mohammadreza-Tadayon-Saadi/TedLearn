const form = document.getElementById("form"); 
const discountCode = document.getElementById("discountCode");
const percent = document.getElementById("percent");
const usableCount = document.getElementById("usableCount");
const startDate = document.getElementById("startDate");
const endDate = document.getElementById("endDate");

discountCode.addEventListener("keyup" , function(){
    checkDiscountCode();
});

startDate.addEventListener("keyup", function () {
    checkStartDate();
});

endDate.addEventListener("keyup", function () {
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
    const startDateLength = startDateValue.length;

    if (startDateLength > 10 || !isDate(startDateValue)) {
        setError(startDate, 'لطفا تاریخ را به درستی وارد کنید.');
        return false;
    } else {
        setSuccess(startDate);
        return true;
    }
}

function checkEndDate() {
    const endDateValue = endDate.value.trim();
    const endDateLength = endDateValue.length;

    if (endDateLength > 10 || !isDate(endDateValue)) {
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
    var formControl = input;
    if (input.classList.contains("date")) {
        formControl = input.parentElement.parentElement;
    } else {
        formControl = formControl.parentElement;
    }
    const span = formControl.querySelector('span.block');
    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2 error';
}

function setSuccess(input){
    var formControl = input;
    if (input.classList.contains("date")) {
        formControl = input.parentElement.parentElement;
    } else {
        formControl = input.parentElement;
    }
    const span = formControl.querySelector('span.block');
    span.classList.add('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2';
}

const datePattern = /^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|([1-2][0-9])|(0[1-9]))))$/;
function isDate(date) {
    return datePattern.test(date);
}
// End Validate
