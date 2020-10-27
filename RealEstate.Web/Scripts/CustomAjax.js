$(function () {
    //**************** JS to show loading progress during ajax call *********************//
    $(document).ajaxStart(function () {
        $("#ajaxLoading").css("display", "block");
        $("#ajaxLoading").css("top", $(window).height() / 1.8);
        $("#ajaxLoading").css("left", $(window).width() / 2);
        $("#ajaxLoading").css("position", "fixed");
        $("#ajax-backdrop").css("display", "block");
    });
    $(document).ajaxComplete(function () {
        $("#ajaxLoading").css("display", "none");
        $("#ajax-backdrop").css("display", "none");
    });
    //************************************************************************************//

    //*************************  Highlight Active Menu  ***********************************//
    jQuery(function () {
        var url = window.location.pathname,
        urlRegExp = new RegExp(url.replace(/\/$/, '') + "$");
        var IsActivated = false;
        //$('#sidebar a').each(function () {
        //    $(this).removeClass("active");
        //    if ($(this).attr('href').toUpperCase() == here.toUpperCase()) {

        //        $('#sidebar').addClass('active')
        //        $('.treeview-menu').addClass('menu-open');
        //        $('.treeview-menu').css('display', 'block');
        //    }
        //});
        $("#sidebar ul li").each(function () {
            $(this).removeClass("active");
            try {
                if ($(this).children().attr("href").toLowerCase() == url.toLowerCase()) {
                    $(this).addClass("active");
                    $(this).closest(".treeview").addClass("active");
                    $(this).closest('.treeview-menu').addClass("menu-open");
                    $(this).closest('.treeview-menu').css('display', 'block');
                    IsActivated = true;
                }
            } catch (e) {

            }
        })
        if (IsActivated == false) {
            $("#sidebar ul li").each(function () {
                $(this).removeClass("active");
                try {
                    if ($(this).children().attr("href").toLowerCase() == $("#ActiveURL").data("value").toLowerCase()) {
                        $(this).addClass("active");
                        $(this).closest(".treeview").addClass("active");
                        $(this).closest('.treeview-menu').addClass("menu-open");
                        $(this).closest('.treeview-menu').css('display', 'block');
                        IsActivated = true;
                    }
                } catch (e) {

                }

            })
        }
        if (IsActivated == false) {
            $("#sidebar a").each(function () {
                $(this).parent().removeClass("active");
                try {
                    if ($(this).attr("href").toLowerCase() == $("#ActiveURL").data("value").toLowerCase()) {
                        $(this).parent().addClass("active");
                        IsActivated = true;
                    }
                } catch (e) {

                }

            })
        }
    });
    //************************************************************************************//

    ShowMessageBox();
    //************************************************************************************//

    //******************************** JS for Grid paging *********************************//
    var getPage = function () {
      
        var $a = $(this);

        if ($a.attr("href").trim() == undefined || $a.attr("href").trim() == "") {
            return;
        }
        var options = {
            url: $a.attr("href")
            , data: $("form").serialize()
            , type: "get"
        }

        $.ajax(options).done(function (data) {
            var $target = $($a.parents("div.ns-grid-pager").attr("data-kw-target"));
            $target.replaceWith(data);
        });
        return false;
    };
    $(".content").on("click", "a.ns-page-link", getPage);

    var getPageForDDL = function () {
        var TargetURL = $(this).parent().attr("data-kw-actionlink");
        TargetURL = TargetURL + "?PageSize=" + $('.page-size').val() + "&PageNo=" + $(this).val()
        var options = {
            url: TargetURL
            , data: $("form").serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-kw-target");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
    };
    $(".content").on("change", ".page-number", getPageForDDL);

    var getPageSizeForDDL = function () {
       
        var TargetURL = $(this).parent().attr("data-kw-actionlink");
        TargetURL = TargetURL + "?PageSize=" + $(this).val()
        var options = {
            url: TargetURL
            , data: $("form").serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-kw-target");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
    };
    $(".content").on("change", ".page-size", getPageSizeForDDL);

    var getSort = function () {

        var $a = $(this);

        if ($a.attr("href").trim() == undefined || $a.attr("href").trim() == "") {
            return;
        }
        var options = {
            url: $a.attr("href")
            , data: $("form").serialize()
            , type: "get"
        }

        $.ajax(options).done(function (data) {
           
            var $target = $($a.attr("data-kw-target"));
            $target.replaceWith(data);
        });
        return false;
    };
    $(".content").on("click", "a.sorting", getSort);



    //************************************************************************************//

    //************** Dropdown Changed event for Cascading Dropdown list ******************//

    //var getDropDownList = function () {

    //    var targeturl = $(this).attr("datakwlink")
    //    targeturl = targeturl +"/"+$(this).val();
    //    var target = $(this).attr("datakwtarget");
    //    var options = {
    //        url: targeturl
    //        , data: $("form").serialize()
    //        , type: "get"
    //        , dataType: 'json'
    //    }

    //    $.ajax(options).done(function (data) {
    //        $(target).empty();
    //        $(target).append('<option value="">--- Select ---</option>');
    //        $.each(data, function (i, item) {
    //            $(target).append('<option value="' + item.Value + '">' + item.Text + '</option>');
              
    //        });
    //    });
    //    return false;
    //};
    //$(".content").on("change", ".dropdown-change", getDropDownList);

    //************************************************************************************//

    $(".content").on("click", ".delete", ShowWarningMessageBox);

    $(".content").on("click", ".resend", ShowEmailMessageBox);


    $(".content").on("click", ".Client", ShowWarningMessageBoxClient);

    $(".content").on("click", ".reopen", ShowWarningMessageBoxReopen);

    var ajaxFormSubmit = function () {
        var $form = $(this);
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $($form.attr("data-kw-target"));
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
        return false;
    };
    $("form[data-kw-ajax='true']").submit(ajaxFormSubmit);


    var xhr = null;
    var ajaxFormSubmit1 = function (e) {
        if ($("#completed5").is(':checked')) {
            if ($("#spnAddress").text() == "") {
                alert("Please add address to complete the Ratified Offer step.");
                return false;
            }
        }
    
       e.preventDefault();
       e.stopPropagation()   //this prevented it from submitting twice i a row
       //if (xhr != null) {
       //    xhr.abort();
       //    xhr = null;
       //}

       var $form = $(this).parent().parent().parent();
       var options = {
           url: $form.attr("action")
           , type:"POST"
           , data: $form.serialize()
           //,async:false
       }

       var target = $($form.attr("data-kw-target"));
       $.ajax(options).done(function (data) {
         
           location.reload(true);
       });
       $.ajax(options).error(function (error) {
       
           //if (error.responseText = "Please add address to complete the Ratified Offer step.") {
           //    alert(error.responseText);
           //}
           //else {
               alert("Sorry, Something went wrong. Please try again.");
          // }
          
       });
      
       return false;
   };

   $(".content").on('click', "#btnZabarAddDeal", ajaxFormSubmit1)


   var ajaxFormSubmit2 = function (e) {
       if ($("#completed6").is(':checked')) {
           if ($("#spnAddress").text() == "") {
               alert("Please add address to complete the House goes live step.");
               return false;
           }
       }

       e.preventDefault();
       e.stopPropagation()   //this prevented it from submitting twice i a row
       //if (xhr != null) {
       //    xhr.abort();
       //    xhr = null;
       //}

       var $form = $(this).parent().parent().parent();
       var options = {
           url: $form.attr("action")
           , type: "POST"
           , data: $form.serialize()
           //,async:false
       }

       var target = $($form.attr("data-kw-target"));
       $.ajax(options).done(function (data) {

           location.reload(true);
       });
       $.ajax(options).error(function (error) {

           //if (error.responseText = "Please add address to complete the Ratified Offer step.") {
           //    alert(error.responseText);
           //}
           //else {
               alert("Sorry, Something went wrong. Please try again.");
          // }

       });

       return false;
   };

   $(".content").on('click', "#btnZabarAddSellerDeal", ajaxFormSubmit2)
  // $("form[data-kw-ajax1='true']").submit(ajaxFormSubmit1);
});

// ************************* This is to show success/error message ********************************//

// Function to call custom Ajax form submit
function CustomAjaxFormSubmit(sender, url) {
    if (url == "#")
    { return false; }
    var $form = $('a[href="' + decodeURI(url) + '"]').closest('form')
    if ($form.attr("data-kw-ajax") == 'true') {
        var options = {
            url: decodeURI(url)
            , type: $form.attr("method")
            , data: $form.serialize()
        }
        var target = $($form.attr("data-kw-target"));
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);

            ShowMessageBox();
        });
        return false;
    }
    else {
        return true;
    }
};

function ShowMessageBox() {

    if ($('#ErrMsgHiddenField').val().length > 1) {
        var msgId = $('#ErrMsgHiddenField').val().substring(0, 4);
        var errMsg = $('#ErrMsgHiddenField').val().substring(4, $('#ErrMsgHiddenField').val().length - 3).toString();
    }
    else {
        var msgId = $('#ErrMsgHiddenField').val();
    }
    if (msgId != null && msgId.toString().trim() != "") {

        if (msgId == "0") {
            $('#myErroModalLabel').text("");
            $('#Msg').text("Info Saved Successfully...");
        }
        else if (msgId == "3") {
            $('#myErroModalLabel').text('Information');
            $('#Msg').text('Session expired, redirecting to login page...');
        }
        else if (msgId == "4") {
            $('#myErroModalLabel').text('Error');
            $('#Msg').text('An error occured while processing your request, please try again...');
        }
        else if (msgId == "5") {
            $('#myErroModalLabel').text('Error');
            $('#Msg').text('Please add address to complete Ratified Offer step..');
        }
        else {
            $('#myErroModalLabel').text('Information');
            $('#Msg').text(errMsg);
        }

        $('#myErroMsgModalYesButton').addClass('hidden');
        $('#myErroMsgModal').show();
    }
}

//// This is to show warning message before delete operation
var ShowWarningMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myModalLabel').text('Information');

        $('#Msg').text('Are you sure you want to delete the record..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}
var ShowWarningMessageBoxClient = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myModalLabel').text('Information');

        $('#Msg').text('Are you sure you want to Mark this user as client..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}




//// This is to show warning message before reopen work operation
var ShowWarningMessageBoxReopen = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myModalLabel').text('Information');

        $('#Msg').text('Are you sure you want to Mark this user as client..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}


var ShowEmailMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myModalLabel').text('Information');

        $('#Msg').text('Do you want to resend the email..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}


// Close message box
function CloseMyModal() {
    $('#myErroMsgModalYesButton').addClass('hidden');
    $('#ErrMsgHiddenField').val("");
    $("#myErroMsgModal").hide();
}
// close message box and procceed for intended action.
function OkMyModal() {
    $("#myErroMsgModal").hide();
    $('#ErrMsgHiddenField').val("");
    // Retrieve the sender infromation from hidden field and pass it to the function
    CustomAjaxFormSubmit($("#eventSender").val().split('|')[1], $("#eventSender").val().split('|')[0]);
}

//**************************** Model Ajax *********************************
$(function () {
    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {
        // hide dropdown if any (this is used wehen invoking modal from link in bootstrap dropdown )
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#AddEditModalContent').load(this.href, function () {
            $('#AddEditModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#AddEditModal').modal('hide');
                    $('#replacetarget').load(result.url); //  Load data from the server and place the returned HTML into the matched element
                } else {
                    $('#AddEditModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });
        return false;
    });
}
