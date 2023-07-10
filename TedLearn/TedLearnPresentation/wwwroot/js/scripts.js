// NavBar Toggle
document.querySelector('#BgNavBar-Header').addEventListener('wheel', preventScroll, {passive: false});
function preventScroll(e){
    e.preventDefault();
    e.stopPropagation();

    return false;
}
const burger = document.getElementById("burger");
const closeNavBar = document.getElementById("closeNavBar");
const BgNavBar = document.getElementById("BgNavBar-Header");
const body = document.querySelector("body");

burger.addEventListener("click" , function(){
    document.getElementById("NavBar-Header").classList.toggle("closeNav");
    BgNavBar.classList.toggle("closeBgNav");
    body.classList.add("overflow-y-hidden");
});
closeNavBar.addEventListener("click" , function(){
    document.getElementById("NavBar-Header").classList.add("closeNav");
    BgNavBar.classList.add("closeBgNav");
    body.classList.remove("overflow-y-hidden");
});
BgNavBar.addEventListener("click" , function(){
    document.getElementById("NavBar-Header").classList.add("closeNav");
    BgNavBar.classList.add("closeBgNav");
    body.classList.remove("overflow-y-hidden");
});
// End NavBar Toggle


const navMenu = document.querySelectorAll(".nav-menu");
navMenu.forEach(item => {
    item.querySelector("p").addEventListener("click" , function(){
        item.classList.toggle('active');
        const subMenu = item.querySelector("ul.nav-subMenu");
        const subMenuIcon = item.querySelector("span#icon > i");

        if(subMenu.style.height) {
            subMenu.style.height = null;
            subMenuIcon.className = "fi fi-rr-caret-left transition-all duration-300 text-xs";
        }
        else{
            subMenu.style.height = subMenu.scrollHeight + 'px';
            subMenuIcon.className = "fi fi-rr-caret-down transition-all duration-300 text-base text-sky-400";
        }
    });
})






