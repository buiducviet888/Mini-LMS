using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class LuaChonDapAn
{
    public int Id { get; set; }

    public int IdCauHoi { get; set; }

    public string NoiDungDapAn { get; set; } = null!;

    public bool? LaDapAnDung { get; set; }

    public virtual ICollection<ChiTietTraLoi> ChiTietTraLois { get; set; } = new List<ChiTietTraLoi>();

    public virtual CauHoi IdCauHoiNavigation { get; set; } = null!;
}
