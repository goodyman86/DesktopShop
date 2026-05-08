# Tài Liệu QA Fresher - DesktopShop

> **Dành cho:** QC Fresher mới bắt đầu  
> **Ứng dụng:** DesktopShop - Sàn thương mại điện tử bán máy tính để bàn  
> **Công nghệ:** ASP.NET MVC + .NET 8 API, SQL Server, Bootstrap 5  

---

## 1. Tổng Quan Ứng Dụng

DesktopShop là một website thương mại điện tử bán máy tính để bàn, có hai khu vực chính:

| Khu vực | Đường dẫn | Đối tượng |
|---|---|---|
| **Cửa hàng** | `/` | Khách hàng - mua sắm |
| **Quản trị** | `/Auth/Login` | Admin - quản lý hệ thống |

**Kiến trúc hệ thống:**
- **Frontend:** Razor Views (.cshtml) + Bootstrap 5 + jQuery
- **Backend API:** ASP.NET Core 8 (chạy tại `http://localhost:5001`)
- **Database:** SQL Server (DesktopShopDB)
- **Xác thực:** Session-based cho MVC, JWT cho API

---

## 2. Các Module Chính Cần Test

```
DesktopShop
├── Module 1: Xác thực Khách hàng
├── Module 2: Trang Cửa hàng (Duyệt sản phẩm)
├── Module 3: Giỏ hàng & Thanh toán
├── Module 4: Quản lý Đơn hàng (Khách hàng)
├── Module 5: Xác thực Admin
├── Module 6: Quản lý Sản phẩm (Admin)
├── Module 7: Quản lý Danh mục (Admin)
├── Module 8: Quản lý Đơn hàng (Admin)
└── Module 9: Dashboard & Doanh thu (Admin)
```

---

## 3. Chi Tiết Phạm Vi Test Theo Module

---

### Module 1: Xác Thực Khách Hàng

**Trang liên quan:** `/CustomerAuth/Register`, `/CustomerAuth/Login`, `/CustomerAuth/Profile`, `/CustomerAuth/ForgotPassword`, `/CustomerAuth/ResetPassword`

#### 1.1 Đăng ký tài khoản

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-R01 | Đăng ký thành công | Điền đầy đủ Username, Password hợp lệ, FullName, Email, Phone, Address → Submit | Tạo tài khoản thành công, chuyển đến trang login |
| TC-R02 | Username đã tồn tại | Nhập username đã có trong hệ thống | Hiển thị lỗi "Username đã được sử dụng" |
| TC-R03 | Password yếu | Nhập password < 6 ký tự, không có số, không có chữ hoa, không có ký tự đặc biệt | Hiển thị lỗi yêu cầu mật khẩu mạnh hơn |
| TC-R04 | Bỏ trống trường bắt buộc | Để trống Username hoặc Password hoặc Email → Submit | Hiển thị lỗi validation cho từng trường |
| TC-R05 | Email không hợp lệ | Nhập email sai định dạng (vd: `abc@`) | Hiển thị lỗi định dạng email |

> **Yêu cầu mật khẩu:** Tối thiểu 6 ký tự, có ít nhất 1 chữ số, 1 chữ hoa, 1 ký tự đặc biệt

#### 1.2 Đăng nhập khách hàng

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-L01 | Đăng nhập thành công | Nhập đúng Username + Password → Login | Chuyển về trang chủ, hiển thị tên khách hàng |
| TC-L02 | Sai mật khẩu | Nhập đúng username, sai password | Hiển thị thông báo lỗi đăng nhập |
| TC-L03 | Tài khoản không tồn tại | Nhập username chưa đăng ký | Hiển thị thông báo lỗi |
| TC-L04 | Bỏ trống | Để trống username hoặc password | Hiển thị lỗi validation |
| TC-L05 | Tài khoản bị khóa | Admin deactivate tài khoản → Khách đăng nhập | Không cho phép đăng nhập |

#### 1.3 Quản lý hồ sơ cá nhân

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-P01 | Cập nhật thông tin | Đăng nhập → Vào Profile → Sửa FullName/Phone/Address → Lưu | Thông tin được cập nhật thành công |
| TC-P02 | Upload avatar | Chọn ảnh hợp lệ (jpg/jpeg/png/webp, ≤ 2MB) → Upload | Avatar được cập nhật, hiển thị ảnh mới |
| TC-P03 | Upload avatar quá dung lượng | Chọn ảnh > 2MB | Hiển thị lỗi giới hạn dung lượng |
| TC-P04 | Upload file sai định dạng | Chọn file .gif hoặc .pdf | Hiển thị lỗi định dạng không được hỗ trợ |

#### 1.4 Quên mật khẩu

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-FP01 | Yêu cầu reset password | Nhập email đã đăng ký → Submit | Hiển thị thông báo đã gửi email |
| TC-FP02 | Email không tồn tại | Nhập email chưa đăng ký | Hiển thị thông báo lỗi hoặc email không tồn tại |
| TC-FP03 | Đặt lại mật khẩu | Mở link trong email → Nhập password mới hợp lệ | Đổi mật khẩu thành công, đăng nhập được bằng pass mới |

---

### Module 2: Trang Cửa Hàng (Duyệt Sản Phẩm)

**Trang liên quan:** `/Home/Index`, `/Shop/Index`, `/Shop/Detail`

#### 2.1 Trang chủ

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-H01 | Load trang chủ | Mở `/` | Hiển thị danh sách sản phẩm nổi bật, banner, menu điều hướng |
| TC-H02 | Điều hướng menu | Click các mục trong menu nav | Chuyển đến đúng trang tương ứng |

#### 2.2 Danh sách sản phẩm

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-S01 | Hiển thị danh sách | Vào `/Shop/Index` | Hiển thị danh sách sản phẩm đang active với ảnh, tên, giá |
| TC-S02 | Tìm kiếm sản phẩm | Nhập từ khóa vào ô search → Submit | Hiển thị các sản phẩm khớp từ khóa |
| TC-S03 | Tìm kiếm không có kết quả | Nhập từ khóa không tồn tại | Hiển thị thông báo không tìm thấy |
| TC-S04 | Lọc theo danh mục | Chọn một danh mục từ filter | Chỉ hiển thị sản phẩm thuộc danh mục đó |
| TC-S05 | Xem sản phẩm hết hàng | Sản phẩm có StockQuantity = 0 | Hiển thị trạng thái hết hàng, nút "Thêm vào giỏ" bị disable |

#### 2.3 Chi tiết sản phẩm

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-D01 | Xem chi tiết | Click vào một sản phẩm | Hiển thị đầy đủ: tên, ảnh, giá, CPU, RAM, GPU, Storage, mô tả, tồn kho |
| TC-D02 | Nút thêm vào giỏ | Click "Thêm vào giỏ" | Sản phẩm được thêm vào giỏ, hiển thị thông báo thành công |
| TC-D03 | Sản phẩm hết hàng | Mở chi tiết sản phẩm hết hàng | Không thể thêm vào giỏ |

---

### Module 3: Giỏ Hàng & Thanh Toán

**Trang liên quan:** `/Cart/Index`, `/Cart/Checkout`, `/Cart/OrderSuccess`

#### 3.1 Giỏ hàng

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-C01 | Xem giỏ hàng | Thêm sản phẩm → Vào `/Cart/Index` | Hiển thị danh sách sản phẩm, số lượng, đơn giá, thành tiền, tổng tiền |
| TC-C02 | Thêm sản phẩm vào giỏ | Click "Thêm vào giỏ" từ trang Shop | Số lượng trong giỏ tăng lên, cập nhật tổng tiền |
| TC-C03 | Thay đổi số lượng | Sửa số lượng trong giỏ hàng | Thành tiền và tổng tiền cập nhật chính xác |
| TC-C04 | Số lượng = 0 hoặc âm | Nhập 0 hoặc số âm vào ô số lượng | Hiển thị lỗi hoặc xóa sản phẩm khỏi giỏ |
| TC-C05 | Số lượng vượt tồn kho | Nhập số lượng > StockQuantity | Hiển thị lỗi không đủ hàng |
| TC-C06 | Xóa sản phẩm khỏi giỏ | Click nút xóa bên cạnh sản phẩm | Sản phẩm bị xóa, tổng tiền cập nhật |
| TC-C07 | Giỏ hàng trống | Xóa hết sản phẩm | Hiển thị thông báo giỏ hàng trống, nút tiếp tục mua sắm |
| TC-C08 | Tiếp tục mua sắm | Click "Tiếp tục mua sắm" từ giỏ hàng | Chuyển về trang Shop |

#### 3.2 Thanh toán (Checkout)

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-CH01 | Hiển thị form checkout | Vào `/Cart/Checkout` | Hiển thị form địa chỉ giao hàng, phương thức thanh toán, ghi chú |
| TC-CH02 | Đặt hàng thành công | Điền đầy đủ thông tin → Chọn phương thức TT → Đặt hàng | Đơn hàng tạo thành công, chuyển đến trang xác nhận đơn hàng |
| TC-CH03 | Bỏ trống địa chỉ | Không nhập địa chỉ giao hàng → Submit | Hiển thị lỗi bắt buộc nhập địa chỉ |
| TC-CH04 | Chọn phương thức thanh toán | Chọn lần lượt: Tiền mặt, Chuyển khoản, Thẻ tín dụng, Ví điện tử | Mỗi lựa chọn được ghi nhận đúng |
| TC-CH05 | Không chọn phương thức TT | Bỏ qua bước chọn phương thức → Submit | Hiển thị lỗi yêu cầu chọn phương thức |
| TC-CH06 | Đặt hàng khi hết hàng | Sản phẩm trong giỏ hết stock trước khi đặt | Hiển thị lỗi không đủ hàng, không tạo đơn |
| TC-CH07 | Ghi chú đơn hàng | Nhập ghi chú vào ô Notes → Đặt hàng | Ghi chú được lưu trong đơn hàng |

> **Phương thức thanh toán hỗ trợ:**  
> - Tiền mặt khi nhận hàng (Cash on Delivery)  
> - Chuyển khoản ngân hàng (Bank Transfer)  
> - Thẻ tín dụng (Credit Card)  
> - Ví điện tử (E-Wallet)

#### 3.3 Xác nhận đơn hàng

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-OS01 | Trang thành công | Sau khi đặt hàng | Hiển thị mã đơn hàng (OrderCode), thông báo đặt hàng thành công |
| TC-OS02 | Tồn kho giảm | Kiểm tra database/admin sau khi đặt | StockQuantity của sản phẩm giảm đúng số lượng đã đặt |

---

### Module 4: Quản Lý Đơn Hàng (Khách Hàng)

**Trang liên quan:** `/CustomerAuth/MyOrders`, `/CustomerAuth/OrderDetail`, `/CustomerAuth/EditOrder`

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-O01 | Xem lịch sử đơn hàng | Đăng nhập → Vào "Đơn hàng của tôi" | Hiển thị danh sách tất cả đơn hàng của khách |
| TC-O02 | Xem chi tiết đơn hàng | Click vào một đơn hàng | Hiển thị: mã đơn, sản phẩm, số lượng, giá, địa chỉ, phương thức TT, trạng thái |
| TC-O03 | Sửa đơn đang chờ | Đơn hàng trạng thái **Pending** → Click sửa | Cho phép chỉnh sửa thông tin đơn |
| TC-O04 | Sửa đơn đã xác nhận | Đơn hàng trạng thái **Confirmed** → Thử sửa | Không cho phép sửa |
| TC-O05 | Đơn hàng của người khác | Thử truy cập URL đơn hàng của khách hàng khác | Không hiển thị, trả về lỗi hoặc chuyển hướng |

---

### Module 5: Xác Thực Admin

**Trang liên quan:** `/Auth/Login`, `/Auth/Profile`, `/Auth/ForgotPassword`

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-AL01 | Đăng nhập admin thành công | Nhập đúng Username + Password admin → Login | Chuyển vào Dashboard Admin |
| TC-AL02 | Sai thông tin đăng nhập | Nhập sai username hoặc password | Hiển thị lỗi, không vào được dashboard |
| TC-AL03 | Truy cập dashboard không đăng nhập | Vào `/DashboardView/Index` không qua login | Chuyển hướng về trang login admin |
| TC-AL04 | Khách hàng truy cập admin | Đăng nhập tài khoản khách → Vào URL admin | Không được phép truy cập |
| TC-AL05 | Cập nhật hồ sơ admin | Đăng nhập → Vào Profile → Sửa thông tin | Cập nhật thành công |

---

### Module 6: Quản Lý Sản Phẩm (Admin)

**Trang liên quan:** `/DashboardView/Products`  
**API liên quan:** `GET/POST/PUT/DELETE /api/products`

#### 6.1 Danh sách sản phẩm

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-PM01 | Xem danh sách sản phẩm | Vào trang Products admin | Hiển thị bảng danh sách với: tên, danh mục, giá, tồn kho, trạng thái |
| TC-PM02 | Tìm kiếm sản phẩm | Nhập từ khóa tìm kiếm | Lọc danh sách theo từ khóa |
| TC-PM03 | Cảnh báo hàng sắp hết | Sản phẩm có StockQuantity ≤ MinStockLevel (mặc định 5) | Hiển thị cảnh báo hàng sắp hết |

#### 6.2 Thêm sản phẩm

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-PA01 | Thêm sản phẩm hợp lệ | Nhập đầy đủ: Tên, Danh mục, Giá, Tồn kho, Thông số kỹ thuật → Lưu | Sản phẩm được tạo, xuất hiện trong danh sách |
| TC-PA02 | Bỏ trống tên sản phẩm | Để trống tên → Submit | Hiển thị lỗi bắt buộc nhập tên |
| TC-PA03 | Giá âm hoặc bằng 0 | Nhập Price ≤ 0 | Hiển thị lỗi giá không hợp lệ |
| TC-PA04 | Tồn kho âm | Nhập StockQuantity < 0 | Hiển thị lỗi tồn kho không hợp lệ |
| TC-PA05 | Upload ảnh sản phẩm | Chọn ảnh hợp lệ (jpg/jpeg/png/webp) | Ảnh được upload, hiển thị trong sản phẩm |
| TC-PA06 | Upload ảnh sai định dạng | Chọn file .pdf, .gif | Hiển thị lỗi định dạng không hỗ trợ |

#### 6.3 Sửa sản phẩm

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-PE01 | Sửa thông tin hợp lệ | Sửa tên/giá/tồn kho hợp lệ → Lưu | Thông tin được cập nhật thành công |
| TC-PE02 | Kích hoạt/Vô hiệu hóa | Toggle IsActive của sản phẩm | Sản phẩm ẩn/hiện trên trang shop |
| TC-PE03 | Cập nhật tồn kho | Thay đổi StockQuantity | Tồn kho được cập nhật, ảnh hưởng đến trạng thái hiển thị shop |

#### 6.4 Xóa sản phẩm

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-PD01 | Xóa sản phẩm | Click xóa → Xác nhận | Sản phẩm bị xóa khỏi hệ thống |
| TC-PD02 | Hủy xóa | Click xóa → Chọn Hủy | Sản phẩm không bị xóa |

---

### Module 7: Quản Lý Danh Mục (Admin)

**Trang liên quan:** `/DashboardView/Categories`  
**API liên quan:** `GET/POST/PUT/DELETE /api/categories`

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-CAT01 | Xem danh sách danh mục | Vào trang Categories admin | Hiển thị danh sách danh mục với tên, mô tả, trạng thái |
| TC-CAT02 | Thêm danh mục mới | Nhập Tên, Mô tả → Lưu | Danh mục được tạo, xuất hiện trong danh sách và trang shop |
| TC-CAT03 | Bỏ trống tên danh mục | Để trống tên → Submit | Hiển thị lỗi bắt buộc nhập tên |
| TC-CAT04 | Sửa danh mục | Sửa tên/mô tả → Lưu | Thông tin được cập nhật |
| TC-CAT05 | Vô hiệu hóa danh mục | Toggle IsActive = false | Danh mục không còn hiển thị trên trang shop |
| TC-CAT06 | Xóa danh mục | Click xóa → Xác nhận | Danh mục bị xóa |
| TC-CAT07 | Xóa danh mục có sản phẩm | Xóa danh mục đang có sản phẩm | Kiểm tra hành vi: lỗi hay xóa cascade |

---

### Module 8: Quản Lý Đơn Hàng (Admin)

**Trang liên quan:** `/DashboardView/Orders`  
**API liên quan:** `GET/PUT/PATCH/DELETE /api/orders`

#### 8.1 Xem và tìm kiếm đơn hàng

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-OA01 | Xem danh sách đơn hàng | Vào trang Orders admin | Hiển thị danh sách đơn hàng với mã, khách hàng, tổng tiền, trạng thái |
| TC-OA02 | Lọc đơn theo ngày | Nhập khoảng ngày (from-to) | Chỉ hiển thị đơn hàng trong khoảng thời gian |
| TC-OA03 | Xem chi tiết đơn hàng | Click vào một đơn hàng | Hiển thị đầy đủ thông tin đơn: sản phẩm, số lượng, giá, địa chỉ giao hàng |

#### 8.2 Cập nhật trạng thái đơn hàng

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-OS01 | Pending → Confirmed | Chọn đơn Pending → Cập nhật thành Confirmed | Trạng thái đổi thành Confirmed |
| TC-OS02 | Confirmed → Completed | Chọn đơn Confirmed → Cập nhật thành Completed | Trạng thái đổi thành Completed |
| TC-OS03 | Hủy đơn hàng | Cập nhật trạng thái thành Cancelled | Trạng thái đổi thành Cancelled |
| TC-OS04 | Luồng trạng thái hoàn chỉnh | Tạo đơn → Pending → Confirmed → Completed | Mỗi bước chuyển đúng trạng thái |

> **Luồng trạng thái đơn hàng:**  
> `Pending` → `Confirmed` → `Completed`  
> `Pending` hoặc `Confirmed` → `Cancelled`

#### 8.3 Xóa đơn hàng

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-OD01 | Xóa đơn hàng | Click xóa → Xác nhận | Đơn hàng bị xóa khỏi hệ thống |

---

### Module 9: Dashboard & Doanh Thu (Admin)

**Trang liên quan:** `/DashboardView/Index`, `/DashboardView/Revenue`  
**API liên quan:** `GET /api/dashboard/*`

#### 9.1 Dashboard tổng quan

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-DB01 | Xem dashboard | Đăng nhập admin → Vào Dashboard | Hiển thị 4 KPI: Tổng doanh thu, Tổng đơn hàng, Tổng sản phẩm, Tổng danh mục |
| TC-DB02 | Biểu đồ doanh thu 6 tháng | Xem biểu đồ trên dashboard | Hiển thị biểu đồ doanh số 6 tháng gần nhất |
| TC-DB03 | Cảnh báo hàng sắp hết | Có sản phẩm ≤ MinStockLevel | Hiển thị cảnh báo tồn kho thấp |

#### 9.2 Báo cáo doanh thu

| # | Kịch bản | Bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC-RV01 | Doanh thu theo ngày | Nhập một ngày cụ thể → Xem báo cáo | Hiển thị tổng doanh thu trong ngày đó |
| TC-RV02 | Doanh thu theo khoảng ngày | Nhập ngày bắt đầu và kết thúc | Hiển thị tổng doanh thu trong khoảng thời gian |
| TC-RV03 | Ngày bắt đầu > ngày kết thúc | Nhập from > to | Hiển thị lỗi hoặc kết quả rỗng |
| TC-RV04 | Khoảng ngày xa (> 1 năm) | Nhập khoảng ngày rất lớn | Hệ thống vẫn phải xử lý được, không bị lỗi |

---

## 4. Test Tích Hợp API

Test tích hợp kiểm tra **toàn bộ luồng dữ liệu** từ HTTP request → Controller → Service → Database → HTTP response. Không mock, dùng dữ liệu thật.

---

### 4.1 Chuẩn Bị Môi Trường

#### Bước 1 — Khởi động API server

Mở terminal, chạy lệnh sau:

```bash
cd DesktopShop/src/DesktopShop.API
dotnet run
```

API sẽ chạy tại: `http://localhost:5001`

> Kiểm tra API đã sống: mở trình duyệt vào `http://localhost:5001/swagger` — nếu thấy giao diện Swagger là thành công.

#### Bước 2 — Chọn công cụ test

| Công cụ | Phù hợp | Tải về |
|---|---|---|
| **Swagger UI** | Nhanh, không cần cài, thao tác trực tiếp trên web | Tự có tại `http://localhost:5001/swagger` |
| **Postman** | Lưu collection, chạy nhiều test, xuất báo cáo | https://www.postman.com/downloads |

#### Bước 3 — Thiết lập Postman (nếu dùng Postman)

1. Mở Postman → Click **New Collection** → Đặt tên `DesktopShop API`
2. Click vào collection → Tab **Variables** → Thêm biến:

| Variable | Initial Value | Ghi chú |
|---|---|---|
| `base_url` | `http://localhost:5001` | URL gốc của API |
| `product_id` | `1` | ID sản phẩm dùng để test |
| `category_id` | `1` | ID danh mục dùng để test |
| `order_id` | `1` | ID đơn hàng dùng để test |

3. Trong mỗi request, dùng `{{base_url}}` thay cho `http://localhost:5001`

---

### 4.2 Test Tích Hợp — Categories API

#### IT-CAT-01: Lấy tất cả danh mục

```
Method : GET
URL    : {{base_url}}/api/categories
Headers: Content-Type: application/json
Body   : (không có)
```

**Kết quả mong đợi:**
```json
Status: 200 OK
Body:
[
  {
    "id": 1,
    "name": "Gaming PC",
    "description": "Máy tính gaming hiệu năng cao",
    "isActive": true
  }
]
```

**Điểm kiểm tra:**
- Status code = 200
- Trả về mảng (array), không phải null
- Mỗi phần tử có đủ các trường: `id`, `name`, `isActive`

---

#### IT-CAT-02: Lấy danh mục đang hoạt động

```
Method : GET
URL    : {{base_url}}/api/categories/active
```

**Kết quả mong đợi:**
- Status 200
- Tất cả phần tử trả về có `isActive: true`
- Không có danh mục đã bị vô hiệu hóa

---

#### IT-CAT-03: Tạo danh mục mới

```
Method : POST
URL    : {{base_url}}/api/categories
Headers: Content-Type: application/json
Body (raw JSON):
{
  "name": "Workstation",
  "description": "Máy trạm làm việc chuyên nghiệp",
  "isActive": true
}
```

**Kết quả mong đợi:**
```json
Status: 201 Created
Body:
{
  "id": 3,
  "name": "Workstation",
  "description": "Máy trạm làm việc chuyên nghiệp",
  "isActive": true
}
```

**Sau khi chạy:** Lưu `id` trả về vào biến `category_id` để dùng cho các test tiếp theo.

---

#### IT-CAT-04: Tạo danh mục thiếu tên (validation)

```
Method : POST
URL    : {{base_url}}/api/categories
Body:
{
  "name": "",
  "description": "Test"
}
```

**Kết quả mong đợi:**
- Status = `400 Bad Request`
- Body chứa thông báo lỗi về trường `name`

---

#### IT-CAT-05: Xóa danh mục

```
Method : DELETE
URL    : {{base_url}}/api/categories/{{category_id}}
```

**Kết quả mong đợi:**
- Status = `204 No Content`
- Gọi lại `GET /api/categories/{{category_id}}` → nhận `404 Not Found`

---

### 4.3 Test Tích Hợp — Products API

#### IT-PRD-01: Lấy tất cả sản phẩm

```
Method : GET
URL    : {{base_url}}/api/products
```

**Kết quả mong đợi:**
- Status 200
- Mảng sản phẩm, mỗi phần tử có: `id`, `name`, `price`, `stockQuantity`, `isActive`

---

#### IT-PRD-02: Tìm kiếm sản phẩm

```
Method : GET
URL    : {{base_url}}/api/products/search?keyword=gaming
```

**Kết quả mong đợi:**
- Status 200
- Tất cả sản phẩm trả về có tên hoặc mô tả chứa từ khóa "gaming"

---

#### IT-PRD-03: Lấy sản phẩm sắp hết hàng

```
Method : GET
URL    : {{base_url}}/api/products/low-stock
```

**Kết quả mong đợi:**
- Status 200
- Tất cả sản phẩm trả về có `stockQuantity <= minStockLevel` (mặc định minStockLevel = 5)

---

#### IT-PRD-04: Tạo sản phẩm mới

```
Method : POST
URL    : {{base_url}}/api/products
Headers: Content-Type: application/json
Body:
{
  "name": "Dell XPS Desktop 8960",
  "categoryId": 1,
  "cpu": "Intel Core i7-13700K",
  "ram": "32GB DDR5",
  "gpu": "NVIDIA RTX 4070",
  "storage": "1TB NVMe SSD",
  "price": 35000000,
  "stockQuantity": 10,
  "minStockLevel": 5,
  "description": "Máy tính để bàn hiệu năng cao",
  "isActive": true
}
```

**Kết quả mong đợi:**
```json
Status: 201 Created
Body:
{
  "id": 5,
  "name": "Dell XPS Desktop 8960",
  "price": 35000000,
  "stockQuantity": 10,
  ...
}
```

**Sau khi chạy:** Lưu `id` vào biến `product_id`.

---

#### IT-PRD-05: Tạo sản phẩm với giá âm (validation)

```
Body:
{
  "name": "Test Product",
  "categoryId": 1,
  "price": -1000,
  "stockQuantity": 5
}
```

**Kết quả mong đợi:** Status `400 Bad Request`

---

#### IT-PRD-06: Cập nhật sản phẩm

```
Method : PUT
URL    : {{base_url}}/api/products/{{product_id}}
Body:
{
  "id": {{product_id}},
  "name": "Dell XPS Desktop 8960 - Updated",
  "categoryId": 1,
  "price": 36000000,
  "stockQuantity": 8,
  "minStockLevel": 5,
  "isActive": true
}
```

**Kết quả mong đợi:**
- Status 200
- Trường `name` và `price` trong response đã được cập nhật

---

#### IT-PRD-07: Lấy sản phẩm không tồn tại

```
Method : GET
URL    : {{base_url}}/api/products/99999
```

**Kết quả mong đợi:** Status `404 Not Found`

---

### 4.4 Test Tích Hợp — Orders API

#### IT-ORD-01: Tạo đơn hàng thành công

```
Method : POST
URL    : {{base_url}}/api/orders
Headers: Content-Type: application/json
Body:
{
  "customerName": "Nguyen Van A",
  "customerPhone": "0901234567",
  "customerEmail": "nguyenvana@email.com",
  "shippingAddress": "123 Nguyen Hue, Q1, TP.HCM",
  "notes": "Giao giờ hành chính",
  "paymentMethod": 0,
  "items": [
    {
      "productId": {{product_id}},
      "quantity": 2
    }
  ]
}
```

> **Giá trị paymentMethod:** `0` = Tiền mặt, `1` = Chuyển khoản, `2` = Thẻ tín dụng, `3` = Ví điện tử

**Kết quả mong đợi:**
```json
Status: 201 Created
Body:
{
  "id": 1,
  "orderCode": "ORD-20260508-XXXX",
  "customerName": "Nguyen Van A",
  "totalAmount": 72000000,
  "status": 0,
  "paymentMethod": 0,
  "orderDetails": [...]
}
```

**Kiểm tra thêm sau khi tạo:**
1. Gọi `GET /api/products/{{product_id}}` → `stockQuantity` phải giảm đúng số lượng đã đặt (giảm 2)
2. Lưu `id` đơn hàng vào biến `order_id`

---

#### IT-ORD-02: Tạo đơn hàng vượt tồn kho

```
Body:
{
  "customerName": "Test User",
  "shippingAddress": "Test Address",
  "paymentMethod": 0,
  "items": [
    {
      "productId": {{product_id}},
      "quantity": 99999
    }
  ]
}
```

**Kết quả mong đợi:**
- Status `400 Bad Request`
- Body chứa thông báo lỗi về không đủ tồn kho
- Kiểm tra database: tồn kho sản phẩm KHÔNG thay đổi (transaction rollback)

---

#### IT-ORD-03: Cập nhật trạng thái đơn hàng — Pending → Confirmed

```
Method : PATCH
URL    : {{base_url}}/api/orders/{{order_id}}/status
Body:
{
  "status": 1
}
```

> **Giá trị status:** `0` = Pending, `1` = Confirmed, `2` = Completed, `3` = Cancelled

**Kết quả mong đợi:**
- Status `204 No Content`
- Gọi `GET /api/orders/{{order_id}}` → trường `status` = 1

---

#### IT-ORD-04: Cập nhật trạng thái — Confirmed → Completed

```
Method : PATCH
URL    : {{base_url}}/api/orders/{{order_id}}/status
Body:
{
  "status": 2
}
```

**Kết quả mong đợi:** Status `204 No Content`, đơn hàng có `status` = 2

---

#### IT-ORD-05: Lọc đơn hàng theo khoảng ngày

```
Method : GET
URL    : {{base_url}}/api/orders/by-date?from=2026-01-01&to=2026-12-31
```

**Kết quả mong đợi:**
- Status 200
- Tất cả đơn hàng trả về có `createdAt` nằm trong khoảng 2026-01-01 đến 2026-12-31

---

#### IT-ORD-06: Xóa đơn hàng

```
Method : DELETE
URL    : {{base_url}}/api/orders/{{order_id}}
```

**Kết quả mong đợi:**
- Status `204 No Content`
- Gọi `GET /api/orders/{{order_id}}` → nhận `404 Not Found`

---

### 4.5 Test Tích Hợp — Dashboard API

#### IT-DB-01: Lấy thống kê tổng quan

```
Method : GET
URL    : {{base_url}}/api/dashboard/summary
```

**Kết quả mong đợi:**
```json
Status: 200 OK
Body:
{
  "totalRevenue": 72000000,
  "totalOrders": 5,
  "totalProducts": 10,
  "totalCategories": 3
}
```

**Kiểm tra:** Các con số phải khớp với dữ liệu thực trong database.

---

#### IT-DB-02: Biểu đồ doanh thu 6 tháng

```
Method : GET
URL    : {{base_url}}/api/dashboard/sales-chart?months=6
```

**Kết quả mong đợi:**
- Status 200
- Trả về mảng 6 phần tử, mỗi phần tử có `month` và `revenue`

---

#### IT-DB-03: Doanh thu theo ngày cụ thể

```
Method : GET
URL    : {{base_url}}/api/dashboard/revenue-by-date?date=2026-05-08
```

**Kết quả mong đợi:**
- Status 200
- Trả về tổng doanh thu của ngày 2026-05-08

---

#### IT-DB-04: Doanh thu theo khoảng ngày

```
Method : GET
URL    : {{base_url}}/api/dashboard/revenue-by-range?from=2026-05-01&to=2026-05-31
```

**Kết quả mong đợi:**
- Status 200
- Tổng doanh thu = tổng `totalAmount` của tất cả đơn có trạng thái Completed trong tháng 5/2026

---

### 4.6 Các HTTP Status Code Cần Kiểm Tra

| Mã | Ý nghĩa | Khi nào xảy ra |
|---|---|---|
| 200 OK | Thành công | GET, PUT thành công |
| 201 Created | Tạo thành công | POST thành công |
| 204 No Content | Xử lý xong, không có nội dung trả về | DELETE, PATCH status thành công |
| 400 Bad Request | Dữ liệu không hợp lệ | Validation lỗi, ID không khớp, hết hàng |
| 404 Not Found | Không tìm thấy | ID không tồn tại |
| 500 Internal Server Error | Lỗi server | Ngoại lệ chưa xử lý |

---

### 4.7 Thứ Tự Chạy Test Tích Hợp Đề Xuất

Chạy theo thứ tự sau để tránh phụ thuộc dữ liệu:

```
1. IT-CAT-01  → Xác nhận có danh mục trong DB
2. IT-CAT-03  → Tạo danh mục mới, lấy category_id
3. IT-PRD-04  → Tạo sản phẩm với category_id vừa tạo, lấy product_id
4. IT-PRD-01  → Kiểm tra sản phẩm xuất hiện trong danh sách
5. IT-PRD-02  → Tìm kiếm sản phẩm vừa tạo
6. IT-ORD-01  → Tạo đơn hàng với product_id, lấy order_id
7. IT-PRD-06  → Kiểm tra stockQuantity đã giảm
8. IT-ORD-03  → Cập nhật đơn hàng Pending → Confirmed
9. IT-ORD-04  → Cập nhật đơn hàng Confirmed → Completed
10. IT-DB-01  → Kiểm tra dashboard summary cập nhật đúng
11. IT-ORD-06 → Xóa đơn hàng test
12. IT-PRD-07 → Xóa sản phẩm test (nếu cần)
13. IT-CAT-05 → Xóa danh mục test
```

---

### 4.8 Kiểm Tra Tính Toàn Vẹn Giao Dịch (Transaction)

Đây là test quan trọng nhất — kiểm tra **toàn bộ transaction** khi tạo đơn hàng không được thực hiện nửa chừng.

**Kịch bản:** Đặt hàng với một sản phẩm hợp lệ và một sản phẩm hết hàng cùng lúc.

```
Method : POST
URL    : {{base_url}}/api/orders
Body:
{
  "customerName": "Test Transaction",
  "shippingAddress": "123 Test Street",
  "paymentMethod": 0,
  "items": [
    { "productId": {{product_id_con_hang}}, "quantity": 1 },
    { "productId": {{product_id_het_hang}}, "quantity": 1 }
  ]
}
```

**Kết quả mong đợi:**
- Status `400 Bad Request`
- Không có đơn hàng nào được tạo trong database
- Tồn kho của sản phẩm còn hàng **không thay đổi** (rollback toàn bộ)

---

## 5. Kiểm Tra Giao Diện (UI/UX)

| # | Hạng mục | Nội dung kiểm tra |
|---|---|---|
| UI-01 | Responsive design | Kiểm tra hiển thị trên màn hình Desktop (1920px), Tablet (768px), Mobile (375px) |
| UI-02 | Hiển thị giá | Giá tiền phải có định dạng phân cách số và đơn vị tiền tệ |
| UI-03 | Thông báo lỗi | Mỗi lỗi validation phải hiển thị thông báo rõ ràng, đúng vị trí |
| UI-04 | Loading state | Các nút submit phải disable khi đang xử lý để tránh double-click |
| UI-05 | Confirm dialog | Các hành động xóa phải có hộp thoại xác nhận (SweetAlert2) |
| UI-06 | Hình ảnh sản phẩm | Khi không có ảnh, hiển thị ảnh placeholder thay thế |
| UI-07 | Menu điều hướng | Kiểm tra tất cả liên kết điều hướng hoạt động đúng |
| UI-08 | Số lượng giỏ hàng | Badge số lượng trên icon giỏ hàng cập nhật realtime |

---

## 6. Kiểm Tra Bảo Mật Cơ Bản

| # | Hạng mục | Cách kiểm tra | Kết quả mong đợi |
|---|---|---|---|
| SEC-01 | Truy cập trái phép | Vào `/DashboardView/Products` khi chưa đăng nhập admin | Chuyển hướng về trang login admin |
| SEC-02 | Cách ly tài khoản | Xem đơn hàng của người khác qua URL trực tiếp | Không được phép xem |
| SEC-03 | File upload | Upload file thực thi (.exe, .php) | Bị từ chối |
| SEC-04 | Dung lượng upload | Upload avatar > 2MB | Hiển thị lỗi giới hạn dung lượng |

---

## 7. Quy Trình End-to-End (E2E) Quan Trọng

### Luồng 1: Mua hàng hoàn chỉnh
```
1. Khách hàng đăng ký tài khoản
2. Đăng nhập
3. Vào trang Shop, tìm kiếm sản phẩm
4. Xem chi tiết sản phẩm
5. Thêm vào giỏ hàng
6. Vào giỏ hàng, kiểm tra số lượng
7. Tiến hành Checkout
8. Nhập địa chỉ giao hàng, chọn phương thức thanh toán
9. Đặt hàng
10. Xác nhận đơn hàng thành công (OrderCode hiển thị)
11. Kiểm tra đơn hàng trong "Đơn hàng của tôi"
```

### Luồng 2: Xử lý đơn hàng từ phía Admin
```
1. Admin đăng nhập
2. Vào Dashboard → Xem đơn hàng mới (Pending)
3. Cập nhật trạng thái → Confirmed
4. Cập nhật trạng thái → Completed
5. Kiểm tra tồn kho đã giảm đúng số lượng
```

### Luồng 3: Quản lý sản phẩm mới
```
1. Admin đăng nhập
2. Vào Categories → Tạo danh mục mới
3. Vào Products → Tạo sản phẩm mới thuộc danh mục vừa tạo
4. Upload ảnh sản phẩm
5. Kiểm tra sản phẩm xuất hiện trên trang Shop
6. Vô hiệu hóa sản phẩm → Kiểm tra ẩn khỏi Shop
```

---

## 8. Dữ Liệu Test Mẫu

### Tài khoản test

| Loại | Username | Password | Ghi chú |
|---|---|---|---|
| Admin | `admin` | `Admin@123` | Tài khoản admin mặc định |
| Khách hàng | `testuser1` | `Test@123` | Tài khoản test khách hàng |

> Nếu chưa có tài khoản, hãy đăng ký mới hoặc hỏi team backend để seed data.

### Dữ liệu sản phẩm mẫu

```
Tên: Dell XPS Desktop
Danh mục: [Danh mục đã có trong hệ thống]
CPU: Intel Core i7-13700K
RAM: 32GB DDR5
GPU: NVIDIA RTX 4070
Storage: 1TB NVMe SSD
Giá: 35,000,000
Tồn kho: 10
MinStockLevel: 5
```

---

## 9. Công Cụ Hỗ Trợ Test

| Công cụ | Mục đích | Link |
|---|---|---|
| **Swagger UI** | Test API trực tiếp | `http://localhost:5001/swagger` |
| **Postman** | Test API, tạo collection | https://www.postman.com |
| **Browser DevTools** | Kiểm tra network, console errors | F12 trong trình duyệt |
| **SQL Server Management Studio** | Kiểm tra dữ liệu trong database | - |

---

## 10. Báo Cáo Bug

Khi phát hiện bug, hãy ghi lại theo mẫu sau:

```
**Tiêu đề:** [Module] - Mô tả ngắn gọn vấn đề

**Mức độ:** Critical / High / Medium / Low

**Môi trường:**
- Trình duyệt: Chrome 120 / Firefox 121 / Edge
- Độ phân giải: 1920x1080
- Tài khoản test: testuser1

**Bước tái hiện:**
1. Bước 1
2. Bước 2
3. Bước 3

**Kết quả thực tế:** [Điều gì xảy ra]

**Kết quả mong đợi:** [Điều gì nên xảy ra]

**Screenshot/Video:** [Đính kèm nếu có]
```

### Phân loại mức độ bug

| Mức độ | Định nghĩa | Ví dụ |
|---|---|---|
| **Critical** | Chặn hoàn toàn luồng chính | Không thể đặt hàng, không thể đăng nhập |
| **High** | Tính năng quan trọng bị lỗi | Tồn kho không giảm sau khi đặt hàng |
| **Medium** | Tính năng bị lỗi nhưng có cách bypass | Lọc sản phẩm không hoạt động |
| **Low** | Lỗi nhỏ, giao diện, chính tả | Sai font, màu sắc không đúng |

---

*Tài liệu này được tạo tự động từ phân tích codebase. Cập nhật lần cuối: 2026-05-08*
