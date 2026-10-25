using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLMS.Models;

namespace MiniLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChuongMucsController : ControllerBase
    {
        private readonly MiniLmsContext _context;

        public ChuongMucsController(MiniLmsContext context)
        {
            _context = context;
        }

        // POST: api/ChuongMucs - Tạo chương mới cho môn học
        [HttpPost]
        public async Task<ActionResult<ChuongMuc>> PostChuongMuc([FromBody] ChuongMuc chuongMuc)
        {
            var count = await _context.ChuongMucs.CountAsync(c => c.IdKhoaHoc == chuongMuc.IdKhoaHoc);
            chuongMuc.ThuTu = count + 1;

            _context.ChuongMucs.Add(chuongMuc);
            await _context.SaveChangesAsync();

            return Ok(chuongMuc);
        }
    }
}