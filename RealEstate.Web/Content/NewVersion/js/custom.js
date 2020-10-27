$(document).ready(function(){
  $("#toggleHeader").click(function(){
    $("header").toggleClass("toggle");
    $(".wraper").toggleClass("toggle");
  });
});