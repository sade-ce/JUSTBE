;(function($){

	'use strict';

	document.addEventListener("touchstart", function() {},false);

	if ('ontouchstart' in document.documentElement) {
		$('body').css('cursor', 'pointer');
	}

	$(function(){

	    /* ---------------------------------------------------- */
		/*	Revolution slider									*/
		/* ---------------------------------------------------- */

	    if ($('#rev-slider').length) {

			jQuery("#rev-slider").revolution({
	            sliderType:"standard",
		    	spinner: "spinner3",
		    	responsiveLevels: [4096,1024,778,480],
		    	delay:6000,
		    	sliderLayout:"fullscreen",
		    	stopLoop: 'on',
			    stopAfterLoops: 0,
			    stopAtSlide: 1,
	            navigation: {
	                arrows:{
	                	enable:true,
	                	left: {
							container:"slider",
				            h_align:"right",
				            v_align:"center",
				            h_offset:30,
				            v_offset:-95
						},
						right: {
				            container:"slider",
				            h_align:"right",
				            v_align:"center",
				            h_offset:30,
				            v_offset:100
						}
	                },
	                mouseScrollNavigation:"on",
					mouseScrollReverse:"default",
					onHoverStop:"off",
					touch:{
						touchenabled:"on",
						swipe_threshold: 75,
						swipe_min_touches: 50,
						swipe_direction: "vertical",
						drag_block_vertical: false
					},
	                bullets:{
			        	style:"",
			        	enable: true,
			        	container: "slider",
			        	hide_onmobile: false,
			        	hide_onleave: false,
			        	hide_delay: 200,
			        	hide_under: 0,
			        	hide_over: 9999,
			        	tmp:'<span class="circle-bullet"></span>', 
			        	direction:"vertical",
			        	space: 20,       
			        	h_align: "right",
			        	v_align: "center",
			        	h_offset: 40
			        }
	            }

	        });

	        $('#rev-slider').on('mousewheel click touchmove', function(event) {
			    $('#wheel').fadeOut('slow');
			});

	    }

	    /* ---------------------------------------------------- */
		/*	Background size screen								*/
		/* ---------------------------------------------------- */

	    if ($('.media-holder').length) {

	    	$(window).on('load resize',function(){
		        $('.media-holder').css('height', window.innerHeight+'px');
		    });

	    }

	    /* ---------------------------------------------------- */
		/*	Table fixed header									*/
		/* ---------------------------------------------------- */

	    if ($('.aa-table').length) {

	    	$(".aa-table").stickyTableHeaders();

	    }

	    /* ---------------------------------------------------- */
		/*	Animate a regular anchor navigation					*/
		/* ---------------------------------------------------- */

	    if ($('a.animated').length) {

	    	$(document).ready(function() {

	    		$('body').localScroll({
		           hash: true,
		           filter: '.animated',
		           onBefore: function(){
	                 this.offset = -100;
	               }
		        });

		        // highlight menu element on scroll
		        calculateScroll();
		        $(window).scroll(function(event) {
		            calculateScroll();
		        });

		        /*  Automatically Highlights Navigation Item */
		        var rangeTop        = 300; //rangeTop is used for changing the class a little sooner that reaching the top of the page
		        var rangeBottom     = 300; //rangeBottom is similar but used for when scrolling bottom to top
		        function calculateScroll() {
		            var winTop = $(window).scrollTop();
		            var contentTop      = [];
		            var contentBottom   = [];
		            $('#menu').find('a.animated').each(function(){
		                contentTop.push( $( $(this).attr('href') ).offset().top );
		                contentBottom.push( $( $(this).attr('href') ).offset().top + $( $(this).attr('href') ).height() );
		            })

		            $.each( contentTop, function(i){
		                if ( winTop > contentTop[i] - rangeTop && winTop < contentBottom[i] - rangeBottom ){
		                    $('#menu > li').removeClass('current').eq(i).addClass('current');
		                }
		            })
		        }

		        $('<button class="page-nav-btn" id="page-nav-btn"></button>').appendTo('.page-nav-menu');

		        $("#page-nav-btn").on('click',function() {
		            $('.page-nav-menu').toggleClass('open-page-nav');
		            return false;
		        });

	    	});

	    }

	    /* ---------------------------------------------------- */
		/*	Scrool event										*/
		/* ---------------------------------------------------- */

		$(window).scroll(function() {    
		    var scroll = $(window).scrollTop();

		    if (scroll >= 500) {
		        $(".page-nav-menu").addClass("menu-style-2");
		    } else {
		        $(".page-nav-menu").removeClass("menu-style-2");
		    }
		});

		/* ---------------------------------------------------- */
        /*	Sticky menu											*/
        /* ---------------------------------------------------- */

		$('body').Temp({
			sticky: true
		});

	    /* ---------------------------------------------------- */
		/*	Table toggle row									*/
		/* ---------------------------------------------------- */

	    $('.toggle-row-header').on('click',function() {
		    var unit_type_id = $(this).attr('data-unit-type-id');
		    if ($(this).hasClass('active')) { $(this).removeClass('active');
		        $('.toggle-row-' + unit_type_id).find('td').slideUp('fast'); } else { $(this).addClass('active');
		        $('.toggle-row-' + unit_type_id).find('td').slideDown('fast'); }
		    return false;
		});

	    /* ---------------------------------------------------- */
        /*	Isotope												*/
        /* ---------------------------------------------------- */

		$( window ).on('load', function() {

		  	var $container = $('.isotope');
		    // filter buttons
		    $('#filters button').on('click', function(){
		    	var $this = $(this);
		        // don't proceed if already selected
		        if ( !$this.hasClass('is-checked') ) {
		          $this.parents('#options').find('.is-checked').removeClass('is-checked');
		          $this.addClass('is-checked');
		        }
				var selector = $this.attr('data-filter');
				$container.isotope({  itemSelector: '.item', filter: selector });
				return false;
		    });

		    $.mad_core.isotope();
		     
		});

		/* ---------------------------------------------------- */
		/*	Menu switcher									    */
		/* ---------------------------------------------------- */

		$(window).on("load",function(){

			$(".navbar-toggle").on('click',function() {
	            $('#navbar-menu').toggleClass('open-navbar');
	            return false;
	        });

	        var $win = $('.wrapper-container'); // or $box parent container
			var $box = $("#navbar-menu");
			
		 	$win.on("click.Bst", function(event){		
				if ( 
	            $box.has(event.target).length === 0 //checks if descendants of $box was clicked
	            &&
	            !$box.is(event.target) //checks if the $box itself was clicked
		        ){
					$('#navbar-menu').removeClass('open-navbar');
				}
			});

			$("#navbar-close").on('click',function() {
	            $('#navbar-menu').removeClass('open-navbar');
	            return false;
	        });

	        $("li.sub-menu > a").on('click',function() {
	            $(this).next('ul').slideToggle(400);
	            return false;
	        });
			  
		});

		/* ---------------------------------------------------- */
        /*	Gallery carousel									*/
        /* ---------------------------------------------------- */

	  	var pageCarousel = $('.owl-carousel');

		if(pageCarousel.length){

			$('.owl-carousel').not('.custom-slider').each(function(){
	
				/* Max items counting */
				var max_items = $(this).data('max-items');
				var tablet_items = max_items;
				if(max_items > 1){
					tablet_items = max_items - 1;
				}
				var mobile_items = 1;

				var autoplay_carousel = $(this).data('autoplay');

				var center_carousel = $(this).data('center');

				var item_margin = $(this).data('item-margin');
				
				/* Install Owl Carousel */
				$(this).owlCarousel({
				    smartSpeed : 450,
				    nav : true,
				    loop  : true,
				    autoplay : autoplay_carousel,
				    center: center_carousel,
				    autoplayTimeout: 3000,
				    navText : false,
				    margin: item_margin,
				    lazyLoad: true,
				    rtl: $.mad_core.SUPPORT.ISRTL ? true : false,
				    responsiveClass:true,
				    responsive : {
				        0:{
				            items:mobile_items
				        },
				        481:{
				            items:tablet_items
				        },
				        992:{
				            items:max_items
				        }
				    }
				});

			});

			if($('.custom-slider').length){

				$('body').on('click.customFancyBox', '.project-link', function(e){

			        var $this = $(this), $siblings, dataValue, $gallery,
			        href = $this.attr('href');

					if($this.data('custom-fb-gallery') ) {
						$siblings = $();
						dataValue = $this.data('custom-fb-gallery');
						$gallery = $('[data-custom-fb-gallery="' + dataValue + '"]');

				        $gallery.each(function(i, item){
							var $item = $(item);
							if( $item.closest('.owl-item.cloned').length ) return;
							$siblings = $siblings.add( $item );
				       	});
			        } else {

			            $siblings = $this.closest('.owl-item');

			            if( !$siblings.length ) return;

			         	$siblings = $siblings.siblings('.owl-item').not('.cloned');
				        if( !$siblings.length ) return;

				        $siblings = $siblings.find( '.project-link' );
				        $siblings = $siblings.add( $this );

				    }

			        $.fancybox.open( $siblings, {
			        // Custom options
			        }, $siblings.index( $siblings.filter('[href="' +href+ '"]').get(0) ) );

			        e.preventDefault();
			     
			   	})

				$('.owl-carousel.custom-slider').each(function(){

					var delay = Math.floor(Math.random() * 2000) + 3000;

					/* Install Owl Carousel */
					$(this).owlCarousel({
					    smartSpeed : 450,
					    nav : false,
					    loop  : true,
					    autoplay : true,
					    items: 1,
					    autoplayHoverPause: true,
					    animateOut: 'fadeOut',
					    dots: false,
					    autoplayTimeout: delay,
					    navText : false
					});

				});


			}

		}

		/* ---------------------------------------------------- */
        /*	SmoothScroll										*/
        /* ---------------------------------------------------- */

		try {
			$.browserSelector();
			var $html = $('html');
			if ( $html.hasClass('chrome') || $html.hasClass('ie11') || $html.hasClass('ie10') ) {
				$.smoothScroll();
			}
		} catch(err) {}

		/* ---------------------------------------------------- */
        /*	Custom Select										*/
        /* ---------------------------------------------------- */

		if ($('.custom-select').length) {
			$('.custom-select').madCustomSelect();
		}

		/* ---------------------------------------------------- */
        /*	Tabs												*/
        /* ---------------------------------------------------- */

        $(window).on("load",function(){

        	var tabs = $('.tabs-section');
			if(tabs.length){
				tabs.tabs({
					beforeActivate: function(event, ui) {
				        var hash = ui.newTab.children("li a").attr("href");
				   	},
					hide : {
						effect : "fadeOut",
						duration : 450
					},
					show : {
						effect : "fadeIn",
						duration : 450
					},
					updateHash : false
				});
			}

        });	

		/* ---------------------------------------------------- */
        /*	Accordion & Toggle									*/
        /* ---------------------------------------------------- */

		var aItem = $('.accordion:not(.toggle) .accordion-item'),
			link = aItem.find('.a-title'),
			$label = aItem.find('label'),
			aToggleItem = $('.accordion.toggle .accordion-item'),
			tLink = aToggleItem.find('.a-title');

			aItem.add(aToggleItem).children('.a-title').not('.active').next().hide();

		function triggerAccordeon($item) {
			$item
			.addClass('active')
			.next().stop().slideDown()
			.parent().siblings()
			.children('.a-title')
			.removeClass('active')
			.next().stop().slideUp();
		}


		if ($label.length) {
			$label.on('click',function(){
				triggerAccordeon($(this).closest('.a-title'))
			});
		} else {
			link.on('click',function(){
				triggerAccordeon($(this))
			});
		}

		tLink.on('click',function(){
			$(this).toggleClass('active')
			.next().stop().slideToggle();

		});

		/* ---------------------------------------------------- */
        /*	Contact Form										*/
        /* ---------------------------------------------------- */

		//if ($('#contact-form').length){

		//	var cf = $('#contact-form');
		//	cf.append('<div class="message-container"></div>');

		//	cf.on("submit",function(event){

		//		var self = $(this),text;

		//		var request = $.ajax({
		//			url:"bat/mail.php",
		//			type : "post",
		//			data : self.serialize()
		//		});

		//		request.then(function(data){
		//			if(data == "1"){

		//				text = "Your message has been sent successfully!";

		//				cf.find('input:not([type="submit"]),textarea').val('');

		//				$('.message-container').html('<div class="alert-box success"><i class="icon-smile"></i><p>'+text+'</p></div>')
		//					.delay(150)
		//					.slideDown(300)
		//					.delay(4000)
		//					.slideUp(300,function(){
		//						$(this).html("");
		//					});

		//			}
		//			else{
		//				if(cf.find('textarea').val().length < 20){
		//					text = "Message must contain at least 20 characters!"
		//				}
		//				if(cf.find('input').val() == ""){
		//					text = "All required fields must be filled!";
		//				}
		//				$('.message-container').html('<div class="alert-box error"><i class="icon-warning"></i><p>'+text+'</p></div>')
		//					.delay(150)
		//					.slideDown(300)
		//					.delay(4000)
		//					.slideUp(300,function(){
		//						$(this).html("");
		//					});
		//			}
		//		},function(){
		//			$('.message-container').html('<div class="alert-box error"><i class="icon-warning"></i><p>Connection to server failed!</p></div>')
		//					.delay(150)
		//					.slideDown(300)
		//					.delay(4000)
		//					.slideUp(300,function(){
		//						$(this).html("");
		//					});
		//		});

		//		event.preventDefault();

		//	});

		//}

		/* ---------------------------------------------------- */
		/*	Google Maps											*/
		/* ---------------------------------------------------- */

		if ($('#googleMap').length) {

			$(document).ready(function() {

			    var myCenter = new google.maps.LatLng(38.8851576, -76.9965074);

				function loadMap() {
				  	var mapProp = {
					    center: myCenter,
					    zoom:14,
					    mapTypeId:google.maps.MapTypeId.ROADMAP,
					    styles : [{featureType:'all',stylers:[{saturation:-100},{gamma:0.0}]}]
					};

					var map = document.getElementById('googleMap');

					if(map !== null){

				    	var map = new google.maps.Map(document.getElementById("googleMap"),mapProp);

					}

		            var marker = new google.maps.Marker({
		               position:myCenter,
		               map: map,
		               icon: 'images/map_marker.png'
		            });
		            
		            marker.setMap(map);

		            //Zoom to 7 when clicked on marker
		            google.maps.event.addListener(marker,'click',function() {
		               map.setZoom(9);
		               map.setCenter(marker.getPosition());
		            });

				}
				google.maps.event.addDomListener(window, 'load', loadMap);

			});
			
		}

		if ($('#googleMap2').length) {

			$(document).ready(function() {

				var home   = ["389e89", "40.7791072", "-73.94832989999999", "389 East 89th Street", "Home", "389e89", "home"],
	            places = [["389e89", "40.7791072", "-73.94832989999999", "389 East 89th Street", "Home", "389e89", "home"], ["DTUT", "40.78081359999999", "-73.9493224", "1744 2nd Ave", "Food & Drink", "dtut", "food-drink"], ["Café Jax", "40.776062", "-73.9518727", "318 E 84th St", "Food & Drink", "cafe-jax", "food-drink"], ["Maison Kayser", "40.77917770000001", "-73.9534367", "1535 3rd Ave", "Food & Drink", "maison-kayser", "food-drink"], ["Libertador", "40.7799956", "-73.95049080000001", "1725 2nd Ave", "Food & Drink", "libertador", "food-drink"], ["Naruto Ramen", "40.7811959", "-73.9525805", "1596 3rd Ave", "Food & Drink", "naruto-ramen", "food-drink"], ["Lexington Candy Shop", "40.7775113", "-73.9572416", "1226 Lexington Ave", "Food & Drink", "lexington-candy-shop", "food-drink"], ["Ottomanelli Brothers", "40.7841477", "-73.952456", "1424 Lexington Ave", "Food & Drink", "ottomanelli-brothers", "food-drink"], ["Three Guys Restaurant", "40.782685", "-73.957787", "1232 Madison Ave #1", "Food & Drink", "three-guys-restaurant", "food-drink"], ["Alice's Tea Cup III", "40.7750972", "-73.9554136", "220 E 81st St", "Food & Drink", "alice-s-tea-cup-iii", "food-drink"], ["Cafe D'Alsace", "40.7791721", "-73.9510841", "1695 2nd Ave", "Food & Drink", "cafe-d-alsace", "food-drink"], ["Demarchelier", "40.780493", "-73.95869990000001", "50 E 86th St", "Food & Drink", "demarchelier", "food-drink"], ["E.A.T.", "40.777365", "-73.9616659", "1064 Madison Ave", "Food & Drink", "e-a-t", "food-drink"], ["East End Kitchen", "40.7720882", "-73.9474772", "539 E 81st St", "Food & Drink", "east-end-kitchen", "food-drink"], ["Elio's", "40.77684199999999", "-73.95285799999999", "1621 2nd Ave", "Food & Drink", "elio-s", "food-drink"], ["Eli's Bread", "40.780005", "-73.9463339", "403 E 91st St", "Food & Drink", "eli-s-bread", "food-drink"], ["Felice 83", "40.7749003", "-73.9510928", "1593 1st Ave", "Food & Drink", "felice-83", "food-drink"], ["Flex Mussels", "40.776257", "-73.95653399999999", "174 E 82nd St", "Food & Drink", "flex-mussels", "food-drink"], ["H&H Bagels", "40.7744927", "-73.95459819999999", "1551 2nd Ave", "Food & Drink", "h-h-bagels", "food-drink"], ["Heidelberg", "40.77745609999999", "-73.95177190000001", "1648 2nd Ave", "Food & Drink", "heidelberg", "food-drink"], ["The Auction House", "40.779562", "-73.9500205", "300 E 89th St", "Food & Drink", "the-auction-house", "food-drink"], ["Luke's Lobster", "40.7747367", "-73.9545345", "242 E 81st St", "Food & Drink", "luke-s-lobster", "food-drink"], ["Móle", "40.78033", "-73.950306", "1735 2nd Ave", "Food & Drink", "mole", "food-drink"], ["Shake Shack", "40.7789629", "-73.95494819999999", "154 E 86th St", "Food & Drink", "shake-shack", "food-drink"], ["The Bagel Mill", "40.77820759999999", "-73.9480658", "1700 1st Avenue", "Food & Drink", "the-bagel-mill", "food-drink"], ["The Gilroy", "40.77487", "-73.95425399999999", "1561 2nd Ave", "Food & Drink", "the-gilroy", "food-drink"], ["The Penrose", "40.7754699", "-73.9532074", "1590 2nd Ave", "Food & Drink", "the-penrose", "food-drink"], ["2 Little Red Hens", "40.777478", "-73.951655", "1652 2nd Ave", "Food & Drink", "2-little-red-hens", "food-drink"], ["The Uptown Restaurant", "40.7806007", "-73.9530791", "1576 3rd Ave", "Food & Drink", "the-uptown-restaurant", "food-drink"], ["Corrado Bread & Pastry", "40.7821886", "-73.9534173", "1361 Lexington Ave", "Food & Drink", "corrado-bread-pastry", "food-drink"], ["Lucy's Whey", "40.783651", "-73.952288", "1417 Lexington Ave", "Food & Drink", "lucy-s-whey", "food-drink"], ["Sarabeth's", "40.7847375", "-73.9557131", "1295 Madison Ave", "Food & Drink", "sarabeth-s", "food-drink"], ["The Infirmary", "40.7799512", "-73.9498632", "1720 2nd Ave", "Food & Drink", "the-infirmary", "food-drink"], ["The Milton", "40.7812199", "-73.94891299999999", "1754 2nd Ave", "Food & Drink", "the-milton", "food-drink"], ["Jones Wood Foundry", "40.770399", "-73.953541", "401 E 76th St", "Food & Drink", "jones-wood-foundry", "food-drink"], ["Poke", "40.7767146", "-73.9504812", "343 E 85th St", "Food & Drink", "poke", "food-drink"], ["The Writing Room", "40.7794236", "-73.95086090000001", "1703 2nd Ave", "Food & Drink", "the-writing-room", "food-drink"], ["Juice Generation", "40.7774268", "-73.9550203", "1486 3rd Ave", "Food & Drink", "juice-generation", "food-drink"], ["Sant Ambroeus", "40.7754061", "-73.96300099999999", "1000 Madison Ave", "Food & Drink", "sant-ambroeus", "food-drink"], ["Earl's Beer & Cheese", "40.787327", "-73.9515911", "1259 Park Ave", "Food & Drink", "earl-s-beer-cheese", "food-drink"], ["Whole Foods", "40.7796749", "-73.95295740000002", "1551 3rd Ave", "Food & Drink", "whole-foods", "food-drink"], ["Fairway Market", "40.7779338", "-73.952452", "240 E 86th St", "Food & Drink", "fairway-market", "food-drink"], ["Imperial Wines", "40.7785746", "-73.9484732", "1705 1st Avenue", "Food & Drink", "imperial-wines", "food-drink"], ["Dean and DeLuca", "40.7803648", "-73.95941479999999", "1150 Madison Ave", "Food & Drink", "dean-and-deluca", "food-drink"], ["C-Town", "40.7790889", "-73.9480846", "1721 1st Avenue", "Food & Drink", "c-town", "food-drink"], ["William Greenberg Bakery", "40.7787515", "-73.960608", "1100 Madison Ave", "Food & Drink", "william-greenberg-bakery", "food-drink"], ["Park East Liquors", "40.776806", "-73.94679599999999", "1657 York Ave", "Food & Drink", "park-east-liquors", "food-drink"], ["Guggenheim", "40.7830178", "-73.95888839999999", "1071 5th Ave", "Cultural", "guggenheim", "cultural"], ["92nd St Y", "40.7830623", "-73.95268039999999", "1395 Lexington Ave", "Cultural", "92nd-st-y", "cultural"], ["Metropolitan Museum of Art", "40.7791655", "-73.9629278", "1000 5th Ave", "Cultural", "metropolitan-museum-of-art", "cultural"], ["Cooper Hewitt Museum", "40.7844391", "-73.9578551", "2 E 91st St", "Cultural", "cooper-hewitt-museum", "cultural"], ["Jewish Museum", "40.7853626", "-73.95764100000001", "1109 5th Ave at 92nd St", "Cultural", "jewish-museum", "cultural"], ["Neue Gallery", "40.781264", "-73.9603267", "1048 5th Ave", "Cultural", "neue-gallery", "cultural"], ["Gracie Mansion Conservancy", "40.7762302", "-73.943794", "88th Street and E End Ave", "Cultural", "gracie-mansion-conservancy", "cultural"], ["Gagosian Gallery", "40.7748162", "-73.9635709", "980 Madison Ave", "Cultural", "gagosian-gallery", "cultural"], ["Hubert Gallery", "40.7769166", "-73.96190229999999", "1046 Madison Ave", "Cultural", "hubert-gallery", "cultural"], ["Sothebys", "40.7662584", "-73.95370989999999", "1334 York Ave", "Cultural", "sothebys", "cultural"], ["Zitomers", "40.7741821", "-73.963499", "969 Madison Ave", "Retail & Services", "zitomers", "retail-services"], ["Infinity", "40.7789401", "-73.9602171", "1116 Madison Ave", "Retail & Services", "infinity", "retail-services"], ["Haute Hippie", "40.777532", "-73.9615309", "1070 Madison Ave", "Retail & Services", "haute-hippie", "retail-services"], ["Yigal Azrouël", "40.7757194", "-73.9623333", "1011 Madison Ave", "Retail & Services", "yigal-azrouel", "retail-services"], ["Intermix", "40.7752168", "-73.9627047", "1003 Madison Ave", "Retail & Services", "intermix", "retail-services"], ["Bonpoint", "40.7837737", "-73.9564214", "1269 Madison Ave", "Retail & Services", "bonpoint", "retail-services"], ["Jonathan Adler", "40.7786146", "-73.9602297", "1097 Madison Ave", "Retail & Services", "jonathan-adler", "retail-services"], ["Warby Parker", "40.7767415", "-73.9573686", "1209 Lexington Ave", "Retail & Services", "warby-parker", "retail-services"], ["Theory", "40.780301", "-73.95900639999999", "1157 Madison Ave", "Retail & Services", "theory", "retail-services"], ["Peter Elliot", "40.777532", "-73.9615309", "1070 Madison Ave", "Retail & Services", "peter-elliot", "retail-services"], ["California Closets", "40.775747", "-73.9475113", "1625 York Ave", "Retail & Services", "california-closets", "retail-services"], ["The Children's General Store", "40.782056", "-73.952382", "168 E 91st St", "Retail & Services", "the-children-s-general-store", "retail-services"], ["Lush Cosmetics", "40.7783469", "-73.953602", "206 E 86th St", "Retail & Services", "lush-cosmetics", "retail-services"], ["Barnes and Noble", "40.7790608", "-73.955106", "150 E 86th St", "Retail & Services", "barnes-and-noble", "retail-services"], ["Sephora", "40.7790905", "-73.95523899999999", "144 E 86th St", "Retail & Services", "sephora", "retail-services"], ["Scoop", "40.770537", "-73.959662", "1275 3rd Ave", "Retail & Services", "scoop", "retail-services"], ["Yumi Kim", "40.7724055", "-73.9584364", "1331 3rd Ave", "Retail & Services", "yumi-kim", "retail-services"], ["Valery Joseph Salon", "40.776812", "-73.962069", "1044 Madison Ave", "Retail & Services", "valery-joseph-salon", "retail-services"], ["Warren Tricomi Salon", "40.7792542", "-73.9597047", "1117 Madison Ave", "Retail & Services", "warren-tricomi-salon", "retail-services"], ["Drybar", "40.772273", "-73.9579569", "209 E 76th St", "Retail & Services", "drybar", "retail-services"], ["Gymboree Play and Music", "40.7757138", "-73.9499633", "1622 1st Avenue", "Recreation & Kids", "gymboree-play-and-music", "recreation-kids"], ["86th St. Cinemas", "40.77834989999999", "-73.9534644", "210 E 86th St", "Recreation & Kids", "86th-st-cinemas", "recreation-kids"], ["NYC Elite Gymnastics", "40.7796212", "-73.94550219999999", "421 E 91st St", "Recreation & Kids", "nyc-elite-gymnastics", "recreation-kids"], ["Yorkville Tennis Club", "40.7782522", "-73.9458884", "1725 York Ave", "Recreation & Kids", "yorkville-tennis-club", "recreation-kids"], ["Asphalt Green", "40.7779504", "-73.9439541", "555 E 90th St", "Recreation & Kids", "asphalt-green", "recreation-kids"], ["Carl Shultz Park", "40.774918", "-73.9447516", "East 86th Street & East End Ave", "Recreation & Kids", "carl-shultz-park", "recreation-kids"], ["CrossFit UES", "40.77570619999999", "-73.9475463", "1623 York Ave", "Recreation & Kids", "crossfit-ues", "recreation-kids"], ["Soul Cycle 83rd St", "40.777129", "-73.955624", "1470 3rd Ave", "Recreation & Kids", "soul-cycle-83rd-st", "recreation-kids"], ["Children's Museum of Manhattan", "40.7858772", "-73.9772827", "212 W 83rd St", "Recreation & Kids", "children-s-museum-of-manhattan", "recreation-kids"], ["The Playroom", "40.7696856", "-73.95512780000001", "1439 1st Avenue", "Recreation & Kids", "the-playroom", "recreation-kids"], ["Art Farm in the City", "40.77969179999999", "-73.945735", "419 E 91st St", "Recreation & Kids", "art-farm-in-the-city", "recreation-kids"], ["Let's Dress Up", "40.77668540000001", "-73.9503948", "345 E 85th St #1", "Recreation & Kids", "let-s-dress-up", "recreation-kids"], ["Kidville", "40.7781595", "-73.95549749999999", "163 E 84th St", "Recreation & Kids", "kidville", "recreation-kids"], ["The Craft Studio", "40.7829679", "-73.95068289999999", "1657 3rd Ave", "Recreation & Kids", "the-craft-studio", "recreation-kids"], ["Chocolate Works", "40.783721", "-73.9526992", "1410 Lexington Ave", "Recreation & Kids", "chocolate-works", "recreation-kids"]];

				var locations = [
				    [
				    "Guggenheim",
				     "1071 5th Ave",
				    "40.7791072","-73.94832989999999"
				    ],
				    [
				    "Hubert Gallery",
				    "1046 Madison Ave",
				    "40.7769166", "-73.96190229999999"
				    ],
				    [
				    "Jewish Museum",
				    "1109 5th Ave at 92nd St",
				    "40.7853626", "-73.95764100000001"
				    ],
				    [
				    "Neue Gallery",
				    "1048 5th Ave",
				    "40.781264", "-73.9603267"
				    ],
				    [
				    "Sothebys",
				    "1334 York Ave",
				    "40.7662584", "-73.95370989999999"
				    ],
				    [
				    "2 Little Red Hens",
				    "1652 2nd Ave",
				    "40.777478", "-73.951655"
				    ],
				    [
				    "Alices Tea Cup III",
				    "220 E 81st St",
				    "40.7750972", "-73.9554136"
				    ],
				    [
				    "C-Town",
				    "1721 1st Avenue",
				    "40.7790889", "-73.9480846" 
				    ],
				    [
				    "Cafe DAlsace",
				    "1695 2nd Ave",
				    "40.7791721", "-73.9510841"
				    ],
				    [
				    "Café Jax",
				    "318 E 84th St",
				    "40.776062", "-73.9518727"
				    ],
				    [
				    "Corrado Bread & Pastry",
				    "1361 Lexington Ave",
				    "40.7821886", "-73.9534173"
				    ],
				    [
				    "Dean and DeLuca",
				    "1150 Madison Ave",
				    "40.7803648", "-73.95941479999999"
				    ],
				    [
				    "Demarchelier",
				    "50 E 86th St",
				    "40.780493", "-73.95869990000001"
				    ],
				    [
				    "86th St. Cinemas",
				    "210 E 86th St",
				    "40.77834989999999", "-73.9534644"
				    ],
				    [
				    "Art Farm in the City",
				    "419 E 91st St",
				    "40.77969179999999", "-73.945735"
				    ],
				    [
				    "Asphalt Green",
				    "555 E 90th St",
				    "40.7779504", "-73.9439541"
				    ],
				    [
				    "Carl Shultz Park",
				    "East 86th Street & East End Ave",
				    "40.774918", "-73.9447516"
				    ],
				    [
				    "Chocolate Works",
				    "1410 Lexington Ave",
				    "40.783721", "-73.9526992"
				    ],
				    [
				    "Kidville",
				    "163 E 84th St",
				    "40.7781595", "-73.95549749999999"
				    ],
				    [
				    "Lets Dress Up",
				    "345 E 85th St #1",
				    "40.77668540000001", "-73.9503948"
				    ],
				    [
				    "Bonpoint",
				    "1269 Madison Ave",
				    "40.7837737", "-73.9564214"
				    ],
				    [
				    "California Closets",
				    "1625 York Ave",
				    "40.775747", "-73.9475113"
				    ],
				    [
				    "Drybar",
				    "209 E 76th St",
				    "40.772273", "-73.9579569"
				    ],
				    [
				    "Haute Hippie",
				    "1070 Madison Ave",
				    "40.777532", "-73.9615309"
				    ],
				    [
				    "Infinity",
				    "1116 Madison Ave",
				    "40.7789401", "-73.9602171"
				    ],
				    [
				    "Intermix",
				    "1003 Madison Ave",
				    "40.7752168", "-73.9627047"
				    ],
				    [
				    "Jonathan Adler",
				    "1097 Madison Ave",
				    "40.7786146", "-73.9602297"
				    ],
				    [
				    "Lush Cosmetics",
				    "206 E 86th St",
				    "40.7783469", "-73.953602"
				    ],
				];

				window.gmarkers = [];

				var map = new google.maps.Map(document.getElementById('googleMap2'), {
				    zoom: 15,
				    center: new google.maps.LatLng(40.7791072,-73.94832989999999),
				    mapTypeId: google.maps.MapTypeId.ROADMAP,
				    styles : [{featureType:'all',stylers:[{saturation:-100},{gamma:0.0}]}]
				});

				var infowindow = new google.maps.InfoWindow();

				function createMarker(latlng, html) {
				    var marker = new google.maps.Marker({
				        position: latlng,
				        map: map,
				        icon: 'images/map_marker2.png'
				    });

				    google.maps.event.addListener(marker, 'click', function() {
				        infowindow.setContent(html);
				        infowindow.open(map, marker);
				    });

				    return marker;
				}

				for (var i = 0; i < locations.length; i++) {
				    gmarkers[locations[i][0]] =
				    createMarker(new google.maps.LatLng(locations[i][2], locations[i][3]), locations[i][0]);
				}

				var myCenter = new google.maps.LatLng(40.7780493,-73.95832989999999);

				var gmarker = new google.maps.Marker({
	               position:myCenter,
	               map: map,
	               icon: 'images/map_marker.png'
	            });
	            
	            gmarker.setMap(map);
	            
			});

		}

		if ($('#googleMap3').length) {

			$(document).ready(function() {

			    var myCenter = new google.maps.LatLng(38.8851576, -76.9965074);

				function loadMap() {
				  	var mapProp = {
				  	    center: new google.maps.LatLng(38.8851576, -76.9965074),
					    zoom:14,
					    mapTypeId:google.maps.MapTypeId.ROADMAP,
					    styles : [{featureType:'all',stylers:[{saturation:-100},{gamma:0.0}]}]
					};

					var map = document.getElementById('googleMap3');

					if(map !== null){

				    	var map = new google.maps.Map(document.getElementById("googleMap3"),mapProp);

					}

		            var marker = new google.maps.Marker({
		               position:myCenter,
		               map: map,
		               icon: 'images/map_marker.png'
		            });
		            
		            marker.setMap(map);

		            //Zoom to 7 when clicked on marker
		            google.maps.event.addListener(marker,'click',function() {
		               map.setZoom(9);
		               map.setCenter(marker.getPosition());
		            });

				}
				google.maps.event.addDomListener(window, 'load', loadMap);

			});
			
		}

	});

})(jQuery);