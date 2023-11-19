/**
* Template Name: Yummy - v1.3.0
* Template URL: https://bootstrapmade.com/yummy-bootstrap-restaurant-website-template/
* Author: BootstrapMade.com
* License: https://bootstrapmade.com/license/
*/
document.addEventListener('DOMContentLoaded', () => {
    "use strict";

    /**
     * Preloader
     */
    const preloader = document.querySelector('#preloader');
    if (preloader) {
        window.addEventListener('load', () => {
            preloader.remove();
        });
    }


    /**
     * Scroll top button
     */
    const scrollTop = document.querySelector('.scroll-top');
    if (scrollTop) {
        const togglescrollTop = function () {
            window.scrollY > 100 ? scrollTop.classList.add('active') : scrollTop.classList.remove('active');
        }
        window.addEventListener('load', togglescrollTop);
        document.addEventListener('scroll', togglescrollTop);
        scrollTop.addEventListener('click', window.scrollTo({
            top: 0,
            behavior: 'smooth'
        }));
    }


    /**
     * Animation on scroll function and init
     */
    function aos_init() {
        AOS.init({
            duration: 1000,
            easing: 'ease-in-out',
            once: true,
            mirror: false
        });
    }
    window.addEventListener('load', () => {
        aos_init();
    });


    /**
     * Init swiper slider with 3 slides at once in desktop view
     */
    new Swiper('.slides-3', {
        speed: 600,
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false
        },
        slidesPerView: 'auto',
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable: true
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        breakpoints: {
            320: {
                slidesPerView: 1,
                spaceBetween: 40
            },

            1200: {
                slidesPerView: 3,
            }
        }
    });

    /**
     * Gallery Slider
     */
    new Swiper('.gallery-slider', {
        speed: 400,
        loop: true,
        centeredSlides: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false
        },
        slidesPerView: 'auto',
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable: true
        },
        breakpoints: {
            320: {
                slidesPerView: 1,
                spaceBetween: 20
            },
            640: {
                slidesPerView: 3,
                spaceBetween: 20
            },
            992: {
                slidesPerView: 5,
                spaceBetween: 20
            }
        }
    });


    $('#slide-next').click(function () {

        var radios = document.getElementsByName('inlineRadioOptions');
        var radioValid = false;

        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                radioValid = true;
                break;
            }
        }

        var caloriesInput = document.getElementById('typeNumber');
        var calories = caloriesInput.value.trim();
        var caloriesValid = calories !== '';

        if (radioValid && caloriesValid) {
            $.ajax({
                url: "/Recipe/GenerateMealPlan",
                type: "GET",
                data: {
                    // Get the value from the "calories" input field
                    calories: $("#typeNumber").val(),

                    // Get the value of the selected radio button
                    inlineRadioOptions: $("input[name='inlineRadioOptions']:checked").val()
                },
                success: function (data) {
                    updateUIWithData(data);
                },
                error: function () {
                    // Handle any errors if the request fails
                    alert("Error fetching data.");
                }
            });
        } else {
            if (!caloriesValid && !radioValid) {
                alert('Please enter the number of calories and select a meal option before proceeding.');
            } else if (!caloriesValid) {
                alert('Please enter the number of calories before proceeding.');
            } else {
                alert('Please select a meal option before proceeding.');
            }
        }
    });

    function updateUIWithData(data) {
        // Update breakfast data
        $("#breakfast-meal-title").html(`<i class="bi bi-link"></i> ${data.meals[0].title}`);
        $("#breakfast-link-container").attr('href', data.meals[0].sourceUrl);
        $("#breakfast-ready-in").html(`<i class="fa fa-clock-o"></i> <span>${data.meals[0].readyInMinutes} minutes</span>`);
        var breakfastServingsText = data.meals[0].servings === 1 ? "serving" : "servings";
        $("#breakfast-servings").html(`<i class="fa fa-cutlery"></i> <span> ${data.meals[0].servings} ${breakfastServingsText} </span>`);

        // Update lunch data
        $("#lunch-meal-title").html(`<i class="bi bi-link"></i> ${data.meals[1].title}`);
        $("#lunch-link-container").attr('href', data.meals[1].sourceUrl);
        $("#lunch-ready-in").html(`<i class="fa fa-clock-o"></i> <span>${data.meals[1].readyInMinutes} minutes</span>`);
        var lunchServingsText = data.meals[1].servings === 1 ? "serving" : "servings";
        $("#lunch-servings").html(`<i class="fa fa-cutlery"></i> <span> ${data.meals[1].servings} ${lunchServingsText} </span>`);

        // Update dinner data
        $("#dinner-meal-title").html(`<i class="bi bi-link"></i> ${data.meals[2].title}`);
        $("#dinner-link-container").attr('href', data.meals[2].sourceUrl);
        $("#dinner-ready-in").html(`<i class="fa fa-clock-o"></i> <span>${data.meals[2].readyInMinutes} minutes</span>`);
        var dinnerServingsText = data.meals[2].servings === 1 ? "serving" : "servings";
        $("#dinner-servings").html(`<i class="fa fa-cutlery"></i> <span> ${data.meals[2].servings} ${dinnerServingsText} </span>`);

        // Update macros data
        var cals = document.getElementById('cals');
        var carbs = document.getElementById('carbs');
        var fat = document.getElementById('fat');
        var protein = document.getElementById('protein');
        cals.textContent = "Calories: " + data.nutrients.calories + "kcal";
        carbs.textContent = "Carbohydrates: " + data.nutrients.carbohydrates + "g";
        fat.textContent = "Fat: " + data.nutrients.fat + "g";
        protein.textContent = "Protein: " + data.nutrients.protein + "g";

        // Display hidden elements
        var slideBox1 = document.getElementById('slideBox1');
        var mealPlanDesc = document.getElementById('meal-plan-description');
        var slideBox2 = document.getElementById('slideBox2');
        var scrollTo = document.getElementById('meal-plan');

        slideBox1.style.display = 'none';
        mealPlanDesc.style.display = 'none';
        slideBox2.style.display = 'block';

        setTimeout(function () {
            slideBox2.style.transform = 'translateX(0)';
            slideBox1.style.transform = 'translateX(100%)';
            mealPlanDesc.style.transform = 'translateX(100%)';
            slideBox1.style.transition = 'transform 0.3s ease';
            mealPlanDesc.style.transition = 'transform 0.3s ease';
            scrollTo.scrollIntoView({ behavior: 'smooth' });
        }, 10);
    }


    $('#new-meal-plan-btn').click(function () {

        // Reset calories input
        var caloriesInput = document.getElementById('typeNumber');
        caloriesInput.value = ''; 
        caloriesInput.placeholder = '####'; 

        // Reset selected meal
        var radios = document.getElementsByName('inlineRadioOptions');
        radios.forEach(function (radio) {
            radio.checked = false; 
        });

         var slideBox1 = document.getElementById('slideBox1');
         var slideBox2 = document.getElementById('slideBox2');
         var mealPlanDesc = document.getElementById('meal-plan-description');
         const scrollTo = document.getElementById('meal-plan');


         slideBox2.style.display = 'none'; 
         slideBox1.style.display = 'block';
         mealPlanDesc.style.display = 'block';

         setTimeout(function () {
             slideBox1.style.transform = 'translateX(0)';
             mealPlanDesc.style.transform = 'translateX(0)'
             slideBox2.style.transform = 'translateX(100%)';
             scrollTo.scrollIntoView({ behavior: 'smooth' });

         }, 10); 

     });
});


