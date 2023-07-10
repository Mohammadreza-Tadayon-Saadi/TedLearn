var replyIdInput = document.getElementById("replyId");
var header = document.getElementById("comment-header");

if ($('#reply').length > 0) {
    var replies = document.querySelectorAll("#reply");
    replies.forEach(item => {
        var commentId = item.getAttribute("comsecure");
        item.addEventListener("click", function () {
            replyIdInput.value = commentId;
            header.innerHTML = "پاسخ خود را وارد کنید!";
            document.body.scrollTop = 700; // For Safari
            document.documentElement.scrollTop = 700; // For Chrome, Firefox, IE and Opera
            document.getElementById("form").classList.add('bg-indigo-700');
            setTimeout(function () { document.getElementById("form").classList.remove('bg-indigo-700'); }, 2000);
        });
    });
}

document.querySelectorAll("#delete-comment").forEach(item => {
    const courseId = item.getAttribute("csecure");
    const commentId = item.getAttribute("comsecure");
    const currentPage = document.getElementById("form").getAttribute("current-page");

    item.addEventListener("click", function () {
        $.ajax({
            type: "POST",
            url: "/Courses/DeleteComment/" + commentId + "/" + courseId + "/" + currentPage,
            success: function (response) {
                $("#result-comment").empty().append(response);
            }
        });
    })
});

