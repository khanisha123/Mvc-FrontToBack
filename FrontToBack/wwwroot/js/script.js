
// Section 2 Start
function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
      tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
      tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
  }
  document.getElementById("defaultOpen").click();
// Section 2 End

// Section 1 Start

  $("#slideHow").vegas({
    slides: [
        { src: "~/img/h3-slider-background.jpg" },
        { src: "~/img/h3-slider-background-2.jpg" },
        { src: "~/img/h3-slider-background-3.jpg" }
    ]
});
// Section 1 End

// Section 7 Start


let rightiicon=document.getElementsByClassName("icon-S7-2");
let leftiicon=document.getElementsByClassName("icon-S7-1")
$('.owl-Slider7').owlCarousel({
  loop:true,
  margin:10,
  nav:true,
  navText:[leftiicon,rightiicon],
  dots:false,
  responsive:{
      0:{
          items:1
      },
      600:{
          items:1
      },
      1000:{
          items:1
      }
  }
})

// Section 7 End
