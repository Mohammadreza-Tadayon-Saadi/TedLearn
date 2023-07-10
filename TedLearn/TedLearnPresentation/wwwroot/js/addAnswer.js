const form = document.getElementById("form"); 
const answer = document.getElementById("answer"); 

ClassicEditor.create(answer, {
    ckfinder: { uploadUrl: '/Home/UploadImage' },
    language: 'fa'
});

answer.addEventListener("keyup" , function(){
    checkAnswer();
});

form.addEventListener('submit', (e)=>{
    if (checkAnswer() === true){
        e.stopPropagation();
    }else{
        e.preventDefault();
    }
});

// Validate
function checkAnswer(){
    const answerValue = answer.value.trim();
    const answerValueLength = answerValue.length;
    const maxChars = answer.getAttribute("data-max-chars");

    if (answerValue === '') {
        setError(answer, 'لطفا پاسخ را وارد کنید.');
        return false;
    }
    else if (answerValueLength > maxChars) {
        setError(answer, 'پاسخ نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + answerValueLength + ')');
        return false;
    }
    else {
        setSuccess(answer);
        return true;
    }
}

function setError(input , message){
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control flex flex-wrap items-center md:justify-center w-full relative error';
}

function setSuccess(input){
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.classList.add('hidden');
    formControl.className = 'form-control flex flex-wrap items-center md:justify-center w-full relative';
}

// End Validate
