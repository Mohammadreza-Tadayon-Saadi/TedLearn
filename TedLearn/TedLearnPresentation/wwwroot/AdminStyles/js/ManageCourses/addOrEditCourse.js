const form = document.getElementById("form");
const title = document.getElementById("title");

const groupSelectList = document.getElementById("GroupId");
const subGroupSelectList = document.getElementById("SubGroupList");
const teacherSelectList = document.getElementById("TeacherId");
const statusSelectList = document.getElementById("StatusId");
const levelSelectList = document.getElementById("courseLevel");

const price = document.getElementById("price");

const courseImageFile = document.getElementById("CourseImageFile");

const showImgForCourse = document.getElementById("ImgCourse");
if (showImgForCourse.getAttribute("src") === '') { showImgForCourse.classList.add("hidden"); }
else showImgForCourse.classList.remove("hidden");

const spanImgChecker = document.querySelector('span#CourseImageChecker');

const demoFile = document.getElementById("demoFile");
const spanDemoChecker = document.querySelector('span#CourseDemoChecker');

const requierments = document.getElementById("requierments");
const tags = document.getElementById("tags");

const textarea = document.getElementById('description');


title.addEventListener("keyup", function () {
    checkTitle();
});
price.addEventListener("keyup", function () {
    checkPrice();
});
requierments.addEventListener("keyup", function () {
    checkRequierments();
});
tags.addEventListener("keyup", function () {
    checkTags();
});
// Show Count Of Character Of Discription And Validate it
textarea.addEventListener('keyup', function () {
    checkDescription();
});

$("#demoFile").change(function () {
    checkDemoInput();
});

$("#CourseImageFile").change(function () {
    checkImgInput();
});

form.addEventListener('submit', (e) => {
    if (checkTitle() === true && checkGroupList() === true &&
        checkTeacherList() === true && checkStatusList() === true &&
        checkLevelList() === true && checkPrice() === true &&
        checkRequierments() === true && checkTags() === true &&
        checkDescription() === true && checkDemoInput() === true &&
        checkImgInput() === true) {
        e.stopPropagation();
    }
    else {
        e.preventDefault();
    }
});

// Validate
function checkTitle() {
    const titleValue = title.value.trim();
    const titleValueLength = titleValue.length;
    const maxChars = title.getAttribute("data-max-chars");

    if (titleValue === '') {
        setError(title, 'لطفا عنوان را وارد کنید.');
        return false;
    } else if (titleValueLength > maxChars) {
        setError(title, 'عنوان وارد شده نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + titleValueLength + ')');
        return false;
    } else {
        setSuccess(title);
        return true;
    }
}

function checkGroupList() {
    const groupListValue = groupSelectList.value;

    if (groupListValue === '') {
        setError(groupSelectList, 'لطفا گروه دوره را مشخص کنید.');
        return false;
    } else {
        setSuccess(groupSelectList);
        return true;
    }
}

function checkTeacherList() {
    const teacherListValue = teacherSelectList.value;

    if (teacherListValue === '') {
        setError(teacherSelectList, 'لطفا استاد دوره را مشخص کنید.');
        return false;
    } else {
        setSuccess(teacherSelectList);
        return true;
    }
}

function checkStatusList() {
    const statusListValue = statusSelectList.value;

    if (statusListValue === '') {
        setError(statusSelectList, 'لطفا وضعیت دوره را مشخص کنید.');
        return false;
    } else {
        setSuccess(statusSelectList);
        return true;
    }
}

function checkLevelList() {
    const levelListValue = levelSelectList.value;

    if (levelListValue === '') {
        setError(levelSelectList, 'لطفا سطح دوره را مشخص کنید.');
        return false;
    } else {
        setSuccess(levelSelectList);
        return true;
    }
}

function checkPrice() {
    const priceValue = price.value.trim();
    const minChars = price.getAttribute("data-min-chars");

    if (priceValue === '') {
        setSuccess(price);
        return true;
    } else if (!isPrice(priceValue)) {
        setError(price, 'قیمت وارد شده صحیح نیست');
        return false;
    } else if (priceValue.length < minChars) {
        setError(price, 'قیمت نمیتواند کمتر از 1000 تومان باشد.');
        return false;
    } else {
        setSuccess(price);
        return true;
    }
}

function checkImgInput() {
    var filePath = courseImageFile.value;
    // Allowing file type
    var allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;


    if (filePath === '' && !courseImageFile.classList.contains('update')) {
        spanImgChecker.innerHTML = 'لطفا عکس را بارگذاری کنید.';
        spanImgChecker.classList.remove('hidden');
        spanImgChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2 error';

        showImgForCourse.classList.add("hidden");
        showImgForCourse.classList.remove("mt-2");

        return false;
    } else if (filePath !== '' && !allowedExtensions.exec(filePath)) {
        courseImageFile.value = '';

        spanImgChecker.innerHTML = 'لطفا فقط عکس بارگذاری کنید.';
        spanImgChecker.classList.remove('hidden');
        spanImgChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2 error';

        showImgForCourse.classList.add("hidden");
        showImgForCourse.classList.remove("mt-2");

        return false;
    } else {
        readURL(courseImageFile);
        showImgForCourse.classList.remove("hidden");
        showImgForCourse.classList.add("mt-2");

        spanImgChecker.classList.add('hidden');
        spanImgChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2';

        return true;
    }
}

function checkDemoInput() {
    var filePath = demoFile.value;

    // Allowing file type
    var allowedExtensions = /(\.mp4|\.mkv)$/i;

    if (filePath === '' && !demoFile.classList.contains('update')) {
        spanDemoChecker.innerHTML = 'لطفا پیش نمایش دوره را بارگذاری کنید.';
        spanDemoChecker.classList.remove('hidden');
        spanDemoChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2 error';

        return false;
    } else if (filePath !== '' && !allowedExtensions.exec(filePath)) {
        demoFile.value = '';

        spanDemoChecker.innerHTML = 'لطفا فقط ویدیو بارگذاری کنید.';
        spanDemoChecker.classList.remove('hidden');
        spanDemoChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2 error';

        return false;
    } else {
        spanDemoChecker.classList.add('hidden');
        spanDemoChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2';

        return true;
    }
}

function checkRequierments() {
    const requiermentsValue = requierments.value.trim();
    const requiermentsValueLength = requiermentsValue.length;
    const maxChars = requierments.getAttribute("data-max-chars");

    if (requiermentsValue === '') {
        setSuccess(requierments);
        return true;
    } else if (requiermentsValueLength > maxChars) {
        setError(requierments, 'پیش نیاز های وارد شده نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + requiermentsValueLength + ')');
        return false;
    } else {
        setSuccess(requierments);
        return true;
    }
}

function checkTags() {
    const tagsValue = tags.value.trim();
    const tagsValueLength = tagsValue.length;
    const maxChars = tags.getAttribute("data-max-chars");

    if (tagsValue === '') {
        setSuccess(tags);
        return true;
    } else if (tagsValueLength > maxChars) {
        setError(tags, 'برچسب های وارد شده نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + tagsValueLength + ')');
        return false;
    } else {
        setSuccess(tags);
        return true;
    }
}

function checkDescription() {
    const textareaValue = textarea.value;
    const showChar = document.querySelector('.count-chars');
    const maxCharsOfDescription = textarea.getAttribute("data-max-chars");
    const textareaValueLength = textareaValue.length;
    const remaining = maxCharsOfDescription - textareaValueLength;

    showChar.innerHTML = remaining;

    if (textareaValue === '') {
        setError(textarea, 'توضیحات دوره نمیتواند خالی باشد');
        return false;
    } else if (textareaValueLength > maxCharsOfDescription) {
        setError(textarea, 'توضیحات دوره وارد شده نباید بیشتر از ' + maxCharsOfDescription + ' کارکتر باشد.( ' + textareaValueLength + ')');
        return false;
    } else {
        setSuccess(textarea);
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

const pricePattern = /^[1-9][0-9]\d*$/;
function isPrice(price) {
    return pricePattern.test(price);
}


function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#ImgCourse').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
// End Validate



// Get SubGroup For Selected Group By Ajax
$("#GroupId").change(function () {
    $("#SubGroupList").empty();
    $.getJSON('/ManageCourses/GetSubGroupForGroup/' + $("#GroupId :selected").val(),
        function (data) {
            $.each(data,
                function () {
                    $("#SubGroupList").append('<option value=' + this.value + '>' + this.text + '</option>');
                }
            );
        });
});


// CKEditor
ClassicEditor.create(textarea, {
    ckfinder: { uploadUrl: '/Home/UploadImage' },
    language: 'fa'
});