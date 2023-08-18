const orderbyType = document.getElementById("orderByType");
const take = document.getElementById("take");
const filterByCourseTitle = document.getElementById("filterByCourseTitle");
const orderbyPriceInput = document.querySelectorAll("#orderby-price-input");
const orderbyPriceLabel = document.querySelectorAll("#orderby-price-label");
const orderbyPrice = document.querySelectorAll(".orderby-price");
const categories = document.querySelectorAll("#category");
const paging = document.querySelectorAll("#paging > a");
var form = document.getElementById("formFilter");

orderbyPriceInput.forEach(item => {
    item.addEventListener("click", function () {
        orderbyPriceInput.forEach(radio => {
            radio.checked = false;
        })
        orderbyPrice.forEach(parent => {
            parent.classList.remove('active');
        });
        orderbyPriceLabel.forEach(label => {
            label.className = "flex justify-center items-center w-full h-full";
        })
        item.parentElement.classList.add('active');
        item.checked = true;
        form.submit();
    });
});

categories.forEach(item => {
    const label = item.parentElement.querySelector('label');
    label.addEventListener("click", function () {
        categories.forEach(radio => {
            radio.checked = false;
        })
        item.checked = true;
        form.submit();
    });
});

orderbyType.addEventListener('change', function () {
    form.submit();

});

paging.forEach(item => {
    const priceType = document.querySelector("#orderby-price-input[name='priceType']:checked");
    const category = document.querySelector("#category[name='category']:checked");
    var pageId = item.getAttribute("pageId");
    const url = `/Home/Courses?category=${category.value}&filterByCourseTitle=${filterByCourseTitle.value}&priceType=${priceType.value}&orderByType=${orderbyType.value}&take=${take.value}&pageId=${pageId}`;
    item.setAttribute("href", url);
});