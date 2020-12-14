$(document).ready(function (){

 //   Home-intro Corusel
    if($("#home-intro").length){
        $('.slider .owl-carousel').owlCarousel({
            margin: 30,
            items: 1,
            dots: false,
            loop: true,
            nav: true,
            mouseDrag: false,
            autoplay:false,
            autoplayTimeout:5000,
            autoplayHoverPause:true,
            transitionStyle: "fade",
    })
    }
 $( ".slider .owl-carousel .owl-prev").html('<i class="fas fa-angle-left"></i>');
 $( ".slider .owl-carousel .owl-next").html('<i class="fas fa-angle-right"></i>');


 //  WowAnimation Plugin
 function wowAnimation() {
    var wow = new WOW({
        boxClass: 'wow',
        animateClass: 'animated',
        offset: 0,
        mobile: false,
        live: true
    });
    wow.init();
 }
 $(window).on('load', function () {
  wowAnimation();
  });

 //   Home-services Corusel
  if($("#services").length){
    $('.services-corusel .owl-carousel').owlCarousel({
        margin: 30,
        items: 4,
        dots: true,
        loop: true,
        nav: false,
        mouseDrag: true,
        navSpeed: 1000,
        transitionStyle: "fade",
        responsive: {
                    0: {
                        items: 1
                    },
                    768: {
                        items: 2
                    },
                    992: {
                        items: 4
                    }
                }
 })
 }
 $( ".services-coruse .owl-carousel .owl-dots .owl-dot").html('<span></span>');

 //   Home-pecialist-doctors Corusel
 if($("#specialist-doctors").length){
  $('.specialist-corusel .owl-carousel').owlCarousel({
      items:  4,
      margin: 30,
      dots: false,
      loop: true,
      nav: false,
      mouseDrag: true,
      navSpeed: 1000,
      transitionStyle: "fade",
      responsive: {
        0: {
            items: 1
        },
        768: {
            items: 2
        },
        992: {
            items: 4
        }
    }
 })
 }

 // Counter Plugin
 if($("#counter").length){
    $(".timer").counterUp({
      delay: 10,
      time: 5000,
    })
  }

 //  Home-Testimonial Corusel
  if($("#testimonial").length){
    $('.testimonial-content .owl-carousel').owlCarousel({
        items: 1,
        margin: 30,
        dots: true,
        loop: true,
        nav: true,
        mouseDrag: true,
        autoplay:false,
        autoplayHoverPause:true,
        transitionStyle: "fade",
 })
 }

 $( ".testimonial-content .owl-carousel .owl-prev").html('<i class="fas fa-angle-left"></i>');
 $( ".testimonial-content .owl-carousel .owl-next").html('<i class="fas fa-angle-right"></i>');
  
 // Home-Brand Corusel 
 if($("#brand").length){
  $('.brand-section .owl-carousel').owlCarousel({
      items: 5,
      margin: 30,
      dots: false,
      loop: true,
      nav: true,
      mouseDrag: true,
      autoplay:false,
      autoplayHoverPause:true,
      transitionStyle: "fade",
      responsive: {
        0: {
            items: 2
        },
        768: {
            items: 3
        },
        992: {
            items: 5
        }
    }
  })
 }
 $( ".brand-section .owl-carousel .owl-prev").html('<i class="fas fa-angle-left"></i>');
 $( ".brand-section .owl-carousel .owl-next").html('<i class="fas fa-angle-right"></i>');
 
 //  Header Scroll start 
  if($("#navbar").length){
   $(window).scroll(function (e) { 
     e.preventDefault();
     let scroll = $(window).scrollTop();
     if(scroll > 100){
       $("#navbar").addClass("show");
     }
     else{
       $("#navbar").removeClass("show");
     }
   });
  }

 // Back-to-top start
 if($(".back-to-top").length){
  $(window).scroll(function (e) { 
    e.preventDefault();
    let scroll = $(window).scrollTop();
    if(scroll > 100){
      $(".back-to-top").addClass("show");
    }
    else{
      $(".back-to-top").removeClass("show");
    }
  });
 }

 //  Doctor-Search Select
 $(".search-select").click(function(e){
   e.preventDefault();
  $(".search-result").slideToggle();
  $(".select-arrow i").toggleClass("fa-angle-up");
 })


//  Appointment Select
  $("#department").click(function(e){
   e.preventDefault();
   $(".department-icon i").toggleClass("fa-angle-up");
 })
 
 $("#doctor").click(function(e){
   e.preventDefault();
   $(".doctor-icon i").toggleClass("fa-angle-up");
 })

  //  Chekout Select 
  $("#country").click(function(e){
    e.preventDefault();
    $(".country-icon i").toggleClass("fa-angle-up");
  })

  $("#city").click(function(e){
    e.preventDefault();
    $(".city-icon i").toggleClass("fa-angle-up");
  })

 })

 // Home-Speciality
 var tablinks = document.querySelectorAll("#speciality .nav-pills .nav-item .nav-link");
 var tabContent = document.querySelectorAll("#speciality .tab-content");

 for(let elem of tablinks){
    elem.addEventListener("click",function(e){
        e.preventDefault();
        
      document.querySelector("#speciality .nav-pills .nav-item .nav-link.active").classList.remove("active");
      document.querySelector(".tab-content.show").classList.remove("show");

      elem.classList.add("active");

      var count = elem.getAttribute("data-index");
      var panel = [...tabContent].filter(elem => elem.getAttribute("data-index") == count);

      panel[0].classList.add("show");
      panel[0].classList.remove("d-none");

    
    for(let item of tabContent){
      if(panel[0] !== item){
        item.classList.add("d-none");
      }
    }
      
    })
}

// About-Choose-us
var title = document.querySelectorAll(".choose-acordin .choose-acordin-item");

for(let item of title){
  item.addEventListener("click",function(e){
    e.preventDefault();
    
    var body = document.querySelectorAll(".choose-acordin .choose-acordin-item .tab-content");
    
    let array = $(this).siblings();
   
    for(let arrayitem of array){
      if(!arrayitem.lastElementChild.classList.contains("show")){
        arrayitem.firstElementChild.style.backgroundColor = "#f1f7fa";
        arrayitem.firstElementChild.lastElementChild.style.color = "#111111";
        arrayitem.firstElementChild.firstElementChild.firstElementChild.style.display = "block";
        arrayitem.firstElementChild.firstElementChild.lastElementChild.style.display = "none";
      }
    }
    
    for(let classValue of item.lastElementChild.classList){
      if(classValue !== "show"){
        item.lastElementChild.classList.toggle("show");
      }
  }
   for(let cardCollaps of body){
    if(cardCollaps !== item.lastElementChild){
      cardCollaps.classList.add("show");
    }
  }

  

  if(item.lastElementChild.classList.contains("show")){
    item.firstElementChild.style.backgroundColor = "#f1f7fa";
    item.firstElementChild.lastElementChild.style.color = "#111111";
    item.firstElementChild.firstElementChild.firstElementChild.style.display = "block";
    item.firstElementChild.firstElementChild.lastElementChild.style.display = "none";
}
else{
    item.firstElementChild.style.backgroundColor = "#396CF0";
    item.firstElementChild.lastElementChild.style.color = "#fff";
    item.firstElementChild.firstElementChild.firstElementChild.style.display = "none";
    item.firstElementChild.firstElementChild.lastElementChild.style.display = "block";
}
  
})
}

// Shop And Add to cart

//var addToBasket = document.querySelectorAll(".medicine-item .team-box .item-icon .add-to-basket");
//var cartCount = document.querySelector(".header-icon-area .search-box-area .search-box .icon-counter .cart-count");
//var cartItems = document.querySelector(".cart-items");

//addToBasket.forEach(elem =>{
//  elem.addEventListener("click",function(e){
//    e.preventDefault();
    
//    var contentName = this.parentNode.parentNode.parentNode.lastElementChild.firstElementChild.innerText;
//    var contentImage = this.parentNode.parentNode.firstElementChild.getAttribute("src");
//    var contentPrice = this.parentNode.parentNode.parentNode.lastElementChild.lastElementChild.firstElementChild.innerText;

//    let basket = JSON.parse(localStorage.getItem("basket"));
   
//    if(basket == null){
//      basket ={
//        items: [],
//        count: 0,
//        total: 0,
//      };
//    }
//    let productId =this.dataset.id;

//    let index = basket.items.findIndex((item)=>{
//         return item.id == productId;
//    })

//    if(index == -1){
      
//      let price = {
//        id: this.dataset.id,
//        name: contentName,
//        img: contentImage,
//        price: parseFloat(contentPrice),
//        qty: 1,
//      };
//      basket.items.push(price);
//    }else{
//      basket.items[index].qty ++;
//    }
    
//    basket = calcTotalAndCount(basket);

//    if(cartCount != null){
//      cartCount.innerText = basket.count;
//    }
    
//    localStorage.setItem("basket",JSON.stringify(basket));
//  })
// })

// let basket = JSON.parse(localStorage.getItem("basket"));
   
//    if(basket != null && cartCount != null){
      
//      cartCount.innerText = basket.count;
//    }

//    if(basket != null && cartItems != null){
//      basket.items.forEach((item,index) =>{
       
//        let td = `<td class="product-remove">
//            <a class="remove-from-basket" data-id="${item.id}" href="#">x</a>
//        </td>
//        <td class="product-thumbnail">
//            <a href="#">
//                <img width="80" height="80" src="${item.img}" alt="">
//            </a>
//        </td>
//        <td class="product-name">
//            <a href="#">${item.name}</a>
//        </td>
//        <td class="product-price">
//            <span>${item.price} ₼</span>
//        </td>
//        <td class="product-quantity">
//            <input type="number" class="form-control" id="exampleInputNumber" value="${item.qty}">
//        </td>
//        <td class="product-subtotal">
//            <span>${item.price*item.qty} ₼</span>
//        </td>`
 
//     let tr = document.createElement("tr");
//     tr.dataset.index = index;
//     tr.innerHTML = td;
//     cartItems.append(tr);

//     });

//    }
   
//    var removeFromBasket = document.querySelectorAll(".remove-from-basket");
//    removeFromBasket.forEach(item =>{
//      item.addEventListener("click",function(e){
//        e.preventDefault();
//        let index = item.parentNode.parentNode.dataset.index;
//        basket.items.splice(index,1);
//        item.parentNode.parentNode.remove();
//        basket = calcTotalAndCount(basket);
//        localStorage.setItem("basket",JSON.stringify(basket));
//      })
//    })

// function calcTotalAndCount(basket){
//  basket.count = 0;
//  basket.total = 0;
//  basket.items.forEach(item =>{
//    basket.total += (item.qty * item.price);
//    basket.count ++;
//  })

//  return basket;
//}

