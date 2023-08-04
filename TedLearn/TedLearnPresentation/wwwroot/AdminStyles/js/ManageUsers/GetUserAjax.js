$(document).ready(function () {
    console.log($("#info"));

    const form_info = document.querySelectorAll("#form-info");
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
                    if (url.startsWith("/Admin/ManageUsers/GetUserInformation/")) {
                        $("#info").empty().append(response);
                    } else {
                        $("#result").empty().append(response);
                    }
                }
            });
        });
    });

    //const span_paging = document.querySelectorAll("#paging > span");
    $("#paging > span").each(function (x, element) {
        var pageId = element.getAttribute("pageid");
        element.addEventListener("click", function () {
            $("#result").load("/Admin/ManageUsers/GetUsers/GetAllUsers?pageId=" + pageId);
        });
    });
});

