function displayMap() {
    $('#map').css('display', 'block');
    var zip = $('#enterbox').val();
    $.get('http://maps.googleapis.com/maps/api/geocode/json?address=' + zip, function(data) {
        initialize(data.results[0].geometry.location);
    });
}
function initialize(loc) {
    var mapProp = {
        center: new google.maps.LatLng(loc.lat, loc.lng),
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map"), mapProp);
    new google.maps.Marker({
        position: { lat: loc.lat+(Math.random()-.5), lng: loc.lng+(Math.random()-.5) },
        map: map,
        title: '1'
    });
}
google.maps.event.addDomListener(window, 'load', initialize);