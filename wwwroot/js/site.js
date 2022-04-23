// DARK MODE
function enableDarkMode() {
    $('html').addClass('dark');
    $('#dark-mode-btn .sun').removeClass('hidden');
    $('#dark-mode-btn .moon').addClass('hidden');
}
function disableDarkMode() {
    $('html').removeClass('dark');
    $('#dark-mode-btn .sun').addClass('hidden');
    $('#dark-mode-btn .moon').removeClass('hidden');
}

$(document).ready(function() {
    if (localStorage.getItem('darkmode') == 'true') {
        enableDarkMode();
    } else {
        disableDarkMode();
    }
});
$('#dark-mode-btn').click(function() {
    console.log(localStorage.getItem('darkmode'))
    if (localStorage.getItem('darkmode') == 'true') {
        localStorage.setItem('darkmode', 'false');
        disableDarkMode();
    } else {
        localStorage.setItem('darkmode', 'true');
        enableDarkMode();
    }
});
// END DARK MODE
// MODAL
function showModal(modal) {
    modal.addClass('active');
    $('body').addClass('modal-active');
}
function hideModal(modal) {
    modal.removeClass('active');
    $('body').removeClass('modal-active');
}
$(document).on('click', '[data-modal]', function() {
    var modalId = $(this).attr('data-modal');
    showModal($('#' + modalId));
});
$(document).on('click', '.modal-close', function() {
    hideModal($(this).closest('.modal'));
});
// HIDE MODAL ON CLICK OUTSIDE
$(document).on('click', '.modal', function(e) {
    if (e.target !== this) {
        return;
    }
    hideModal($(this));
});
// END MODAL