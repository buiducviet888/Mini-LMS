using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class DangKyKhoaHoc
{
    public int Id { get; set; }

    public int IdKhoaHoc { get; set; }

    public int IdSinhVien { get; set; }

    public DateTime? NgayDangKy { get; set; }

    public string? TrangThai { get; set; }

    // Đổi thành nullable để không bị chặn validation khi nhận request POST
    public virtual KhoaHoc? IdKhoaHocNavigation { get; set; }

    public virtual NguoiDung? IdSinhVienNavigation { get; set; }
}