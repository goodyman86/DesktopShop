# BÁO CÁO TÀI LIỆU DỰ ÁN DESKTOPSHOP

**Ngày lập báo cáo:** 29/04/2026  
**Phiên bản:** 1.0  
**Dự án:** DesktopShop – Hệ thống bán máy tính để bàn trực tuyến

---

## MỤC LỤC

1. [Tổng quan dự án](#1-tổng-quan-dự-án)
2. [Kiến trúc hệ thống](#2-kiến-trúc-hệ-thống)
3. [Công nghệ sử dụng](#3-công-nghệ-sử-dụng)
4. [Cấu trúc thư mục](#4-cấu-trúc-thư-mục)
5. [Mô hình dữ liệu](#5-mô-hình-dữ-liệu)
6. [Các module chức năng](#6-các-module-chức-năng)
7. [API & Controllers](#7-api--controllers)
8. [Giao diện người dùng](#8-giao-diện-người-dùng)
9. [Bảo mật & Xác thực](#9-bảo-mật--xác-thực)
10. [Cấu hình & Triển khai](#10-cấu-hình--triển-khai)
11. [Phụ thuộc thư viện](#11-phụ-thuộc-thư-viện)

---

## 1. TỔNG QUAN DỰ ÁN

**DesktopShop** là một ứng dụng web thương mại điện tử được phát triển bằng ASP.NET MVC 5, chuyên phục vụ việc mua bán máy tính để bàn (desktop computer). Hệ thống bao gồm hai phần chính:

- **Cổng mua sắm (Shop Portal):** Nơi khách hàng duyệt sản phẩm, thêm vào giỏ hàng và đặt đơn.
- **Trang quản trị (Admin Dashboard):** Nơi người quản trị quản lý sản phẩm, danh mục, đơn hàng và xem báo cáo doanh thu.

**Đặc điểm nổi bật:**
- Ngôn ngữ giao diện: Tiếng Việt
- Hỗ trợ nhiều phương thức thanh toán
- Kiến trúc phân lớp rõ ràng (Domain – Application – Infrastructure)
- Tích hợp WebAPI cho các thao tác AJAX và quản lý backend

---

## 2. KIẾN TRÚC HỆ THỐNG

Dự án áp dụng **kiến trúc phân lớp (Layered Architecture)** kết hợp **Repository Pattern** và **Unit of Work Pattern**:

```
┌─────────────────────────────────────────────┐
│             Presentation Layer               │
│  ASP.NET MVC Controllers + Razor Views       │
│  WebAPI Controllers (AJAX/REST endpoints)    │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│            Application Layer                 │
│  Services (CategoryService, ProductService,  │
│  OrderService, DashboardService)             │
│  DTOs, Validators (FluentValidation)         │
│  Mappings (AutoMapper)                       │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│            Domain Layer                      │
│  Entities: Product, Category, Order,         │
│  OrderDetail, Customer, AdminUser            │
│  Enums: OrderStatus, PaymentMethod           │
│  Repository Interfaces                       │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│          Infrastructure Layer                │
│  Entity Framework Core (DbContext)           │
│  Repository Implementations                  │
│  Unit of Work                                │
│  Identity / Password Seeding                 │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│              Database Layer                  │
│  SQL Server (LocalDB / SQLEXPRESS)           │
│  Database: DesktopShopDB                     │
└─────────────────────────────────────────────┘
```

### Luồng xử lý yêu cầu

```
Trình duyệt → MVC Controller → Service → Repository → DbContext → SQL Server
                     ↓
              Razor View (.cshtml)
                     ↓
              Phản hồi HTML
```

---

## 3. CÔNG NGHỆ SỬ DỤNG

| Thành phần | Công nghệ / Phiên bản |
|---|---|
| Framework chính | ASP.NET MVC 5.2.9 |
| API | ASP.NET Web API 5.2.9 |
| Nền tảng .NET | .NET Framework 4.7.2 |
| ORM | Entity Framework Core 3.1.32 |
| Cơ sở dữ liệu | SQL Server (SQLEXPRESS) |
| Dependency Injection | Microsoft.Extensions.DependencyInjection 3.1.32 |
| Validation | FluentValidation 11.9.0 |
| Object Mapping | AutoMapper 10.1.1 |
| JSON Serialization | Newtonsoft.Json 13.0.3 |
| CSS Framework | Bootstrap 5.2.3 |
| JavaScript Library | jQuery 3.7.0 |
| Icon Font | Font Awesome 6.5.1 |
| Alert UI | SweetAlert2 (CDN) |
| Build Tool | Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1 |
| IDE | Visual Studio 2022 |

---

## 4. CẤU TRÚC THƯ MỤC

```
/DesktopShop/
├── App_Start/                   # Cấu hình khởi động ứng dụng
│   ├── BundleConfig.cs          # Bundle CSS/JS
│   ├── DependencyConfig.cs      # Đăng ký Dependency Injection
│   ├── FilterConfig.cs          # Bộ lọc toàn cục
│   ├── RouteConfig.cs           # Định tuyến URL
│   └── WebApiConfig.cs          # Cấu hình Web API
│
├── Controllers/                 # MVC Controllers (8 controllers)
│   ├── AuthController.cs        # Xác thực Admin
│   ├── CustomerAuthController.cs# Xác thực Khách hàng
│   ├── CartController.cs        # Giỏ hàng
│   ├── ShopController.cs        # Trang mua sắm
│   ├── DashboardViewController.cs # Bảng điều khiển Admin
│   ├── SalesController.cs       # Bán hàng tại quầy (POS)
│   ├── HomeController.cs        # Trang chủ
│   └── ValuesController.cs      # WebAPI test
│
├── Views/                       # Razor Views (29 file .cshtml)
│   ├── Auth/                    # Giao diện xác thực Admin
│   ├── CustomerAuth/            # Giao diện xác thực Khách hàng
│   ├── Shop/                    # Giao diện cửa hàng
│   ├── Cart/                    # Giao diện giỏ hàng & thanh toán
│   ├── DashboardView/           # Giao diện quản trị
│   ├── Sales/                   # Giao diện POS
│   ├── Home/                    # Trang chủ
│   └── Shared/                  # Layout dùng chung
│
├── Filters/
│   └── AdminAuthFilter.cs       # Bộ lọc xác thực Admin
│
├── Content/                     # Tài nguyên CSS
│   ├── bootstrap*.css           # Bootstrap 5.2.3
│   ├── admin.css                # CSS tùy chỉnh cho Admin (17 KB)
│   └── Site.css                 # CSS tùy chỉnh chung
│
├── Scripts/                     # Tài nguyên JavaScript
│   ├── jquery-3.7.0*.js         # jQuery
│   ├── bootstrap.bundle*.js     # Bootstrap Bundle
│   └── app/                     # Script tùy chỉnh
│       ├── dashboard.js         # Logic quản trị (25 KB)
│       ├── sales.js             # Logic POS (17 KB)
│       ├── layout.js            # Xử lý layout (1.6 KB)
│       └── helpers.js           # Hàm tiện ích (3 KB)
│
├── DesktopShop/                 # Các project lớp trong
│   └── src/
│       ├── DesktopShop.Domain/       # Thực thể, Enum, Interface
│       ├── DesktopShop.Application/  # Service, DTO, Validator, Mapping
│       ├── DesktopShop.Infrastructure/ # DbContext, Repository, Identity
│       ├── DesktopShop.API/          # WebAPI Controllers
│       └── DesktopShop.Web/          # Web layer thay thế
│
├── Global.asax.cs               # Khởi tạo ứng dụng
├── Web.config                   # Cấu hình ứng dụng
├── packages.config              # Danh sách NuGet packages
└── WebApplication1.csproj       # File project Visual Studio
```

---

## 5. MÔ HÌNH DỮ LIỆU

### Sơ đồ quan hệ thực thể (ERD)

```
┌──────────────┐       ┌──────────────────┐       ┌──────────────┐
│   Category   │       │     Product       │       │    Order     │
├──────────────┤       ├──────────────────┤       ├──────────────┤
│ Id (PK)      │1─────N│ Id (PK)          │       │ Id (PK)      │
│ Name         │       │ Name             │       │ OrderCode    │
│ Description  │       │ CategoryId (FK)  │       │ CustomerName │
│ IsActive     │       │ CPU              │       │ CustomerPhone│
└──────────────┘       │ RAM              │       │ CustomerEmail│
                       │ GPU              │       │ ShippingAddr │
                       │ Storage          │       │ TotalAmount  │
                       │ Price            │       │ Status       │
                       │ StockQuantity    │       │ PaymentMethod│
                       │ MinStockLevel    │       │ Notes        │
                       │ ImageUrl         │       │ CreatedByUser│
                       │ Description      │       └──────┬───────┘
                       │ IsActive         │              │
                       └────────┬─────────┘             │
                                │                        │
                       ┌────────▼──────────────────────▼┐
                       │         OrderDetail             │
                       ├─────────────────────────────────┤
                       │ Id (PK)                         │
                       │ OrderId (FK)                    │
                       │ ProductId (FK)                  │
                       │ ProductName (denormalized)      │
                       │ UnitPrice                       │
                       │ Quantity                        │
                       │ SubTotal                        │
                       └─────────────────────────────────┘

┌──────────────────────────────────────┐
│             Customer                  │
├──────────────────────────────────────┤
│ Id (PK)                              │
│ Username (unique)                    │
│ PasswordHash                         │
│ FullName                             │
│ Email (unique)                       │
│ Phone                                │
│ Address                              │
│ AvatarUrl                            │
│ Role ("Admin" | "Customer")          │
│ IsActive                             │
└──────────────────────────────────────┘
```

### Mô tả các thực thể

#### **BaseEntity** (Abstract – lớp cha chung)
| Cột | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `Id` | int (PK, auto) | Khóa chính tự tăng |
| `CreatedAt` | DateTime (UTC) | Thời điểm tạo |
| `UpdatedAt` | DateTime? | Thời điểm cập nhật lần cuối |

#### **Customer** (Khách hàng & Quản trị viên)
| Cột | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `Username` | string (unique) | Tên đăng nhập |
| `PasswordHash` | string | Mật khẩu băm SHA-256 |
| `FullName` | string | Họ tên đầy đủ |
| `Email` | string (unique) | Địa chỉ email |
| `Phone` | string? | Số điện thoại |
| `Address` | string? | Địa chỉ |
| `AvatarUrl` | string? | Đường dẫn ảnh đại diện |
| `Role` | string | Vai trò: `"Admin"` hoặc `"Customer"` |
| `IsActive` | bool | Trạng thái kích hoạt |

#### **Product** (Sản phẩm)
| Cột | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `Name` | string | Tên sản phẩm |
| `CategoryId` | int (FK) | Danh mục |
| `CPU` | string? | Thông số CPU (vd: Intel i7-13700K) |
| `RAM` | string? | Dung lượng RAM (vd: 16GB DDR5) |
| `GPU` | string? | Card đồ họa (vd: RTX 4070) |
| `Storage` | string? | Lưu trữ (vd: 512GB SSD) |
| `Price` | decimal | Giá bán |
| `StockQuantity` | int | Số lượng tồn kho |
| `MinStockLevel` | int (default: 5) | Mức tồn kho tối thiểu |
| `ImageUrl` | string? | Đường dẫn hình ảnh |
| `Description` | string? | Mô tả sản phẩm |
| `IsActive` | bool | Trạng thái hiển thị |

#### **Category** (Danh mục)
| Cột | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `Name` | string | Tên danh mục |
| `Description` | string? | Mô tả danh mục |
| `IsActive` | bool | Trạng thái kích hoạt |

#### **Order** (Đơn hàng)
| Cột | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `OrderCode` | string (max 50) | Mã đơn hàng (vd: ORD-20240101120000-XXX) |
| `CustomerName` | string | Tên người nhận |
| `CustomerPhone` | string? | SĐT người nhận |
| `CustomerEmail` | string? | Email người nhận |
| `ShippingAddress` | string? | Địa chỉ giao hàng |
| `TotalAmount` | decimal | Tổng giá trị đơn hàng |
| `Status` | OrderStatus (enum) | Trạng thái đơn hàng |
| `PaymentMethod` | PaymentMethod (enum) | Phương thức thanh toán |
| `Notes` | string? | Ghi chú đơn hàng |
| `CreatedByUserId` | string? | ID khách hàng tạo đơn |

#### **OrderDetail** (Chi tiết đơn hàng)
| Cột | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `OrderId` | int (FK) | ID đơn hàng |
| `ProductId` | int (FK) | ID sản phẩm |
| `ProductName` | string | Tên SP (lưu dự phòng) |
| `UnitPrice` | decimal | Đơn giá tại thời điểm đặt |
| `Quantity` | int | Số lượng |
| `SubTotal` | decimal | Thành tiền (UnitPrice × Quantity) |

### Enum

**OrderStatus** (Trạng thái đơn hàng):
| Giá trị | Tên | Ý nghĩa |
|---|---|---|
| 0 | Pending | Chờ xác nhận |
| 1 | Confirmed | Đã xác nhận |
| 2 | Completed | Hoàn thành |
| 3 | Cancelled | Đã hủy |

**PaymentMethod** (Phương thức thanh toán):
| Giá trị | Tên | Ý nghĩa |
|---|---|---|
| 0 | CashOnDelivery | Thanh toán khi nhận hàng (COD) |
| 1 | BankTransfer | Chuyển khoản ngân hàng |
| 2 | CreditCard | Thẻ tín dụng |
| 3 | EWallet | Ví điện tử |

---

## 6. CÁC MODULE CHỨC NĂNG

### 6.1. Module Cửa hàng (Shop Module)

**Mô tả:** Giao diện mua sắm dành cho khách hàng.

**Chức năng:**
- Duyệt danh sách sản phẩm với phân trang (8 sản phẩm/trang)
- Tìm kiếm sản phẩm theo tên (keyword)
- Lọc sản phẩm theo danh mục
- Sắp xếp theo giá (tăng dần / giảm dần)
- Xem chi tiết sản phẩm với thông số kỹ thuật
- Hiển thị sản phẩm liên quan (cùng danh mục)

**URL:** `/Shop/Index`, `/Shop/Detail/{id}`

---

### 6.2. Module Giỏ hàng & Thanh toán (Cart Module)

**Mô tả:** Quản lý giỏ hàng và quy trình đặt hàng.

**Chức năng:**
- Thêm sản phẩm vào giỏ hàng (lưu trong Session)
- Cập nhật số lượng sản phẩm trong giỏ
- Xóa sản phẩm khỏi giỏ
- Hiển thị số lượng sản phẩm trên navbar (AJAX)
- Thanh toán: nhập thông tin giao hàng, chọn phương thức thanh toán
- Xác nhận đặt hàng thành công với mã đơn hàng

**URL:** `/Cart/Index`, `/Cart/Checkout`, `/Cart/OrderSuccess`

> **Lưu ý:** Giỏ hàng được lưu trong Session (không lưu database). Dữ liệu sẽ mất khi phiên đăng nhập hết hạn.

---

### 6.3. Module Xác thực Khách hàng (Customer Auth Module)

**Mô tả:** Đăng ký, đăng nhập và quản lý tài khoản khách hàng.

**Chức năng:**
- Đăng ký tài khoản mới
- Đăng nhập / Đăng xuất
- Xem và cập nhật hồ sơ cá nhân
- Đổi mật khẩu
- Cập nhật ảnh đại diện
- Quên mật khẩu / Đặt lại mật khẩu
- Xem lịch sử đơn hàng
- Xem chi tiết từng đơn hàng
- Chỉnh sửa / Hủy đơn hàng (nếu chưa xác nhận)

**URL:** `/CustomerAuth/*`

---

### 6.4. Module Quản trị (Admin Dashboard Module)

**Mô tả:** Bảng điều khiển quản lý toàn bộ hệ thống (yêu cầu quyền Admin).

**Chức năng:**

| Trang | Chức năng |
|---|---|
| Dashboard | Tổng quan: tổng sản phẩm, đơn hàng, doanh thu, khách hàng |
| Quản lý Sản phẩm | Thêm / Sửa / Xóa / Tìm kiếm sản phẩm |
| Quản lý Danh mục | Thêm / Sửa / Xóa danh mục |
| Quản lý Đơn hàng | Xem / Cập nhật trạng thái đơn hàng |
| Doanh thu | Biểu đồ doanh thu theo ngày/tháng/năm |

**URL:** `/DashboardView/*`

---

### 6.5. Module Bán hàng tại quầy (POS – Sales Module)

**Mô tả:** Giao diện bán hàng trực tiếp tại cửa hàng.

**Chức năng:**
- Tìm kiếm và chọn sản phẩm
- Quản lý giỏ hàng POS
- Xử lý thanh toán
- In biên lai

**URL:** `/Sales/Index`

---

### 6.6. Module Xác thực Quản trị (Admin Auth Module)

**Mô tả:** Đăng nhập và quản lý tài khoản Admin.

**Chức năng:**
- Đăng nhập Admin
- Đăng xuất
- Xem và cập nhật hồ sơ Admin
- Đổi mật khẩu Admin
- Cập nhật ảnh đại diện
- Quên / Đặt lại mật khẩu

**URL:** `/Auth/*`

---

## 7. API & CONTROLLERS

### 7.1. MVC Controllers

| Controller | Route | Bảo vệ bởi | Mô tả |
|---|---|---|---|
| `HomeController` | `/Home/*` | Không | Trang chủ |
| `ShopController` | `/Shop/*` | Không | Cửa hàng |
| `CartController` | `/Cart/*` | Một phần (Checkout) | Giỏ hàng |
| `CustomerAuthController` | `/CustomerAuth/*` | Không | Auth khách hàng |
| `AuthController` | `/Auth/*` | Không | Auth admin |
| `DashboardViewController` | `/DashboardView/*` | `AdminAuthFilter` | Bảng điều khiển Admin |
| `SalesController` | `/Sales/*` | `AdminAuthFilter` | POS |

### 7.2. Web API Endpoints (AJAX)

Các API endpoint được gọi qua AJAX bởi `dashboard.js` và `sales.js`:

#### Sản phẩm
| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/api/products` | Lấy danh sách sản phẩm |
| GET | `/api/products/{id}` | Lấy chi tiết sản phẩm |
| POST | `/api/products` | Tạo sản phẩm mới |
| PUT | `/api/products/{id}` | Cập nhật sản phẩm |
| DELETE | `/api/products/{id}` | Xóa sản phẩm |

#### Danh mục
| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/api/categories` | Lấy danh sách danh mục |
| POST | `/api/categories` | Tạo danh mục mới |
| PUT | `/api/categories/{id}` | Cập nhật danh mục |
| DELETE | `/api/categories/{id}` | Xóa danh mục |

#### Đơn hàng
| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/api/orders` | Lấy danh sách đơn hàng |
| GET | `/api/orders/{id}` | Lấy chi tiết đơn hàng |
| PUT | `/api/orders/{id}/status` | Cập nhật trạng thái đơn |
| DELETE | `/api/orders/{id}` | Xóa đơn hàng |

#### Dashboard
| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/api/dashboard/summary` | Thống kê tổng quan |
| GET | `/api/dashboard/revenue` | Dữ liệu doanh thu |

#### Giỏ hàng (MVC AJAX)
| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/Cart/GetCartCount` | Lấy số lượng item trong giỏ |

### 7.3. Cấu hình Web API

- **Định dạng phản hồi:** JSON duy nhất (loại bỏ XML)
- **Tên thuộc tính:** camelCase
- **Xử lý vòng lặp tham chiếu:** Bỏ qua (Ignore)
- **Định tuyến:** Attribute-based routing + Default route `api/{controller}/{id}`

---

## 8. GIAO DIỆN NGƯỜI DÙNG

### 8.1. Layout chung

Hệ thống sử dụng 3 layout chính:

| Layout | File | Dùng cho |
|---|---|---|
| Layout mặc định | `_Layout.cshtml` | Trang chủ |
| Layout cửa hàng | `_ShopLayout.cshtml` | Shop, Cart, CustomerAuth |
| Layout Admin | `_AdminLayout.cshtml` | Dashboard, Auth, Sales |

### 8.2. Layout cửa hàng (`_ShopLayout.cshtml`)

- Thanh điều hướng Bootstrap 5 với gradient màu (`#1a1a2e → #16213e`)
- Hiển thị số lượng sản phẩm trong giỏ (cập nhật qua AJAX)
- Menu responsive cho thiết bị di động
- Liên kết đăng nhập/đăng ký hoặc tên người dùng (khi đã đăng nhập)
- CDN: Bootstrap 5.3.2, Font Awesome 6.5.1

### 8.3. Layout Admin (`_AdminLayout.cshtml`)

- Thanh sidebar điều hướng với icon (Font Awesome)
- Đánh dấu trang đang active
- Tích hợp SweetAlert2 cho thông báo
- Giao diện responsive

### 8.4. Trang cửa hàng (`Shop/Index.cshtml`)

- Grid sản phẩm: 3-4 cột (responsive)
- Thanh tìm kiếm theo từ khóa
- Bộ lọc danh mục dạng pill buttons
- Sắp xếp theo giá (tăng/giảm dần)
- Phân trang với thông tin số lượng sản phẩm

### 8.5. Trang thanh toán (`Cart/Checkout.cshtml`)

Form thanh toán với:
- Thông tin người nhận (tên, SĐT, email, địa chỉ)
- 4 phương thức thanh toán:
  - Thanh toán khi nhận hàng (COD)
  - Chuyển khoản ngân hàng
  - Thẻ tín dụng
  - Ví điện tử
- Ghi chú đơn hàng

---

## 9. BẢO MẬT & XÁC THỰC

### 9.1. Cơ chế xác thực

Hệ thống sử dụng **Session-based Authentication** (không dùng ASP.NET Identity):

**Luồng đăng nhập Admin:**
```
POST /Auth/Login
    → Truy vấn bảng Customers (Role = "Admin")
    → Xác minh PasswordHash (SHA-256)
    → Lưu Session: AdminId, AdminName, AdminUsername
    → Chuyển hướng → /DashboardView/Index
```

**Luồng đăng nhập Khách hàng:**
```
POST /CustomerAuth/Login
    → Truy vấn bảng Customers (Role = "Customer" hoặc null)
    → Xác minh PasswordHash (SHA-256)
    → Lưu Session: CustomerId, CustomerName, CustomerUsername
    → Chuyển hướng → /Shop/Index
```

### 9.2. Phân quyền (Authorization)

| Bộ lọc | Loại | Bảo vệ | Kiểm tra |
|---|---|---|---|
| `AdminAuthFilter` | ActionFilterAttribute | DashboardView, Sales | `Session["AdminId"]` != null |

Khi truy cập không có quyền → Chuyển hướng về `/Auth/Login`

### 9.3. Băm mật khẩu

- Thuật toán: **SHA-256**
- Thực hiện trong: `IdentitySeed.cs` (Infrastructure layer)
- Tài khoản admin mặc định: username `admin`, password `admin123`

### 9.4. Các lưu ý bảo mật

- Mật khẩu yêu cầu tối thiểu 6 ký tự
- Username và Email được kiểm tra duy nhất khi đăng ký
- Xác thực phía client được bật (`ClientValidationEnabled = true`)

---

## 10. CẤU HÌNH & TRIỂN KHAI

### 10.1. Chuỗi kết nối cơ sở dữ liệu

```xml
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=LT-BNKBH0211\SQLEXPRESS;
                         Database=DesktopShopDB;
                         Trusted_Connection=True;
                         MultipleActiveResultSets=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 10.2. Cấu hình ứng dụng (AppSettings)

| Khóa | Giá trị | Mô tả |
|---|---|---|
| `webpages:Version` | 3.0.0.0 | Phiên bản WebPages |
| `webpages:Enabled` | false | Tắt Razor WebPages trực tiếp |
| `ClientValidationEnabled` | true | Bật validation phía client |
| `UnobtrusiveJavaScriptEnabled` | true | Bật unobtrusive JS |

### 10.3. Khởi tạo cơ sở dữ liệu

Khi ứng dụng khởi động (`Global.asax.cs`), hệ thống tự động:
1. Kết nối vào SQL Server `master`
2. Tạo database `DesktopShopDB` nếu chưa tồn tại
3. Tạo các bảng cần thiết (`Customers`, `AdminUsers`, `Products`, `Categories`, `Orders`, `OrderDetails`)
4. Thêm/cập nhật cột theo schema mới nhất
5. Seed tài khoản Admin mặc định (`admin` / `admin123`)

### 10.4. Định tuyến URL

| Route | Controller/Action | Mô tả |
|---|---|---|
| `/` (mặc định) | `Shop/Index` | Trang chủ = trang cửa hàng |
| `/{controller}/{action}/{id?}` | Theo tên | Route chung |
| `/api/{controller}/{id?}` | Web API | REST API |

### 10.5. Bundle & Minification

| Bundle | Nội dung |
|---|---|
| `~/bundles/jquery` | jquery-3.7.0.js |
| `~/bundles/modernizr` | modernizr-2.8.3.js |
| `~/bundles/bootstrap` | bootstrap.js |
| `~/Content/css` | bootstrap.css + site.css |

### 10.6. Mã hóa

- Toàn bộ ứng dụng sử dụng **UTF-8**
- Cấu hình trong `Web.config`:
  - `requestEncoding = "utf-8"`
  - `responseEncoding = "utf-8"`
  - `fileEncoding = "utf-8"`

---

## 11. PHỤ THUỘC THƯ VIỆN

### Nhóm ASP.NET MVC/WebAPI
| Package | Phiên bản |
|---|---|
| Microsoft.AspNet.Mvc | 5.2.9 |
| Microsoft.AspNet.WebApi | 5.2.9 |
| Microsoft.AspNet.WebPages | 3.2.9 |
| Microsoft.AspNet.Razor | 3.2.9 |
| Microsoft.Web.Infrastructure | 2.0.0 |

### Nhóm Entity Framework
| Package | Phiên bản |
|---|---|
| Microsoft.EntityFrameworkCore | 3.1.32 |
| Microsoft.EntityFrameworkCore.SqlServer | 3.1.32 |
| Microsoft.EntityFrameworkCore.Relational | 3.1.32 |
| Microsoft.EntityFrameworkCore.Abstractions | 3.1.32 |

### Nhóm Dependency Injection & Extensions
| Package | Phiên bản |
|---|---|
| Microsoft.Extensions.DependencyInjection | 3.1.32 |
| Microsoft.Extensions.Logging | 3.1.32 |
| Microsoft.Extensions.Configuration | 3.1.32 |
| Microsoft.Extensions.Options | 3.1.32 |
| Microsoft.Extensions.Caching.Memory | 3.1.32 |

### Nhóm tiện ích
| Package | Phiên bản | Mục đích |
|---|---|---|
| Newtonsoft.Json | 13.0.3 | JSON serialization |
| FluentValidation | 11.9.0 | Validation nghiệp vụ |
| AutoMapper | 10.1.1 | Ánh xạ đối tượng |
| Bootstrap | 5.2.3 | CSS Framework |
| jQuery | 3.7.0 | JavaScript Library |
| Modernizr | 2.8.3 | Tương thích trình duyệt |

---

## PHỤ LỤC: SƠ ĐỒ LUỒNG NGƯỜI DÙNG

### Luồng mua hàng (Customer Flow)

```
Truy cập trang chủ (/Shop/Index)
    │
    ├─ Duyệt / Tìm kiếm / Lọc sản phẩm
    │
    ├─ Xem chi tiết sản phẩm (/Shop/Detail/{id})
    │
    ├─ Thêm vào giỏ hàng (/Cart/Add)
    │
    ├─ Xem giỏ hàng (/Cart/Index)
    │
    ├─ Thanh toán (/Cart/Checkout)
    │       │
    │       └─ Yêu cầu đăng nhập → /CustomerAuth/Login
    │
    └─ Xác nhận đặt hàng (/Cart/OrderSuccess)
```

### Luồng quản trị (Admin Flow)

```
Đăng nhập Admin (/Auth/Login)
    │
    └─ Bảng điều khiển (/DashboardView/Index)
            │
            ├─ Quản lý Sản phẩm (/DashboardView/Products)
            │       └─ CRUD qua AJAX ↔ WebAPI
            │
            ├─ Quản lý Danh mục (/DashboardView/Categories)
            │       └─ CRUD qua AJAX ↔ WebAPI
            │
            ├─ Quản lý Đơn hàng (/DashboardView/Orders)
            │       └─ Cập nhật trạng thái qua AJAX ↔ WebAPI
            │
            ├─ Báo cáo Doanh thu (/DashboardView/Revenue)
            │       └─ Biểu đồ qua AJAX ↔ WebAPI
            │
            └─ Bán hàng tại quầy (/Sales/Index)
```

---

*Tài liệu này được tạo tự động từ phân tích mã nguồn dự án DesktopShop.*  
*Ngày tạo: 29/04/2026*
