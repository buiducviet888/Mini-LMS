using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class BaiNop
{
    public int Id { get; set; }

    public int IdBaiTap { get; set; }

    public int IdSinhVien { get; set; }

    public DateTime? ThoiGianNop { get; set; }

    public string? DuongDanFile { get; set; }

    public double? TongDiem { get; set; }

    public string? NhanXetGiangVien { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<ChiTietTraLoi> ChiTietTraLois { get; set; } = new List<ChiTietTraLoi>();

    public virtual BaiTap IdBaiTapNavigation { get; set; } = null!;

    public virtual NguoiDung IdSinhVienNavigation { get; set; } = null!;
}
