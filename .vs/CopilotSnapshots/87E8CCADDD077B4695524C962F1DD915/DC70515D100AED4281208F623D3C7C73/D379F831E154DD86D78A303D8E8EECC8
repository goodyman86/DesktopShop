/* ============================================================
   DesktopShop – Sales / POS JS
   Product grid, cart management, checkout via AJAX
   ============================================================ */

$(function () {
    var API_BASE = '/api';

    // ---------- State ----------
    var allProducts = [];
    var categories = [];
    var cart = []; // { product: {...}, quantity: N }
    var activeCategory = 0;
    var searchKeyword = '';

    // ---------- Load Categories ----------
    function loadCategories() {
        $.ajax({
            url: API_BASE + '/categories',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                categories = Array.isArray(data) ? data : (data.data || data.Data || []);
                renderCategoryFilters();
            },
            error: function () {
                categories = [
                    { Id: 1, Name: 'Gaming' },
                    { Id: 2, Name: 'Văn phòng' },
                    { Id: 3, Name: 'Workstation' }
                ];
                renderCategoryFilters();
            }
        });
    }

    function renderCategoryFilters() {
        var $container = $('#categoryFilters');
        // Keep "Tất cả" button, add category buttons
        for (var i = 0; i < categories.length; i++) {
            var c = categories[i];
            var id = c.Id || c.id;
            var name = c.Name || c.name;
            $container.append(
                '<button class="btn-outline-modern" data-category="' + id + '">' +
                '<i class="fa-solid fa-tag me-1"></i>' + name +
                '</button>'
            );
        }
    }

    // ---------- Load Products ----------
    function loadProducts() {
        $.ajax({
            url: API_BASE + '/products',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                allProducts = Array.isArray(data) ? data : (data.data || data.Data || []);
                renderProducts();
            },
            error: function () {
                allProducts = generateDemoProducts();
                renderProducts();
            }
        });
    }

    function generateDemoProducts() {
        var names = [
            'PC Gaming RTX 4070 Super',
            'PC Gaming RTX 4060 Ti',
            'PC Văn phòng Intel i5-13400',
            'PC Văn phòng AMD Ryzen 5',
            'Workstation Xeon W-2245',
            'Mini PC Intel NUC 13 Pro',
            'PC Gaming RTX 4080 Super',
            'PC Văn phòng i3-13100',
            'PC Gaming RTX 4090',
            'PC All-in-One HP 24'
        ];
        var products = [];
        for (var i = 0; i < names.length; i++) {
            var isGaming = names[i].indexOf('Gaming') >= 0;
            var isWork = names[i].indexOf('Workstation') >= 0;
            products.push({
                Id: i + 1,
                Name: names[i],
                CategoryId: isGaming ? 1 : (isWork ? 3 : 2),
                CategoryName: isGaming ? 'Gaming' : (isWork ? 'Workstation' : 'Văn phòng'),
                CPU: isGaming ? 'Intel Core i7-13700K' : 'Intel Core i5-13400',
                RAM: isGaming ? '32GB DDR5' : '16GB DDR4',
                GPU: isGaming ? 'RTX 4070 Super 12GB' : 'Intel UHD 730',
                Storage: '512GB NVMe SSD',
                Price: isGaming ? (25990000 + i * 3000000) : (12990000 + i * 1000000),
                StockQuantity: Math.floor(Math.random() * 20) + 1,
                MinStockLevel: 3,
                ImageUrl: null,
                IsActive: true
            });
        }
        return products;
    }

    // ---------- Filter & Render Products ----------
    function getFilteredProducts() {
        return allProducts.filter(function (p) {
            var isActive = p.IsActive !== false && p.isActive !== false;
            var matchCategory = activeCategory === 0 ||
                (p.CategoryId || p.categoryId) === activeCategory;
            var matchSearch = !searchKeyword ||
                (p.Name || p.name || '').toLowerCase().indexOf(searchKeyword.toLowerCase()) >= 0;
            return isActive && matchCategory && matchSearch;
        });
    }

    function renderProducts() {
        var filtered = getFilteredProducts();
        var $grid = $('#productGrid');

        if (!filtered.length) {
            $grid.html(
                '<div class="col-12 text-center py-5 text-muted">' +
                '<i class="fa-solid fa-box-open fa-3x mb-3 d-block" style="opacity:.3"></i>' +
                '<p>Không tìm thấy sản phẩm nào</p>' +
                '</div>'
            );
            return;
        }

        var html = '';
        for (var i = 0; i < filtered.length; i++) {
            var p = filtered[i];
            var id = p.Id || p.id;
            var name = p.Name || p.name;
            var cpu = p.CPU || p.cpu || '';
            var ram = p.RAM || p.ram || '';
            var gpu = p.GPU || p.gpu || '';
            var price = p.Price || p.price || 0;
            var stock = p.StockQuantity != null ? p.StockQuantity : p.stockQuantity;
            var minStock = p.MinStockLevel != null ? p.MinStockLevel : (p.minStockLevel || 3);
            var imgUrl = p.ImageUrl || p.imageUrl;
            var outOfStock = stock <= 0;
            var lowStock = stock > 0 && stock <= minStock;

            var stockClass = outOfStock ? 'out' : (lowStock ? 'low' : '');
            var stockText = outOfStock ? 'Hết hàng' : ('Kho: ' + stock);

            html += '<div class="col-xl-3 col-lg-4 col-md-6 animate-in">' +
                '<div class="product-card">' +
                    '<div class="product-card-img">' +
                        (imgUrl
                            ? '<img src="' + imgUrl + '" alt="' + name + '" />'
                            : '<i class="fa-solid fa-desktop placeholder-icon"></i>') +
                    '</div>' +
                    '<div class="product-card-body">' +
                        '<div class="product-name">' + name + '</div>' +
                        '<div class="product-specs">' +
                            (cpu ? '<div><i class="fa-solid fa-microchip"></i>' + cpu + '</div>' : '') +
                            (ram ? '<div><i class="fa-solid fa-memory"></i>' + ram + '</div>' : '') +
                            (gpu ? '<div><i class="fa-solid fa-display"></i>' + gpu + '</div>' : '') +
                        '</div>' +
                    '</div>' +
                    '<div class="product-card-footer">' +
                        '<div>' +
                            '<div class="product-price">' + formatVND(price) + '</div>' +
                            '<div class="product-stock ' + stockClass + '">' + stockText + '</div>' +
                        '</div>' +
                        '<button class="btn-add-cart" data-id="' + id + '"' +
                            (outOfStock ? ' disabled' : '') + '>' +
                            '<i class="fa-solid fa-plus"></i>' +
                            (outOfStock ? 'Hết' : 'Thêm') +
                        '</button>' +
                    '</div>' +
                '</div>' +
            '</div>';
        }
        $grid.html(html);
    }

    // ---------- Category Filter Click ----------
    $(document).on('click', '#categoryFilters .btn-outline-modern', function () {
        $('#categoryFilters .btn-outline-modern').removeClass('active');
        $(this).addClass('active');
        activeCategory = parseInt($(this).data('category')) || 0;
        renderProducts();
    });

    // ---------- Search ----------
    var searchTimer;
    $('#searchInput').on('input', function () {
        clearTimeout(searchTimer);
        var val = $(this).val();
        searchTimer = setTimeout(function () {
            searchKeyword = val;
            renderProducts();
        }, 300);
    });

    // ---------- Add to Cart ----------
    $(document).on('click', '.btn-add-cart', function () {
        var productId = parseInt($(this).data('id'));
        var product = allProducts.find(function (p) {
            return (p.Id || p.id) === productId;
        });
        if (!product) return;

        var stock = product.StockQuantity != null ? product.StockQuantity : product.stockQuantity;

        // Check existing cart item
        var existing = cart.find(function (item) {
            return (item.product.Id || item.product.id) === productId;
        });

        if (existing) {
            if (existing.quantity >= stock) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Hết hàng!',
                    text: 'Số lượng trong kho không đủ.',
                    confirmButtonColor: '#1a73e8'
                });
                return;
            }
            existing.quantity++;
        } else {
            if (stock <= 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Hết hàng!',
                    text: 'Sản phẩm này hiện đã hết hàng.',
                    confirmButtonColor: '#1a73e8'
                });
                return;
            }
            cart.push({ product: product, quantity: 1 });
        }

        showToast('success', 'Đã thêm "' + (product.Name || product.name) + '" vào đơn hàng');
        renderCart();
    });

    // ---------- Render Cart ----------
    function renderCart() {
        var $body = $('#cartBody');
        var $count = $('#cartCount');
        var $total = $('#cartTotal');
        var $btn = $('#btnCheckout');

        if (!cart.length) {
            $body.html(
                '<div class="cart-empty">' +
                '<i class="fa-solid fa-basket-shopping"></i>' +
                '<p>Chưa có sản phẩm nào</p>' +
                '<small>Nhấn "Thêm" để thêm sản phẩm vào đơn hàng</small>' +
                '</div>'
            );
            $count.text('0');
            $total.text('0đ');
            $btn.prop('disabled', true);
            return;
        }

        var totalQty = 0;
        var totalAmount = 0;
        var html = '';

        for (var i = 0; i < cart.length; i++) {
            var item = cart[i];
            var p = item.product;
            var id = p.Id || p.id;
            var name = p.Name || p.name;
            var price = p.Price || p.price || 0;
            var lineTotal = price * item.quantity;
            totalQty += item.quantity;
            totalAmount += lineTotal;

            html += '<div class="cart-item">' +
                '<div class="cart-item-info">' +
                    '<div class="cart-item-name" title="' + name + '">' + name + '</div>' +
                    '<div class="cart-item-price">' + formatVND(price) + '</div>' +
                    '<div class="cart-item-qty">' +
                        '<button class="cart-qty-minus" data-id="' + id + '"><i class="fa-solid fa-minus"></i></button>' +
                        '<span>' + item.quantity + '</span>' +
                        '<button class="cart-qty-plus" data-id="' + id + '"><i class="fa-solid fa-plus"></i></button>' +
                    '</div>' +
                '</div>' +
                '<div class="cart-item-remove" data-id="' + id + '" title="Xóa"><i class="fa-solid fa-xmark"></i></div>' +
            '</div>';
        }

        $body.html(html);
        $count.text(totalQty);
        $total.text(formatVND(totalAmount));
        $btn.prop('disabled', false);
    }

    // ---------- Cart Quantity Controls ----------
    $(document).on('click', '.cart-qty-plus', function () {
        var id = parseInt($(this).data('id'));
        var item = cart.find(function (c) { return (c.product.Id || c.product.id) === id; });
        if (!item) return;
        var stock = item.product.StockQuantity != null ? item.product.StockQuantity : item.product.stockQuantity;
        if (item.quantity >= stock) {
            showToast('warning', 'Đã đạt số lượng tối đa trong kho');
            return;
        }
        item.quantity++;
        renderCart();
    });

    $(document).on('click', '.cart-qty-minus', function () {
        var id = parseInt($(this).data('id'));
        var idx = cart.findIndex(function (c) { return (c.product.Id || c.product.id) === id; });
        if (idx < 0) return;
        cart[idx].quantity--;
        if (cart[idx].quantity <= 0) cart.splice(idx, 1);
        renderCart();
    });

    $(document).on('click', '.cart-item-remove', function () {
        var id = parseInt($(this).data('id'));
        cart = cart.filter(function (c) { return (c.product.Id || c.product.id) !== id; });
        renderCart();
    });

    // ---------- Checkout ----------
    $('#btnCheckout').on('click', function () {
        var customerName = $.trim($('#customerName').val());
        if (!customerName) {
            Swal.fire({
                icon: 'warning',
                title: 'Thiếu thông tin',
                text: 'Vui lòng nhập tên khách hàng.',
                confirmButtonColor: '#1a73e8'
            });
            $('#customerName').focus();
            return;
        }

        if (!cart.length) return;

        // Build order payload matching CreateOrderDto
        var orderData = {
            CustomerName: customerName,
            CustomerPhone: $.trim($('#customerPhone').val()) || null,
            CustomerEmail: $.trim($('#customerEmail').val()) || null,
            ShippingAddress: $.trim($('#shippingAddress').val()) || null,
            Notes: $.trim($('#orderNotes').val()) || null,
            PaymentMethod: parseInt($('#paymentMethod').val()) || 0,
            Items: cart.map(function (item) {
                return {
                    ProductId: item.product.Id || item.product.id,
                    Quantity: item.quantity
                };
            })
        };

        // Calculate total for display
        var totalAmount = 0;
        cart.forEach(function (item) {
            totalAmount += (item.product.Price || item.product.price) * item.quantity;
        });

        // Confirm dialog
        Swal.fire({
            title: 'Xác nhận thanh toán?',
            html:
                '<div style="text-align:left;font-size:.9rem;line-height:1.8">' +
                '<strong>Khách hàng:</strong> ' + customerName + '<br>' +
                (orderData.CustomerPhone ? '<strong>SĐT:</strong> ' + orderData.CustomerPhone + '<br>' : '') +
                (orderData.CustomerEmail ? '<strong>Email:</strong> ' + orderData.CustomerEmail + '<br>' : '') +
                (orderData.ShippingAddress ? '<strong>Địa chỉ:</strong> ' + orderData.ShippingAddress + '<br>' : '') +
                '<strong>Số sản phẩm:</strong> ' + cart.length + ' loại<br>' +
                '<strong>Tổng tiền:</strong> <span style="color:#e02424;font-weight:800">' + formatVND(totalAmount) + '</span>' +
                '</div>',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: '<i class="fa-solid fa-check me-1"></i>Thanh toán',
            cancelButtonText: 'Hủy',
            confirmButtonColor: '#0d9f6e',
            cancelButtonColor: '#6b7280',
            reverseButtons: true
        }).then(function (result) {
            if (result.isConfirmed) {
                submitOrder(orderData, totalAmount);
            }
        });
    });

    function submitOrder(orderData, totalAmount) {
        var $btn = $('#btnCheckout');
        $btn.prop('disabled', true).html('<i class="fa-solid fa-spinner fa-spin me-2"></i>Đang xử lý...');

        $.ajax({
            url: API_BASE + '/orders',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(orderData),
            dataType: 'json',
            success: function (response) {
                var orderCode = response.OrderCode || response.orderCode || response.Id || response.id || '';
                Swal.fire({
                    icon: 'success',
                    title: 'Thanh toán thành công!',
                    html: '<p>Mã đơn hàng: <strong>' + orderCode + '</strong></p>' +
                          '<p>Tổng tiền: <strong class="text-danger">' + formatVND(totalAmount) + '</strong></p>',
                    confirmButtonColor: '#1a73e8'
                });

                // Reset cart and form
                cart = [];
                renderCart();
                $('#customerName').val('');
                $('#customerPhone').val('');
                $('#customerEmail').val('');
                $('#shippingAddress').val('');
                $('#orderNotes').val('');

                // Reload products to get updated stock
                loadProducts();
            },
            error: function (xhr) {
                var msg = 'Có lỗi xảy ra khi tạo đơn hàng.';
                try {
                    var errData = JSON.parse(xhr.responseText);
                    msg = errData.Message || errData.message || msg;
                } catch (e) { }

                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi thanh toán',
                    text: msg,
                    confirmButtonColor: '#1a73e8'
                });
            },
            complete: function () {
                $btn.prop('disabled', false).html('<i class="fa-solid fa-credit-card me-2"></i>Xác nhận thanh toán');
            }
        });
    }

    // ---------- Init ----------
    loadCategories();
    loadProducts();
});
