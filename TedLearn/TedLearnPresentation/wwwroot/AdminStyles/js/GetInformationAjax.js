setTimeout(function () {
    const okBtn = document.getElementById("ok");
    const blackBg = document.querySelector("#Bg-Navbar-Black");
    const notice = document.getElementById("notice");
    okBtn.addEventListener("click", function () {
        notice.classList.add("scale-down-center");
        setTimeout(function () { blackBg.classList.add("hidden"); }, 70);
    });

    blackBg.addEventListener("click", function () {
        notice.classList.add("scale-down-center");
        setTimeout(function () { blackBg.classList.add("hidden"); }, 70);
    });
}, 100);