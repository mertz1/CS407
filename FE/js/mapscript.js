//link for service address
var service = "http://civilmaptestservice.cloudapp.net/CivilMapService.svc/";

//sample request object
var request = {
    'type': "",
    'data_range': "",
    'data_start': "",
    'date_end': "",
    'beat': ""
};

function getAllCrime() {
    request["type"] = "crime";
    RequestPoints(request);
}

function getAllArrests() {
    request["type"] = "arrest";
    RequestPoints(request);
}

// Request points
function RequestPoints(request) {
    var request2 = request["type"];
    var points = {};
    $.ajax({
        url: service + "RequestPoints",
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(request),
        dataType: 'json',
        success: function(data) {
            $.each(data["Points"], function(i) {
                var point = data["Points"][i];
                points[i] = { longitude: point["longitude"], latitude: point["latitude"], date: point["date"] };
            });
            points["count"] = data["count"];
            //call your function for map render as a callback function of succeed action, example:
            plotmap(points, request2);
        },
        error: function(data) {
            if (data.d["error"] !== 'undefined') {
                alert("Error: " + JSON.stringify(data.d["error"]));
            } else {
                alert("Unhandled Error: " + JSON.stringify(data));
            }
        }
    });
    //reset 'request' object after each request
    request = { type: "", data_range: "", data_start: "", date_end: "", beat: "" };

    //if return model is needed
    return points;
}

//example function of retreving points from ajax call
function plotmap(points, type) {
    //loop through whole list  
    map.getLayers().item(2).getSource().clear();
    for (var i = 0; i < points.count; i++) {
        //date is in a format of YYYY-MM-DD, string editing might needed
        if (type == "crime") {
            addMarker(parseFloat(points[i].longitude), parseFloat(points[i].latitude), new Date(points[i].date));
        } else if (type == "arrest") {
            addMarker2(parseFloat(points[i].longitude), parseFloat(points[i].latitude), new Date(points[i].date));
        }
    }
}

// global variable
var featurecount = 0;
var features = [];

// add all features to source
function getAllFeatures() {
    source.addFeatures(features);
}

// clusters design
var clustersFill = new ol.style.Fill({
    color: 'rgba(255, 153, 0, 0.8)'
});

var clusterStroke = new ol.style.Stroke({
    color: 'rgba(255, 204, 0, 0.2)',
    width: 1
});

var textFill = new ol.style.Fill({
    color: '#fff'
});

var textStroke = new ol.style.Stroke({
    color: 'rgba(0,0,0, 0.6)',
    width: 3
});

var invisibleFill = new ol.style.Fill({
    color: 'rgba(255, 255, 255, 0.01)'
});

function createClusterStyle(feature) {
    return new ol.style.Style({
        geometry: feature.getGeometry(),
        image: new ol.style.RegularShape({
            radius1: 20,
            radius2: 3, 
            points: 5,
            angle: Math.PI,
            fill: clustersFill,
            stroke: clusterStroke
        })
    });
}

// calculate cluster information
var maxFeatureCount;
var clusters;
function calculateClusterInfo(resolution) {
    maxFeatureCount = 0;
    var features = clusters.getSource().getFeatures();
    var feature, radius;
    for (var i = features.length - 1; i >= 0; --i) {
        feature = features[i];
        var originalFeatures = feature.get('features');
        var extent = ol.extent.createEmpty();
        var j, jj;
        for (j = 0, jj = originalFeatures.length; j < jj; ++j) {
            ol.extent.extend(extent, originalFeatures[j].getGeometry().getExtent());
        }
        maxFeatureCount = Math.max(maxFeatureCount, jj);
        radius = 0.25 * (ol.extent.getWidth(extent) + ol.extent.getHeight(extent))/resolution;
        feature.set('radius', radius);
    }
}

// function stylefunction to be used in cluster source creation
var currentResolution;
function styleFunction(feature, resolution) {
    if (resolution != currentResolution) {
        calculateClusterInfo(resolution);
        currentResolution = resolution;
    }
    var style;
    var size = feature.get('features').length;
    if (size > 1) {
        style = new ol.style.Style({
            image: new ol.style.Circle({
                radius: feature.get('radius'),
                fill: new ol.style.Fill({
                    color: [255, 153, 0, Math.min(0.8, 0.4 + (size/maxFeatureCount))]
                })
            }),
            text: new ol.style.Text({
                text: size.toString(),
                fill: textFill,
                stroke: textStroke
            })
        }); 
    } else {
        var originalFeature = feature.get('features')[0];
        style = createClusterStyle(originalFeature);
    }
    return style;
}

// features source
var source = new ol.source.Vector({
    features: features
})

// clusters layer
var clusterSource = new ol.source.Cluster({
    distance: 10,
    source: source
});

var styleCache = {};
clusters = new ol.layer.Vector({
    source: clusterSource,
    style: styleFunction
});

// heat map layer
var heatmapSource = new ol.layer.Heatmap({
    source: clusterSource
});

// add heat map layer to map
function initilizeHeatMap() {
    map.addLayer(heatmapSource);
    heatmapSource.getSource().on('addfeature', function(event) {
        event.feature.set('weight', event.feature.length);
    });
}

// defines a namespace for the application
window.app = {};
var app = window.app;
var button2;
// defines custom toolbar control
app.CustomToolbar = function(opt_options) {
    var options = opt_options || {};
    // create two buttons for satellite and clusters
    var button = document.createElement('button');
    var button1 = document.createElement('button');
    button2 = document.createElement('button');
    // satellite button icon and cluster button icon
    button.innerHTML = '<img src="../img/WechatIMG6.jpeg" width="18" height="18"/>';
    button1.innerHTML = 'C';
    button2.innerHTML = 'H';

    var this_ = this;
    var clicked = 1; // 0 is viewing osm, 1 is viewing satellite
    var handleSatellite = function(e) {
        if (clicked == 0) { // enable satellite view
            // the reason why i used osm here is because in my map,
            // osm is rendered on top of sat
            osm.setVisible(false);
            button.innerHTML = '<img src="../img/WechatIMG6.jpeg" width="18" height="18"/>';
            clicked = 1;
        } else if (clicked == 1) { // disable satellite view 
            osm.setVisible(true);
            button.innerHTML = '<img src="../img/WechatIMG7.jpeg" width="18" height="18"/>';
            clicked = 0;
        }
    };
    var canyouseecluster = 0;
    var clustersView = function(e) {
        if (canyouseecluster == 0) {
            getAllFeatures();
            clusters.setVisible(true);
            markers.setVisible(false);
            canyouseecluster = 1;
        } else if (canyouseecluster == 1) {
            clusters.setVisible(false);
            markers.setVisible(true);
            canyouseecluster = 0;
        }
    };

    var canyouseeheatmap = 0;
    var heatMapView = function(e) {
        if (canyouseeheatmap == 0) {
            canyouseeheatmap = 1;
            initilizeHeatMap();
        } else if (canyouseeheatmap == 1) {
            canyouseeheatmap = 0;
        }
    };
    // adding button listeners
    button.addEventListener('click', handleSatellite, false);
    button.addEventListener('touchstart', handleSatellite, false);
    button1.addEventListener('click', clustersView, false);
    button1.addEventListener('touchstart', clustersView, false);
    button2.addEventListener('click', heatMapView, false);
    button2.addEventListener('touchstart', heatMapView, false);

    var element = document.createElement('div');
    var element2 = document.createElement('div');
    element2.id = "testdiv2";
    element.appendChild(element2);
    element.id = "testdiv";
    element.className = 'ol-unselectable ol-mycontrol';
    element.appendChild(button);
    element2.appendChild(button1);
    //element.appendChild(button1);
    element.appendChild(button2);

    // button painted over the map
    ol.control.Control.call(this, {
        element: element,
        target: options.target
    });
};
ol.inherits(app.CustomToolbar, ol.control.Control);

// satellite view using bing maps
var sat = new ol.layer.Tile({
    preload: Infinity,
    source: new ol.source.BingMaps({
        key: 'jfsevjVLbtE50g3oQzNo~SMWh4wsoOO2_zVKzppeEBA~ApzK3CaB4KNrn2tXuMpdkqBmC9KEMxGmmkBkT8ACfHYudQZe33AuU3zJty0HC8Vv',
        imagerySet: 'AerialWithLabels'
    })
});

// open source map view
var osm = new ol.layer.Tile({
    source: new ol.source.OSM()
});

// marker layer
var markers = new ol.layer.Vector({
    source: new ol.source.Vector({ features: [], projection: 'EPSG:4326' })
});

// select style function
function selectStyleFunction(feature) {
    var styles = [new ol.style.Style({
        image: new ol.style.Circle({
            radius: feature.get('radius'),
            fill: invisibleFill
        })
    })];
    var originalFeatures = feature.get('features');
    var originalFeature;
    for (var i = originalFeatures.length - 1; i >= 0; --i) {
        originalFeature = originalFeatures[i];
        styles.push(createClusterStyle(originalFeature));
    }
    return styles;
}

// creates the map
var map = new ol.Map({
    controls: ol.control.defaults({
        attributionOptions: /** @type {olx.control.AttributionOptions} */ ({
            collapsible: false
        })
    }).extend([
        new app.CustomToolbar()
    ]),
    target: 'map',
    layers: [sat, osm, markers, clusters],
    interactions: new ol.interaction.defaults().extend([new ol.interaction.Select({
        condition: function(evt) {
            return evt.type == 'pointermove' ||
                evt.type == 'singleclick';
        },
        style: selectStyleFunction
    })]),
    view: new ol.View({
        center: ol.proj.fromLonLat([-87.65, 41.88]), // centered in downtown chicago
        zoom: 14,
        maxZoom: 20,
        minZoon: 3
    })
});

// detect change of resolution
map.getView().on('change:resolution', changeProjection);
function changeProjection() {
    var zoomLevel = map.getView().getZoom();
    var x = document.getElementById("testdiv2");
    // see if zoom level is less than 12
    if (zoomLevel >= 17) { // when you can see building numbers
        clusters.setVisible(false);
        markers.setVisible(true);
        x.style.display = "none";
        getCurrentCenterAndRadius();

    } else if (zoomLevel > 12 && zoomLevel < 17 ) {
        // you can choose to see it in clusters
        // activate and deactivate clusters button
        //button2.setVisible(true);
        x.style.display = "block";
        getCurrentCenterAndRadius();
    } else if (zoomLevel <= 12) { // too zoomed out
        // just look at it in clusters
        clusters.setVisible(true);
        markers.setVisible(false);
        x.style.display = "none";
        getCurrentCenterAndRadius();
    }
}

function getCurrentCenterAndRadius() {
    var size = map.getSize();
    var center = map.getView().getCenter();
    var sourceProj = map.getView().getProjection();
    var extent = map.getView().calculateExtent(size);
    extent = ol.proj.transformExtent(extent, sourceProj, 'EPSG:4326');
    var posSW = [extent[0], extent[1]];
    var posNE = [extent[2], extent[3]];
    center = ol.proj.transform(center, sourceProj, 'EPSG:4326');
    console.log(center);
    var wgs84Sphere = new ol.Sphere(6378137);
    var centerToSW = wgs84Sphere.haversineDistance(center, posSW);
    var centerToNE = wgs84Sphere.haversineDistance(center, posNE);
    console.log(centerToNE);
    console.log(centerToSW);
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
    features.push(feature);
    featurecount = featurecount + 1;
    feature.setStyle([
        new ol.style.Style({
            image: new ol.style.Icon(({
                anchor: [0.5, 1],
                anchorXUnits: 'fraction',
                anchorYUnits: 'fraction',
                opacity: 1,
                scale: 0.05,
                src: '../img/crime.png' 

            }))
        })
    ]);
    map.getLayers().item(2).getSource().addFeature(feature);
    map.getLayers().item(2).getSource().changed();
}

// function to add arrests markers
function addMarker2(lon, lat, time) {
    //create a point
    var geom = new ol.geom.Point(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857'));
    var feature = new ol.Feature({
        geometry: geom,
        longitude: lon,
        latitude: lat,
        tim: time
    });
    features.push(feature);
    featurecount = featurecount + 1;
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
    //changeToCluster();
}

// change mouse cursor when over marker
map.on('pointermove', function(e) {
    var hit = this.forEachFeatureAtPixel(e.pixel, function(feature, layer) {
        return true;
    });
    if (hit) {
        this.getTargetElement().style.cursor = 'pointer';
    } else {
        this.getTargetElement().style.cursor = '';
    }
});

// create pop up from icon
var element = document.getElementById('popup');

var popup = new ol.Overlay({
    element: element,
    positioning: 'bottom-center',
    stopEvent: false,
    offset: [0, -50]
});
map.addOverlay(popup);

// display popup on click
map.on('click', function(evt) {

    var feature = map.forEachFeatureAtPixel(evt.pixel, function(feature) {
        return feature;
    });

    if (feature) { // if it is a feature when clicked 
        $(element).popover('show');
        var coordinates = feature.getGeometry().getCoordinates();
        popup.setPosition(coordinates);
        $(element).attr('data-placement', 'middle');
        $(element).attr('data-html', true);
        $(element).attr('data-content', "Longitude:" + feature.get('longitude') + " Latitude:" + feature.get('latitude') + " Time: " + feature.get('tim'));

        $(element).popover('show');
    } else { // if it is not a feature when clicked 
        $(element).popover('destroy');
    }
});



//export pdf
/*var format = new ol.format.WKT();
var feature = format.readFeature(
    'POLYGON((10.689697265625 -25.0927734375, 34.595947265625 ' +
    '-20.1708984375, 38.814697265625 -35.6396484375, 13.502197265625 ' +
    '-39.1552734375, 10.689697265625 -25.0927734375))');
feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
*/
var dims = {
    a0: [1189, 841],
    a1: [841, 594],
    a2: [594, 420],
    a3: [420, 297],
    a4: [297, 210],
    a5: [210, 148]
};

var loading = 0;
var loaded = 0;

var exportButton = document.getElementById('export-pdf');

exportButton.addEventListener('click', function() {
    exportButton.disabled = true;
    document.body.style.cursor = 'progress';

    var format = document.getElementById('format').value;
    var resolution = document.getElementById('resolution').value;
    var dim = dims[format];
    var width = Math.round(dim[0] * resolution / 25.4);
    var height = Math.round(dim[1] * resolution / 25.4);
    var size = /** @type {ol.Size} */ (map.getSize());
    var extent = map.getView().calculateExtent(size);

    var source = osm.getSource();

    var tileLoadStart = function() {
       ++loading;
    };

    var tileLoadEnd = function() {
        ++loaded;
        if (loading === loaded) {
            var canvas = this;
            window.setTimeout(function() {
              loading = 0;
              loaded = 0;
              //canvas.crossOrigin = "Anonymous";
              var data = canvas.toDataURL('image/png');
              var pdf = new jsPDF('landscape', undefined, format);
              pdf.addImage(data, 'JPEG', 0, 0, dim[0], dim[1]);
              pdf.save('map.pdf');
              source.un('tileloadstart', tileLoadStart);
              source.un('tileloadend', tileLoadEnd, canvas);
              source.un('tileloaderror', tileLoadEnd, canvas);
              map.setSize(size);
              map.getView().fit(extent);
              map.renderSync();
              exportButton.disabled = false;
              document.body.style.cursor = 'auto';
            }, 100);
          }
    };

    map.once('postcompose', function(event) {
        source.on('tileloadstart', tileLoadStart);
        source.on('tileloadend', tileLoadEnd, event.context.canvas);
        source.on('tileloaderror', tileLoadEnd, event.context.canvas);
    });

    map.setSize([width, height]);
    map.getView().fit(extent);
    map.renderSync();

}, false);