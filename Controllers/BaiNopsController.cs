using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLMS.Models;
using System.Globalization;
using System.Net;
using System.Text;

namespace MiniLMS.Controllers
{
    public class ChamDiemDto
    {
        public double DiemSo { get; set; }
        public string? NhanXet { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class BaiNopsController : ControllerBase
    {
        private readonly MiniLmsContext _context;
        private readonly IWebHostEnvironment _env;

        public BaiNopsController(MiniLmsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Hàm lấy đường dẫn thư mục uploads chuẩn xác
        private string GetUploadDirectory()
        {
            var webRoot = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
            }
            var uploadDir = Path.Combine(webRoot, "uploads");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            return uploadDir;
        }

        // Hàm xóa dấu tiếng Việt và ký tự đặc biệt khỏi tên file (Khử cả Đ và đ)
        private string RemoveDiacriticsAndSpecialChars(string text)
        {
            text = text.Replace("đ", "d").Replace("Đ", "D");
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    if (char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.')
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        stringBuilder.Append('_');
                    }
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        // GET: api/BaiNops/download/5 - Tải file thông minh và an toàn
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadSubmissionFile(int id)
        {
            var submission = await _context.BaiNops.FindAsync(id);
            if (submission == null || string.IsNullOrEmpty(submission.DuongDanFile))
            {
                return NotFound(new { message = "Không tìm thấy dữ liệu bài nộp trong CSDL!" });
            }

            var uploadDir = GetUploadDirectory();
            var rawFileName = Path.GetFileName(submission.DuongDanFile);
            var decodedFileName = WebUtility.UrlDecode(rawFileName);

            // 1. Kiểm tra trực tiếp tên file nguyên bản
            var filePath = Path.Combine(uploadDir, rawFileName);

            // 2. Thử với tên đã giải mã URL
            if (!System.IO.File.Exists(filePath))
            {
                filePath = Path.Combine(uploadDir, decodedFileName);
            }

            // 3. Nếu vẫn không thấy, tìm kiếm trong thư mục uploads bất kỳ file nào có chứa tên này
            if (!System.IO.File.Exists(filePath))
            {
                var matchingFile = Directory.GetFiles(uploadDir)
                    .FirstOrDefault(f => Path.GetFileName(f).Equals(rawFileName, StringComparison.OrdinalIgnoreCase) ||
                                         Path.GetFileName(f).Equals(decodedFileName, StringComparison.OrdinalIgnoreCase));

                if (matchingFile != null)
                {
                    filePath = matchingFile;
                }
            }

            // Nếu thực sự file vật lý không có trên máy chủ
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new
                {
                    message = $"Bài nộp ID #{id} này là bản ghi cũ, file vật lý chưa được ghi xuống ổ đĩa máy chủ. Vui lòng sang tài khoản Sinh viên nộp lại bài mới!"
                });
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, "application/octet-stream", decodedFileName);
        }

        // GET: api/BaiNops/kiem-tra?idBaiTap=1&idSinhVien=3
        [HttpGet("kiem-tra")]
        public async Task<ActionResult<object>> GetStudentSubmission([FromQuery] int idBaiTap, [FromQuery] int idSinhVien)
        {
            var submission = await _context.BaiNops
                .Where(b => b.IdBaiTap == idBaiTap && b.IdSinhVien == idSinhVien)
                .OrderByDescending(b => b.ThoiGianNop)
                .Select(b => new
                {
                    b.Id,
                    b.IdBaiTap,
                    b.IdSinhVien,
                    DuongDanFile = b.DuongDanFile ?? "",
                    b.ThoiGianNop,
                    b.TongDiem,
                    NhanXetGiangVien = b.NhanXetGiangVien ?? "",
                    TrangThai = b.TrangThai ?? ""
                })
                .FirstOrDefaultAsync();

            if (submission == null)
            {
                return Ok(new { daNop = false });
            }

            return Ok(new { daNop = true, data = submission });
        }

        // GET: api/BaiNops/giang-vien/2
        [HttpGet("giang-vien/{idGiangVien}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSubmissionsForInstructor(int idGiangVien)
        {
            var list = await _context.BaiNops
                .Include(b => b.IdSinhVienNavigation)
                .Include(b => b.IdBaiTapNavigation)
                    .ThenInclude(bt => bt!.IdKhoaHocNavigation)
                .Where(b => b.IdBaiTapNavigation != null &&
                            b.IdBaiTapNavigation.IdKhoaHocNavigation != null &&
                            b.IdBaiTapNavigation.IdKhoaHocNavigation.IdGiangVien == idGiangVien)
                .OrderByDescending(b => b.ThoiGianNop)
                .Select(b => new
                {
                    b.Id,
                    b.IdBaiTap,
                    b.IdSinhVien,
                    TenSinhVien = b.IdSinhVienNavigation != null ? b.IdSinhVienNavigation.HoVaTen : "",
                    TenDangNhap = b.IdSinhVienNavigation != null ? b.IdSinhVienNavigation.TenDangNhap : "",
                    TieuDeBaiTap = b.IdBaiTapNavigation != null ? b.IdBaiTapNavigation.TieuDe : "",
                    MaKhoaHoc = (b.IdBaiTapNavigation != null && b.IdBaiTapNavigation.IdKhoaHocNavigation != null)
                                ? b.IdBaiTapNavigation.IdKhoaHocNavigation.MaKhoaHoc
                                : "",
                    b.ThoiGianNop,
                    DuongDanFile = b.DuongDanFile ?? "",
                    b.TongDiem,
                    NhanXetGiangVien = b.NhanXetGiangVien ?? "",
                    TrangThai = b.TrangThai ?? ""
                })
                .ToListAsync();

            return Ok(list);
        }

        // PUT: api/BaiNops/5/cham-diem
        [HttpPut("{id}/cham-diem")]
        public async Task<IActionResult> GradeSubmission(int id, [FromBody] ChamDiemDto dto)
        {
            var submission = await _context.BaiNops.FindAsync(id);
            if (submission == null)
            {
                return NotFound(new { message = "Không tìm thấy bài nộp của sinh viên!" });
            }

            submission.TongDiem = dto.DiemSo;
            submission.NhanXetGiangVien = dto.NhanXet;
            submission.TrangThai = "DA_CHAM";

            await _context.SaveChangesAsync();
            return Ok(new { message = "Lưu kết quả chấm điểm thành công!", submission });
        }

        // POST: api/BaiNops/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadBaiNop([FromForm] int idBaiTap, [FromForm] int idSinhVien, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Vui lòng chọn tệp tin hợp lệ!" });
            }

            try
            {
                var svExists = await _context.NguoiDungs.AnyAsync(u => u.Id == idSinhVien);
                if (!svExists)
                {
                    return BadRequest(new { message = $"Không tìm thấy tài khoản sinh viên có ID: {idSinhVien}" });
                }

                var btExists = await _context.BaiTaps.AnyAsync(b => b.Id == idBaiTap);
                if (!btExists)
                {
                    return BadRequest(new { message = $"Không tìm thấy bài tập có ID: {idBaiTap}" });
                }

                var uploadDir = GetUploadDirectory();

                // Chuẩn hóa tên file: khử ký tự đặc biệt, dấu tiếng Việt và chữ Đ
                var rawFileName = Path.GetFileName(file.FileName);
                var cleanFileName = RemoveDiacriticsAndSpecialChars(rawFileName);

                var uniqueFileName = $"{DateTime.Now.Ticks}_{cleanFileName}";
                var fullPath = Path.Combine(uploadDir, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = $"/uploads/{uniqueFileName}";

                var existing = await _context.BaiNops
                    .FirstOrDefaultAsync(b => b.IdBaiTap == idBaiTap && b.IdSinhVien == idSinhVien);

                if (existing != null)
                {
                    existing.DuongDanFile = relativePath;
                    existing.ThoiGianNop = DateTime.Now;
                    existing.TrangThai = "DA_NOP";
                    _context.BaiNops.Update(existing);
                }
                else
                {
                    var newSub = new BaiNop
                    {
                        IdBaiTap = idBaiTap,
                        IdSinhVien = idSinhVien,
                        DuongDanFile = relativePath,
                        ThoiGianNop = DateTime.Now,
                        TrangThai = "DA_NOP"
                    };
                    _context.BaiNops.Add(newSub);
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Nộp bài thành công!",
                    filePath = relativePath,
                    fileName = cleanFileName
                });
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                return BadRequest(new { message = "Lỗi CSDL: " + innerMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}