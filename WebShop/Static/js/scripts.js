var itemCount = 0;

$(".add").click(function () {
    itemCount++;
    $("#itemCount").html(itemCount).css("display", "block");
});

$(".clear").click(function () {
    itemCount = 0;
    $("#itemCount").html("").css("display", "none");
    $("#cartItems").html("");
});

$("#myLink").click(function (e) {

    e.preventDefault();
    $.ajax({

        url: $(this).attr("href"), // comma here instead of semicolon   
        success: function () {
            alert("Value Added");  // or any other indication if you want to show
        }

    });

});


//$(function () {
//    $("#myDIV").hide();
//    $("#preview").on("click", function () {
//        $("#myDIV").toggle();
//    });
//});