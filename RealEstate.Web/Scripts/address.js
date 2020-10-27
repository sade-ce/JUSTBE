$(function () {
    $(document).ajaxStart(function () {
        $("#ajaxLoading").css("display", "block");
        $("#ajaxLoading").css("top", $(window).height() / 2);
        $("#ajaxLoading").css("left", $(window).width() / 2);
        $("#ajaxLoading").css("position", "fixed");
    });
    $(document).ajaxComplete(function () {
        $("#ajaxLoading").css("display", "none");
    });
})
//**************** JS to show loading progress during ajax call *********************//


var map;
var mapparams = {};
mapparams.type = 'google';
mapparams.key = 'ApXUJeG7YK_DeN_desiDocj4KFZKvQREAWkeURPzuSdjK_srlrdJAp-Np6rC-Eg4';
mapparams.canvas = 'map';
mapparams.description = 'No Detail Available';
var addressParam = {};
addressParam.street = '';
addressParam.address = '';
addressParam.city = '';
addressParam.state = '';
addressParam.zipcode = '';
addressParam.country = '';
addressParam.unit_number = '';
var countryCode = 'US';

var place;
var hasrun = false;
var addyconfirmed = false;
var bounds = new google.maps.LatLngBounds();
var panorama;
var input = document.getElementById('addressme');

function initialize(lat, lng) {
    var center = null;
    if (!lat || !lng) {
        center = new google.maps.LatLng(39.639538, -100.634766);
    } else {
        center = new google.maps.LatLng(lat, lng);
    }
    var mapOptions = {
        center: center,
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    map = new google.maps.Map(document.getElementById('map_address'), mapOptions);
    var fenway = map.getCenter();
    var panoramaOptions = {
        position: fenway,
        pov: {
            heading: 34,
            pitch: 10
        }
    };
    panorama = new google.maps.StreetViewPanorama(document.getElementById("step2_street_view_canvas"), panoramaOptions);
    google.maps.event.addListener(panorama, 'pano_changed', function () {
        jQuery("#step2_panorama_id").val(panorama.pano);
    });
    var showUserLocation = 0;
    var strcountry = 'US';
    /* new code  start*/
    if (showUserLocation == 1) {
        jQuery.ajax({
            type: "GET",
            dataType: "json",
            async: false,
            success: function (response) {
                if (response.status == google.maps.GeocoderStatus.OK) {
                    // console.log("custom");
                    jQuery.each(response.results, function (index, place) {
                        gmapLatlng = new google.maps.LatLng(place.geometry.location.lat, place.geometry.location.lng);
                        bounds.extend(gmapLatlng);
                    });
                } else {
                    // console.log("admin custom");
                    strcountry = 'US';
                    getSiteBounds();
                }
                BindWithMap();
            }
        });
    } else {
        //console.log("else admin custom");
        getSiteBounds();
        BindWithMap();
    }
    function getSiteBounds() {
        gmapLatlng = new google.maps.LatLng(32.735306509649064, -117.20489501953125);
        bounds.extend(gmapLatlng);
        gmapLatlng = new google.maps.LatLng(32.72606457613067, -117.13760375976562);
        bounds.extend(gmapLatlng);
        gmapLatlng = new google.maps.LatLng(32.68330817307213, -117.13005065917969);
        bounds.extend(gmapLatlng);
    }
    function BindWithMap() {
        map.fitBounds(bounds);
        var options = {
            bounds: bounds,
            types: ['geocode'],
            componentRestrictions: { country: strcountry }
        };
        autocomplete = new google.maps.places.Autocomplete(input, options);
        autocomplete.bindTo('bounds', map);

        google.maps.event.addListener(autocomplete, 'place_changed', updatemyinfo);
    }
    /* new c0de end */
    function updatemyinfo() {

        place = autocomplete.getPlace();

        for (var i = 0; i < place.address_components.length; i++) {
            for (var j = 0; j < place.address_components[i].types.length; j++) {
                if (place.address_components[i].types[j] == "postal_code") {
                    document.getElementById('txtCity').value = place.address_components[i].long_name;
                    $("#Error").text("");

                }
            }
        }

        setAddressFromPlace(place);
        panorama.setPosition(place.geometry.location);

        if (panorama.pano) {
            //jQuery("#step2_panorama_id").val(panorama.pano);
            //jQuery('#rotate-1').panoramic({
            //    panoid: panorama.pano,
            //    width: '100%',
            //    height: '100%'
            //});
        }
        if (typeof input !== "undefined") {
            /*input.className = '';
            if (!place.geometry) {
                // Inform the user that the place was not found and return.
                input.className = 'notfound';
                return;
            }
            map.setCenter(place.geometry.location);
            map.setZoom(18);
            google.maps.event.trigger(map, 'resize');
            consloe.log(panorama);
            panorama.setPosition(place.geometry.location);
            jQuery("#step2_panorama_id").val(panorama.pano);
            consloe.log("after...");
            consloe.log(panorama);
            mapCenter = map.getCenter();
            mapparams.latitude 	= mapCenter.lat();
            mapparams.longitude = mapCenter.lng();
            mapparams.icon 		= place.icon;
            hasrun = true;*/
            addyconfirmed = true;
        }
    }

    /* Enter Key Handler*/
    google.maps.event.addDomListener(input, 'keypress', function (e) {
        if (e.keyCode == 13) {
            if (jQuery("#temp_address").val()) { // for initial address loadup ig GPS Setting is On
                if (jQuery("#temp_address").val() === jQuery("#addressme").val())
                    checkValidAddressStep1(e);
                else { //not initial load case for Mouse then Enter Event Handler
                    buildAddress();
                    checkValidAddressStep1(e);
                }
            } else {
                buildAddress();
                checkValidAddressStep1(e);
            }
        }
    });
    /* Enter Key Handler End*/

}
//initialize();

function extractFromAdress(components, type, address) {
    var strNearByTypeValue = '';
    for (var i = 0; i < components.length; i++) {
        for (var j = 0; j < components[i].types.length; j++) {
            if (components[i].types[j] == type) {
                //return getExcetAddress(components[i].short_name,components[i].long_name,address);
                return components[i].short_name;
            } else {
                if (type && components[i].types[j].indexOf(type) > -1) {
                    //strNearByTypeValue=getExcetAddress(components[i].short_name,components[i].long_name,address);
                    strNearByTypeValue = components[i].short_name;
                }
            }
        }
    }
    return strNearByTypeValue;
}
function getExcetAddress(short_name, long_name, address) {
    if (long_name.toLowerCase() && address.toLowerCase().indexOf(long_name.toLowerCase()) > -1) {
        return long_name;
    } else if (short_name.toLowerCase() && address.toLowerCase().indexOf(short_name.toLowerCase()) > -1) {
        return short_name;
    } else {
        return short_name;
    }
}
jQuery(document).ready(function () {

    $("#addressme").blur(function () {
        buildAddress();
        //build address on address change event
    });

    jQuery('#addressme').focus(function () {

        //jQuery('#addressme').validationEngine('hide');
    });



    jQuery('#findhomevalue').submit(function (e) {

        var checkAdd = checkValidAddressStep1(e);
        if (checkAdd == false) {

            //var validateAddress = "no";
            //if (addyconfirmed == false) {
            //    jQuery("#isValidAddress").val("no");
            //    if (validateAddress == 'yes') {
            //        jQuery('#addressme').validationEngine('showPrompt', 'This is not a valid address.', 'fail', 'topRight', 1);
            //    }
            //}
            //Check Valid Unit Address if turned on Start

            return false;
        }
        else {
            objUserAddress = document.getElementById('addressme');
            var zip = extractFromAdress(place.address_components, "postal_code", objUserAddress.value);
            if (zip == '') {
                zip = extractFromAdress(place.address_components, "zip_code", objUserAddress.value);
            }
            if (zip == '') {
                zip = extractFromAdress(place.address_components, "postal", objUserAddress.value);
            }
            if (zip == '') {
                zip = extractFromAdress(place.address_components, "zip", objUserAddress.value);

            }
            $('#zip').val(zip);
            varifyAddress();

            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        var id = result.id;
                        var href = '/home/loadleadform';
                        var fullhref = href + '/' + id;
                        $('#leadContent').load(fullhref, function () {
                            //$('#leadModal').modal({
                            //    /*backdrop: 'static',*/
                            //    keyboard: true
                            //}, 'show');
                            //
                            document.getElementById('leadform').click();
                            //bindForm(this);
                        });
                    }
                }

            });
        }


        //Check Valid Unit Address if turned on End
        //showloader('Please Wait...');
        //if (jQuery('#addressme').val() == '') {
        //    jQuery('#addressme').validationEngine('showPrompt', 'Please enter your address.', 'fail', 'topRight', 1);
        //    hideloader();
        //    return false;
        //}


        //var validateAddress = "no";
        //if (addyconfirmed == false) {
        //    jQuery("#isValidAddress").val("no");
        //    if (validateAddress == 'yes') {
        //        jQuery('#addressme').validationEngine('showPrompt', 'This is not a valid address.', 'fail', 'topRight', 1);
        //    } else {
        //        addressParam.latitude = mapparams.latitude;
        //        addressParam.longitude = mapparams.longitude;
        //        jQuery("#addressParam").val(JSON.stringify(addressParam));
        //        jQuery("#leadMapparams").val(JSON.stringify(mapparams));
        //        showloader('Please wait while we submit your information...');
        //        return true;
        //    }
        //} else {
        //    jQuery("#isValidAddress").val("yes");
        //    addressParam.latitude = mapparams.latitude;
        //    addressParam.longitude = mapparams.longitude;
        //    jQuery("#addressParam").val(JSON.stringify(addressParam));
        //    jQuery("#leadMapparams").val(JSON.stringify(mapparams));
        //    showloader('Please wait while we submit your information...');
        //    return true;
        //}
        //hideloader();
        return false;
    });

    //add the current address to temp for default address check from autocomplete
});




function buildAddress() {
    jQuery("#temp_address").val('');
    var length = $(".pac-item").length;
    if (length > 1) {
        for (i = 1; i <= $(".pac-item").length; i++) {
            var str = $(".pac-container .pac-item:nth-child(" + i + ")").text();
            str = str.replace(/\s*,\s*|\s+,/g, ''); //remove commas
            str = str.replace(/\s+/g, ''); //remove spaces
            if (str)
                jQuery("#temp_address").val(str + ";" + jQuery("#temp_address").val());
        }
    } else {
        var str = $(".pac-container .pac-item:nth-child(1)").text();
        str = str.replace(/\s*,\s*|\s+,/g, ''); //remove commas
        str = str.replace(/\s+/g, ''); //remove spaces
        jQuery("#temp_address").val(str);
    }
    //console.log('Build Address = '+jQuery("#temp_address").val());
}


function checkValidAddressStep1(e) {
    var address = jQuery('#addressme').val();

    address = address.replace(/\s*,\s*|\s+,/g, '');
    address = address.replace(/\s+/g, '');
    var tempAddress = jQuery("#temp_address").val();
    if (tempAddress.indexOf(';') > -1) {
        var tempAddress = jQuery("#temp_address").val().split(";");

    }

    if (tempAddress == '') {
        jQuery('#Error').text("* Please Enter Valid address").show();
        e.preventDefault();
        e.stopPropagation();
        return false;
    }

    if (address && jQuery.isArray(tempAddress)) {
        var addInArray = jQuery.inArray(address, tempAddress);
        if (addInArray !== undefined && addInArray > -1)
            var match = true;
        else
            var match = false;
    } else {
        if (address === tempAddress) {
            var match = true;
        } else
            var match = false;
    }
    /*for(i=0;i<tempAddress.length-1;i++)
    {
        alert(address);
        alert(tempAddress[i]);
        if(address===tempAddress[i]){
            var match = true;
            break;
        }
        var match = false;
    }*/
    if (match === false) {
        //    jQuery('#Error').validationEngine('showPrompt', 'select an address from the dropdown.', 'fail', 'topRight', 1);
        jQuery('#Error').text("* Please select address").show();

        e.preventDefault();
        e.stopPropagation();
        return false;

    }
    if (match === true) {
        return true;
        //showloader('Please wait while we submit your information...');

    }
}

function getUnitNumber(addy, arrUN) {
    if (arrUN) {
        for (var u in arrUN) {
            addressParam.unit_number = arrUN[u].replace(/[^0-9a-zA-Z]/ig, '');
            if (addressParam.unit_number) {
                addy = addy.replace(arrUN[u], '');
                return addy;
            }
        }
    }
    return addy;
}
function getUnitNumber2(addy, arrUN) {
    if (arrUN) {
        for (var u in arrUN) {
            addressParam.unit_number = arrUN[u].replace(/((Unit|apt|apartment|condo|condotorium)[\-:#\s\\/=]*)/ig, '');
            if (addressParam.unit_number) {
                addy = addy.replace(arrUN[u], '');
                return addy;
            }
        }
    }
    return addy;
}
function addCountryCode(addy) {

    if (typeof countryCode != "undefined" && countryCode != '' && addy.indexOf(countryCode) == -1) {
        return addy + ', ' + countryCode;
    }

    return addy;
}
function varifyAddress() {
    var addy = jQuery('#addressme').val();
    //Code For Unit Number START
    addressParam.unit_number = '';
    var arrUN = addy.match(/((Unit|apt|apartment|condo|condotorium)?\.*[\s]*#[\s]*[0-9a-zA-Z]*)/ig);
    addy = getUnitNumber(addy, arrUN);
    if (addressParam.unit_number == '') {
        //arrUN = addy.match(/((Unit|apt|apartment|condo|condotorium)\.*[\s]*#?[\s]*[0-9a-zA-Z]*)/ig);
        arrUN = addy.match(/((Unit|apt|apartment|condo|condotorium)[\-:#\s\\/=]*[\d]+[0-9a-zA-Z]*)/ig);
        addy = getUnitNumber2(addy, arrUN);
    }
    addyconfirmed = true;

    addy = addCountryCode(addy);

    //Code For Unit Number END
    if (hasrun == false) {
        var goodaddress;
        var typeofaddress;
        var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + addy + "&key=AIzaSyDqvBo6UykHTd0rWOokQTStUjBRzy_0Rr4";
        jQuery.ajax({
            type: "GET",
            url: url,
            dataType: "json",
            async: false,
            success: function (response) {
                if (response.status == google.maps.GeocoderStatus.OK) {
                    jQuery.each(response.results, function (index, place) {
                        mapparams.latitude = place.geometry.location.lat;
                        mapparams.longitude = place.geometry.location.lng;
                        map.setCenter(place.geometry.location);
                        panorama.setPosition(place.geometry.location);
                        if (panorama.pano) {
                            //jQuery("#step2_panorama_id").val(panorama.pano);
                            //jQuery('#rotate-1').panoramic({
                            //    panoid: panorama.pano,
                            //    width: '100%',
                            //    height: '100%'
                            //});
                        }
                        setAddressFromPlace(place);
                    });
                }
                return false;
            }
        });
    }
}
function setAddressFromPlace(place) {
    addyconfirmed = false;
    objUserAddress = document.getElementById('addressme');
    //alert('place.address_components = '+place.address_components);
    addressParam.street = extractFromAdress(place.address_components, "street_number", objUserAddress.value);

    if (addressParam.street == '') {
        addressParam.street = extractFromAdress(place.address_components, "premise", objUserAddress.value);
    }
    addressParam.address = extractFromAdress(place.address_components, "route", objUserAddress.value);
    addressParam.city = '';
    tmpcity1 = extractFromAdress(place.address_components, "locality", objUserAddress.value);
    if (tmpcity1 && (objUserAddress.value).indexOf(tmpcity1) > -1) {
        addressParam.city = tmpcity1;
    }
    if (addressParam.city == '') {
        tmpcity2 = extractFromAdress(place.address_components, "sublocality", objUserAddress.value);
        if (tmpcity2 && (objUserAddress.value).indexOf(tmpcity2) > -1) {
            addressParam.city = tmpcity2;
        }
    }
    if (addressParam.city == '') {
        tmpcity3 = extractFromAdress(place.address_components, "city", objUserAddress.value);
        if (tmpcity3 && (objUserAddress.value).indexOf(tmpcity3) > -1) {
            addressParam.city = tmpcity3;
        }
    }
    if (addressParam.city == '') {
        tmpcity4 = extractFromAdress(place.address_components, "neighborhood", objUserAddress.value);
        if (tmpcity4 && (objUserAddress.value).indexOf(tmpcity4) > -1) {
            addressParam.city = tmpcity4;
        } else if (tmpcity1 != '') {
            addressParam.city = tmpcity1;
        } else if (tmpcity2 != '') {
            addressParam.city = tmpcity2;
        } else if (tmpcity3 != '') {
            addressParam.city = tmpcity3;
        }
    }
    addressParam.state = extractFromAdress(place.address_components, "administrative_area_level_1", objUserAddress.value);
    if (addressParam.state == '') {
        addressParam.state = extractFromAdress(place.address_components, "state", objUserAddress.value);
    }
    if (addressParam.state == '') {
        addressParam.state = extractFromAdress(place.address_components, "province", objUserAddress.value);
    }
    if (addressParam.state == '') {
        addressParam.state = extractFromAdress(place.address_components, "administrative_area_level_2", objUserAddress.value);
    }
    addressParam.zipcode = extractFromAdress(place.address_components, "postal_code", objUserAddress.value);
    if (addressParam.zipcode == '') {
        addressParam.zipcode = extractFromAdress(place.address_components, "zip_code", objUserAddress.value);
        //alert(addressParam.zipcode);
        //document.getElementById('txtCity').value = place.address_components[i].long_name;
    }
    if (addressParam.zipcode == '') {
        addressParam.zipcode = extractFromAdress(place.address_components, "postal", objUserAddress.value);
    }
    if (addressParam.zipcode == '') {
        addressParam.zipcode = extractFromAdress(place.address_components, "zip", objUserAddress.value);

    }
    //alert(addressParam.zipcode)
    addressParam.country = extractFromAdress(place.address_components, "country", objUserAddress.value);
    if (!addressParam.street) {
        if (objUserAddress.value.match(/^[0-9]+/)) {
            arrMatchStreet = objUserAddress.value.match(/^[0-9]+/);
            addressParam.street = arrMatchStreet[0];
        }
    }
    if (place.types.indexOf("street_address") != -1) {

        addyconfirmed = true;
    } else if (addressParam.street && addressParam.address && addressParam.city && addressParam.state && addressParam.zipcode) {
        addyconfirmed = true;
    }
    var address = '';
    if (place.address_components) {
        address = [
            (addressParam.street || ''),
            (addressParam.address || ''),
            (addressParam.city || ''),
            (addressParam.state || '')
            //,
            //(addressParam.country || '')
        ].join(' ');

    

        document.getElementById('txtAddress').value = address;
    }
    if (typeof place.name == 'undefined') {
        place.name = addressParam.street + ' ' + addressParam.address;
    }
    mapparams.description = '<div><strong>' + place.name + '</strong><br>' + address;

}
initialize();

//function bindForm(dialog) {
//    $('#form', dialog).submit(function (e) {
//        e.preventDefault();
//        $("#ajaxLoading").css("display", "block");
//        $.ajax({
//            url: this.action,
//            type: this.method,
//            data: $(this).serialize(),
//            success: function (result) {
//                $("#ajaxLoading").css("display", "none");
//                if (result.success) {
//                    alert("address.js file called");
//                    window.location.href = result.url;
//                } else {
//                    $('#leadContent').html(result);
//                    document.getElementById('leadform').click();
//                   // $('#leadModal').modal('show');
//                }
//            },
//            error: function (result) {
//                //alert('errror');
//            }
//        });

//        return false;
//    });
//}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
function CloseModal() {
    $("#leadModal").modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}