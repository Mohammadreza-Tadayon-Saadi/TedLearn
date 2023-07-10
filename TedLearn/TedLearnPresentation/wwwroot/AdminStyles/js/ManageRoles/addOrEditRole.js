const form = document.getElementById("form"); 
const roleName = document.getElementById("roleName");
roleName.addEventListener("keyup" , function(){
    checkRoleName();
});

form.addEventListener('submit', (e)=>{
    if (checkRoleName() === true){
        e.stopPropagation();
    }else{
        e.preventDefault();
    }
});

// Validate
function checkRoleName(){
    const roleNameValue = roleName.value.trim();
    const roleNameValueLength = roleNameValue.length;
    const maxChars = roleName.getAttribute("data-max-chars");

    if (roleNameValue === '') {
        setError(roleName, 'لطفا نام نقش را وارد کنید.');
        return false;
    } else if (roleNameValueLength > maxChars) {
        setError(roleName, 'نام نقش نباید بیشتر از ' + maxChars + ' کارکتر باشد.( ' + roleNameValueLength + ')');
        return false;
    } else{
        setSuccess(roleName);
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
