using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class BinhLuanDienDan
{
    public int Id { get; set; }

    public int IdBaiViet { get; set; }

    public int IdTacGia { get; set; }

    public string NoiDung { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public virtual BaiVietDienDan IdBaiVietNavigation { get; set; } = null!;

    public virtual NguoiDung IdTacGiaNavigation { get; set; } = null!;
}
