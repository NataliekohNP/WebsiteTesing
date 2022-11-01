// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var count = 0;
    var length = 0;
    function Contains(value_1, value_2) {
        if (value_1.indexOf(value_2) != -1) {
            return true;
        }
    };
    $("#customerName").keyup(function () {
        var value_1 = $("#customerName").val().toLowerCase();
        var name = document.getElementsByClassName("name");
        var id = document.getElementsByClassName("id");
        $(".customerName").each(function () {
            if (Contains(name[count].textContent.toLowerCase(), value_1)) {
                length++;
                $(this).show();
                $("#recordCount").text("Record Count: " + length);
                count++;
            }
            else if (Contains(id[count].textContent.toLowerCase(), value_1)) {
                length++;
                $(this).show();
                $("#recordCount").text("Record Count: " + length);
                count++;
            }
            else {
                $(this).hide();
                $("#recordCount").text("Record Count: " + length);
                count++;
            }
        });
        length = 0;
        count = 0;
    });
});
