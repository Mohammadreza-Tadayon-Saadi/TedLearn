const form = document.getElementById("form"); 
const title = document.getElementById("title");
const question = document.getElementById("question"); 

ClassicEditor.create(question, {
    ckfinder: { uploadUrl: '/Home/UploadImage' },
    language: 'fa'
});

title.addEventListener("keyup" , function(){
    checkTitle();
});
question.addEventListener("keyup" , function(){
    checkQuestion();
});

form.addEventListener('submit', (e)=>{
    if (checkTitle() === true && checkQuestion() === true){
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
    }
    else if (titleValueLength > maxChars) {
        setError(title, 'عنوان نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + titleValueLength + ')');
        return false;
    }
    else{
        setSuccess(title);
        return true;
    }
}

function checkQuestion(){
    const questionValue = question.value.trim();
    const questionValueLength = questionValue.length;
    const maxChars = question.getAttribute("data-max-chars");

    if (questionValue === '') {
        setError(question, 'لطفا سوال را وارد کنید.');
        return false;
    }
    else if (questionValueLength > maxChars) {
        setError(question, 'سوال نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + questionValueLength + ')');
        return false;
    }
    else {
        setSuccess(question);
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
