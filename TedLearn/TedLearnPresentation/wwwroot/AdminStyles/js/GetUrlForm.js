$(document).ready(function () {
    //const form = document.querySelector('.deleted-or-active-group');
    //const btn_form = form.firstElementChild;
    //const form_url = form.getAttribute('action');

    const form_info = document.querySelectorAll("#form-info");
    form_info.forEach(item => {
        item.addEventListener("submit", function (e) {
            e.preventDefault();
        });
        const url = item.getAttribute("action");
        const button = item.firstElementChild;
        //TODO ChangeUrl
        //if (url.startsWith('/Admin/ManageCourseGroups/DeleteGroup/')) {
        //    form_url.replace('/GetCourseGroups/GetAllDeletedGroups');
        //    btn_form.innerHTML.replace('گروه های حذف شده');
        //    console.log(form_url);
        //    console.log(btn_form);
        //} else if (url.startsWith('/Admin/ManageCourseGroups/RestoreGroup')) {
        //    form_url.replace('/GetCourseGroups/GetAllCourseGroups');
        //    btn_form.innerHTML.replace('مدیریت گروه ها');
        //}
        button.addEventListener("click", function () {
            $.ajax({
                type: "POST",
                url: url,
                success: function (response) {
                    //if request if made successfully then the response represent the data
                    //$("#result").empty().append(response);
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


