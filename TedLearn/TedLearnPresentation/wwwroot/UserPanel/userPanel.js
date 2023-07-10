const profileButton = document.querySelector(".profile-button");
const profileImg = document.querySelector(".profile-img");
const profileDropDown = document.querySelector("#profile-dropdown");

profileButton.addEventListener("click", function () {
    if (profileDropDown.classList.contains("profileClass")) {
        profileDropDown.classList.remove("profileClass");
    } else {
        profileDropDown.classList.add("profileClass");
    }
});
profileImg.addEventListener("click", function () {
    if (profileDropDown.classList.contains("profileClass")) {
        profileDropDown.classList.remove("profileClass");
    } else {
        profileDropDown.classList.add("profileClass");
    }
});
document.addEventListener("click", function (e) {
    const profile = document.getElementById("profile");

    if (!profile.contains(e.target)) {
        profileDropDown.classList.add("profileClass");
    }
});   

// Navbar
const blinker = document.getElementById("blinker");
const bgNavbar = document.getElementById("BgNavBar-Header");
const body_tag = document.querySelector("body");

blinker.addEventListener("click" , function(){
    
    var element = document.getElementById("userPanelNavbar");
    element.classList.toggle("userPanelNavbar-Open");
    
    var logoNavbar = document.getElementById("logo-userPanel-Navbar");
    
    blinker.classList.toggle("bg-focus");
    
    var fingerPrint = document.getElementById("fingerprint");
    fingerPrint.classList.toggle("fingerprint-open")
    
    var NavbarClose = document.getElementById("userPanelNavbar-Unactive");
    var NavbarOpen = document.getElementById("userPanelNavbar-Active");
    
    if(element.classList.contains("userPanelNavbar-Open")){
        bgNavbar.classList.remove("closeBgNav");
        body_tag.classList.add("overflow-y-hidden");
        NavbarClose.classList.add("userPanelNavbar-Close");
        NavbarOpen.classList.remove("userPanelNavbar-Close");
        element.classList.remove("userPanelNavbar");
        logoNavbar.classList.remove("invisible");
    
    }else{
        bgNavbar.classList.add("closeBgNav");
        body_tag.classList.remove("overflow-y-hidden");
        NavbarClose.classList.remove("userPanelNavbar-Close");
        NavbarOpen.classList.add("userPanelNavbar-Close");
        element.classList.add("userPanelNavbar");
        logoNavbar.classList.add("invisible");
    }
});
bgNavbar.addEventListener("click", function(){

    var element = document.getElementById("userPanelNavbar");
    element.classList.remove("userPanelNavbar-Open");
    
    blinker.classList.toggle("bg-focus");
    
    var fingerPrint = document.getElementById("fingerprint");
    fingerPrint.classList.toggle("fingerprint-open")
    
    var NavbarClose = document.getElementById("userPanelNavbar-Unactive");
    var NavbarOpen = document.getElementById("userPanelNavbar-Active");
    
    if(element.classList.contains("userPanelNavbar-Open")){
        bgNavbar.classList.remove("closeBgNav");
        body_tag.classList.add("overflow-y-hidden");
        NavbarClose.classList.add("userPanelNavbar-Close");
        NavbarOpen.classList.remove("userPanelNavbar-Close");
        element.classList.remove("userPanelNavbar");
    }else{
        bgNavbar.classList.add("closeBgNav");
        body_tag.classList.remove("overflow-y-hidden");
        NavbarClose.classList.remove("userPanelNavbar-Close");
        NavbarOpen.classList.add("userPanelNavbar-Close");
        element.classList.add("userPanelNavbar");
    }
});

// End Navbar


// Orders(UserPanel)
const order = document.querySelectorAll(".order");
order.forEach(item => {
    item.addEventListener("click" , function(){
        item.classList.toggle('active');
        const orderDetails = item.nextElementSibling;
        if(orderDetails.style.height) orderDetails.style.height = null;
        else{
            orderDetails.style.height = orderDetails.scrollHeight + 'px';
        }
    });
});
// End Orders(UserPanel)