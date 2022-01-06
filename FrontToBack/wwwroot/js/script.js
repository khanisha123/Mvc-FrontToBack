
// Section 2 Start
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
