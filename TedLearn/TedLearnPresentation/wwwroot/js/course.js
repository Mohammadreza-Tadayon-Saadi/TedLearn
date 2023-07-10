const orderbyPriceInput = document.querySelectorAll("#orderby-price-input");
const orderbyPriceLabel = document.querySelectorAll("#orderby-price-label");
const orderbyPrice = document.querySelectorAll(".orderby-price");
const categories = document.querySelectorAll("#categories");
const paging = document.querySelectorAll("#paging > span");
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
        const label = document.querySelector(".active > label");
        label.className = "flex justify-center items-center w-full h-full bg-indigo-700 text-slate-50";
    });
});

categories.forEach(item => {
    item.addEventListener("click", function () {
        categories.forEach(radio => {
            radio.checked = false;
        })
        item.checked = true;
    });
});

paging.forEach(item => {
    var pageId = item.getAttribute("pageId");

    item.addEventListener("click", function () {
        paging.forEach(dis => {
            dis.classList.remove("bg-indigo-700");
            dis.classList.add("bg-indigo-300");
        })
        if (item.classList.contains("bg-indigo-300"))
            item.classList.remove("bg-indigo-300");

        item.classList.add("bg-indigo-700");

        $("input#pageId").val(pageId);
        form.submit();
    });
});