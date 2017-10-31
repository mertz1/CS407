//var service = "http://localhost:50295/CivilMapService.svc/";
var service = "http://civilmaptestservice.cloudapp.net/CivilMapService.svc/";
var hours = 2;

window.onload = function() {
    document.getElementById('psw').onkeydown = function(e) {
        if (e.keyCode == 13) {
            submitForm();
        }
    };
    document.getElementById('user').onkeydown = function(e) {
        if (e.keyCode == 13) {
            submitForm();
        }
    };
    checkToken();
}

function submitForm() {
    var form = document.getElementById("form").elements;
    var userInfo = { 'username': form.username.value, 'password': form.password.value };
    console.log(userInfo);

    $.ajax({
        url: service + "Login",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(userInfo),
        dataType: 'json',
        success: function(data) {
            //addBaseInfoItem();
            console.log(JSON.stringify(data));
            if (!(data.includes("Error"))) {
                writeCookie("username", form.username.value, hours);
                writeCookie("token", data, hours);
                window.location.href = "main.html";
            } else {
                alert(data);
            }
        }
    });
}

function checkToken() {
    if (document.cookie.length != 0) {
        console.log("token checking：", document.cookie);
        var userInfo_token = document.cookie.split("=");
        if (userInfo_token[2] != "undefined") {
            $.ajax({
                url: service + "Login",
                method: 'POST',
                contentType: '/application/json; charset=utf-8',
                data: JSON.stringify(userInfo_token[1]),
                dataType: 'json',
                success: function(data) {
                    if (data[0] != 0) {
                        alert(data[0]);
                    }
                }
            });
        }
    }
    console.log("token checking：", document.cookie);
}

function writeCookie(key, value, hours) {
    console.log("write cookie: key+value+hours: ", key, value, hours);
    if (hours) {
        var date = new Date();
        date.setTime(date.getTime() + (hours * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    } else {
        var expires = "";
    }
    document.cookie = key + "=" + value + expires + "; path=/";
    document.cookie = "hahahaha" + "hahahahxi";
    console.log(document.cookie);
}