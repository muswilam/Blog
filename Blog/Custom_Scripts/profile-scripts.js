$(document).ready(function () {

    //admins div : add admin
    $("#admins").on('click', '#saveAdmin', function () {
        var linkUrl = $(this).attr("data-href");
        $.ajax({
            url: linkUrl,
            type: "Post",
            data: $("#addAdminForm").serialize()
        })
        .done(function (result) {
            if (result.success) {

                if (result.isMaster == false) {
                    $("#adminsList").append("<li data-name='" + result.adminUserName + "'>" + result.adminUserName + "  <span class='deleteAdmin' style='float:right'> &times; </span> </li>")
                } else {
                    $("#adminsList").prepend("<li data-name='" + result.adminUserName + "'style='color:#08236d'>" + result.adminUserName + "  <span  style='float:right'> <i class='fa fa-shield' ></i> </span> </li>")
                }
                $("#Name").val('');
                $("#Email").val('');
                $("#userName").val('');
                $("#password").val('');
                $("#isMaster").prop('checked', false);
            } else {
                $("#addAdminErrorDiv").show();
                $("#addAdminErrorDiv").html("<i class='fa fa-bell'></i> " + result.message);
            }
        });
    });

    //admins div : delete admin without refresh after addding
    $("#admins").on('click', '.deleteAdmin', function () {
        var li = $(this).parent();
        var liName = li.attr("data-name");

        var confirmation = confirm("Are you sure, You wanna delete " + liName + ".");
        var linkUrl = "/accounts/DeleteAdmin?userName=" + liName;

        if (confirmation) {
            $.ajax({
                url: linkUrl,
                type: 'Post'
            })
            .done(function (result) {
                if (result.success) {
                    li.remove();
                } else {
                    $("#addAdminErrorDiv").show();
                    $("#addAdminErrorDiv").html("<i class='fa fa-bell'></i> " + result.message);
                }
            });
        }
    });

    //upload image
    $("#selectPic").change(function () {
        var pic = this.files[0];
        var linkUrl = $(this).attr("pic-href");
        var formData = new FormData();
        formData.append("Picture", pic);

        $.ajax({
            url: linkUrl,
            type: 'Post',
            data: formData,
            processData: false,
            contentType: false
        })
        .done(function (ImageUrl) {

            $("#picArea").empty();

            var imgHTML = $("#picTemplate").clone();
            imgHTML.find("img").attr("src", ImageUrl);

            $("#picArea").append(imgHTML.html());
            $("#profileImgUrl").val("~" + ImageUrl);
            debugger;
        })
    });



    //edit profile
    $("#save").click(function () {
        var linkUrl = $(this).attr("data-href");
        $.ajax({
            url: linkUrl,
            type: 'Post',
            data: $("#editForm").serialize()
        })
        .done(function (result) {
            if (result.success) {
                location.reload();
            } else {
                $("#errorDiv").show();
                $("#errorDiv").html("<i class='fa fa-bell'></i> " + result.message);
            }
        });
    });

    //add skill
    $("#addSkill").click(function () {
        var linkUrl = $(this).attr("data-href");
        $.ajax({
            url: linkUrl,
            type: 'Post',
            data: $("#addSkillsForm").serialize()
        })
        .done(function (result) {
            if (result.success) {
                $("#skillName").val('');
                location.reload();
            } else {
                $("#skillsErrorDiv").show();
                $("#skillsErrorDiv").html("<i class='fa fa-bell'></i> " + result.message);
            }
        });
    })

    //delete skill
    $('.deleteSkill').on('click', function () {
        var li = $(this).parent();
        var liId = li.attr("data-id");
        var linkUrl = "/accounts/DeleteSkill/" + liId;
        $.ajax({
            url: linkUrl,
            type: 'Post',
        }).done(function (result) {
            if (result.success) {
                li.remove();
            } else {
                $("#deleteError").show();
                $("#deleteError").html("<i class='fa fa-bell'></i> " + result.message);
            }
        })
        debugger;
    });
});