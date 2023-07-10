const form = document.getElementById("form"); 
const userName = document.getElementById("userName");
const password = document.getElementById("password"); 
const togglePassword = document.querySelector(".toggle-password");

userName.addEventListener("keyup" , function(){
    checkUserName();
});
password.addEventListener("keyup" , function(){
    checkPassword();
});

form.addEventListener('submit', (e)=>{
    if(checkUserName() === true && checkPassword() === true){
        e.stopPropagation();
    }else{
        e.preventDefault();
    }
});

// Validate
function checkUserName(){
    const userNameValue = userName.value.trim();

    if (userNameValue === '') {
        setError(userName, 'لطفا نام کاربری را وارد کنید.');
        return false;
    }
    else{
        setSuccess(userName);
        return true;
    }
}

function checkPassword(){
    const passwordValue = password.value.trim();

    if (passwordValue === '') {
        setError(password, 'لطفا رمز عبور را وارد کنید.');
        return false;
    }
    else{
        setSuccess(password);
        return true;
    }
}

function setError(input , message){
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.innerHTML = message;
    span.classList.remove('hidden');
    formControl.className = 'form-control flex flex-wrap items-center w-full relative error';
}

function setSuccess(input){
    const formControl = input.parentElement;
    const span = formControl.querySelector('span.block');

    span.classList.add('hidden');
    formControl.className = 'form-control flex flex-wrap items-center w-full relative';
}
// End Validate


togglePassword.addEventListener("click" , function(){
    if(password.type === 'password'){
        password.setAttribute('type' , 'text');
        togglePassword.classList.add("hide");
    }else{
        password.setAttribute('type' , 'password');
        togglePassword.classList.remove("hide");
    }
});