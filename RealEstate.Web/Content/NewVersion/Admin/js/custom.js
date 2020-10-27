$(document).ready(function(){
  $("#toggleHeader").click(function(){
    $("header").toggleClass("toggle");
    $(".wrapper").toggleClass("toggle");
  });

  $( "#datepicker" ).datepicker({
    dayNamesMin: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
    firstDay: 1
  });

});