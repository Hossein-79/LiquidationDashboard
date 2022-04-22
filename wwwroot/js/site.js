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