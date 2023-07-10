$(".time-picker").hunterTimePicker({
    timeFormat: 'HH:mm:ss',
    lan:'en'
});


const form = document.getElementById("form"); 
const title = document.getElementById("title");

const episodeFile = document.getElementById("episodeFile");
const spanEpisodeChecker = document.querySelector('span#EpisodeFileChecker');

title.addEventListener("keyup" , function(){
    checkTitle();
});
$("#episodeFile").change(function () {
    checkEpisodeInput();
});

form.addEventListener('submit', (e) => {
    if (checkTitle() === true && checkEpisodeInput() === true) {
        e.stopPropagation();
    }else{
        e.preventDefault();
    }
});

// Validate
function checkTitle(){
    const titleValue = title.value.trim();
    const titleValueLength = titleValue.length;
    const maxChars = title.getAttribute("data-max-chars");

    if (titleValue === '') {
        setError(title, 'لطفا عنوان را وارد کنید.');
        return false;
    } else if (titleValueLength > maxChars) {
        setError(title, 'عنوان وارد شده نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + titleValueLength + ')');
        return false;
    } else{
        setSuccess(title);
        return true;
    }
}

function checkEpisodeInput() {
    var filePath = episodeFile.value;

    // Allowing file type
    var allowedExtensions = /(\.rar)$/i;

    if (filePath === '' && !episodeFile.classList.contains('update')) {
        spanEpisodeChecker.innerHTML = 'لطفا فایل قسمت دوره را بارگذاری کنید.';
        spanEpisodeChecker.classList.remove('hidden');
        spanEpisodeChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2 error';

        return false;
    } else if (filePath !== '' && !allowedExtensions.exec(filePath)) {
        episodeFile.value = '';

        spanEpisodeChecker.innerHTML = 'لطفا فقط فایل با پسوند .rar بارگذاری کنید.';
        spanEpisodeChecker.classList.remove('hidden');
        spanEpisodeChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2 error';

        return false;
    } else {
        spanEpisodeChecker.classList.add('hidden');
        spanEpisodeChecker.parentElement.className = 'form-control text-gray-700 my-4 mx-2';

        return true;
    }
}


function setError(input , message){
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2 error';
}

function setSuccess(input){
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.classList.add('hidden');
    formControl.className = 'form-control text-gray-700 my-4 mx-2';
}
// End Validate
