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
                    .ThenInclude(kh => kh!.IdGiangVienNavigation)
                .OrderByDescending(d => d.NgayDangKy)
                .Select(d => new
                {
                    d.Id,
                    d.IdKhoaHoc,
                    MaKhoaHoc = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.MaKhoaHoc : "",
                    TenKhoaHoc = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.TenKhoaHoc : "",
                    HocKy = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.HocKy : "",
                    IdGiangVien = d.IdKhoaHocNavigation != null ? d.IdKhoaHocNavigation.IdGiangVien : 0,
                    TenGiangVien = (d.IdKhoaHocNavigation != null && d.IdKhoaHocNavigation.IdGiangVienNavigation != null)
                        ? (d.IdKhoaHocNavigation.IdGiangVienNavigation.HoVaTen ?? d.IdKhoaHocNavigation.IdGiangVienNavigation.TenDangNhap ?? "")
                        : "",
                    TrangThai = d.TrangThai ?? "CHO_DUYET",
                    d.NgayDangKy
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/DangKyKhoaHocs/bang-diem-chi-tiet?idSinhVien=3 - Đồng bộ tiến độ, điểm số, giảng viên và trạng thái duyệt
        [HttpGet("bang-diem-chi-tiet")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentGradeOverview([FromQuery] int idSinhVien)
        {
            var enrollments = await _context.DangKyKhoaHocs
                .Include(dk => dk.IdKhoaHocNavigation)
                    .ThenInclude(kh => kh!.BaiTaps)
                .Include(dk => dk.IdKhoaHocNavigation)
                    .ThenInclude(kh => kh!.IdGiangVienNavigation)
                .Where(dk => dk.IdSinhVien == idSinhVien)
                .ToListAsync();

            var resultList = new List<object>();

            foreach (var e in enrollments)
            {
                if (e.IdKhoaHocNavigation == null) continue;

                var course = e.IdKhoaHocNavigation;
                var assignments = course.BaiTaps.ToList();
                int totalAssignments = assignments.Count;

                var assignmentIds = assignments.Select(a => a.Id).ToList();
                var submissions = await _context.BaiNops
                    .Where(bn => bn.IdSinhVien == idSinhVien && assignmentIds.Contains(bn.IdBaiTap))
                    .ToListAsync();

                int submittedCount = submissions.Count(s => !string.IsNullOrEmpty(s.DuongDanFile));

                // Tính tỷ lệ hoàn thành
                int percent = totalAssignments > 0 ? (int)Math.Round((double)submittedCount / totalAssignments * 100) : 0;
                if (percent > 100) percent = 100;

                // Điểm bài tập quá trình
                var gradedSubmissions = submissions.Where(s => s.TongDiem != null).ToList();
                string diemQuaTrinhText = "—";
                if (gradedSubmissions.Count > 0)
                {
                    double avgScore = gradedSubmissions.Average(s => s.TongDiem!.Value);
                    diemQuaTrinhText = $"{Math.Round(avgScore, 1)} / 10.0";
                }
                else if (submittedCount > 0)
                {
                    diemQuaTrinhText = "Chờ chấm";
                }

                // Lấy Họ tên thực tế của Giảng viên từ bảng nguoi_dung
                string tenGiangVien = course.IdGiangVienNavigation != null
                    ? (course.IdGiangVienNavigation.HoVaTen ?? course.IdGiangVienNavigation.TenDangNhap ?? $"Giảng viên #{course.IdGiangVien}")
                    : $"Giảng viên #{course.IdGiangVien}";

                resultList.Add(new
                {
                    enrollmentId = e.Id,
                    courseId = course.Id,
                    courseCode = course.MaKhoaHoc,
                    courseName = course.TenKhoaHoc,
                    idGiangVien = course.IdGiangVien,
                    tenGiangVien = tenGiangVien,
                    percent = percent,
                    submittedCount = submittedCount,
                    totalAssignments = totalAssignments,
                    diemQuaTrinh = diemQuaTrinhText,
                    trangThai = e.TrangThai ?? "CHO_DUYET" // CHO_DUYET, DANG_HOC, TU_CHOI
                });
            }

            return Ok(resultList);
        }

        // POST: api/DangKyKhoaHocs - Sinh viên gửi yêu cầu đăng ký (Mặc định ở trạng thái CHO_DUYET)
        [HttpPost]
        public async Task<ActionResult<DangKyKhoaHoc>> PostDangKy([FromBody] DangKyKhoaHoc dangKy)
        {
            var exists = await _context.DangKyKhoaHocs
                .AnyAsync(d => d.IdKhoaHoc == dangKy.IdKhoaHoc && d.IdSinhVien == dangKy.IdSinhVien);

            if (exists)
            {
                return BadRequest(new { message = "Bạn đã gửi yêu cầu đăng ký học phần này trước đó rồi!" });
            }

            dangKy.NgayDangKy = DateTime.Now;
            dangKy.TrangThai = "CHO_DUYET"; // Chờ Admin phê duyệt mới được vào học

            _context.DangKyKhoaHocs.Add(dangKy);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Gửi yêu cầu đăng ký môn học thành công! Vui lòng chờ Quản trị viên xét duyệt.", data = dangKy });
        }

        // PUT: api/DangKyKhoaHocs/5/duyet - Admin phê duyệt cho sinh viên vào lớp
        [HttpPut("{id}/duyet")]
        public async Task<IActionResult> ApproveEnrollment(int id)
        {
            var enrollment = await _context.DangKyKhoaHocs.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound(new { message = "Không tìm thấy yêu cầu đăng ký của sinh viên!" });
            }

            enrollment.TrangThai = "DANG_HOC";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã phê duyệt sinh viên vào học phần thành công!" });
        }

        // PUT: api/DangKyKhoaHocs/5/tu-choi - Admin từ chối yêu cầu đăng ký
        [HttpPut("{id}/tu-choi")]
        public async Task<IActionResult> RejectEnrollment(int id)
        {
            var enrollment = await _context.DangKyKhoaHocs.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound(new { message = "Không tìm thấy yêu cầu đăng ký của sinh viên!" });
            }

            enrollment.TrangThai = "TU_CHOI";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã từ chối yêu cầu đăng ký học phần này!" });
        }
    }
}