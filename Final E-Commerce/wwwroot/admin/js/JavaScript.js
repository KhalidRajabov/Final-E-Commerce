$(document).ready(function () {
    
    var searchUrl = `https://localhost:44393/admin/`
    $(document).on("keyup", "#search", function () {
        let inputValue = $(this).val();
        $("#SearchList li").remove();
        $("#SearchList").html()
        $.ajax({
            url: `${searchUrl}AdminSearch/SearchAll?search=` + inputValue,
            method: "get",
            success: function (res) {
                    $("#SearchList").append(res);
            }
        })
    });
});