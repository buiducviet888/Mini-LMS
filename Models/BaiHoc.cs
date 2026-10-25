using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class BaiHoc
{
    public int Id { get; set; }

    public int IdChuongMuc { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? LoaiNoiDung { get; set; }

    public string? DuongDanTaiLieu { get; set; }

    public int ThuTu { get; set; }

    public virtual ChuongMuc IdChuongMucNavigation { get; set; } = null!;
}
