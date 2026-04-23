/* ============================================================
   DesktopShop – Dashboard JS
   Loads stats, charts, and recent orders via AJAX
   ============================================================ */

$(function () {
    var API_BASE = '/api';

    // ---------- Module-level data cache for export ----------
    var dashboardData = {
        totalRevenue: 0,
        totalOrders: 0,
        totalProducts: 0,
        lowStockCount: 0,
        topSelling: [],
        revenueLabels: [],
        revenueValues: [],
        categoryLabels: [],
        categoryValues: [],
        recentOrders: []
    };

    // ---------- Load Dashboard Summary ----------
    function loadSummary() {
        $.ajax({
            url: API_BASE + '/dashboard/summary',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                dashboardData.totalRevenue = data.TotalRevenue || data.totalRevenue || 0;
                dashboardData.totalOrders = data.TotalOrders || data.totalOrders || 0;
                dashboardData.totalProducts = data.TotalProducts || data.totalProducts || 0;
                dashboardData.lowStockCount = data.LowStockCount || data.lowStockCount || 0;
                $('#statRevenue').text(formatVND(dashboardData.totalRevenue));
                $('#statOrders').text(dashboardData.totalOrders);
                $('#statProducts').text(dashboardData.totalProducts);
                $('#statLowStock').text(dashboardData.lowStockCount);
                var topProducts = data.TopSellingProducts || data.topSellingProducts || [];
                dashboardData.topSelling = topProducts;
                renderTopSelling(topProducts);
            },
            error: function () {
                // Show demo data if API not available
                dashboardData.totalRevenue = 259900000;
                dashboardData.totalOrders = 48;
                dashboardData.totalProducts = 124;
                dashboardData.lowStockCount = 7;
                $('#statRevenue').text(formatVND(259900000));
                $('#statOrders').text('48');
                $('#statProducts').text('124');
                $('#statLowStock').text('7');
                renderTopSellingDemo();
            }
        });
    }

    // ---------- Top Selling Products ----------
    function renderTopSelling(products) {
        var $body = $('#topSellingBody');
        if (!products.length) {
            $body.html('<tr><td colspan="4" class="text-center py-4 text-muted">Chưa có dữ liệu</td></tr>');
            return;
        }
        var html = '';
        for (var i = 0; i < products.length && i < 5; i++) {
            var p = products[i];
            html += '<tr>' +
                '<td><span class="fw-bold text-primary">' + (i + 1) + '</span></td>' +
                '<td>' + (p.ProductName || p.productName) + '</td>' +
                '<td class="text-end fw-semibold">' + (p.TotalSold || p.totalSold) + '</td>' +
                '<td class="text-end text-danger fw-bold">' + formatVND(p.TotalRevenue || p.totalRevenue) + '</td>' +
                '</tr>';
        }
        $body.html(html);
    }

    function renderTopSellingDemo() {
        var demo = [
            { name: 'PC Gaming RTX 4070', sold: 32, rev: 51200000 },
            { name: 'PC Văn phòng i5-13400', sold: 28, rev: 39200000 },
            { name: 'Workstation Xeon W', sold: 15, rev: 89700000 },
            { name: 'PC Gaming RTX 4060', sold: 12, rev: 23880000 },
            { name: 'Mini PC Intel NUC', sold: 10, rev: 12990000 }
        ];
        dashboardData.topSelling = [];
        for (var i = 0; i < demo.length; i++) {
            dashboardData.topSelling.push({
                ProductName: demo[i].name,
                TotalSold: demo[i].sold,
                TotalRevenue: demo[i].rev
            });
        }
        var html = '';
        for (var i = 0; i < demo.length; i++) {
            html += '<tr>' +
                '<td><span class="fw-bold text-primary">' + (i + 1) + '</span></td>' +
                '<td>' + demo[i].name + '</td>' +
                '<td class="text-end fw-semibold">' + demo[i].sold + '</td>' +
                '<td class="text-end text-danger fw-bold">' + formatVND(demo[i].rev) + '</td>' +
                '</tr>';
        }
        $('#topSellingBody').html(html);
    }

    // ---------- Revenue Chart (Bar) ----------
    var revenueChart = null;

    function loadRevenueChart() {
        $.ajax({
            url: API_BASE + '/dashboard/sales-chart',
            method: 'GET',
            data: { days: 7 },
            dataType: 'json',
            success: function (data) {
                var labels = data.Labels || data.labels || [];
                var values = data.Values || data.values || [];
                dashboardData.revenueLabels = labels;
                dashboardData.revenueValues = values;
                renderRevenueChart(labels, values);
            },
            error: function () {
                // Demo data
                var labels = [];
                var values = [];
                for (var i = 6; i >= 0; i--) {
                    var d = new Date();
                    d.setDate(d.getDate() - i);
                    labels.push(String(d.getDate()).padStart(2,'0') + '/' + String(d.getMonth() + 1).padStart(2,'0'));
                    values.push(Math.floor(Math.random() * 50000000) + 10000000);
                }
                dashboardData.revenueLabels = labels;
                dashboardData.revenueValues = values;
                renderRevenueChart(labels, values);
            }
        });
    }

    function renderRevenueChart(labels, values) {
        var ctx = document.getElementById('revenueChart').getContext('2d');
        if (revenueChart) revenueChart.destroy();

        revenueChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Doanh thu (VNĐ)',
                    data: values,
                    backgroundColor: 'rgba(26, 115, 232, 0.7)',
                    borderColor: '#1a73e8',
                    borderWidth: 1,
                    borderRadius: 6,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        callbacks: {
                            label: function (ctx) {
                                return ' ' + formatVND(ctx.parsed.y);
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function (val) {
                                if (val >= 1000000) return (val / 1000000) + 'tr';
                                if (val >= 1000) return (val / 1000) + 'k';
                                return val;
                            },
                            font: { family: 'Inter', size: 11 }
                        },
                        grid: { color: '#f3f4f6' }
                    },
                    x: {
                        ticks: { font: { family: 'Inter', size: 11 } },
                        grid: { display: false }
                    }
                }
            }
        });
    }

    // ---------- Category Pie Chart ----------
    var categoryChart = null;

    function loadCategoryChart() {
        $.ajax({
            url: API_BASE + '/dashboard/category-sales',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                var labels = data.Labels || data.labels || [];
                var values = data.Values || data.values || [];
                dashboardData.categoryLabels = labels;
                dashboardData.categoryValues = values;
                renderCategoryChart(labels, values);
            },
            error: function () {
                var labels = ['Gaming', 'Văn phòng', 'Workstation', 'Mini PC'];
                var values = [45, 30, 15, 10];
                dashboardData.categoryLabels = labels;
                dashboardData.categoryValues = values;
                renderCategoryChart(labels, values);
            }
        });
    }

    function renderCategoryChart(labels, values) {
        var ctx = document.getElementById('categoryChart').getContext('2d');
        if (categoryChart) categoryChart.destroy();

        var colors = ['#1a73e8', '#0d9f6e', '#f59e0b', '#7c3aed', '#e02424', '#06b6d4'];

        categoryChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: colors.slice(0, labels.length),
                    borderWidth: 2,
                    borderColor: '#fff'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '55%',
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            padding: 16,
                            usePointStyle: true,
                            pointStyleWidth: 10,
                            font: { family: 'Inter', size: 12 }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function (ctx) {
                                var total = ctx.dataset.data.reduce(function (a, b) { return a + b; }, 0);
                                var pct = total > 0 ? Math.round(ctx.parsed / total * 100) : 0;
                                return ' ' + ctx.label + ': ' + pct + '%';
                            }
                        }
                    }
                }
            }
        });
    }

    // ---------- Recent Orders ----------
    function loadRecentOrders() {
        $.ajax({
            url: API_BASE + '/orders',
            method: 'GET',
            dataType: 'json',
            success: function (orders) {
                var list = Array.isArray(orders) ? orders : (orders.data || orders.Data || []);
                dashboardData.recentOrders = list.slice(0, 5);
                renderRecentOrders(dashboardData.recentOrders);
            },
            error: function () {
                renderRecentOrdersDemo();
            }
        });
    }

    function renderRecentOrders(orders) {
        var $body = $('#recentOrdersBody');
        if (!orders.length) {
            $body.html('<tr><td colspan="5" class="text-center py-4 text-muted">Chưa có đơn hàng</td></tr>');
            return;
        }
        var html = '';
        for (var i = 0; i < orders.length; i++) {
            var o = orders[i];
            html += '<tr>' +
                '<td class="fw-semibold">' + (o.OrderCode || o.orderCode || '#' + o.Id || o.id) + '</td>' +
                '<td>' + (o.CustomerName || o.customerName) + '</td>' +
                '<td class="text-end text-danger fw-bold">' + formatVND(o.TotalAmount || o.totalAmount) + '</td>' +
                '<td>' + getStatusBadge(o.Status != null ? o.Status : o.status) + '</td>' +
                '<td class="text-muted">' + formatDate(o.CreatedAt || o.createdAt) + '</td>' +
                '</tr>';
        }
        $body.html(html);
    }

    function renderRecentOrdersDemo() {
        var demo = [
            { code: 'ORD-20250101', name: 'Nguyễn Văn A', amount: 25990000, status: 2, date: new Date() },
            { code: 'ORD-20250102', name: 'Trần Thị B', amount: 18500000, status: 0, date: new Date() },
            { code: 'ORD-20250103', name: 'Lê Văn C', amount: 42000000, status: 1, date: new Date() },
            { code: 'ORD-20250104', name: 'Phạm Thị D', amount: 15990000, status: 2, date: new Date() },
            { code: 'ORD-20250105', name: 'Hoàng Văn E', amount: 32500000, status: 0, date: new Date() }
        ];
        dashboardData.recentOrders = [];
        for (var i = 0; i < demo.length; i++) {
            dashboardData.recentOrders.push({
                OrderCode: demo[i].code,
                CustomerName: demo[i].name,
                TotalAmount: demo[i].amount,
                Status: demo[i].status,
                CreatedAt: demo[i].date
            });
        }
        var html = '';
        for (var i = 0; i < demo.length; i++) {
            var d = demo[i];
            html += '<tr>' +
                '<td class="fw-semibold">' + d.code + '</td>' +
                '<td>' + d.name + '</td>' +
                '<td class="text-end text-danger fw-bold">' + formatVND(d.amount) + '</td>' +
                '<td>' + getStatusBadge(d.status) + '</td>' +
                '<td class="text-muted">' + formatDate(d.date) + '</td>' +
                '</tr>';
        }
        $('#recentOrdersBody').html(html);
    }

    // ---------- Export Helpers ----------
    function getStatusText(status) {
        var map = { 0: 'Chờ xử lý', 1: 'Đã xác nhận', 2: 'Hoàn thành', 3: 'Đã hủy' };
        return map[status] || String(status);
    }

    function getReportDate() {
        var d = new Date();
        var pad = function (n) { return n < 10 ? '0' + n : n; };
        return pad(d.getDate()) + '/' + pad(d.getMonth() + 1) + '/' + d.getFullYear()
            + ' ' + pad(d.getHours()) + ':' + pad(d.getMinutes());
    }

    function getFileDate() {
        var d = new Date();
        var pad = function (n) { return n < 10 ? '0' + n : n; };
        return d.getFullYear() + pad(d.getMonth() + 1) + pad(d.getDate());
    }

    // ---------- Export CSV ----------
    function exportCSV() {
        var BOM = '\uFEFF';
        var lines = [];

        lines.push('BÁO CÁO DASHBOARD - DESKTOPSHOP');
        lines.push('Ngày xuất: ' + getReportDate());
        lines.push('');

        // Summary
        lines.push('=== TỔNG QUAN ===');
        lines.push('Chỉ số,Giá trị');
        lines.push('Tổng doanh thu,' + (dashboardData.totalRevenue || 0));
        lines.push('Số đơn hàng,' + (dashboardData.totalOrders || 0));
        lines.push('Sản phẩm trong kho,' + (dashboardData.totalProducts || 0));
        lines.push('Sắp hết hàng,' + (dashboardData.lowStockCount || 0));
        lines.push('');

        // Revenue chart
        lines.push('=== DOANH THU 7 NGÀY GẦN NHẤT ===');
        lines.push('Ngày,Doanh thu (VNĐ)');
        for (var i = 0; i < dashboardData.revenueLabels.length; i++) {
            lines.push(dashboardData.revenueLabels[i] + ',' + (dashboardData.revenueValues[i] || 0));
        }
        lines.push('');

        // Category sales
        lines.push('=== DANH MỤC BÁN CHẠY ===');
        lines.push('Danh mục,Số lượng');
        var catTotal = dashboardData.categoryValues.reduce(function (a, b) { return a + b; }, 0);
        for (var i = 0; i < dashboardData.categoryLabels.length; i++) {
            var pct = catTotal > 0 ? Math.round(dashboardData.categoryValues[i] / catTotal * 100) : 0;
            lines.push(dashboardData.categoryLabels[i] + ',' + dashboardData.categoryValues[i] + ' (' + pct + '%)');
        }
        lines.push('');

        // Top selling
        lines.push('=== SẢN PHẨM BÁN CHẠY ===');
        lines.push('STT,Sản phẩm,Đã bán,Doanh thu');
        for (var i = 0; i < dashboardData.topSelling.length; i++) {
            var p = dashboardData.topSelling[i];
            var name = (p.ProductName || p.productName || '').replace(/,/g, ' ');
            lines.push((i + 1) + ',' + name + ',' + (p.TotalSold || p.totalSold || 0) + ',' + (p.TotalRevenue || p.totalRevenue || 0));
        }
        lines.push('');

        // Recent orders
        lines.push('=== ĐƠN HÀNG GẦN ĐÂY ===');
        lines.push('Mã đơn,Khách hàng,Tổng tiền,Trạng thái,Ngày tạo');
        for (var i = 0; i < dashboardData.recentOrders.length; i++) {
            var o = dashboardData.recentOrders[i];
            var code = (o.OrderCode || o.orderCode || '#' + (o.Id || o.id || '')).replace(/,/g, ' ');
            var cName = (o.CustomerName || o.customerName || '').replace(/,/g, ' ');
            var amount = o.TotalAmount || o.totalAmount || 0;
            var status = getStatusText(o.Status != null ? o.Status : o.status);
            var date = formatDate(o.CreatedAt || o.createdAt);
            lines.push(code + ',' + cName + ',' + amount + ',' + status + ',' + date);
        }

        var csv = BOM + lines.join('\r\n');
        var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        var url = URL.createObjectURL(blob);
        var a = document.createElement('a');
        a.href = url;
        a.download = 'BaoCao_Dashboard_' + getFileDate() + '.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);

        showToast('success', 'Đã xuất báo cáo CSV thành công!');
    }

    // ---------- Export PDF ----------
    function exportPDF() {
        try {
            var doc = new jspdf.jsPDF('p', 'mm', 'a4');
            var pageW = doc.internal.pageSize.getWidth();
            var margin = 15;
            var y = 20;
            var lineH = 7;

            // Title
            doc.setFontSize(18);
            doc.setTextColor(26, 115, 232);
            doc.text('BÁO CÁO DASHBOARD', pageW / 2, y, { align: 'center' });
            y += 8;
            doc.setFontSize(11);
            doc.setTextColor(100);
            doc.text('DesktopShop - ' + getReportDate(), pageW / 2, y, { align: 'center' });
            y += 12;

            // Divider
            doc.setDrawColor(200);
            doc.line(margin, y, pageW - margin, y);
            y += 8;

            // Summary
            doc.setFontSize(13);
            doc.setTextColor(30);
            doc.text('TỔNG QUAN', margin, y);
            y += lineH + 2;

            doc.setFontSize(10);
            doc.setTextColor(60);
            var summaryItems = [
                ['Tổng doanh thu:', formatVND(dashboardData.totalRevenue)],
                ['Số đơn hàng:', String(dashboardData.totalOrders)],
                ['Sản phẩm trong kho:', String(dashboardData.totalProducts)],
                ['Sắp hết hàng:', String(dashboardData.lowStockCount)]
            ];
            for (var i = 0; i < summaryItems.length; i++) {
                doc.setFont(undefined, 'bold');
                doc.text(summaryItems[i][0], margin + 2, y);
                doc.setFont(undefined, 'normal');
                doc.text(summaryItems[i][1], margin + 50, y);
                y += lineH;
            }
            y += 4;

            // Revenue chart image
            var revenueCanvas = document.getElementById('revenueChart');
            if (revenueCanvas) {
                doc.setFontSize(13);
                doc.setTextColor(30);
                doc.text('DOANH THU 7 NGÀY GẦN NHẤT', margin, y);
                y += 4;
                var imgData = revenueCanvas.toDataURL('image/png');
                var imgW = pageW - margin * 2;
                var imgH = imgW * 0.45;
                doc.addImage(imgData, 'PNG', margin, y, imgW, imgH);
                y += imgH + 8;
            }

            // Category chart image
            var categoryCanvas = document.getElementById('categoryChart');
            if (categoryCanvas) {
                if (y + 80 > doc.internal.pageSize.getHeight() - 20) {
                    doc.addPage();
                    y = 20;
                }
                doc.setFontSize(13);
                doc.setTextColor(30);
                doc.text('DANH MỤC BÁN CHẠY', margin, y);
                y += 4;
                var catImg = categoryCanvas.toDataURL('image/png');
                var catW = 80;
                var catH = 80;
                doc.addImage(catImg, 'PNG', (pageW - catW) / 2, y, catW, catH);
                y += catH + 8;
            }

            // Top selling table
            if (dashboardData.topSelling.length > 0) {
                if (y + 40 > doc.internal.pageSize.getHeight() - 20) {
                    doc.addPage();
                    y = 20;
                }
                doc.setFontSize(13);
                doc.setTextColor(30);
                doc.text('SẢN PHẨM BÁN CHẠY', margin, y);
                y += lineH + 2;

                // Table header
                doc.setFontSize(9);
                doc.setFillColor(240, 240, 240);
                doc.rect(margin, y - 4, pageW - margin * 2, lineH, 'F');
                doc.setFont(undefined, 'bold');
                doc.setTextColor(50);
                doc.text('STT', margin + 2, y);
                doc.text('Sản phẩm', margin + 15, y);
                doc.text('Đã bán', margin + 105, y);
                doc.text('Doanh thu', margin + 130, y);
                y += lineH;

                doc.setFont(undefined, 'normal');
                doc.setTextColor(70);
                for (var i = 0; i < dashboardData.topSelling.length && i < 10; i++) {
                    var p = dashboardData.topSelling[i];
                    doc.text(String(i + 1), margin + 2, y);
                    doc.text(String(p.ProductName || p.productName || '').substring(0, 40), margin + 15, y);
                    doc.text(String(p.TotalSold || p.totalSold || 0), margin + 105, y);
                    doc.text(formatVND(p.TotalRevenue || p.totalRevenue || 0), margin + 130, y);
                    y += lineH;
                }
                y += 4;
            }

            // Recent orders table
            if (dashboardData.recentOrders.length > 0) {
                if (y + 40 > doc.internal.pageSize.getHeight() - 20) {
                    doc.addPage();
                    y = 20;
                }
                doc.setFontSize(13);
                doc.setTextColor(30);
                doc.text('ĐƠN HÀNG GẦN ĐÂY', margin, y);
                y += lineH + 2;

                doc.setFontSize(9);
                doc.setFillColor(240, 240, 240);
                doc.rect(margin, y - 4, pageW - margin * 2, lineH, 'F');
                doc.setFont(undefined, 'bold');
                doc.setTextColor(50);
                doc.text('Mã đơn', margin + 2, y);
                doc.text('Khách hàng', margin + 35, y);
                doc.text('Tổng tiền', margin + 85, y);
                doc.text('Trạng thái', margin + 120, y);
                doc.text('Ngày tạo', margin + 150, y);
                y += lineH;

                doc.setFont(undefined, 'normal');
                doc.setTextColor(70);
                for (var i = 0; i < dashboardData.recentOrders.length; i++) {
                    if (y + lineH > doc.internal.pageSize.getHeight() - 20) {
                        doc.addPage();
                        y = 20;
                    }
                    var o = dashboardData.recentOrders[i];
                    doc.text(String(o.OrderCode || o.orderCode || '#' + (o.Id || o.id || '')).substring(0, 18), margin + 2, y);
                    doc.text(String(o.CustomerName || o.customerName || '').substring(0, 20), margin + 35, y);
                    doc.text(formatVND(o.TotalAmount || o.totalAmount || 0), margin + 85, y);
                    doc.text(getStatusText(o.Status != null ? o.Status : o.status), margin + 120, y);
                    doc.text(formatDate(o.CreatedAt || o.createdAt), margin + 150, y);
                    y += lineH;
                }
            }

            // Footer
            var pageCount = doc.internal.getNumberOfPages();
            for (var pg = 1; pg <= pageCount; pg++) {
                doc.setPage(pg);
                doc.setFontSize(8);
                doc.setTextColor(150);
                doc.text('DesktopShop Dashboard Report - Trang ' + pg + '/' + pageCount, pageW / 2, doc.internal.pageSize.getHeight() - 10, { align: 'center' });
            }

            doc.save('BaoCao_Dashboard_' + getFileDate() + '.pdf');
            showToast('success', 'Đã xuất báo cáo PDF thành công!');
        } catch (e) {
            console.error('PDF export error:', e);
            showToast('error', 'Lỗi khi xuất PDF. Vui lòng thử lại!');
        }
    }

    // ---------- Export Button Handlers ----------
    $('#btnExportCSV').on('click', function (e) {
        e.preventDefault();
        exportCSV();
    });

    $('#btnExportPDF').on('click', function (e) {
        e.preventDefault();
        exportPDF();
    });

    // ---------- Init ----------
    loadSummary();
    loadRevenueChart();
    loadCategoryChart();
    loadRecentOrders();
});
