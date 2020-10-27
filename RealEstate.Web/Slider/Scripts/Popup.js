jQuery('.foundry_modal[modal-link]').remove();

if ($('.foundry_modal').length && (!jQuery('.modal-screen').length)) {
    // Add a div.modal-screen if there isn't already one there.
    var modalScreen = jQuery('<div />').addClass('modal-screen').appendTo('body');

}

jQuery('.foundry_modal').click(function () {
    jQuery(this).addClass('modal-acknowledged');
});

jQuery(document).on('wheel mousewheel scroll', '.foundry_modal, .modal-screen', function (evt) {
    $(this).get(0).scrollTop += (evt.originalEvent.deltaY);
    return false;
});

$('.modal-container:not([modal-link])').each(function (index) {
    if (jQuery(this).find('iframe[src]').length) {
        jQuery(this).find('.foundry_modal').addClass('iframe-modal');
        var iframe = jQuery(this).find('iframe');
        iframe.attr('data-src', iframe.attr('src'));
        iframe.attr('src', '');

    }
    jQuery(this).find('.btn-modal').attr('modal-link', index);

    // Only clone and append to body if there isn't already one there
    if (!jQuery('.foundry_modal[modal-link="' + index + '"]').length) {
        jQuery(this).find('.foundry_modal').clone().appendTo('body').attr('modal-link', index).prepend(jQuery('<i class="ti-close close-modal">'));
    }
});

$('.btn-modal').unbind('click').click(function () {
    var linkedModal = jQuery('.foundry_modal[modal-link="' + jQuery(this).attr('modal-link') + '"]'),
        autoplayMsg = "";
    jQuery('.modal-screen').toggleClass('reveal-modal');
    if (linkedModal.find('iframe').length) {
        if (linkedModal.find('iframe').attr('data-autoplay') === '1') {
            var autoplayMsg = '&autoplay=1'
        }
        linkedModal.find('iframe').attr('src', (linkedModal.find('iframe').attr('data-src') + autoplayMsg));
    }
    if (linkedModal.find('video').length) {
        linkedModal.find('video').get(0).play();
    }
    linkedModal.toggleClass('reveal-modal');
    return false;
});
$('.foundry_modal[data-hide-after]').each(function () {
    var modal = $(this);
    var delay = modal.attr('data-hide-after');
    if (typeof modal.attr('data-cookie') != "undefined") {
        if (!mr_cookies.hasItem(modal.attr('data-cookie'))) {
            setTimeout(function () {
                if (!modal.hasClass('modal-acknowledged')) {
                    modal.removeClass('reveal-modal');
                    $('.modal-screen').removeClass('reveal-modal');
                }
            }, delay);
        }
    } else {
        setTimeout(function () {
            if (!modal.hasClass('modal-acknowledged')) {
                modal.removeClass('reveal-modal');
                $('.modal-screen').removeClass('reveal-modal');
            }
        }, delay);
    }
});

jQuery('.close-modal:not(.modal-strip .close-modal)').unbind('click').click(function () {
    var modal = jQuery(this).closest('.foundry_modal');
    modal.toggleClass('reveal-modal');
    if (typeof modal.attr('data-cookie') !== "undefined") {
        mr_cookies.setItem(modal.attr('data-cookie'), "true", Infinity);
    }
    if (modal.find('iframe').length) {
        modal.find('iframe').attr('src', '');
    }
    jQuery('.modal-screen').removeClass('reveal-modal');
});


