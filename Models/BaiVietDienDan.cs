using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class BaiVietDienDan
{
    public int Id { get; set; }

    public int IdKhoaHoc { get; set; }

    public int IdTacGia { get; set; }

    public string TieuDe { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public bool? GhimBai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BinhLuanDienDan> BinhLuanDienDans { get; set; } = new List<BinhLuanDienDan>();

    public virtual KhoaHoc IdKhoaHocNavigation { get; set; } = null!;

    public virtual NguoiDung IdTacGiaNavigation { get; set; } = null!;
}
