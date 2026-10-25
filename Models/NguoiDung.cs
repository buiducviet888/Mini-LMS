using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class NguoiDung
{
    public int Id { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HoVaTen { get; set; } = null!;

    public string VaiTro { get; set; } = null!;

    public string? Anh { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BaiNop> BaiNops { get; set; } = new List<BaiNop>();

    public virtual ICollection<BaiVietDienDan> BaiVietDienDans { get; set; } = new List<BaiVietDienDan>();

    public virtual ICollection<BinhLuanDienDan> BinhLuanDienDans { get; set; } = new List<BinhLuanDienDan>();

    public virtual ICollection<DangKyKhoaHoc> DangKyKhoaHocs { get; set; } = new List<DangKyKhoaHoc>();

    public virtual ICollection<KhoaHoc> KhoaHocs { get; set; } = new List<KhoaHoc>();

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
