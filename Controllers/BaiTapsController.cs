using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLMS.Models;

namespace MiniLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaiTapsController : ControllerBase
    {
        private readonly MiniLmsContext _context;

        public BaiTapsController(MiniLmsContext context)
        {
            _context = context;
        }

        // GET: api/BaiTaps - Lấy toàn bộ bài tập trong hệ thống
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBaiTaps()
        {
            var list = await _context.BaiTaps
                .OrderByDescending(b => b.Id)
                .Select(b => new
                {
                    b.Id,
                    b.IdKhoaHoc,
                    TenKhoaHoc = b.IdKhoaHocNavigation != null ? b.IdKhoaHocNavigation.TenKhoaHoc : "",
                    MaKhoaHoc = b.IdKhoaHocNavigation != null ? b.IdKhoaHocNavigation.MaKhoaHoc : "",
                    b.TieuDe,
                    b.MoTa,
                    b.LoaiBaiTap,
                    b.HanNop,
                    b.DiemToiDa,
                    b.ThoiGianLamPhut,
                    SoLuongBaiNop = b.BaiNops.Count()
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/BaiTaps/khoa-hoc/1 - Lấy danh sách bài tập của một môn học
        [HttpGet("khoa-hoc/{idKhoaHoc}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBaiTapsByCourse(int idKhoaHoc)
        {
            var list = await _context.BaiTaps
                .Where(b => b.IdKhoaHoc == idKhoaHoc)
                .OrderByDescending(b => b.Id)
                .Select(b => new
                {
                    b.Id,
                    b.IdKhoaHoc,
                    b.TieuDe,
                    b.MoTa,
                    b.LoaiBaiTap,
                    b.HanNop,
                    b.DiemToiDa,
                    b.ThoiGianLamPhut
                })
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/BaiTaps - Lưu bài tập vào CSDL
        [HttpPost]
        public async Task<ActionResult<BaiTap>> PostBaiTap([FromBody] BaiTap baiTap)
        {
            if (string.IsNullOrWhiteSpace(baiTap.TieuDe))
            {
                return BadRequest(new { message = "Tiêu đề bài tập không được để trống!" });
            }

            if (baiTap.HanNop == default)
            {
                baiTap.HanNop = DateTime.Now.AddDays(7);
            }

            if (baiTap.DiemToiDa == null || baiTap.DiemToiDa <= 0)
            {
                baiTap.DiemToiDa = 10.0;
            }

            _context.BaiTaps.Add(baiTap);
            await _context.SaveChangesAsync();

            return Ok(baiTap);
        }
    }
}