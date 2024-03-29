﻿$(document).ready(function () {

    //add comment
    $('#commentSubmitBtn').click(function () {
        var linkUrl = $(this).attr("data-href");
        var linkTarget = $(this).attr("target-href");
        $.ajax({
            url: linkUrl,
            type: 'Post',
            data: $('#commentForm').serialize()
        })
        .done(function (result) {
            if (result.modelNotValid) {

                //validation if modelstate is not valid
                var nameError = $("#nameError");
                var emailError = $("#emailError");
                var commentError = $("#commentError");

                $("#commentName").removeClass("input-validation-error");
                $("#commentEmail").removeClass("input-validation-error");
                $("#commentBody").removeClass("input-validation-error");

                nameError.empty();
                emailError.empty();
                commentError.empty();

                nameError.append(result.nameError);
                emailError.append(result.emailError);
                commentError.append(result.commentError);

                if (nameError.text()) {
                    $("#commentName").addClass("input-validation-error");
                };
                if (emailError.text()) {
                    $("#commentEmail").addClass("input-validation-error");
                };
                if (commentError.text()) {
                    $("#commentBody").addClass("input-validation-error");
                };
            }
            if (result.success) {
                $.ajax({
                    url: linkTarget,
                    type: 'Get',
                    contentType: 'application/json; charset=utf-8',
                })
                .done(function (response) {
                    var comments = response.comments

                    //validation labels (Errors)
                    $("#nameError").empty();
                    $("#emailError").empty();
                    $("#commentError").empty();

                    //validation input red-classes (errors)
                    $("#commentName").removeClass("input-validation-error");
                    $("#commentEmail").removeClass("input-validation-error");
                    $("#commentBody").removeClass("input-validation-error");

                    //get the current date of comment
                    var date = new Date();
                    var currentDay = (date.getDate() < 10 ? '0' : '') + date.getDate();
                    var currentMonth = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
                    var strDate = "at " + date.getHours() + ":" + date.getMinutes() + " on " + currentDay + "-" + currentMonth + "-" + date.getFullYear();

                    $.each(comments, function (index, comment) {
                        //last comment that added
                        var value = comment[comment.length - 1];

                        var commenthtml = "<div class='comment'> <div class='commentName'> <i class='fa fa-user' aria-hidden='true'></i> " + value.Name + "  said : </div> <div class='commentBody'>"
                            + value.Body + "</div> <div class='commentTime'>" + strDate + "</div> </div> </br>";

                        //first comment
                        if (comment.length == 1) {
                            var commentsText = $("<div class='commentsText'>Comments</div></br>");
                            var commentsHTML = $("<div class='comments'></div>");

                            $("#commentContainer").append(commentsText);
                            $("#commentContainer").append(commentsHTML);
                        }

                        $(".comments").prepend(commenthtml);
                        $("#commentBody").val('');
                    })
                });
            } else {
                $("#commentError").append(result.message);
            };
        });
    });

    //delete comment
    // (on) need to use delegation based event handlers for wiring up future elements
    $(document).on('click', '#deleteLink', function () {
        var commentName = $(this).attr("data-name");
        var confirmation = confirm("Are you sure, You wanna delete " + commentName + "'s comment.");
        var link = $(this).attr("data-href");

        if (confirmation) {
            $.ajax({
                url: link,
                type: 'Post'
            })
             .done(function (result) {
                 if (result.success) {
                     $(".comments").load(window.location.href + " #commentsDiv"); // little space before # so important for adding not passing
                 } else {
                     alert(result.message);
                 };
             });
        };
    });

    //pin comment
    $('.comments').on('click', '.pinComment', function () {
        var pinnedComment = $(this);
        var linkUrl = $(this).attr("data-href");
        $.ajax({
            url: linkUrl,
            type: 'Post',
        })
        .done(function (result) {
            if (result.success) {
                $(".comments").load(window.location.href + " #commentsDiv"); // little space before # so important for adding not passing
            } else {
                alert(result.message);
            };
        });
    });

    //un pin comment
    $('.comments').on('click', '.unPinComment', function () {
        var linkUrl = $(this).attr("data-href");
        $.ajax({
            url: linkUrl,
            type: 'Post',
        })
        .done(function (result) {
            if (result.success) {
                $(".comments").load(window.location.href + " #commentsDiv");
            } else {
                alert(result.message);
            };
        });
    });

});