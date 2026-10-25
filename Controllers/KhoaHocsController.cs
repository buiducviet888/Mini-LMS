using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLMS.Models;

namespace MiniLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhoaHocsController : ControllerBase
    {
        private readonly MiniLmsContext _context;

        public KhoaHocsController(MiniLmsContext context)
        {
            _context = context;
        }

        // GET: api/KhoaHocs - Lấy danh sách khóa học kèm số lượng sinh viên đã đăng ký
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetKhoaHocs()
        {
            var list = await _context.KhoaHocs
                .OrderByDescending(k => k.Id)
                .Select(k => new
                {
                    k.Id,
                    k.MaKhoaHoc,
                    k.TenKhoaHoc,
                    k.HocKy,
                    k.IdGiangVien,
                    k.MoTa,
                    k.NgayTao,
                    // Đếm sĩ số sinh viên ghi danh vào môn
                    SoLuongSinhVien = k.DangKyKhoaHocs.Count()
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/KhoaHocs/1 - Lấy chi tiết một khóa học
        [HttpGet("{id}")]
        public async Task<ActionResult<KhoaHoc>> GetKhoaHoc(int id)
        {
            var khoaHoc = await _context.KhoaHocs.FindAsync(id);

            if (khoaHoc == null)
            {
                return NotFound(new { message = "Không tìm thấy khóa học!" });
            }

            return Ok(khoaHoc);
        }

        // GET: api/KhoaHocs/1/sinh-vien - Lấy thông tin chi tiết từng sinh viên trong môn
        [HttpGet("{id}/sinh-vien")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentsInCourse(int id)
        {
            var students = await _context.DangKyKhoaHocs
                .Where(d => d.IdKhoaHoc == id)
                .Include(d => d.IdSinhVienNavigation)
                .OrderByDescending(d => d.NgayDangKy)
                .Select(d => new
                {
                    d.Id,
                    IdSinhVien = d.IdSinhVien,
                    TenDangNhap = d.IdSinhVienNavigation != null ? d.IdSinhVienNavigation.TenDangNhap : "",
                    HoVaTen = d.IdSinhVienNavigation != null ? d.IdSinhVienNavigation.HoVaTen : "",
                    Email = d.IdSinhVienNavigation != null ? d.IdSinhVienNavigation.Email : "",
                    NgayDangKy = d.NgayDangKy,
                    TrangThai = d.TrangThai
                })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/KhoaHocs/1/tai-lieu - Lấy danh sách chương mục và tài liệu/bài học trong môn
        [HttpGet("{id}/tai-lieu")]
        public async Task<ActionResult<IEnumerable<object>>> GetCourseMaterials(int id)
        {
            var chapters = await _context.ChuongMucs
                .Where(c => c.IdKhoaHoc == id)
                .OrderBy(c => c.ThuTu)
                .Select(c => new
                {
                    c.Id,
                    c.TenChuong,
                    c.ThuTu,
                    BaiHocs = c.BaiHocs
                        .OrderBy(b => b.ThuTu)
                        .Select(b => new
                        {
                            b.Id,
                            b.TieuDe,
                            b.LoaiNoiDung, // PDF, SLIDE, VIDEO, VAN_BAN
                            b.DuongDanTaiLieu,
                            b.ThuTu
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(chapters);
        }

        // POST: api/KhoaHocs/chuong-muc - Admin tạo chương mục mới từ web
        [HttpPost("chuong-muc")]
        public async Task<ActionResult<ChuongMuc>> PostChuongMuc([FromBody] ChuongMuc chuongMuc)
        {
            _context.ChuongMucs.Add(chuongMuc);
            await _context.SaveChangesAsync();
            return Ok(chuongMuc);
        }

        // POST: api/KhoaHocs/bai-hoc - Admin đăng tải bài học/tài liệu vào chương mục từ web
        [HttpPost("bai-hoc")]
        public async Task<ActionResult<BaiHoc>> PostBaiHoc([FromBody] BaiHoc baiHoc)
        {
            _context.BaiHocs.Add(baiHoc);
            await _context.SaveChangesAsync();
            return Ok(baiHoc);
        }

        // POST: api/KhoaHocs - Admin tạo khóa học mới
        [HttpPost]
        public async Task<ActionResult<KhoaHoc>> PostKhoaHoc([FromBody] KhoaHoc khoaHoc)
        {
            var exists = await _context.KhoaHocs.AnyAsync(k => k.MaKhoaHoc == khoaHoc.MaKhoaHoc);
            if (exists)
            {
                return BadRequest(new { message = "Mã môn học này đã tồn tại!" });
            }

            khoaHoc.NgayTao = DateTime.Now;

            _context.KhoaHocs.Add(khoaHoc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetKhoaHoc), new { id = khoaHoc.Id }, khoaHoc);
        }

        // DELETE: api/KhoaHocs/5 - Xóa khóa học
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhoaHoc(int id)
        {
            var khoaHoc = await _context.KhoaHocs.FindAsync(id);
            if (khoaHoc == null)
            {
                return NotFound(new { message = "Khóa học không tồn tại!" });
            }

            _context.KhoaHocs.Remove(khoaHoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}