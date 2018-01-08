//var service = "http://localhost:50295/CivilMapService.svc/";
var service = "http://civilmaptestservice.cloudapp.net/CivilMapService.svc/";

$(function() {
    $("#datepicker_start").datepicker();
    $("#datepicker_end").datepicker();
});

var hm = false;
var longitude;
var latitude;

function getRadiusAndCenter(lon, lat) {
    longitude =  lon;
    latitude = lat;
    console.log(longitude);
    console.log(latitude);
    form.longitude.value = lon;
    form.latitude.value = lat;
}

$(document).ready(function(){
    var open = false;
    $('#nav-icon1,#nav-icon2,#nav-icon3,#nav-icon4').click(function(){
        $(this).toggleClass('open');
        if(open == false) {
            document.getElementById("side").style.display = "block";
            document.getElementById("main").style.right = "3vw";
            open = true;
        } else {
            document.getElementById("side").style.display = "none";
            document.getElementById("main").style.right = "9vw";
            open = false;
        }
    });

    var pre_slide = false;
    $("#pre_map").click(function(){
        if(pre_slide == false) {
            $("#pre_map_panel").slideDown("fast");
            pre_slide = true;
        }
        else {
            $("#pre_map_panel").slideUp("fast");
            pre_slide = false;
        }
    });
    $("#p1").click(function(){
        $("#pre_map_panel").slideUp("fast");
        document.getElementById("pre_map").innerHTML = "Past Week";
        pre_slide = false;
        predefinedmapplot(0);
    });
    $("#p2").click(function(){
        $("#pre_map_panel").slideUp("fast");
        document.getElementById("pre_map").innerHTML = "Past Year";
        pre_slide = false;
        predefinedmapplot(1);
    });
    $("#p3").click(function(){
        $("#pre_map_panel").slideUp("fast");
        document.getElementById("pre_map").innerHTML = "This Year";
        pre_slide = false;
        predefinedmapplot(2);
    });
    $("#p4").click(function(){
        $("#pre_map_panel").slideUp("fast");
        document.getElementById("pre_map").innerHTML = "Past 2 Years";
        pre_slide = false;
        predefinedmapplot(3);
    });



    var c_slide = false;
    form.crime_type.value = "crime";
    $("#crime_select").click(function(){
        if(c_slide == false) {
            $("#crime_select_panel").slideDown("fast");
            c_slide = true;
        }
        else {
            $("#crime_select_panel").slideUp("fast");
            c_slide = false;
        }
    });

    $("#c1").click(function(){
        form.crime_type.value = "crime";
        $("#crime_select_panel").slideUp("fast");
        document.getElementById("crime_select").innerHTML = "Crime";
        c_slide = false;
    });

    $("#c2").click(function(){
        form.crime_type.value = "arrest";
        $("#crime_select_panel").slideUp("fast");
        document.getElementById("crime_select").innerHTML = "Arrest";
        c_slide = false;
    });



    var h_slide = false;
    $("#map_select_id").click(function(){
        if(h_slide == false) {
            $("#map_select_panel").slideDown("fast");
            h_slide = true;
        }
        else {
            $("#map_select_panel").slideUp("fast");
            h_slide = false;
        }
    });

    $("#h1").click(function(){
        hm = false;
        $("#map_select_panel").slideUp("fast");
        document.getElementById("map_select_id").innerHTML = "Location";
        h_slide = false;
    });

    $("#h2").click(function(){
        hm = true;
        $("#map_select_panel").slideUp("fast");
        document.getElementById("map_select_id").innerHTML = "HeatMap";
        h_slide = false;
    });


});

var sth = ["12", "13", "14", "15", 'a', 'b', 'c'];
var beats_list = new Array();

window.onload = function() {
    checkToken();
    getBeat();
    var options = {
        data: [],
        list: {
            match: {
                enabled: true
            }
        }
    };
    options.data = beats_list;
    $("#searchAny").easyAutocomplete(options);
    console.log(document.cookie);
}

function checkToken() {
    if (document.cookie.length != 0) {
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
                        window.location.href = "login.html";
                    }
                    /*else {

                    }*/
                }
            });
        }
    }
}


function getBeat() {
    var arrest = { "type": "arrest" };
    var crime = { "type": "crime" };

    $.ajax({
        url: service + "getBeats",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(arrest),
        dataType: 'json',
        success: function(data) {
            if (!(data.includes("error"))) {
                $.each(data, function(i) {
                    beats_list.push(data[i] + "(Arrest)");
                });
            }
        }
    });

    $.ajax({
        url: service + "getBeats",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(crime),
        dataType: 'json',
        success: function(data) {
            if (!(data.includes("error"))) {
                $.each(data, function(i) {
                    beats_list.push(data[i] + "(Crime)");
                });
            }
        }
    });
}



function submitForm() {
    exportButton.disabled = false;
    resetGlobalVars();

    var form = document.getElementById("form").elements;
    var days = Number(form.days.value);

    console.log(form.longitude.value);

   /* if( form.longitude.value != "longitude" && form.latitude.value != "latitude" 
        && form.radius.value != "radius" ) {
        console.log("in");
        submitRadiusAndCenter();
        return;
    } */

    //check range input
    if (Number(days) < 0 || Number(days) > 30 || isNaN(Number(days))) {
        alert("ERROR: Invalid Date Range.");
        return;
    }

    //check date input
    console.log(form.date_start.value);
    var date = editDate(form.date_start.value, form.date_end.value);

    if (date == 0) {
        alert("ERROR: Invalid Date Range.");
        return;
    }
    if (date[0].includes("undefined") || date[1].includes("undefined")) {
        date[0] = "";
        date[1] = "";
    }

    /*Validate the input for the beat search value*/
    var searchbar_value = "";
    if (!(beats_list.includes(form.searchbar.value)) && form.searchbar.value != "") {
        alert("Invalid Beat.");
        return;
    }

    if (form.searchbar.value !== undefined && form.searchbar.value != null && form.searchbar.value != "") {
        var searchbar_value = (form.searchbar.value).split("(");
        var type = searchbar_value[1].split(")");

        if (type[0].includes("Arrest")) {
            form.crime_type.value = "arrest";
        } else {
            form.crime_type.value = "crime";
        }
    }

    console.log(form.crime_type.value);


    var map_info = {
        'date_start': date[0],
        'date_end': date[1],
        'date_range': days,
        'type': form.crime_type.value,
        'beat': searchbar_value[0]
    };

    var request2 = map_info["type"];
    var points = {};
    $.ajax({
        url: service + "RequestPoints",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(map_info),
        dataType: 'json',
        success: function(data) {
            console.log("request points succeed");

            $.each(data["Points"], function(i) {
                var point = data["Points"][i];
                points[i] = { longitude: point["longitude"], latitude: point["latitude"], date: point["date"] };
            });
            points["count"] = data["count"];

            //call your function for map render as a callback function of succeed action, example:  
            console.log(points);
            plotmap(points, request2);
                // get heatmap dropdown selection by id
            if (hm == true) {
                getAllFeatures();
                initilizeHeatMap();
            } else {
                getAllFeatures();
                toggleMarkers();
            }
        },
        error: function(data) {
            if (data.d["error"] !== 'undefined') {
                alert("Error: " + JSON.stringify(data.d["error"]));
            } else {
                alert("Unhandled Error: " + JSON.stringify(data));
            }
        }
    });

    //Clear all previous result;
    //document.getElementById("form").reset(); // this causes a bug, do not uncomment

    resetRequest();
}
// function to plot by zoom
function plotByZoom(long3, lat3, radius3) {
    resetGlobalVars(); // reset for new map plotting

    /*var form = document.getElementById("form").elements;
    var days = Number(form.days.value);

    //check range input
    if (Number(days) < 0 || Number(days) > 30 || isNaN(Number(days))) {
        alert("ERROR: Invalid Date Range.");
        return;
    }

    //check date input
    console.log(form.date_start.value);
    var date = editDate(form.date_start.value, form.date_end.value);

    if (date == 0) {
        alert("ERROR: Invalid Date Range.");
        return;
    }
    if (date[0].includes("undefined") || date[1].includes("undefined")) {
        date[0] = "";
        date[1] = "";
    }
    if (date == 0) {
        alert("ERROR: Invalid Date Range.");
        return;
    }
    if (date[0].includes("undefined") || date[1].includes("undefined")) {
        date[0] = "";
        date[1] = "";
    }
*/
    /*Validate the input for the beat search value*/
    /*var searchbar_value = "";
    if (!(beats_list.includes(form.searchbar.value)) && form.searchbar.value != "") {
        alert("Invalid Beat.");
        return;
    }

    if (form.searchbar.value !== undefined && form.searchbar.value != null && form.searchbar.value != "") {
        var searchbar_value = (form.searchbar.value).split("(");
        var type = searchbar_value[1].split(")");

        if (type[0].includes("Arrest")) {
            form.crime_type.value = "arrest";
        } else {
            form.crime_type.value = "crime";
        }
    }    */
    var map_info = {
        'date_start': "",
        'date_end': "",
        'date_range': "",
        'type': form.crime_type.value,
        'beat': "",
        'latitude': lat3,
        'longitude': long3,
        'radius': radius3
    };
    var request2 = map_info["type"];
    var points = {};
    $.ajax({
        url: service + "RequestPointsByRadius",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(map_info),
        dataType: 'json',
        success: function(data) {
            console.log("request points succeed");

            $.each(data["Points"], function(i) {
                var point = data["Points"][i];
                points[i] = { longitude: point["longitude"], latitude: point["latitude"], date: point["date"] };
            });
            points["count"] = data["count"];

            //call your function for map render as a callback function of succeed action, example:  
            console.log(points);
            plotmap(points, request2);
                // get heatmap dropdown selection by id
            if (hm == true) {
                getAllFeatures();
                initilizeHeatMap();
            } else {
                getAllFeatures();
            }
        },
        error: function(data) {
            if (data.d["error"] !== 'undefined') {
                alert("Error: " + JSON.stringify(data.d["error"]));
            } else {
                alert("Unhandled Error: " + JSON.stringify(data));
            }
        }
    });

    //Clear all previous result;
    //document.getElementById("form").reset(); // this causes a bug, do not uncomment

    resetRequest();
}


function submitRadiusAndCenter() {
    var longitude = document.getElementsByName("longitude")[0].value;
    var latitude = document.getElementsByName("latitude")[0].value;
    var radius = document.getElementsByName("radius")[0].value;

    if(longitude == "longitude" && latitude == "latitude"){
        alert("Invalid Action: Please select a valid center from the map");
        return;
    }else if(radius == "radius"){
        alert("Invalid Action: Please input a valid radius");
        return;
    }

    resetGlobalVars(); // reset for new map plotting

    var form = document.getElementById("form").elements;
    var days = Number(form.days.value);

    //check range input
    if (Number(days) < 0 || Number(days) > 30 || isNaN(Number(days))) {
        alert("ERROR: Invalid Date Range.");
        return;
    }

    //check date input
    console.log(form.date_start.value);
    var date = editDate(form.date_start.value, form.date_end.value);

    if (date == 0) {
        alert("ERROR: Invalid Date Range.");
        return;
    }
    if (date[0].includes("undefined") || date[1].includes("undefined")) {
        date[0] = "";
        date[1] = "";
    }

    /*Validate the input for the beat search value*/
    var searchbar_value = "";
    if (!(beats_list.includes(form.searchbar.value)) && form.searchbar.value != "") {
        alert("Invalid Beat.");
        return;
    }

    if (form.searchbar.value !== undefined && form.searchbar.value != null && form.searchbar.value != "") {
        var searchbar_value = (form.searchbar.value).split("(");
        var type = searchbar_value[1].split(")");

        if (type[0].includes("Arrest")) {
            form.crime_type.value = "arrest";
        } else {
            form.crime_type.value = "crime";
        }
    }
    var map_info = {
        'date_start': date[0],
        'date_end': date[1],
        'date_range': days,
        'type': form.crime_type.value,
        'beat': searchbar_value[0],
        'latitude': latitude,
        'longitude': longitude,
        'radius': radius
    };

var request2 = map_info["type"];
    var points = {};
    $.ajax({
        url: service + "RequestPointsByRadius",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(map_info),
        dataType: 'json',
        success: function(data) {
            console.log("request points succeed");

            $.each(data["Points"], function(i) {
                var point = data["Points"][i];
                points[i] = { longitude: point["longitude"], latitude: point["latitude"], date: point["date"] };
            });
            points["count"] = data["count"];

            //call your function for map render as a callback function of succeed action, example:  
            console.log(points);
            plotmap(points, request2);
                // get heatmap dropdown selection by id
            if (hm == true) {
                getAllFeatures();
                initilizeHeatMap();
            } else {
                getAllFeatures();
            }
        },
        error: function(data) {
            if (data.d["error"] !== 'undefined') {
                alert("Error: " + JSON.stringify(data.d["error"]));
            } else {
                alert("Unhandled Error: " + JSON.stringify(data));
            }
        }
    });

    //Clear all previous result;
    //document.getElementById("form").reset(); // this causes a bug, do not uncomment

    resetRequest();
}

/*
//example function of retreving points from ajax call
function plotmap(points, type) {
    //loop through whole list  
    console.log(type);
    console.log(map.getLayers().items(2));
    console.log(map.getLayers().items(3));

    for (var i = 0; i < points.count; i++) {
        console.log(points[i]);
        //date is in a format of YYYY-MM-DD, string editing might needed
        if (type == "crime") {
            addMarker(parseFloat(points[i].longitude), parseFloat(points[i].latitude), new Date(points[i].date));
        } else if (type == "arrest") {
            addMarker2(parseFloat(points[i].longitude), parseFloat(points[i].latitude), new Date(points[i].date));
        }
    }
}

// function to add crime marker
function addMarker(lon, lat, time) {
    //create a point

    var geom = new ol.geom.Point(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857'));
    var feature = new ol.Feature({
        geometry: geom,
        longitude: lon,
        latitude: lat,
        tim: time
    });
    feature.setStyle([
        new ol.style.Style({
            image: new ol.style.Icon(({
                anchor: [0.5, 1],
                anchorXUnits: 'fraction',
                anchorYUnits: 'fraction',
                opacity: 1,
                scale: 0.05,
                src: '../img/security.png'

            }))
        })
    ]);
    map.getLayers().item(2).getSource().addFeature(feature);
    map.getLayers().item(2).getSource().changed();
}

// function to add arrests markers
function addMarker2(lon, lat, time) {
    //create a point
    console.log(lon);
    console.log(lat);
    console.log(time);
    var geom = new ol.geom.Point(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857'));
    var feature = new ol.Feature({
        geometry: geom,
        longitude: lon,
        latitude: lat,
        tim: time
    });
    feature.setStyle([
        new ol.style.Style({
            image: new ol.style.Icon(({
                anchor: [0.5, 1],
                anchorXUnits: 'fraction',
                anchorYUnits: 'fraction',
                opacity: 1,
                scale: 0.1,
                src: '../img/arrests.png'

            }))
        })
    ]);
    map.getLayers().item(2).getSource().addFeature(feature);
}*/

function logout() {
    document.getElementById("form").reset();
    resetRequest();
    window.location.href = "login.html";
}

function editDate(start, end) {
    var s_temp = start.split("/");
    var e_temp = end.split("/");

    if (s_temp[2] > e_temp[2]) {
        return 0;
    } else {
        if (s_temp[0] > e_temp[0]) {
            return 0;
        } else {
            if (s_temp[1] > e_temp[1]) {
                return 0;
            }
        }
    }

    var s = "2014-01-".concat(s_temp[1]);
    var e = "2014-01-".concat(e_temp[1]);
    var date = new Array();
    date[0] = s;
    date[1] = e;
    return date;
}

function resetRequest() {
    var map_info = {
        'date_start': "",
        'date_end': "",
        'date_range': "",
        'type': "",
        'beat': ""
    };
}

function inputMapID() {
    var id_sec = document.getElementById("input_id_sec");
    id_sec.innerHTML = "";
    var html = '';
    html =  '<input id="input_id_id" type="text" value=""></input>'+
            '<button class="input_id_submit" onclick="plotMapFromSectionID()">submit</button>';
    $('#input_id_sec').append(html);
}

var showP = false;
function showPolygon() {
    if(!showP) {
        document.getElementById("polygon_sec").style.display = "block";
        showP = true;
    }
    else { 
        document.getElementById("polygon_sec").style.display = "none";
        showP = false;
    }
}

var showM = false;
function showMapInput() {
    if(!showM) {
        document.getElementById("map_input_sec").style.display = "block";
        showM = true;
    }
    else { 
        document.getElementById("map_input_sec").style.display = "none";
        showM = false;
    }
}


function showPolygonOption() {
    console.log("sth");
    var type = document.getElementById("sel_type").value;
    var area = document.getElementById("sel_area");
    var dist = document.getElementById("sel_dist");
    var sect = document.getElementById("sel_sec");
    var beat = document.getElementById("sel_beat");
    console.log(type);
    if (type === "area") {
        area.style.background = "white";
        area.disabled = false;
        dist.style.background = "#989494";
        dist.disabled = true;
        sect.style.background = "#989494";
        sect.disabled = true;
        beat.style.background = "#989494";
        beat.disabled = true;
        getAreaNumber("getCpdAreas");
    } else if (type === "district") {
        dist.style.background = "white";
        dist.disabled = false;
        area.style.background = "white";
        area.disabled = false;
        sect.style.background = "#989494";
        sect.disabled = true;
        beat.style.background = "#989494";
        beat.disabled = true;
        getAreaNumber("getCpdAreas");
        getDistNumber("getCpdDistricts");
    } else if (type === "sector") {
        area.style.background = "white";
        area.disabled = false;
        dist.style.background = "white";
        dist.disabled = false;
        sect.style.background = "white";
        sect.disabled = false;
        beat.style.background = "#989494";
        beat.disabled = true;
        getAreaNumber("getCpdAreas");
        getDistNumber("getCpdDistricts");
        getSecNumber("getCpdSectors");
    } else if (type === "beat") {
        area.style.background = "white";
        area.disabled = false;
        beat.style.background = "white";
        beat.disabled = false;
        dist.style.background = "white";
        dist.disabled = false;
        sect.style.background = "white";
        sect.disabled = false;
        getAreaNumber("getCpdAreas");
        getDistNumber("getCpdDistricts");
        getSecNumber("getCpdSectors");
        getBeatNumber("getCpdBeats");
    }
}

function showAreaNumber(num) {
    console.log(num);
    var html = '';
    for (i = 0; i < num.length; i++)
        html += '<option value="'+num[i]+'">'+num[i]+'</option>';
    $('#sel_area').append(html);
}
function showDistNumber(num) {
    console.log(num);
    var html = '';
    for (i = 0; i < num.length; i++)
        html += '<option value="'+num[i]+'">'+num[i]+'</option>';
    $('#sel_dist').append(html);
}
function showSecNumber(num) {
    console.log(num);
    var html = '';
    for (i = 0; i < num.length; i++)
        html += '<option value="'+num[i]+'">'+num[i]+'</option>';
    $('#sel_sec').append(html);
}
function showBeatNumber(num) {
    console.log(num);
    var html = '';
    for (i = 0; i < num.length; i++)
        html += '<option value="'+num[i]+'">'+num[i]+'</option>';
    $('#sel_beat').append(html);
}

function getAreaNumber(m) {
    $.ajax({
        url: service + m,
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: "",
        dataType: 'json',
        success: function(data) {
            if (!(data.includes("error"))) {
                //console.log(data);
                showAreaNumber(data);
            } else {
                console.log("error");
                console.log(data);
            }
        }
    });
}
function getDistNumber(m, opt) {
    var poly = document.getElementById("polygon_sec");
    $.ajax({
        url: service + m,
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: "",
        dataType: 'json',
        success: function(data) {
            if (!(data.includes("error"))) {
                showDistNumber(data);
            } else {
                console.log("error");
                console.log(data);
            }
        }
    });
}
function getSecNumber(m) {
    $.ajax({
        url: service + m,
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: "",
        dataType: 'json',
        success: function(data) {
            if (!(data.includes("error"))) {
                showSecNumber(data);
            } else {
                console.log("error");
                console.log(data);
            }
        }
    });
}
function getBeatNumber(m) {
    $.ajax({
        url: service + m,
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: "",
        dataType: 'json',
        success: function(data) {
            if (!(data.includes("error"))) {
                showBeatNumber(data);
            } else {
                console.log("error");
                console.log(data);
            }
        }
    });
}
    ifIsPoly = 0; // area
    ifIsDistrict = 0; // district 
    ifIsBeat = 0;
    ifIsSector = 0;
    var poly;

function drawPolygon() {
    resetGlobalVars();
    document.getElementById("polygon_sec").style.display = "none";

    var type = document.getElementById("sel_type").value;
    var area = document.getElementById("sel_area").value;
    var dist = document.getElementById("sel_dist").value;
    var sect = document.getElementById("sel_sec").value;
    var beat = document.getElementById("sel_beat").value;
    var ac_type = document.getElementById("form").elements.crime_type.value;

    var polygon = {"option":"","area":"","district":"","sector":"","type":"","beat":""};
    polygon["option"] = type;
    polygon["area"] = area;
    polygon["district"] = dist;
    polygon["sector"] = sect;
    polygon["type"] = ac_type;
    polygon["beat"] = beat;
    console.log(polygon);
    
    //
    var request2 = polygon["type"];
    var points = {};
    $.ajax({
        url: service + "GetPointsForPolygon",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(polygon),
        dataType: 'json',
        success: function(data) {
            $.each(data["Points"], function(i) {
                var point = data["Points"][i];
                points[i] = { longitude: point["longitude"], latitude: point["latitude"], date: point["date"] };
            });
            points["count"] = data["count"];
            //call your function for map render as a callback function of succeed action, example:
            plotmap(points, request2);
            console.log(points);
            getAllFeatures();
        },
        error: function(data) {
            if (data.d["error"] !== 'undefined') {
                alert("Error: " + JSON.stringify(data.d["error"]));
            } else {
                alert("Unhandled Error: " + JSON.stringify(data));
            }
        }
    });
    console.log(points);
    //
    if (polygon["option"] == "area") {
        areaSource.forEachFeature(function(feature) {
            if (feature.get('AREA_NUM') == polygon["area"]) {
                //console.log("It should go in");
                var tempSource = new ol.source.Vector({
                    features: feature
                });
                console.log(feature);
                poly = feature.get('geometry');
                console.log(poly);
                console.log(poly.getExtent());
                poly = poly.getExtent();
                //var polytest = feature[0].get('geometry');
                //console.log(polytest);
                ifIsPoly = 1;
                tempSource.addFeature(feature);
                var areas = new ol.layer.Vector({
                    source: tempSource,
                    style: new ol.style.Style({
                        stroke: new ol.style.Stroke({
                            color: '#319FD3',
                            width: 3
                        })
                    })
                })
                a = areas;
                 //areas.setZIndex(2);
                map.getLayers().insertAt(2, areas);
                areas.setVisible(true);
               
                //areas.setActive(false);
            }
    
        });
    } else if (polygon["option"] == "district") {
        districtSource.forEachFeature(function(feature){
           if (feature.get('DISTRICT') == polygon["district"]) {
                var tempSource = new ol.source.Vector({
                    features: feature
                });
                ifIsDistrict = 1;
                tempSource.addFeature(feature);
                var districts = new ol.layer.Vector({
                    source: tempSource,
                    style: new ol.style.Style({
                        stroke: new ol.style.Stroke({
                            color: '#FFA500',
                            width: 3
                        })
                    })
                })
                d = districts;
                //map.addLayer(districts);
                map.getLayers().insertAt(3, districts);
                districts.setVisible(true);
           }
        });
                areaSource.forEachFeature(function(feature) {
            if (feature.get('AREA_NUM') == polygon["area"]) {
                //console.log("It should go in");
                var tempSource = new ol.source.Vector({
                    features: feature
                });
                console.log(feature);
                poly = feature.get('geometry');
                console.log(poly);
                console.log(poly.getExtent());
                poly = poly.getExtent();
                //var polytest = feature[0].get('geometry');
                //console.log(polytest);
                ifIsPoly = 1;
                tempSource.addFeature(feature);
                var areas = new ol.layer.Vector({
                    source: tempSource,
                    style: new ol.style.Style({
                        stroke: new ol.style.Stroke({
                            color: '#319FD3',
                            width: 7
                        })
                    })
                })
                a = areas;
                 //areas.setZIndex(2);
                map.getLayers().insertAt(3, areas);
                areas.setVisible(true);
               
                //areas.setActive(false);
            }
    
        });
    } else if (polygon["option"] == "sector") { // sector id and district name
        sectorSource.forEachFeature(function(feature){
            console.log(feature.get('SECTOR'));
           if (feature.get('SECTOR') == polygon["sector"]) {
                var tempSource = new ol.source.Vector({
                    features: feature
                });
                ifIsSector = 1;
                tempSource.addFeature(feature);
                var sectors = new ol.layer.Vector({
                    source: tempSource,
                    style: new ol.style.Style({
                        stroke: new ol.style.Stroke({
                            color: '#FF0000',
                            width: 3
                        })
                    })
                })
                s = sectors;
                //map.addLayer(sectors);
                map.getLayers().insertAt(2, sectors);
                sectors.setVisible(true);
           }
        });
    } else if (polygon["option"] == "beat") { // beat, district and sector
        beatsSource.forEachFeature(function(feature){
           if (feature.get('BEAT') == polygon["beat"]) {
                var tempSource = new ol.source.Vector({
                    features: feature
                });
                ifIsBeat = 1;
                tempSource.addFeature(feature);
                var beats = new ol.layer.Vector({
                    source: tempSource,
                    style: new ol.style.Style({
                        stroke: new ol.style.Stroke({
                            color: '#00FF00',
                            width: 3
                        })
                    })
                })
                b = beats;
                map.addLayer(beats);
                beats.setVisible(true);
           }
        });
    }
}
