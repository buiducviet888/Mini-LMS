using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLMS.Models;

namespace MiniLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DangKyKhoaHocsController : ControllerBase
    {
        private readonly MiniLmsContext _context;

        public DangKyKhoaHocsController(MiniLmsContext context)
        {
            _context = context;
        }

        // GET: api/DangKyKhoaHocs/sinhvien/3 - Lấy danh sách môn học đã đăng ký của 1 sinh viên
        [HttpGet("sinhvien/{idSinhVien}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCoursesByStudent(int idSinhVien)
        {
            var list = await _context.DangKyKhoaHocs
                .Where(d => d.IdSinhVien == idSinhVien)
                .Include(d => d.IdKhoaHocNavigation)
                .OrderByDescending(d => d.NgayDangKy)
                .Select(d => new
                {
                    d.Id,
                    d.IdKhoaHoc,
                    MaKhoaHoc = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.MaKhoaHoc : "",
                    TenKhoaHoc = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.TenKhoaHoc : "",
                    HocKy = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.HocKy : "",
                    IdGiangVien = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.IdGiangVien : 0,
                    d.TrangThai,
                    d.NgayDangKy
                })
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/DangKyKhoaHocs - Sinh viên ghi danh môn học
        [HttpPost]
        public async Task<ActionResult<DangKyKhoaHoc>> PostDangKy([FromBody] DangKyKhoaHoc dangKy)
        {
            // Kiểm tra sinh viên đã đăng ký môn này chưa
            var exists = await _context.DangKyKhoaHocs
                .AnyAsync(d => d.IdKhoaHoc == dangKy.IdKhoaHoc && d.IdSinhVien == dangKy.IdSinhVien);

            if (exists)
            {
                return BadRequest(new { message = "Bạn đã đăng ký khóa học này rồi!" });
            }

            dangKy.NgayDangKy = DateTime.Now;
            dangKy.TrangThai = "DANG_HOC";

            _context.DangKyKhoaHocs.Add(dangKy);
            await _context.SaveChangesAsync();

            return Ok(dangKy);
        }
    }
}