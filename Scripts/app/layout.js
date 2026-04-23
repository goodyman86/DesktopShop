/* ============================================================
   DesktopShop – Layout JS (Sidebar toggle & Clock)
   ============================================================ */

$(function () {
    // ---------- Sidebar Toggle ----------
    var $sidebar = $('#sidebar');
    var $mainWrapper = $('.main-wrapper');
    var SIDEBAR_KEY = 'ds_sidebar_open';

    function closeSidebar() {
        $sidebar.removeClass('open');
    }

    $('#sidebarToggle').on('click', function () {
        if ($(window).width() <= 991) {
            $sidebar.toggleClass('open');
        } else {
            $sidebar.toggleClass('collapsed');
            $mainWrapper.toggleClass('expanded');
        }
    });

    // Close sidebar on outside click (mobile)
    $(document).on('click', function (e) {
        if ($(window).width() <= 991 && $sidebar.hasClass('open')) {
            if (!$(e.target).closest('#sidebar, #sidebarToggle').length) {
                closeSidebar();
            }
        }
    });

    // ---------- Topbar Clock ----------
    function updateClock() {
        var now = new Date();
        var pad = function (n) { return n < 10 ? '0' + n : n; };
        var days = ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'];
        var str = days[now.getDay()] + ', ' +
                  pad(now.getDate()) + '/' + pad(now.getMonth() + 1) + '/' + now.getFullYear() +
                  ' ' + pad(now.getHours()) + ':' + pad(now.getMinutes()) + ':' + pad(now.getSeconds());
        $('#topbarClock').text(str);
    }

    updateClock();
    setInterval(updateClock, 1000);
});
