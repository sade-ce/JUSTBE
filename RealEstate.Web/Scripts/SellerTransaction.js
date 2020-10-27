//https://www.sitepoint.com/community/t/add-days-to-date-while-ignoring-weekends-and-holidays-possible/3827/4

//https://stackoverflow.com/questions/21297323/calculate-an-expected-delivery-date-accounting-for-holidays-in-business-days-u


$(function () {
    $('.chkStep').each(function () {
        if ($(this).is(':checked')) {
            $(this).val(true)
        }
        else {
            $(this).val(false)
        }
    });
    //var Ratifieddate = $('#txtRatifiedDate').val();
    $(".content").on("click", "#txtRatifiedDate", function () {
        $(this).datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "mm/dd/yy",
            onSelect: function (date, inst) {
                $('.chkStep').each(function () {
                    var statusId = $(this).parent().parent().parent().find('.getStatus').val();
                    var getDay = $(this).parent().parent().next('div').find('.getDate')
                    //if ($(this).val() == 'true') {
                        AccessRules(date, statusId, '', getDay);
                    //}
                });

            }
        });
        $(this).datepicker("show");
    });




    $(".content").on("change", ".getDate", function (e) {
        var Ratifieddate = $('#txtRatifiedDate').val();
        var statusId = $(this).parent().parent().find('.getStatus').val();
        //if (statusId == 18) {
        //    if ($('#txtCondoRecieveDate').val() == '') {
        //        return false;
        //    }
        //}
        AccessRules(Ratifieddate, statusId, '', $(this));

    });

    //$(".content").on("click", ".IsCompleteCheck", function (e) {
    //    var Ratifieddate = $('#txtRatifiedDate').val();
    //    var statusId = $(this).parent().parent().find('.getStatus').val();
    //    var getDay = $(this).parent().next('div').find('.getDate')

    //    if ($(this).find('input[type=checkbox]').is(':checked')) {
    //                AccessRules(Ratifieddate, statusId, 'Yes', getDay);
    //    }
    //    else {
    //        AccessRules(Ratifieddate, statusId, 'No', getDay);
    //    }

    //});




    $(".content").on("change", ".chkStep", function (e) {
        var Ratifieddate = $('#txtRatifiedDate').val();
        var statusId = $(this).parent().parent().parent().find('.getStatus').val();
        var getDay = $(this).parent().parent().next('div').find('.getDate')
        if ($(this).is(':checked')) {
            $(this).val(true)
  
            if ($(this).parent().find('input').attr("name") == "[9].IsCompleted") {
               // if ($("#spnAddress").text() != '') {
                    $('.checkRatified').removeAttr("disabled");
                //}
            }
        //    if (statusId == 17) {
        //        $("#txtAIPostSet").val($("#MstClosingConfig_utblMstClosingDate_ClosingDate").val())
        //    }
        //       if (statusId == 18) {
        //    if ($('#txtCondoRecieveDate').val() == '') {
        //        return false;
        //    }
        //}
        //    AccessRules(Ratifieddate, statusId, 'Yes', getDay);
        }
        else {
            $(this).val(false)
            if ($(this).parent().find('input').attr("name") == "[9].IsCompleted") {

                $('.checkRatified').attr("disabled", "disabled");
                $('.checkRatified').find('div').find(".chkStep").prop("checked", false);
                $('.checkRatified').find(".getDate").val(0);
            }
            //if ($(this).parent().find('input').attr("name") == "[6].IsCompleted") {

            //    $("#txtCondoRecieveDate").val("");

            //}
            //AccessRules(Ratifieddate, statusId, 'No', getDay);
        }

    })
    
    $(".content").on("click", "#CADetails", function (e) {
        $("#MstClosingConfig_utblMstClosingDate_ListingTypeID").val("")
        $("#MstClosingConfig_utblMstClosingDate_Address").val("");
        $("#MstClosingConfig_utblMstClosingDate_MLSID").val("");
        $("#ListingSHA").val("")
        $("#MstClosingConfig_utblMstClosingDate_ListingTypeID").val("")
        $("#MstClosingConfig_utblMstClosingDate_StateId").val("");
        $("#MstClosingConfig_utblMstClosingDate_HomeType").val("")
        $("#drpCondoType").val("")
        //$("#MstClosingConfig_utblMstClosingDate_ClosingDate").val("")
  


        $('#lblErrorConype').css('display', 'none')
        $('#lblErrorState').css('display', 'none')
        $('#lblErrorHomeType').css('display', 'none')
        $('#lblClosingDate').css('display', 'none')
        $('#lblAddress').css('display', 'none')
        $('#lblMLSID').css('display', 'none')
        $('#lblListingType').css('display', 'none')// do in buyer
        if ($("#spnAddress").text()!="") {

            var ListinTypeId = $("#hdnListingType").val();
            var Address = $("#spnAddress").text();

            var State =""

            if ($("#spnState").text() == "DC") {
                State=1
            }
            else if ($("#spnState").text() == "Virginia") { State = 2 }
            else if ($("#spnState").text() == "Maryland") { State = 3 }
           
            var Hometype ="";
            if ($("#spnHomeType").text() == "Condo") {
                Hometype = 1
            }
            else if ($("#spnHomeType").text() == "HOA") { Hometype = 2 }
            else if ($("#spnHomeType").text() == "Fee Simple") { Hometype = 3 }
            else if ($("#spnHomeType").text() == "Multi Family") { Hometype = 4 }
         
            var CondoType = "";
            if ($("#spnCondoType").text() == "Re-Sale") {
                CondoType = 1
            }
            else if ($("#spnCondoType").text() == "New Construction") { CondoType = 2 }
      


           // var SettlementDate = $("#spnSettlementDate").text();
            if (ListinTypeId == "1") {
                $("#MstClosingConfig_utblMstClosingDate_MLSID").val(Address);
              
                $("#ListingSHA").val($("#hdnListingSHA").val())
            }
            else if (ListinTypeId == "2") {
                $("#MstClosingConfig_utblMstClosingDate_Address").val(Address);
               
               
            }
            else {

            }
            $("#MstClosingConfig_utblMstClosingDate_ListingTypeID").val(ListinTypeId)
            $("#MstClosingConfig_utblMstClosingDate_StateId").val(State);
            $("#MstClosingConfig_utblMstClosingDate_HomeType").val(Hometype)
            $("#drpCondoType").val(CondoType)
            //$("#MstClosingConfig_utblMstClosingDate_ClosingDate").val(SettlementDate)

            CheckListing();
       
          
            if ($("#MstClosingConfig_utblMstClosingDate_HomeType").val() == '3'||$("#MstClosingConfig_utblMstClosingDate_HomeType").val() == '4') {
                $("#divCondo").hide();
            }
            else {
                $("#divCondo").show();
            }

       
        }
     
    });

    $(".content").on("click", "#btnAddRatified", function (e) {
        var Ratifieddate = $('#txtRatifiedDate').val();
        $('.chkStep').each(function () {
            var statusId = $(this).parent().parent().parent().find('.getStatus').val();
            var getDay = $(this).parent().parent().next('div').find('.getDate')
            //if ($(this).val() == 'true') {
                var Area = $('#MstClosingConfig_utblMstClosingDate_StateId option:selected').text();
                var HomeType = $('#MstClosingConfig_utblMstClosingDate_HomeType option:selected').text();
                //if (statusId == 18) {
                //    if ($(this).is(':checked')) {
                //        AccessRules('', 18, 'Yes', getDay, Area, HomeType);
                //    }
                //}
                //else {
                    AccessRules(Ratifieddate, statusId, '', getDay, Area, HomeType);
               // }
            //}
        });
    });




    //$(".content").on("click", "#txtCondoRecieveDate", function () {
    //    var getDay = $(this).parent().prev('div').prev('div').find('.getDate');
    //    $(this).datepicker({
    //        changeMonth: true,
    //        changeYear: true,
    //        dateFormat: "mm/dd/yy",
    //        onSelect: function (date, inst) {
    //            AccessRules('', 18, 'Yes', getDay, '', '');

    //        }

    //    });
    //});
});



function AccessRules(date, statusId, setDays, obj, Area, Hometype) {

    var Area = $('#MstClosingConfig_utblMstClosingDate_StateId option:selected').text();
 
    var HomeType = $('#MstClosingConfig_utblMstClosingDate_HomeType option:selected').text();
    var CondoType = $('#drpCondoType option:selected').text();

    var ResultDate;
    var Day = 0;
    var Time;
    var D;
    //if (statusId == 16 || statusId == 17) {//Pre-Settlement Occupancy Agreement or Post-Settlement Occupancy Agreement
    //    date = $('#MstClosingConfig_utblMstClosingDate_ClosingDate').val(); //Settlement Date
    //}
    //if (statusId == 18) {//Condo / HOA Document Review Period
    //    date = $('#txtCondoRecieveDate').val(); //Recieved Date
    //}
    if (location.hostname === "localhost" || location.hostname === "127.0.0.1" || location.hostname === "") {

        var temp = date.split('/');
        temp = date.split('-');
        var dateLocal = temp[1] + "-" + temp[0] + "-" + temp[2];
        D = new Date(dateLocal.toString())
   
    }
    else {
        D = new Date(date.toString())
    }
    //Home Inspection set defaultumber of days  if step is marked  completed
    //if (statusId == 8) {
    //    if (setDays == 'Yes') {
    //        obj.val(7);
    //    }
    //}
    //if (statusId == 10 || statusId == 9) {//Appraisal or Financing Contingency
    //    if (setDays == 'Yes') {
    //        obj.val(21);
    //    }

    //}
    //if (statusId == 18) {//Condo / HOA Document Review Period
    //    if (setDays == 'Yes') {
    //        //var res = validateCondo();
    //        //if (res == false) {
    //        //    return false;
    //        //}
    //        if (Area == 'DC') {
    //            if (obj.val() == "0") {
    //                if (HomeType == "Condo" && CondoType == "Re-Sale") {
    //                    obj.val(3);
    //                    //obj.val(parseInt(obj.val())+3);
    //                }
    //                if (HomeType == "Condo" && CondoType == "New Construction") {
    //                    obj.val(3);
    //                }
    //            }
    //        }
    //        else if (Area == 'Maryland') {
    //            if (obj.val() == "0") {
    //                if (HomeType == "Condo" && CondoType == "Re-Sale") {
    //                    obj.val(7);

    //                }
    //                if (HomeType == "Condo" && CondoType == "New Construction") {
    //                    obj.val(15);

    //                }
    //                if (HomeType == "HOA" && CondoType == "Re-Sale") {
    //                    obj.val(5);

    //                }
    //                if (HomeType == "HOA" && CondoType == "New Construction") {
    //                    //if (('#txtlots').val() > 12) {

    //                    //}
    //                    obj.val(5);

    //                }
    //            }
    //        }
    //        else if ('Virginia') {
    //            if (obj.val() == "0") {
    //                if (HomeType == "Condo" && CondoType == "Re-Sale") {
    //                    obj.val(3);

    //                }
    //                if (HomeType == "Condo" && CondoType == "New Construction") {
    //                    obj.val(5);

    //                }
    //                if (HomeType == "HOA" && CondoType == "Re-Sale") {
    //                    obj.val(3);

    //                }
    //                if (HomeType == "HOA" && CondoType == "New Construction") {
    //                    //if (('#txtlots').val() > 12) {

    //                    //}
    //                    obj.val(3);

    //                }
    //            }
    //        }
    //        else {

    //        }

    //    }
    //}
    //if (setDays == 'No') {
    //    obj.val(0);
    //}
    //setDays = '';




    if (obj.val() > 0) {
        Day = obj.val();
        //obj.parent().next('div').find('.spnTime').css('display', 'block');
        if (Area == 'DC') {

            //if (statusId == 16) {//Pre-Settlement Occupancy Agreement

            //    ResultDate = formatDateToString(D);
            //    $('#txtAIPreSet').val(formatDateToString(D.addallDays(-Day)));

            //}
            //else if (statusId == 18) {//Condo / HOA Document Review Period
            //    if (HomeType == "Condo" && CondoType == "Re-Sale") {
            //        ResultDate = formatDateToString(D.addbizDays(Day));
            //    }
            //    if (HomeType == "Condo" && CondoType == "New Construction") {
            //        ResultDate = formatDateToString(D.addallDays(Day));
            //    }
            //    if (HomeType == "Fee Simple") {
            //        ResultDate = formatDateToString(D.addallDays(Day));
            //    }

            //}
            //else {
                ResultDate = formatDateToString(D.addallDays(Day));
                // Time='6:00 PM'
            //}
            Time = '6:00 PM'
        }
        else if (Area == 'Maryland') {

            //if (statusId == 16) {//Pre-Settlement Occupancy Agreement

            //    ResultDate = formatDateToString(D);
            //    $('#txtAIPreSet').val(formatDateToString(D.addallDays(-Day)));
            //}
            //else if (statusId == 18) {//Condo / HOA Document Review Period
            //    ResultDate = formatDateToString(D.addallDays(Day));


            //}
            //else {
                ResultDate = formatDateToString(D.addallDays(Day));
                // Time='6:00 PM'
            //}
            Time = '6:00 PM'
        }
        else if ('Virginia') {
            //if (statusId == 16) {//Pre-Settlement Occupancy Agreement

            //    ResultDate = formatDateToString(D);
            //    $('#txtAIPreSet').val(formatDateToString(D.addallDays(-Day)));
            //}
            //else if (statusId == 18) {//Condo / HOA Document Review Period
            //    ResultDate = formatDateToString(D.addallDays(Day));


            //}
            //else {
                ResultDate = formatDateToString(D.addallDays(Day));
                // Time = '9:00 PM'
            //}
            Time = '9:00 PM'
        }
        else {
            Time = '6:00 PM'
        }
        obj.parent().next('div').find('.setDate').val(ResultDate);

        var spann = obj.parent().next('div').find('.spnTime').find('i');
        //alert(spann)
        if (spann == undefined || spann == null) {

        }
        else {
            obj.parent().next('div').find('.spnTime').css('display', 'flex');
            obj.parent().next('div').find('.spnTime').empty();
            obj.parent().next('div').find('.spnTime').append('<i class="material-icons">alarm</i>');

            obj.parent().next('div').find('.spnTime').find('i').after(Time);
            obj.parent().next('div').find('.hdnTime').val(Time);
        }

    }
    else {
        //if (statusId == 16) {//Pre-Settlement Occupancy Agreement
        //    //    date = $('#MstClosingConfig_utblMstClosingDate_ClosingDate').val(); //Settlement Date

        //    $('#txtAIPreSet').val('');

        //}
        //else if (statusId == 18) {//cONDO
        //    //    date = $('#MstClosingConfig_utblMstClosingDate_ClosingDate').val(); //Settlement Date

        //    // $('#drpCondoType').val('');
            
        //    if ($("#txtCondoRecieveDate").val() == "") {


        //        D = new Date()
        //    }
        //    else {

        //    }
        //}
        //else {

            D = new Date()

        //}
        ResultDate = formatDateToString(D.addallDays(Day))
        obj.parent().next('div').find('.setDate').val(ResultDate);
        var spann = obj.parent().next('div').find('.spnTime').find('i')
      //  alert(spann)
        if (spann == undefined || spann == null) {

        }
        else {
          
            obj.parent().next('div').find('.spnTime').find('i').remove();
            obj.parent().next('div').find('.spnTime').css('display', 'none');

            obj.parent().next('div').find('.hdnTime').val('');
        }

    }



}

//function validateCondo() {
//    var message = "";
//    var isValid = true;
//    //if ($('#drpCondoType').val() == "") {
//    //    message = "Please select type\n"
//    //    isValid = false;
//    //}
//    //else {
//    //}
//    if ($('#txtCondoRecieveDate').val() == "") {
//        message = message + "Please enter date recieved"
//        isValid = false;
//    }
//    else {

//    }
//    if (message != "") {
//        alert(message)
//    }
//    return isValid;
//}


//check current one https://www.timeanddate.com/time/zone/usa/virginia
//function convertToServerTimeZone() {

//    //EST
//    offset = -4.0
//    //offset = -5.0
//    clientDate = new Date();
//    utc = clientDate.getTime() + (clientDate.getTimezoneOffset() * 60000);

//    serverDate = new Date(utc + (3600000 * offset));

//    alert(serverDate.toLocaleString());


//}

function formatDateToStringLocal(date) {
    // 01, 02, 03, ... 29, 30, 31
    var dd = (date.getDate() < 10 ? '0' : '') + date.getDate();
    // 01, 02, 03, ... 10, 11, 12
    var MM = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
    // 1970, 1971, ... 2015, 2016, ...
    var yyyy = date.getFullYear();

    // create the format you want
    return (MM + "/" + dd + "/" + yyyy);
}


function formatDateToString(date) {
    // 01, 02, 03, ... 29, 30, 31
    var dd = (date.getDate() < 10 ? '0' : '') + date.getDate();
    // 01, 02, 03, ... 10, 11, 12
    var MM = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
    // 1970, 1971, ... 2015, 2016, ...
    var yyyy = date.getFullYear();

    // create the format you want
    return (dd + "-" + MM + "-" + yyyy);
}

var Bday = {
    bizDays: function (d1, d2) {
        d1 = new Date(d1);
        d2 = new Date(d2);
        var interval = Math.abs(d1 - d2);
        var days = Math.floor(interval / 8.46e7);
        var tem = days % 7;
        var weeks = Math.floor(days / 7);
        var temp = tem;
        var wd1 = d1.getDay();
        var wd2 = d2.getDay();
        if (wd1 == 6) tem -= 2;
        else if (wd1 == 0) tem -= 1;
        if (wd2 == 0) tem -= 1;
        var diff = Bday.holBetween(d1, d2);
        tem -= diff[0];
        return weeks * 5 + tem;;
    },
    getdayOffset: function (y, wot) {
        var month, count, D, temp;
        if (wot.length > 3) {
            if (y < wot[3]) return false;
            if (wot[4] && y > wot[4]) return false;
        }
        day = wot[1];
        if (wot[0] == 'last') {
            month = wot[2];
            count = -1;
        }
        else {
            count = wot[0] - 1;
            month = wot[2] - 1;
        }
        D = new Date(y, month, 1);
        while (D.getDay() != day) D.setDate(D.getDate() + 1);
        D.setDate((7 * count) + D.getDate());
        return [D.getMonth(), D.getDate()];
    },
    getHolidays: function (year) {
        year = year || new Date().getFullYear();
        if (Bday.holidays['y_' + year]) return Bday.holidays['y_' + year];

        var hol = [ //months are indexed by +1
		['New Years Day', [1, 1]],
		['Independence Day', [7, 4]],
		['Christmas Day', [12, 25]],
		['Thanksgiving Day', [4, 4, 11]],
		['Columbus Day', [2, 1, 10]],
		['Mothers\\ Day', [2, 0, 5]],
		['Fathers\\ Day', [3, 0, 6]],
		['Labor Day', [1, 1, 9]],
		['Martin Luther King Day', [3, 1, 1]],
		['Presidents Day', [3, 1, 2]],
		['Memorial Day', ['last', 1, 5]],
		['Veterans\\ Day', [11, 11]]
        ]
        if (year < 1796) hol = [];
        else {
            var I = year - 1937;
            if (I > 0 && I % 4 == 0) hol.push(['Inauguration Day', [1, 20]]);
        }
        if (year < 1900) hol.splice(4);
        if (year < 1861) hol.splice(3);
        return Bday.setHolArray(year, hol)
    },
    holBetween: function (d1, d2) {
        var A = [], tem, day, count = 0;
        while (d2 - d1 > 0) {
            d1.setDate(d1.getDate() + 1);
            tem = d1.isHoliday();
            if (!tem) continue;
            A.push(tem);
            day = d1.getDay();
            if (day != 0 && day != 6)++count;
        }
        return [count, A];
    },
    holidays: {},
    setHolArray: function (y, hol) {
        var L = hol.length;
        var A = [], name, temp;
        for (var i = 0; i < 12; i++) A[i] = [];
        for (var i = 0; i < L; i++) {
            name = hol[i][0];
            temp = hol[i][1];
            if (!temp) continue;
            if (temp.length >= 3) {
                temp = Bday.getdayOffset(y, temp);
            }
            else temp[0] -= 1;
            A[temp[0]][temp[1]] = name;
        }
        return A;
    }
}
Date.prototype.isHoliday = function () {
    var y = this.getFullYear();
    var ys = 'y_' + y;
    if (!Bday.holidays[ys]) {
        Bday.holidays[ys] = Bday.getHolidays(y);
    }
    var hol = Bday.holidays[ys];
    var m = this.getMonth();
    var d = this.getDate();
    if (hol[m] && hol[m][d]) return hol[m][d];
    return false;
}
Date.prototype.addbizDays = function (n) {
    var D = this;
    var num = Math.abs(n);
    var tem, count = 0;
    var dir = (n < 0) ? -1 : 1;
    while (count < num) {
        D = new Date(D.setDate(D.getDate() + dir));
        if (D.isHoliday()) continue;
        tem = D.getDay();
        if (tem != 0 && tem != 6)++count;
    }
    return D;
}
Date.prototype.addallDays = function (n) {
    var D = this;
    var num = Math.abs(n);
    var tem, count = 0;
    var dir = (n < 0) ? -1 : 1;
    while (count < num) {
        D = new Date(D.setDate(D.getDate() + dir));
        ++count;
    }
    return D;
}

function getDays(d1, d2) {
    var date1 = new Date(d1);
    var date2 = new Date(d2);
    var res = Math.abs(date1 - date2) / 1000;
    var days = Math.floor(res / 86400);
    return days;
}




//------------------------------------------------Deal Documents-------------------------------------------

function getExtension(filename) {
    return filename.split('.').pop().toLowerCase();
}


$(document).ready(function () {
    checkNotes();
    //ChecklistItems();
   
    loadData();
    loadGalData();
    loadDealVendor();




});

//Load Data function    Recent change replace loaddata()
function loadData() {
    $.ajax({
        url: "/Coordinator/DealTrackerV2/SellerDealDocumentList",
        type: "GET",
        data: {
            TransactionID: $('#DealTracker_TransactionID').val(),
            Email: $('#UserInfo_Email').val()
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //alert(result.ClientDealDocumentList)
            var html = '';

            if (Object.keys(result.MstSellerDocumentListView).length > 0) {
                html += '<div class="tableHeadRow">'
                html += '                                <div class="tableCell">File Name</div>'
                html += '                                 <div class="tableCell">Date Uploaded</div>'
                html += '                                <div class="tableCell">Download</div>'
                html += '                                <div class="tableCell">Action</div>'
                html += '                      </div>'
                $.each(result.MstSellerDocumentListView, function (key, item) {

                    //html += '<li><a download href="/UploadedFiles/TrackDeal/'+item.DocFile+'" target="_blank"><img src="../../../../Content/AppDashboard/assets/img/tmp/doc.png" alt="" class="doc-img" > '+item.Title+' </a>'

                    html += '<div class="tableRow">'
                    html += '<div class="tableCell">' + item.Title + '</div>'
                    html += '<div class="tableCell">' + item.UpdatedOn + '</div>'
                    html += '<div class="tableCell"><a href="/UploadedFiles/TrackDeal/' + item.SellerDealDocID + item.TrackDocFile + '" class="download-link" target="_blank" ><img src="/Content/NewVersion/Admin/img/download-icon.svg" /></a></div>'
                    html += '<div class="tableCell"><a href="#!" data-TrackingId="' + item.SellerTrackingID + '"  data-DocId="' + item.SellerDealDocID + '"  class="delete-link" onclick="DeleleDocument(this)"><img src="/Content/NewVersion/Admin/img/delete-icon.svg" /></a></div>'
                    html += '</div>'
                });
            }
            else {


                html += '<p style="font-size:14px;padding:10px 0 0 0;text-align: center;">No record found.</p>'

            }
            $('.tDocumentbody').html("");
            $('.tDocumentbody').html(html);

            var Clienthtml = '';

            if (Object.keys(result.ClientDealDocumentList).length > 0) {
                Clienthtml += '<div class="tableHeadRow">'
                Clienthtml += '                                <div class="tableCell">File Name</div>'
                Clienthtml += '                                 <div class="tableCell">Date Uploaded</div>'
                Clienthtml += '                                <div class="tableCell">Download</div>'
                Clienthtml += '                                <div class="tableCell">Action</div>'
                Clienthtml += '                      </div>'
                $.each(result.ClientDealDocumentList, function (key, item) {

                    //html += '<li><a download href="/UploadedFiles/TrackDeal/'+item.DocFile+'" target="_blank"><img src="../../../../Content/AppDashboard/assets/img/tmp/doc.png" alt="" class="doc-img" > '+item.Title+' </a>'

                    Clienthtml += '<div class="tableRow">'
                    Clienthtml += '<div class="tableCell">' + item.Title + '</div>'
                    Clienthtml += '<div class="tableCell">' + item.dateupdated + '</div>'
                    Clienthtml += '<div class="tableCell"><a href="/UploadedFiles/TrackDeal/' + item.DocFile + '" class="download-link" target="_blank" ><img src="/Content/NewVersion/Admin/img/download-icon.svg" /></a></div>'
                    Clienthtml += '<div class="tableCell"><a href="#!"  data-DocId="' + item.DealTrackDocID + '"  class="delete-link" onclick="DeleleClientDocument(this)"><img src="/Content/NewVersion/Admin/img/delete-icon.svg" /></a></div>'
                    Clienthtml += '</div>'
                });
            }
            else {


                Clienthtml += '<p style="font-size:14px;padding:10px 0 0 0;text-align: center;">No record found.</p>'

            }
            $('.tClientDocumentbody').html("");
            $('.tClientDocumentbody').html(Clienthtml);
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert('Sorry, Something went wrong. Please try again.');
        }
    });
}

//Add Data Function
function AddDocument() {
    var res = validate();
    if (res == false) {
        return false;
    }

    var form = $('#FormUploadDoc')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: "/Coordinator/DealTrackerV2/AddSellerDealDocument",
        data: dataString,
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        dataType: "json",
        success: function (result) {
            //    alert(result) remove alert
            if (result == "0") {
                $('#uploadDocumentModal').modal('hide');
                $('#lblDocError').css('display', 'none')
                loadData();


            }
            else {

                $('#lblDocError').css('display', 'block')
                $('#lblDocError').text(result)
            }


        },
        error: function (errormessage) {
            $('#lblDocError').css('display', 'block');
            $('#lblDocError').text('Sorry, Something went wrong. Please try again.');
            //alert(errormessage.responseText);
        }
    });
}


//function for deleting employee's record
function DeleleDocument(obj) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        var ID = $(obj).attr("data-DocId")
        var TracId = $(obj).attr("data-TrackingId")
        $.ajax({
            url: "/Coordinator/DealTrackerV2/DeleteSellerDealDocument?DealTrackDocID=" + ID + "&TrackingID=" + TracId,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {

                loadData();
            },
            error: function (errormessage) {
                alert('Sorry, Something went wrong. Please try again.');
            }
        });
    }
}

//function for deleting employee's record
function DeleleClientDocument(obj) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        var ID = $(obj).attr("data-DocId")
      
        $.ajax({
            url: "/Coordinator/DealTrackerV2/DeleteClientDealDocument?DealTrackDocID=" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {

                loadData();
            },
            error: function (errormessage) {
                alert('Sorry, Something went wrong. Please try again.');
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBox() {
    $('#hdnDocID').val("");
    $('#txtDocTitle').val("");
    $('#txtDocDescription').val("");
    $('#divDocDescription').css("display", "none");
    $('#flDocFile').val("");
    $('#lblDocTitle').css('display', 'none');
    $('#lblDocDescription').css('display', 'none');
    $('#lblDocFile').css('display', 'none');
    $('#lblDocError').css('display', 'none')
}
//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#txtDocTitle').val().trim() == "") {
        $('#lblDocTitle').css('display', 'block');
        isValid = false;
    }
    else {
        $('#lblDocTitle').css('display', 'none');
    }

    if ($('#flDocFile').val() == "") {
        $('#lblDocFile').css('display', 'block');
        isValid = false;
    }
    else {
        $('#lblDocFile').css('display', 'none');
    }
    if ($("#txtDocDescription").is(":visible")) {
        if ($("#txtDocDescription").val() == '') {
            $('#lblDocDescription').css('display', 'block');
            isValid = false;
        }
        else {
            $('#lblDocDescription').css('display', 'none');
        }
    }

    return isValid;
}

$('#txtDocTitle').autocomplete({
    source: function (request, response) {
        var itemnamecodes = new Array();
        $.ajax({
            type: 'POST',
            url: '/Coordinator/DealTrackerV2/SellerDocumentautocomplete/',
            dataType: 'json',
            data: { term: request.term, Email: $('#UserInfo_Email').val(), TransactionID: $('#DealTracker_TransactionID').val() },
            success: function (data) {
                if (!data.length) {
                    $("#divDocDescription").css('display', 'flex')

                    response($.map(data, function (item) {

                        return { label: "", Id: "" };

                    }))

                }
                else {
                    $("#divDocDescription").css('display', 'none')
                    response($.map(data, function (item) {

                        return { label: item.Key, Id: item.Value.split('_')[0] };

                    }))
                }
            }
        })
    },
    select: function (e, i) {
        var id = i.item.Id.split('_')[0];
        var desc = i.item.Id.split('_')[1];
        $('#hdnDocID').val(id);
        $('#txtDocDescription').val(desc)
        //if (id = "") {

        //    $("#divDocDescription").css('display', 'flex')
        //    $("#divDocDescription").val(desc);

        //}
        //else {
        //    $("#divDocDescription").css('display', 'none')
        //}
    }

})
//    .focus(function () {
//    $(this).autocomplete("search", "");
//    $('#txtDocTitle').autocomplete({
//        autoFocus: true
//    })
   
//});


function loadGalData() {
    $.ajax({
        url: "/Coordinator/DealTrackerV2/GalleryList",
        type: "GET",
        data: {
            TransactionID: $('#DealTracker_TransactionID').val(),
            Email: $('#UserInfo_Email').val()
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
       
            var html = '';

            if (Object.keys(result).length > 0) {
                html += '<div class="row">'
                $.each(result, function (key, item) {
                    html += ' <div class="col-12 col-sm-6 col-md-4">'
                    html += ' <div class="image-wrap">'
                    if (item.PhotoThumb == null || item.PhotoThumb == "") {
                        html += ' <img src="/UploadedFiles/PhotoGallery/' + item.GallaryID + item.PhotoNormal + '" class="img-fluid" />'
                    }
                    else {
                        html += ' <img src="' + item.PhotoThumb + '" class="img-fluid" />'

                    }
                    html += ' <div class="image-del-download">'


                    if (item.PhotoNormal != "") {
                        html += ' <a href="/UploadedFiles/PhotoGallery/' + item.GallaryID + item.PhotoNormal + '" target="_blank" class="downloadIcon"><i class="material-icons">publish</i></a>'
                    }
                    else {
                        html += ' <span>Not Available</span>'
                    }
                    html += '<a href="#!" class="deleteIcon" data-GalleryId="' + item.GallaryID + '"  onclick="DeleteGallery(this)" ><i class="material-icons">delete_forever</i></a>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'
                });
                html += '</div>'
            }
            else {
             
                html += ' <div class="custom-table">'            
                html += '<div><p style="font-size:14px;padding:10px 0 0 0;text-align: center;">No record found.</p></div>'                         
                html +='</div>'
            
            }
            $('.tGallerybody').html("");
            $('.tGallerybody').html(html);


            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert('Sorry, Something went wrong. Please try again.');
        }
    });
}




function AddGallery() {
    var res = validategallery();
    if (res == false) {
        return false;
    }

    var form = $('#FormUploadGal')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: "/Coordinator/DealTrackerV2/AddDealGallery",
        data: dataString,
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        dataType: "json",
        success: function (result) {
            //    alert(result) remove alert
            if (result == "0") {
                loadGalData();
                $('#uploadImageModal').modal('hide');
                $('#lblGalError').css('display', 'none')
                cropper.destroy();
                newcropper();
              

            }
            else {

                $('#lblGalError').css('display', 'block')
                $('#lblGalError').text(result)
            }


        },
        error: function (errormessage) {
            $('#lblGalError').css('display', 'block');
            $('#lblGalError').text('Sorry, Something went wrong. Please try again.');
            //alert(errormessage.responseText);
        }
    });


}

function DeleteGallery(obj) {
    var ans = confirm("Are you sure you want to delete this record?");
    if (ans) {
        var ID = $(obj).attr("data-GalleryId")
        $.ajax({
            url: "/Coordinator/DealTrackerV2/DeleteGalleryDocument?GallaryID=" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {

                loadGalData();
            },
            error: function (errormessage) {
                alert('Sorry, Something went wrong. Please try again.');
            }
        });
    }
}
var cropper;
function clearGalTextBox() {
    if (cropper != undefined) {
        cropper.destroy();
    }
    newcropper()
  
    $('#lblGalFile').css("display","none");
    $('#lblGalError').css("display", "none");
}
function newcropper() {
    cropper = new Slim(document.getElementById('my-cropper'), {
        didRemove: "imageWillBeRemoved",
        size: "600,450",
    }); 
    $("#my-cropper").css("max-width", "260px")
}

function validategallery() {
    var isValid = true;

    var val = $("#my-cropper").find("input[type=hidden]").val();

    //if ($("#my-cropper").has(".image-exist").length) {
    //    if (val == 'PhotoDeleted') {
    //        $('#lblGalFile').css('display', 'block');
    //        isValid = false;
    //    }


    //    else {
    //        $('#lblGalFile').css('display', 'none');
    //    }

    //}
    //else {
        if (val == '' || val == 'PhotoDeleted') {
            $('#lblGalFile').css('display', 'block');
            isValid = false;
        }
        else {
            $('#lblGalFile').css('display', 'none');
        }
    // }
        return isValid;
}

function imageWillBeRemoved() {

    if ($("#my-cropper").length) {
        $("#my-cropper").find("input[type=hidden]").val('PhotoDeleted')
    }
}

function actionEdit(data, ready) {

    $("#hdnaction").val('Yes')

}
function actionCancel(data, ready) {

    $("#hdnaction").val('')

}
   

    //$(document).ajaxStart(function () {
    //    $("#ajaxLoading").css("display", "none");
    //    $("#ajax-backdrop").css("display", "none");
    //});txtClientId
    $('#VendorType').autocomplete({
        source: function (request, response) {
            //$("#ajaxLoading").attr("style", "display: none");
            //$("#ajax-backdrop").attr("style", "display: none");
            var itemnamecodes = new Array();
            $.ajax({
                type: 'POST',
                url: '/Coordinator/DealTrackerV2/Vendorautocomplete',
                dataType: 'json',
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Key, Id: item.Value };
                    }))
                }
            })
        },
        minLength: 0,

        select: function (e, i) {
            $('#txtTitleV').val("");

        }
    }).focus(function() {
        $(this).autocomplete("search", "");
        $('#VendorType').autocomplete({
            autoFocus: true
        })
    });

    $('#txtTitleV').autocomplete({
        source: function (request, response) {
            //$("#ajaxLoading").css("display", "none");
            //$("#ajax-backdrop").css("display", "none");
            var itemnamecodes = new Array();
            $.ajax({
                type: 'POST',
                url: '/Coordinator/DealTrackerV2/ClientVendorautocomplete',
                dataType: 'json',
                data: { term: request.term, VendorType:$('#VendorType').val()},
                success: function (data) {
                    console.log(data)
                    response($.map(data, function (item) {
                        return { label: item.Key, Id: item.Value };
                    }))
                }
            })
        },
        minLength: 0,

        select: function (e, i) {
            var mystring=i.item.Id;
            console.log(mystring);

         
            var res = mystring.split("}");
   
            $("#txtVendorPhone").val(res[0]);
            $("#txtVendorEmail").val(res[1]);
            $("#ListingCVendor").val(res[3]);
            $("#VendorType").val(res[2]);
       

            //if($("#txtVenId").val().length > 0 )
            //{
            //    $("#txtSubTitle").val(res[3]);
            //    $("#txtWebsitelink").val(res[4]);
            //    $("#txtLocation").val(res[5]);
            //    $("#txtVenImage").val(res[6]);
            //    $("#txtAbout").val(res[7]);
            //}

        }
    }).focus(function() {
        $(this).autocomplete("search", "");
        $('#txtTitleV').autocomplete({
            autoFocus: true
        })
    });


    $('#txtContactName').autocomplete({
        source: function (request, response) {
            //$("#ajaxLoading").attr("style", "display: none");
            //$("#ajax-backdrop").attr("style", "display: none");
            var itemnamecodes = new Array();
            $.ajax({
                type: 'POST',
                url: '/Coordinator/DealTrackerV2/GetVendorContactByVendorId',
                dataType: 'json',
                data: { term: request.term, VendorId:$("#ListingCVendor").val()},
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Key, Id: item.Value };
                    }))
                }
            })
        },
        minLength: 0,

        select: function (e, i) {
            var mystring = i.item.Id;
            var res = mystring.split("}");
            $("#txtContactPhone").val(res[0]);
            $("#txtContactTitle").val(res[1]);
            $("#VendorContactId").val(res[2]);
        }
    }).focus(function() {
        $(this).autocomplete("search", "");
        //$('#txtContactName').autocomplete({
        //    autoFocus: true
        //})
    });

    function clearVendorData() {
        $('#ListingCVendor').val("");
        $('#VendorType').val("");
        $('#lblVendorType').css('display', 'none');
        $('#txtTitleV').val("");
        $('#lblTitleV').css('display', 'none');
        $('#txtContactName').val("");
        $('#txtContactTitle').val("");
        $('#txtContactPhone').val("");
        $('#txtVendorEmail').val("");
        $('#txtVendorPhone').val("");
        $('#VendorContactId').val("");
        $('#lblVendorError').css("display", "none");
        $('#lblVendorEmail').css("display", "none");
        $('#lblVendorPhone').css("display", "none");
        $('#lblContactPhone').css("display", "none");
    }


var selected = new Array();
$('.content').on("click", ".chkchecklist", function () {
    var selected = new Array();
    var items='';
    $(".chkchecklist:checked").each(function () {
        selected.push(this.value);
    });

    //Display the selected CheckBox values.
    if (selected.length > 0) {
      items=  items + selected.join(",");
    }

    $("#txtItems").val(items);

});

function loadDealVendor() {
    $.ajax({
        url: "/Coordinator/DealTrackerV2/DealVendorList",
        type: "GET",
        data: {
            TransactionID: $('#DealTracker_TransactionID').val(),
            Email: $('#UserInfo_Email').val()
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //alert(result.ClientDealDocumentList)
            var html = '';

            if (Object.keys(result).length > 0) {
                html += '<div class="tableHeadRow">'
                //html += '                                <div class="tableCell vendorImage"></div>'
                html += '                                 <div class="tableCell">Name</div>'
                //html += '                                <div class="tableCell">Type</div>'
                html += '                                <div class="tableCell">Email</div>'
                html += '                                <div class="tableCell">Phone</div>'
                html += '                                <div class="tableCell">Contact Details</div>'
                html += '                                <div class="tableCell"></div>'
                html += '                      </div>'
            
                $.each(result, function (key, item) {

                    //html += '<li><a download href="/UploadedFiles/TrackDeal/'+item.DocFile+'" target="_blank"><img src="../../../../Content/AppDashboard/assets/img/tmp/doc.png" alt="" class="doc-img" > '+item.Title+' </a>'

                    html += '<div class="tableRow">'
                    //html += '<div class="tableCell vendorImage"></div>'
                    html += '<div class="tableCell">' + item.Vendor + '</br>('+item.VendorType +')</div>'
                    //html += '<div class="tableCell">' + item.VendorType + '</div>'
              
                    html += '<div class="tableCell">' + item.VendorEmail + '</div>'
                    html += '<div class="tableCell">' + item.VendorPhone + '</div>'
                    html += '<div class="tableCell">' + item.ContactName + '</br> ' + item.ContactTitle + '</br> ' + item.ContactPhone + '</div>'

                    html += '<div class="tableCell"><a href="#!" data-DealVendorId="' + item.DealVendorId + '"  class="delete-link" onclick="DeleleVendor(this)"><img src="/Content/NewVersion/Admin/img/delete-icon.svg" /></a></div>'
                    html += '</div>'
                });
            }
            else {


                html += '<p style="font-size:14px;padding:10px 0 0 0;text-align: center;">No record found.</p>'

            }
            $('.tDealVendor').html("");
            $('.tDealVendor').html(html);

         
        
        },
        error: function (errormessage) {
            alert('Sorry, Something went wrong. Please try again.');
        }
    });
}

function AddVendor() {
    var res = validateVendor();
    if (res == false) {
        return false;
    }

    var form = $('#FormAddVendor')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: "/Coordinator/DealTrackerV2/AddDealVendor",
        data: dataString,
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        dataType: "json",
        success: function (result) {
            //    alert(result) remove alert
            if (result == "0") {
                $('#addNewVendorModal').modal('hide');
                $('.modal-backdrop').remove();
                loadDealVendor()


            }
            else {

                $('#lblVendorError').css('display', 'block')
                $('#lblVendorError').text(result)
            }


        },
        error: function (errormessage) {
            $('#lblVendorError').css('display', 'block');
            $('#lblVendorError').text('Sorry, Something went wrong. Please try again.');
            //alert(errormessage.responseText);
        }
    });
}

function validateVendor() {
    var isValid = true;
    if ($('#VendorType').val().trim() == "") {
        $('#lblVendorType').css('display', 'block');
        isValid = false;
    }
    else {
        $('#lblVendorType').css('display', 'none');
    }

    if ($('#txtTitleV').val() == "") {
        $('#lblTitleV').css('display', 'block');
        isValid = false;
    }
    else {
        $('#lblTitleV').css('display', 'none');
    }





    if ($('#txtVendorEmail').val() != "") {

        if (!ValidateEmail($("#txtVendorEmail").val())) {
            $('#lblVendorEmail').css('display', 'block');
            isValid = false;
        }
        else {
            $('#lblVendorEmail').css('display', 'none');
        }


    }
    else {
        $('#lblVendorEmail').css('display', 'none');
    }

    if ($('#txtVendorPhone').val() != "") {
  
        if ($('#txtVendorPhone').val().length < 12) {
            $('#lblVendorPhone').css('display', 'block');
            isValid = false;
        }
        else {
            $('#lblVendorPhone').css('display', 'none');
        }


    }
    else {
        $('#lblVendorPhone').css('display', 'none');
    }
    if ($('#txtContactPhone').val() != "") {

        if ($('#txtContactPhone').val().length < 12) {
            $('#lblContactPhone').css('display', 'block');
            isValid = false;
        }
        else {
            $('#lblContactPhone').css('display', 'none');
        }


    }
    else {
        $('#lblContactPhone').css('display', 'none');
    }
    return isValid;
}

function DeleleVendor(obj) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        var ID = $(obj).attr("data-DealVendorId")
        $.ajax({
            url: "/Coordinator/DealTrackerV2/DeleteDealVendor?DealVendorId=" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {

                loadDealVendor();
            },
            error: function (errormessage) {
                alert('Sorry, Something went wrong. Please try again.');
            }
        });
    }
}

function ValidateEmail(email) {
    var expr = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/
    return expr.test(email);
};






$('.validPhone').keydown(function (e) {
    var key = e.which || e.charCode || e.keyCode || 0;
    $phone = $(this);

    // Don't let them remove the starting '('


    // Auto-format- do not expose the mask as the user begins to type
    if (key !== 8 && key !== 9) {
        if ($phone.val().length === 3) {
            $phone.val($phone.val() + '.');
        }
        if ($phone.val().length === 7) {
            $phone.val($phone.val() + '.');
        }

    }

    // Allow numeric (and tab, backspace, delete) keys only
    return (key == 8 ||
            key == 9 ||
            key == 46 ||
            (key >= 48 && key <= 57) ||
            (key >= 96 && key <= 105));
})

     .bind('focus click', function () {
         $phone = $(this);


     })

     .blur(function () {
         $phone = $(this);


     });


//function ChecklistItems() {
//    var Items = $("#txtItems").val();
//    var nameArr = Items.split(',');
  
//    $('.chkchecklist').each(function () {
//            $(this).prop("checked", ($.inArray($(this).val(), nameArr) != -1));
//        });
//}


//nOTES

$(document).ready(function () {
    CKEDITOR.config.toolbar = 'Basic';
    CKEDITOR.config.toolbar_Basic =
[
['Bold', 'Italic', 'Underline', 'Link', 'Unlink', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat', 'BulletedList', 'NumberedList', 'Blockquote', 'Table', 'Image', 'Flash']
];
    //CKEDITOR.config.extraPlugins = [
    //    ['html5video','widget','widgetselection','clipboard','lineutils']
    //];
    CKEDITOR.replace("txtNoteDescription");
    //CKEDITOR.replace("txtDescription1");
    //, { htmlEncodeOutput: true }
    //   CKEDITOR.config.removePlugins = 'save';


});
var ActiveStatus;
function checkNotes() {
    $('.StepNotes').each(function () {
        if ($(this).val() == null || $(this).val() == '') {
            $(this).parent().find('.AddNewNote').css("display", "block")
            $(this).parent().find('.EditNote').css("display", "none")
        }
        else {
            $(this).parent().find('.AddNewNote').css("display", "none")
            $(this).parent().find('.EditNote').css("display", "block")
        }
    });
}
function GetNotes(statusid) {
    ActiveStatus = statusid;
   
    CKEDITOR.instances.txtNoteDescription.setData($("#Notes" + statusid).val());
    $('#staticBackdrop1').modal('show');
}
function AddNotes() {
 
    var text = CKEDITOR.instances.txtNoteDescription.getData();

    if (text == '') {
        $('#lblNotesError').css("display", "block")
        return false;
    }
    else {
        $('#lblNotesError').css("display", "none")
    }
    var id = "#Notes" + ActiveStatus

    $(id).val(text)


    $('#staticBackdrop1').modal('hide');
    CKEDITOR.instances.txtNoteDescription.setData('');
    $("#Notes" + ActiveStatus).parent().find('.AddNewNote').css("display", "none")
    $("#Notes" + ActiveStatus).parent().find('.EditNote').css("display", "block")
    $("#lblNotesError").css("display", "none")
    ActiveStatus = '';
}


function ViewNotes(statusid) {
    $("#divNotesDescription").html($("#Notes" + statusid).val())
    $('#staticBackdrop').modal('show');
}
function DeleteNotes(statusid) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $("#Notes" + statusid).val('');
        $("#Notes" + statusid).parent().find('.AddNewNote').css("display", "block")
        $("#Notes" + statusid).parent().find('.EditNote').css("display", "none")
    }
}


function clearNote() {
    CKEDITOR.instances.txtNoteDescription.setData('');
}

//Add dEAL pPRICE
function SetDealPrice() {
    //if ($("#DealPrice").val() == "0.00" || $("#DealPrice").val() == "") {
    //    $("#lblDealPrice").css("display", "block");
    //    return false;
    //}
    //else {
    //    $("#lblDealPrice").css("display","none");
    //}
    var form = $('#frmDealPrice')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: "/Coordinator/DealTrackerV2/SetDealPrice",
        data: dataString,
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        dataType: "json",
        success: function (result) {
            //    alert(result) remove alert
            if (result == "0") {
                $('#uploaddealPriceModal').modal('hide');
                $('.modal-backdrop').remove();
                $('#lblDealError').css('display', 'none');
                $('#lblDealError').text("");
                if ($("#DealPrice").val() == '') {
                    $('#lblDealPrice').text('0.00');
                }
                else {
                    $('#lblDealPrice').text($("#DealPrice").val());
                }


            }
            else {

                $('#lblDealError').css('display', 'block')
                $('#lblDealError').text(result)
            }


        },
        error: function (errormessage) {
            $('#lblDealError').css('display', 'block')
            $('#lblDealError').text('Sorry, Something went wrong. Please try again.');
            //alert(errormessage.responseText);
        }
    });
}


function DealPrice() {
    $('#DealPrice').val($("#lblDealPrice").text());
}
