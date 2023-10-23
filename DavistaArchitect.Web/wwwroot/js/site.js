$(document).ready(function () {
    // UserScroll function
    $(window).scroll(function () {
        const navbar = $('.navbar');
        const toTopBtn = $('#to-top');

        // Check if the URL does not contain 'Client'
        const currentUrl = window.location.pathname;
        const isClientUrl = currentUrl.indexOf('Client') !== -1;

        if ($(this).scrollTop() > 50 || !isClientUrl) {
            navbar.addClass('bg-dark');
            toTopBtn.addClass('show');
        } else {
            navbar.removeClass('bg-dark');
            toTopBtn.removeClass('show');
        }
    });

    // ScrollToTop function
    $('#to-top').on('click', function () {
        $('html, body').animate({ scrollTop: 0 }, 'medium');
    });
});
