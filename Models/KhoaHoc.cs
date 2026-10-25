using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class KhoaHoc
{
    public int Id { get; set; }

    public string MaKhoaHoc { get; set; } = null!;

    public string TenKhoaHoc { get; set; } = null!;

    public string? MoTa { get; set; }

    public int IdGiangVien { get; set; }

    public string HocKy { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BaiTap> BaiTaps { get; set; } = new List<BaiTap>();

    public virtual ICollection<BaiVietDienDan> BaiVietDienDans { get; set; } = new List<BaiVietDienDan>();

    public virtual ICollection<ChuongMuc> ChuongMucs { get; set; } = new List<ChuongMuc>();

    public virtual ICollection<DangKyKhoaHoc> DangKyKhoaHocs { get; set; } = new List<DangKyKhoaHoc>();

    public virtual NguoiDung IdGiangVienNavigation { get; set; } = null!;
}
