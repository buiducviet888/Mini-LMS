using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class BaiTap
{
    public int Id { get; set; }

    public int IdKhoaHoc { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? LoaiBaiTap { get; set; }

    public int? ThoiGianLamPhut { get; set; }

    public DateTime HanNop { get; set; }

    public double? DiemToiDa { get; set; }

    public virtual ICollection<BaiNop> BaiNops { get; set; } = new List<BaiNop>();

    public virtual ICollection<CauHoi> CauHois { get; set; } = new List<CauHoi>();

    // Đổi thành nullable để không bị lỗi validation khi nhận JSON
    public virtual KhoaHoc? IdKhoaHocNavigation { get; set; }
}