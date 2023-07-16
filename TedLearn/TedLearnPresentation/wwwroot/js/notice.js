const okBtn = document.getElementById("ok");
const blackBg = document.querySelector("#Bg-Navbar-Black");
const notice = document.getElementById("notice");
document.querySelector('body').classList.add('overflow-hidden');
okBtn.addEventListener("click", function () {
    notice.classList.add("scale-down-center");
    document.querySelector('body').classList.remove('overflow-hidden');
    setTimeout(function () { blackBg.classList.add("hidden"); }, 70);
});

blackBg.addEventListener("click", function () {
    notice.classList.add("scale-down-center");
    document.querySelector('body').classList.remove('overflow-hidden');
    setTimeout(function () { blackBg.classList.add("hidden"); }, 70);
});