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

    public virtual KhoaHoc IdKhoaHocNavigation { get; set; } = null!;

    public virtual NguoiDung IdSinhVienNavigation { get; set; } = null!;
}
