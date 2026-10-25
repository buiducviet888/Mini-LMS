# Mini LMS EduConnect 🎓

**Mini LMS EduConnect** là hệ thống quản lý học tập trực tuyến (Learning Management System) được phát triển nhằm kết nối giữa Quản trị viên (Admin), Giảng viên và Sinh viên, hỗ trợ việc quản lý khóa học, phân phối tài liệu, giao/nộp bài tập và theo dõi kết quả học tập.

---

## 🚀 Tính năng chính của hệ thống

### 1. Phân quyền Người dùng & Bảo mật
* **Hệ thống xác thực**: Phân quyền đăng nhập dựa trên Role (`ADMIN`, `GIANG_VIEN`, `SINH_VIEN`).
* **Chốt chặn điều hướng**: Tự động chuyển hướng người dùng về đúng không gian làm việc ngay khi đăng nhập (Bảo vệ tuyến đường / Route Guard).

### 2. Quản trị viên (Admin)
* **Quản lý người dùng**: Thêm mới tài khoản, phân quyền hệ thống và quản lý trạng thái tài khoản.
* **Quản lý học phần**: Tạo khóa học mới, phân công giảng viên phụ trách, kiểm tra sĩ số lớp học.
* **Phê duyệt ghi danh**: Quản lý danh sách sinh viên đăng ký học phần, thực hiện phê duyệt (`DANG_HOC`) hoặc từ chối (`TU_CHOI`) yêu cầu vào lớp.
* **Quản lý học liệu**: Tạo chương mục (Chương 1, 2,...) và tải lên hệ thống tài liệu học tập (PDF, Slide, Video).

### 3. Sinh viên (Student)
* **Đăng ký học phần**: Tra cứu các môn học đang mở và gửi yêu cầu ghi danh (trạng thái mặc định: Chờ duyệt).
* **Không gian học tập (My Courses)**: Theo dõi tiến độ hoàn thành khóa học tự động tính toán từ hệ thống (Course Completed %). Môn học chỉ mở truy cập khi được Admin phê duyệt.
* **Nộp bài tập**: Xem yêu cầu đề bài, đính kèm tệp báo cáo (`.pdf`, `.zip`, `.docx`), hỗ trợ chống lỗi ký tự tiếng Việt khi lưu file.
* **Bảng điểm (Learning results)**: Theo dõi tỷ lệ hoàn thành học phần và điểm số bài tập quá trình được giảng viên chấm trực tiếp từ cơ sở dữ liệu.

---

## 🛠 Công nghệ sử dụng

* **Backend**: ASP.NET Core Web API, Entity Framework Core.
* **Database**: SQL Server.
* **Frontend**: HTML5, CSS3, JavaScript (ES6), Bootstrap 5, Bootstrap Icons.
* **Quản lý mã nguồn**: Git / GitHub.

---

## 📂 Cấu trúc thư mục dự án

```text
MiniLMS_EduConnect/
│
├── Controllers/            # Chứa các Web API Controllers (DangKyKhoaHocs, BaiNops, BaiTaps, KhoaHocs,...)
├── Models/                 # Các Entity Models và DbContext tương tác SQL Server
├── wwwroot/                # Giao diện Frontend (HTML/CSS/JS)
│   ├── index.html          # Không gian quản trị của Admin
│   ├── sinhvien.html       # Không gian học tập của Sinh viên
│   ├── giangvien.html      # Không gian làm việc của Giảng viên
│   ├── login.html          # Trang đăng nhập hệ thống
│   └── uploads/            # Thư mục lưu trữ tệp tin bài tập sinh viên nộp
├── Program.cs              # Cấu hình dịch vụ và Middleware của ứng dụng
└── README.md               # Tài liệu giới thiệu dự án
