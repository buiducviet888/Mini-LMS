using System;
using System.Collections.Generic;

namespace MiniLMS.Models;

public partial class ChuongMuc
{
    public int Id { get; set; }

    public int IdKhoaHoc { get; set; }

    public string TenChuong { get; set; } = null!;

    public int ThuTu { get; set; }

    public virtual ICollection<BaiHoc> BaiHocs { get; set; } = new List<BaiHoc>();

    // Đổi thành nullable để không bị chặn validation khi gửi JSON POST
    public virtual KhoaHoc? IdKhoaHocNavigation { get; set; }
}