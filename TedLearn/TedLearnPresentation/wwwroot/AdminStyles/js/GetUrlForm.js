$(document).ready(function () {

    const form_info = document.querySelectorAll("#form-info");
    form_info.forEach(item => {
        item.addEventListener("submit", function (e) {
            e.preventDefault();
        });
        const url = item.getAttribute("action");
        const button = item.firstElementChild;
        const input = item.querySelector('input[name="__RequestVerificationToken"]');
        button.addEventListener("click", function () {
            let data = {
                // Other form data here
            };
            if (input) {
                data['__RequestVerificationToken'] = input.value;
            }

            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: function (response) {
                    if (url.startsWith("/GetCourseDetails")) {
                        $("#info").empty().append(response);
                    }else {
                        $("#result").empty().append(response);
                    }
                }
            });
        });
    });
});