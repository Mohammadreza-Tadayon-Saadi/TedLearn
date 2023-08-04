$(document).ready(function () {
    const form_info = document.querySelectorAll("#form-role");
    form_info.forEach(item => {
        item.addEventListener("submit", function (e) {
            e.preventDefault();
        });
        const url = item.getAttribute("action");
        const button = item.querySelector('button');
        const input = item.querySelector('input');

        let data = {
            // Other form data here
        };
        if (input) {
            data['__RequestVerificationToken'] = input.value;
        }
        button.addEventListener("click", function () {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: function (response) {
                    //if request if made successfully then the response represent the data
                    $("#result").empty().append(response);
                }
            });
        });
    });
});

