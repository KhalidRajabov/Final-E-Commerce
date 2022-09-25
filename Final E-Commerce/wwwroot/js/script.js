(function ($) {
  //'use strict';



    //add product to basket

    let addBtn = document.querySelectorAll(".add")
    let bTotal = document.getElementById("basketTotal")
    let tPrice = document.getElementById("basketPrice")
    let cartlist = $("#cartlist")
    addBtn.forEach(add =>

        add.addEventListener("click", function () {
            let dataId = this.getAttribute("data-id")
            //alert("clicked")
            let quantity = $("#quantity").val()
            axios.post("/basket/additem?id=" + dataId+"&quantity="+quantity)
                .then(function (response) {
                    // handle success
                    //console.log(response.data.count)
                    if (response.data.online) {
                        if (response.data.productcount==1) {
                        bTotal.innerHTML = response.data.count
                        tPrice.innerHTML = ` $${response.data.price}`
                            let cartitem = `<li id="cart-item${dataId}"  class="d-flex border-bottom">
                                            <img width="70" src="https://localhost:44393/images/products/${response.data.image}" alt="product-img">
                                            <div class="mx-3">
                                                <h6>${response.data.name}</h6>
                                                <span id="oneproductCount${response.data.id}">${response.data.productcount} X</span> 
                                                <span>$${response.data.itemprice}</span>
                                            </div>
                                        </li>`
                            cartlist.append(cartitem)
                            Swal.fire({
                                timer: 1000,
                                timerProgressBar: true,
                                title: 'Added to basket!',
                                text: `${response.data.name}`,
                                imageUrl: `https://localhost:44393/images/products/${response.data.image}`,
                                imageWidth: 400,
                                imageHeight: 200,
                                imageAlt: 'Custom image',
                            })
                        }
                        else if (response.data.productcount > 1) {
                            bTotal.innerHTML = response.data.count
                            tPrice.innerHTML = ` $${response.data.price}`
                            $(`#oneproductCount${response.data.id}`).html(`${response.data.productcount} X`)
                        }
                    }
                    else {
                        window.location.href = "account/login"
                    }
                    //console.log(response);
                })
                .catch(function (error) {
                    // handle error
                    //console.log("error "+error);
                })
        })
    )

    //plus item in basket

    let plusBtn = document.querySelectorAll(".plusitem")
    plusBtn.forEach(add =>

        add.addEventListener("click", function () {

            let dataId = this.getAttribute("data-id")
            let span = this.previousElementSibling;
            let tabletotalprice = this.parentElement.parentElement.parentElement.nextElementSibling;
            axios.post("/basket/plus?id=" + dataId)
                .then(function (response) {

                    // handle success
                    bTotal.innerText = response.data.count
                    tPrice.innerText = "$"+response.data.price
                    span.innerText = response.data.main
                    tabletotalprice.innerText = '$' + response.data.itemTotal
                    $(`#oneproductCount${dataId}`).html(`${response.data.productcount}`)
                    //console.log(response.data.main)
                })
                .catch(function (error) {
                    console.log(error);
                })
        })
    )


    //minus item in basket


    let minusBtn = document.querySelectorAll(".minusitem")
    minusBtn.forEach(add =>
        add.addEventListener("click", function () {

            let dataId = this.getAttribute("data-id")
            let span = this.nextElementSibling
            let tr = span.parentElement.parentElement.parentElement.parentElement;
            let table = tr.parentElement.parentElement;
            let cart_list_item = $(`#cart-item${dataId}`)
            let checkoutBtn = table.nextElementSibling;
            let tabletotalprice = this.parentElement.parentElement.parentElement.nextElementSibling;
            let cntnr = $("#basketcontainer")
            let emptywarning = document.createElement("div")
            emptywarning.classList.add("container", "d-flex", "flex-row", "justify-content-center")
            let emptywarninglink = document.createElement("a")
            emptywarninglink.classList.add("text-light", "btn", "btn-danger")
            emptywarninglink.setAttribute("href", "home/index")
            emptywarninglink.innerText = "Your cart is empty"
            emptywarning.append(emptywarninglink)
            axios.post("/basket/minus?id=" + dataId)
                .then(function (response) {
                    //console.log(response.data.count)

                    if (response.data.productcount ==0) {
                        //console.log("data zero")
                        bTotal.innerText = response.data.main
                        tPrice.innerText = response.data.price
                        tr.remove();
                        cart_list_item.remove()
                        if (response.data.main == 0) {
                            table.remove();
                            checkoutBtn.remove();
                            cntnr.append(emptywarning)
                           // cntnr.html(`<div class="container d-flex flex-row justify-content-center">< a class="text-light btn btn-danger" asp-controller="home" asp-action="index" > Your cart is empty</a ></div >`);
                        }
                    }
                    else {
                        bTotal.innerText = response.data.main
                        tPrice.innerText = "$"+response.data.price 
                        span.innerText = response.data.count
                        tabletotalprice.innerText = response.data.itemTotal;
                        //console.log("count " + response.data.count);
                        $(`#oneproductCount${dataId}`).html(`${response.data.count} X`)
                        //console.log("productcount " + response.data.productcount);
                        //console.log(itemcount)
                        
                    }
                    //console.log(response);
                })
                .catch(function (error) {
                    // handle error

                    //tr.remove();


                    console.log(error.message);
                })
        })
    )


    //delete item in basket


    let delBtn = document.querySelectorAll(".deleteitem")
    delBtn.forEach(add =>

        add.addEventListener("click", function () {
            
            let dataId = this.getAttribute(`data-id`)
            let tr = this.parentElement.parentElement.parentElement;
            let table = tr.parentElement.parentElement;
            let checkoutBtn = table.nextElementSibling;
            let cart_list_item = $(`#cart-item${dataId}`)
            let cntnr = $("#basketcontainer")
            let emptywarning = document.createElement("div")
            emptywarning.classList.add("container", "d-flex", "flex-row", "justify-content-center")
            let emptywarninglink = document.createElement("a")
            emptywarninglink.classList.add("text-light", "btn", "btn-danger")
            emptywarninglink.setAttribute("href", "home/index")
            emptywarninglink.innerText = "Your cart is empty"
            emptywarning.append(emptywarninglink)
            //console.log(dataId)
            Swal.fire({
           
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: 'rgb(25,135,84)',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {

                    axios.post("/basket/RemoveItem?id=" + dataId)
                        .then(function (response) {


                            bTotal.innerText = response.data.count;
                            tPrice.innerText = response.data.price;
                            tr.remove();
                            cart_list_item.remove()
                            if (response.data.count == 0) {
                                table.remove();
                                checkoutBtn.remove();
                                cntnr.append(emptywarning)
                                //window.location.reload();
                                
                            }
                        })
                        .catch(function (error) {

                            console.log(error);
                        })

                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Product has been deleted from basket',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
            })
           
        })
    )


    //delete from basket cart

    /*let deleteBtn = document.querySelectorAll(".deletefromcart")
    deleteBtn.forEach(add =>

        add.addEventListener("click", function () {

            let dataId = this.getAttribute(`data-id`)
            let basketcart = this.parentElement;
            
            console.log(dataId)
            axios.post("/basket/RemoveItem?id=" + dataId)
                .then(function (response) {


                    bTotal.innerText = response.data.count;
                    tPrice.innerText = response.data.price;
                    basketcart.remove();
                })
                .catch(function (error) {
                    console.log(error);
                })
        })
    )*/

    //add product to wishlist
    let wishlistAddBtn = document.querySelectorAll(".add-to-wishlist")
    wishlistAddBtn.forEach(add =>

        add.addEventListener("click", function () {
            let dataId = this.getAttribute("data-id")
            axios.post("/wishlist/add?id=" + dataId)
                .then(function (response) {
                    if (response.data.online) {
                        add.remove();
                    }
                    else {
                        Swal.fire({
                            title: 'You need to login to add to wishlist',
                            showDenyButton: false,
                            showCancelButton: true,
                            confirmButtonText: 'Login',
                            denyButtonText: `Don't save`,
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            if (result.isConfirmed) {
                                location.replace("https://localhost:44393/account/login")
                            }
                        })
                    }
                    //console.log(response);
                })
                .catch(function (error) {
                    // handle error
                    console.log("error " + error);
                })
        })
    )



    //remove from wishlist
    let wishlistRmvBtn = document.querySelectorAll(".remove-from-wishlist")
    wishlistRmvBtn.forEach(add =>

        add.addEventListener("click", function () {
            let wishlistItemCard = this.parentElement.parentElement.parentElement.parentElement;
            let dataId = this.getAttribute("data-id")
            axios.post("/wishlist/remove?id=" + dataId)
                .then(function (response) {
                    // handle success
                        wishlistItemCard.remove();
                    //console.log(response);
                })
                .catch(function (error) {
                    // handle error
                    console.log("error " + error);
                })
        })
    )



    //add product to wishlist from cards
    let cardHeartAddBtn = document.querySelectorAll(".add-to-wishlist-from-card")
    cardHeartAddBtn.forEach(add =>

        add.addEventListener("click", function () {
            let dataId = this.getAttribute("data-id")
            let icon = document.createElement("i")
                axios.post("/wishlist/add?id=" + dataId)
                    .then(function (response) {
                        add.innerHTML = "";

                        icon.classList.add("fa-heart");
                        icon.classList.add("fa-solid");
                        icon.classList.add("text-danger");
                        //console.log(icon)
                        add.append(icon);

                    })
                    .catch(function (error) {
                        // handle error
                        console.log("error " + error);
                    })
            }
        )
    )



    //cart-modal
    let modalBtn = document.querySelectorAll(".modal-open-btn")
    modalBtn.forEach(add =>

        add.addEventListener("click", function () {
            let modalImage = $("#modal-image")
            let dataId = this.getAttribute(`data-id`)
            let modalAdd = $("#modal-add-btn")
            let productName = $("#modal-product-name")
            let productPrice = $("#modal-product-price")
            let productDesc = $("#modal-product-desc")

           // console.log(dataId, modalAdd, productName, productPrice, productDesc)
            //console.log(dataId)
            axios.post("/search/GetProductForModal?id=" + dataId)
                .then(function (response) {
                    modalAdd.attr(`href`, `home/detail/${dataId}`);
                    productName.text(response.data.name)
                    productPrice.text(`$`+response.data.price) 
                    productDesc.text(response.data.description)
                    modalImage.attr(`src`, `https://localhost:44393/images/products/${response.data.image}`)
                    modalImage.attr(`width`, `100%`)
                    
                })
                .catch(function (error) {
                    console.log(error);
                })
        })
    )








  //search


    $(document).on("keyup", "#search", function () {
        let inputValue = $(this).val();
        $("#SearchList li").slice(1).remove();
        $("#SearchList").html()
        $.ajax({
            url: "https://localhost:44393/search/searchProduct?search=" + inputValue,
            method: "get",
            success: function (res) {
                $("#SearchList").append(res);
                //console.log("successfully brought searched objects")
            }
        })
    });
    $(document).on("click", "#search", function () {
        let inputValue = $(this).val();

        if (inputValue.length==0) {
            $("#SearchList li").slice(1).remove();
            $("#SearchList").html()
            $.ajax({
                url: "https://localhost:44393/search/PopularProducts/",
                method: "get",
                success: function (res) {
                    $("#SearchList").append(res);
                    //console.log("popular products successfully brought for onlcick on search input")
                },
                error: function (res) {
                    console.log("error ", res.responseText)
                }
            })
        }

    });









  
  // Background-images
  $('[data-background]').each(function () {
    $(this).css({
      'background-image': 'url(' + $(this).data('background') + ')'
    });
  });

  //  Search Form Open
  $('#searchOpen').on('click', function () {
    $('.search-wrapper').toggleClass('open');
    $('.search-btn').toggleClass('search-close');
  });

  //  Cart Open
  $('#cartOpen').on('click', function () {
    $('.cart-wrapper').addClass('open');
  });
  $('#cartClose').on('click', function () {
    $('.cart-wrapper').removeClass('open');
  });

  //Hero Slider
  $('.hero-slider').slick({
    autoplay: true,
    autoplaySpeed: 7500,
    lazyLoad: 'progressive',
    speed: 100,
    pauseOnFocus: false,
    pauseOnHover: false,
    infinite: true,
    arrows: true,
    prevArrow: '<button type=\'button\' class=\'prevArrow\'></button>',
    nextArrow: '<button type=\'button\' class=\'nextArrow\'></button>',
    dots: false,
    responsive: [{
      breakpoint: 576,
      settings: {
        arrows: false
      }
    }]
  });
  $('.hero-slider').slickAnimation();

  // collection slider
  $('.collection-slider').slick({
    dots: true,
    speed: 300,
    autoplay: true,
    autoplaySpeed: 5000,
    arrows: false,
    slidesToShow: 4,
    slidesToScroll: 4,
    responsive: [{
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3
        }
      },
      {
        breakpoint: 768,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
    ]
  });

  //  collection item quick view
  $('.venobox').venobox({
    framewidth: '80%',
    frameheight: '100%'
  });

  // deal timer
  var dealYear = $('#simple-timer').attr('data-year');
  var dealMonth = $('#simple-timer').attr('data-month');
  var dealDay = $('#simple-timer').attr('data-day');
  var dealHour = $('#simple-timer').attr('data-hour');
  $('#simple-timer').syotimer({
    year: dealYear,
    month: dealMonth,
    day: dealDay,
    hour: dealHour,
    minute: 0
  });


  // sale timer
  var saleYear = $('#sale-timer').attr('data-year');
  var saleMonth = $('#sale-timer').attr('data-month');
  var saleDay = $('#sale-timer').attr('data-day');
  var saleHour = $('#sale-timer').attr('data-hour');
  var saleMinute = $('#sale-timer').attr('data-minute');

  $('#sale-timer').syotimer({
    year: saleYear,
    month: saleMonth,
    day: saleDay,
    hour: saleHour,
    minute: saleMinute
  });

  // Count Down JS
  $('#comingSoon').syotimer({
    year: 2025,
    month: 5,
    day: 9,
    hour: 20,
    minute: 30
  });

  // instafeed
  if (($('#instafeed').length) !== 0) {
    var userId = $('#instafeed').attr('data-userId');
    var accessToken = $('#instafeed').attr('data-accessToken');
    var userFeed = new Instafeed({
      get: 'user',
      userId: userId,
      resolution: 'low_resolution',
      accessToken: accessToken,
      limit: 6,
      template: '<div class="col-lg-2 col-md-3 col-sm-4 col-6 px-0 mb-4"><div class="instagram-post mx-2"><img class="img-fluid w-100" src="{{image}}" alt="instagram-image"><ul class="list-inline text-center"><li class="list-inline-item"><a href="{{link}}" target="_blank" class="text-white"><i class="ti-heart mr-2"></i>{{likes}}</a></li><li class="list-inline-item"><a href="{{link}}" target="_blank" class="text-white"><i class="ti-comments mr-2"></i>{{comments}}</a></li></ul></div></div>'
    });
    userFeed.run();
  }

  // product Slider
  $('.product-slider').slick({
    autoplay: false,
    infinite: true,
    arrows: true,
    prevArrow: '<button type=\'button\' class=\'prevArrow\'><i class=\'ti-arrow-left\'></i></button>',
    nextArrow: '<button type=\'button\' class=\'nextArrow\'><i class=\'ti-arrow-right\'></i></button>',
    dots: true,
    customPaging: function (slider, i) {
      var image = $(slider.$slides[i]).data('image');
      return '<img class="d-none img-fluid" src="' + image + '" alt="product-img">';
    }
  });

  // image zoom
  $('.image-zoom')
    .wrap('<span></span>')
    .css('display', 'block')
    .parent()
    .zoom({
      on: 'click',
      url: $(this).find('img').attr('data-zoom')
    });

  // touchspin
  $('input[name=\'quantity\']').TouchSpin({
    verticalbuttons: true,
    initval: 1,
    verticalupclass: 'angle-up',
    verticaldownclass: 'angle-down'
  });
  $('input[name=\'cart-quantity\']').TouchSpin({
    initval: 40
  });

 
  // checked
  $('.label').click(function () {
    $(this).find('.size-checkbox').toggleClass('checked');
  });

  // bootstrap slider range
  $('.range-track').slider({});
  $('.range-track').on('slide', function (slideEvt) {
    $('.value').text('$' + slideEvt.value[0] + ' - ' + '$' + slideEvt.value[1]);
  });

  // tooltip
  $(function () {
    $('[data-toggle="tooltip"]').tooltip();
  });

  // sticky-menu
  var navbar = $('#navbar');
  var mainWrapper = $('.main-wrapper');
  var sticky = navbar.offset().top;
  $(window).scroll(function () {
    if ($(document).scrollTop() >= sticky) {
      navbar.addClass('sticky');
      mainWrapper.addClass('main-wrapper-section');
    } else {
      navbar.removeClass('sticky');
      mainWrapper.removeClass('main-wrapper-section');
    }
  });


})(jQuery);