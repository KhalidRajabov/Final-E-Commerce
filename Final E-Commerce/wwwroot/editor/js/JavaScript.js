
$(document).ready(function () {

    

    var searchUrl = `https://localhost:44393/`
    $(document).on("keyup", "#search", function () {
        let inputValue = $(this).val();
    
        $("#SearchList li").slice(1).remove();
        $("#SearchList").html()
        $.ajax({
            url: `${searchUrl}editor/Editor/SearchBlog?search=` + inputValue,
            method: "get",
            success: function (res) {
                console.log(res)
                $("#SearchList").append(res);
                //console.log("successfully brought searched objects")
            },
            error: function (err) {
                console.log(err)
            }
        })
    });
});