using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class ThongBao
{
    public int Id { get; set; }

    public int IdNguoiNhan { get; set; }

    public string TieuDe { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public bool? DaDoc { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual NguoiDung IdNguoiNhanNavigation { get; set; } = null!;
}
