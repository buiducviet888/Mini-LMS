using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLMS.Models;

namespace MiniLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungsController : ControllerBase
    {
        private readonly MiniLmsContext _context;

        public NguoiDungsController(MiniLmsContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public string TenDangNhap { get; set; } = string.Empty;
            public string MatKhau { get; set; } = string.Empty;
        }

        // POST: api/NguoiDungs/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => (u.TenDangNhap == req.TenDangNhap || u.Email == req.TenDangNhap)
                                       && u.MatKhau == req.MatKhau);

            if (user == null)
            {
                return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu!" });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                user = new
                {
                    user.Id,
                    user.HoVaTen,
                    user.Email,
                    user.VaiTro,
                    user.Anh
                }
            });
        }

        // GET: api/NguoiDungs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetNguoiDungs()
        {
            // Trả về dữ liệu sạch, tránh lỗi vòng lặp tham chiếu quan hệ (Circular Reference)
            return await _context.NguoiDungs
                .Select(u => new
                {
                    u.Id,
                    u.TenDangNhap,
                    u.HoVaTen,
                    u.Email,
                    u.VaiTro,
                    u.NgayTao
                })
                .ToListAsync();
        }

        // GET: api/NguoiDungs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetNguoiDung(int id)
        {
            var user = await _context.NguoiDungs
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.TenDangNhap,
                    u.HoVaTen,
                    u.Email,
                    u.VaiTro,
                    u.Anh,
                    u.NgayTao
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/NguoiDungs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNguoiDung(int id, NguoiDung nguoiDung)
        {
            if (id != nguoiDung.Id)
            {
                return BadRequest();
            }

            _context.Entry(nguoiDung).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NguoiDungExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/NguoiDungs
        [HttpPost]
        public async Task<ActionResult<NguoiDung>> PostNguoiDung(NguoiDung nguoiDung)
        {
            nguoiDung.NgayTao = DateTime.Now;
            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNguoiDung", new { id = nguoiDung.Id }, nguoiDung);
        }

        // DELETE: api/NguoiDungs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNguoiDung(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            _context.NguoiDungs.Remove(nguoiDung);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.Id == id);
        }
    }
}