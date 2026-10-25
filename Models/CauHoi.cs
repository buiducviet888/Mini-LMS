using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class CauHoi
{
    public int Id { get; set; }

    public int IdBaiTap { get; set; }

    public string NoiDungCauHoi { get; set; } = null!;

    public string? LoaiCauHoi { get; set; }

    public double? DiemCauHoi { get; set; }

    public int ThuTu { get; set; }

    public virtual ICollection<ChiTietTraLoi> ChiTietTraLois { get; set; } = new List<ChiTietTraLoi>();

    public virtual BaiTap IdBaiTapNavigation { get; set; } = null!;

    public virtual ICollection<LuaChonDapAn> LuaChonDapAns { get; set; } = new List<LuaChonDapAn>();
}
