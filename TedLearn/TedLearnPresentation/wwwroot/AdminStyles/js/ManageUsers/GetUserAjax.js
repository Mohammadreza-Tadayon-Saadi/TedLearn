$(document).ready(function () {
    const form_info = document.querySelectorAll("#form-info");
    form_info.forEach(item => {
        item.addEventListener("submit", function (e) {
            e.preventDefault();
        });
        const url = item.getAttribute("action");
        const button = item.firstElementChild;
        const input = item.querySelector('input');
        
        button.addEventListener("click", function () {
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    // Other form data here
                    '__RequestVerificationToken' : input.value
                },
                success: function (response) {
                    //if request if made successfully then the response represent the data
                    if (url.startsWith("/Admin/ManageUsers/DeleteUser/") || url.startsWith("/ManageUsers/DeleteUser/") ||
                        url.startsWith("/Admin/ManageUsers/RestoreUser/" || url.startsWith("/ManageUsers/RestoreUser/"))) {
                        $("#result").empty().append(response);
                    } if (url.startsWith("/GetUserInformation/")) {
                        $("#info").empty().append(response);
                    }
                }
            });
        });
    });

    //const span_paging = document.querySelectorAll("#paging > span");
    $("#paging > span").each(function (x, element) {
        var pageId = element.getAttribute("pageid");
        element.addEventListener("click", function () {
            $("#result").load("/GetUsers/GetAllUsers?pageId=" + pageId);
        });
    });
});

