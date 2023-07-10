window.addEventListener("load", function () {
    $("#comment-section > *").fadeOut();
    $("#description-section > *").fadeOut();
    $("#requirement-section > *").fadeOut();

    $("#comment").on("click", function () {
        $("#course-option > *").addClass("hidden");
        $("#course-option-item > li").removeClass("active");
        $("#comment").addClass("active");
        $("#comment-section").removeClass("hidden");
        $("#comment-section > *").fadeIn(1000);
        $("#description-section > *").fadeOut();
        $("#requirement-section > *").fadeOut();
        $("#head-season-section > *").fadeOut();

        const courseId = document.getElementById("result-comment").getAttribute("csecure");

        $("#result-comment").load("/Courses/ShowComment/" + courseId);

        const status = document.getElementById("status").getAttribute("cstatus");
        if (status == 'پایان یافته' && $('#comment-users').length) {
            ClassicEditor.create(document.getElementById('comment-users'), {
                ckfinder: { uploadUrl:'/Home/UploadImage'},
                language: 'fa',
                toolbar: ['bold', 'italic', 'bulletedList', 'numberedList', '|' , 'blockQuote', 'link']
            });

            // userComment
            const form = document.getElementById('form');
            const textarea = document.getElementById('comment-users');
            const showChar = document.querySelector('.count-chars');
            const maxChars = textarea.getAttribute("data-max-chars");
            showChar.innerHTML = maxChars;
            var commentIdInput = document.getElementById("replyId");

            textarea.addEventListener('keyup', function () {
                const textareaValue = textarea.value;
                const textareaValueLength = textareaValue.length;
                const remaining = maxChars - textareaValueLength;
                showChar.innerHTML = remaining;
                if (textareaValueLength > maxChars) {
                    showChar.classList.add('text-red-600');
                } else {
                    showChar.classList.remove('text-red-600');
                }
            });

            form.addEventListener('submit', (e) => {
                e.preventDefault();
                const currentPage = form.getAttribute("current-page");
                const textareaValue = textarea.value;
                const textareaValueLength = textareaValue.length;
                const commentIdvalue = commentIdInput.value;
                if (textareaValue === '' || textareaValueLength > maxChars) { e.preventDefault(); }
                else {
                    $.ajax({
                        type: "POST",
                        url: "/Courses/AddComment/" + courseId,
                        data: { comment: textareaValue, replyId: commentIdvalue, pageId: currentPage },
                        success: function (response) {
                            $("#result-comment").empty().append(response);
                            textarea.value = '';
                            showChar.innerHTML = maxChars;
                        }
                    });
                }
            });
        }
    });

    $("#description").on("click", function () {
        $("#course-option > *").addClass("hidden");
        $("#course-option-item > li").removeClass("active");
        $("#description").addClass("active");
        $("#description-section").removeClass("hidden");
        $("#description-section > *").fadeIn(1000);
        $("#comment-section > *").fadeOut();
        $("#requirement-section > *").fadeOut();
        $("#head-season-section > *").fadeOut();
    });

    $("#requirement").on("click", function () {
        $("#course-option > *").addClass("hidden");
        $("#course-option-item > li").removeClass("active");
        $("#requirement").addClass("active");
        $("#requirement-section").removeClass("hidden");
        $("#requirement-section > *").fadeIn(1000);
        $("#comment-section > *").fadeOut();
        $("#description-section > *").fadeOut();
        $("#head-season-section > *").fadeOut();
    });

    $("#head-season").on("click", function () {
        $("#course-option > *").addClass("hidden");
        $("#course-option-item > li").removeClass("active");
        $("#head-season").addClass("active");
        $("#head-season-section").removeClass("hidden");
        $("#head-season-section > *").fadeIn(1000);
        $("#comment-section > *").fadeOut();
        $("#description-section > *").fadeOut();
        $("#requirement-section > *").fadeOut();
    });





    // Episode ShowHide
    const seasonTitle = document.querySelectorAll(".season");
    seasonTitle.forEach(item => {
        item.addEventListener("click", function () {
            item.classList.toggle('active');
            const episode = item.nextElementSibling;
            if (episode.style.height) episode.style.height = null;
            else {
                episode.style.height = episode.scrollHeight + 'px';
            }
        });
    });


    // Show Online Episode
    if ($('#play-episode-img').length) {
        const playImgs = document.querySelectorAll('#play-episode-img');
        playImgs.forEach(item => {
            const button = item.parentElement;
            const courseTitle = button.getAttribute('csecure');
            const episodeTitle = button.getAttribute('esecure');

            item.addEventListener("click", function () {
                $.ajax({
                    type: "POST",
                    url: "/Home/ShowCourse/OnlineEpisode/" + courseTitle + "/" + episodeTitle,
                    success: function (response) {
                        if (response.startsWith('/Account')) {
                            window.location.href = response;
                        } else if (response == 404) {
                            window.location.href = response;
                        } else if (response == 403) {
                            window.location.href = response;
                        } else {
                            $("#episodeResult").empty().append(response);
                            document.body.scrollTop = 200; // For Safari
                            document.documentElement.scrollTop = 200; // For Chrome, Firefox, IE and Opera
                        }
                    }
                });
            });
        });
    }
});

