const toggleMenu = () => document.body.classList.toggle("open");

$('a.jt').cluetip({
    showTitle: false,
    dropShadow: false,
    cluetipClass: 'custom',
    positionBy: 'fixed',
    leftOffset: -13,
    hoverIntent: false,
    sticky: true,
    mouseOutClose: true,
    closePosition: '',
});

var swiper = new Swiper('.mySwiper', {
    autoplay: {
        delay: 10000,
        disableOnInteraction: false,
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    loop: true,
});

$(document).ready(function () {
    var currentPage = window.location.pathname;
    $(".nav-link").each(function () {
        if ($(this).attr("href") == currentPage) {
            $(this).addClass("active");
        }
    });
});

const togglePassword = document.querySelector('#togglePassword');
const password = document.querySelector('#password');
const confPassword = document.querySelector('#confPassword')

if (togglePassword) {
    togglePassword.addEventListener('click', function (e) {

        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);

        if (confPassword) {
            const type1 = confPassword.getAttribute('type') === 'password' ? 'text' : 'password';
            confPassword.setAttribute('type', type1);
        }

        this.classList.toggle('fa-eye');
    });
}

$(document).ready(function () {
    // Get the current URL
    var currentURL = window.location.href;
    // Get the anchor tags
    var anchors = $(".myA");
    // Iterate through the anchor tags
    anchors.each(function () {
        // Check if the anchor tag's href matches the current URL
        if (this.href == currentURL) {
            // Add active class to the anchor tag
            $(this).addClass("activeEpisode");
        }
    });

    // Check if a value is stored in local storage
    if (sessionStorage.getItem("selectedOption")) {
        // Set the selected option
        $("#mySelect").val(sessionStorage.getItem("selectedOption"));
        // Show the corresponding div
        $(".myDiv").hide();
        $("#div_" + sessionStorage.getItem("selectedOption")).show();
    } else {
        // Show the first div by default
        $(".myDiv").hide();
        $(".myDiv").first().show();
    }

    $("#mySelect").change(function () {
        // Store the selected option in local storage
        sessionStorage.setItem("selectedOption", $(this).val());
        // Show the corresponding div
        $(".myDiv").hide();
        $("#div_" + $(this).val()).show();
    });
});

//var movieId = $(".abc").data("movie-id");

//$("#items").on("load", function () {
//    $.get("/FavoriteMovies/CheckFavorite/" + movieId, function (data) {
//        console.log(data)
//        if (data.isFavorite) {
//            $("#heart-icon-" + movieId).addClass("fa-solid");
//        } else {
//            $("#heart-icon-" + movieId).removeClass("fa-solid");
//        }
//    });
//});
