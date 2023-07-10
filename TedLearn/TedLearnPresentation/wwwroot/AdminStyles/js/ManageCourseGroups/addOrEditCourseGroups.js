const form = document.getElementById("form"); 
const title = document.getElementById("title");
title.addEventListener("keyup" , function(){
    checkTitle();
});

form.addEventListener('submit', (e)=>{
    if (checkTitle() === true){
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
