$("#result").load("/GetUsers/GetAllUsers");

const form_orderby = document.getElementById("form-orderby");
const input_orderby = document.querySelector("#form-orderby > input");

form_orderby.addEventListener("submit", function (e) {
    e.preventDefault();
});
input_orderby.addEventListener("keyup", function () {
    $("#result").load("/GetUsers/GetAllUsers?orderBy=" + input_orderby.value);
});