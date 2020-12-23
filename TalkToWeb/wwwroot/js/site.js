function TesteCors()
{
    var tokenJWT = ""
    var service = ""
    $("#result").html("Loading...")

    $.ajax({
        url: service,
        method: "GET",
        crossDomain: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer" + tokenJWT);
        },
        success: function (data, status, xhr) {
            $("#result").html(data);
        }
    })
}
