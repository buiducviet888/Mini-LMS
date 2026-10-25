using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class ChiTietTraLoi
{
    public int Id { get; set; }

    public int IdBaiNop { get; set; }

    public int IdCauHoi { get; set; }

    public int IdLuaChon { get; set; }

    public bool? KetQuaDungSai { get; set; }

    public virtual BaiNop IdBaiNopNavigation { get; set; } = null!;

    public virtual CauHoi IdCauHoiNavigation { get; set; } = null!;

    public virtual LuaChonDapAn IdLuaChonNavigation { get; set; } = null!;
}
