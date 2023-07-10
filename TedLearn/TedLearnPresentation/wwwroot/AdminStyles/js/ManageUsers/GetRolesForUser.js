$(document).ready(function () {
    const form = document.querySelectorAll("#form-info");
    form.forEach(item => {
        item.addEventListener("submit", function (e) {
            e.preventDefault();
        });
        const url = item.getAttribute("action");
        const button = item.firstElementChild;

        button.addEventListener("click", function () {
            $.ajax({
                type: "POST",
                url: url,
                success: function (response) {
                    //if request if made successfully then the response represent the data
                    $("#result").empty().append(response);
                }
            });
        });
    });
});