/* ============================================================
   DesktopShop – Shared Helpers
   ============================================================ */

/**
 * Format a number to VNĐ currency string.
 * Example: formatVND(25990000) → "25.990.000đ"
 */
function formatVND(amount) {
    if (amount == null || isNaN(amount)) return '0đ';
    return Number(amount).toLocaleString('vi-VN') + 'đ';
}

/**
 * Return a Bootstrap-style badge class for an order status.
 */
function getStatusBadge(status) {
    var statusMap = {
        0: { text: 'Chờ xử lý',    css: 'badge-pending' },
        1: { text: 'Đã xác nhận',  css: 'badge-confirmed' },
        2: { text: 'Hoàn thành',   css: 'badge-completed' },
        3: { text: 'Đã hủy',       css: 'badge-cancelled' },
        'Pending':   { text: 'Chờ xử lý',   css: 'badge-pending' },
        'Confirmed': { text: 'Đã xác nhận', css: 'badge-confirmed' },
        'Completed': { text: 'Hoàn thành',  css: 'badge-completed' },
        'Cancelled': { text: 'Đã hủy',      css: 'badge-cancelled' }
    };
    var info = statusMap[status] || { text: status, css: 'badge-pending' };
    return '<span class="badge-status ' + info.css + '">' + info.text + '</span>';
}

/**
 * Return a styled badge for payment method.
 */
function getPaymentMethodBadge(paymentMethod) {
    var methodMap = {
        0: { text: 'COD', icon: 'fa-money-bill-wave', css: 'bg-success' },
        1: { text: 'Chuyển khoản', icon: 'fa-university', css: 'bg-primary' },
        2: { text: 'Thẻ', icon: 'fa-credit-card', css: 'bg-info' },
        3: { text: 'Ví điện tử', icon: 'fa-wallet', css: 'bg-warning' },
        'CashOnDelivery': { text: 'COD', icon: 'fa-money-bill-wave', css: 'bg-success' },
        'BankTransfer': { text: 'Chuyển khoản', icon: 'fa-university', css: 'bg-primary' },
        'CreditCard': { text: 'Thẻ', icon: 'fa-credit-card', css: 'bg-info' },
        'EWallet': { text: 'Ví điện tử', icon: 'fa-wallet', css: 'bg-warning' }
    };
    var info = methodMap[paymentMethod] || { text: 'COD', icon: 'fa-money-bill-wave', css: 'bg-success' };
    return '<span class="badge ' + info.css + ' text-white" style="font-size:.75rem;padding:4px 8px;"><i class="fa-solid ' + info.icon + ' me-1"></i>' + info.text + '</span>';
}

/**
 * SweetAlert2 toast shortcut.
 */
function showToast(icon, title) {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: title,
        showConfirmButton: false,
        timer: 2500,
        timerProgressBar: true
    });
}

/**
 * Format a date string to DD/MM/YYYY HH:mm
 */
function formatDate(dateStr) {
    if (!dateStr) return '';
    var d = new Date(dateStr);
    var pad = function (n) { return n < 10 ? '0' + n : n; };
    return pad(d.getDate()) + '/' + pad(d.getMonth() + 1) + '/' + d.getFullYear()
         + ' ' + pad(d.getHours()) + ':' + pad(d.getMinutes());
}
