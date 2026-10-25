using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniLMS.Models;

public partial class MiniLmsContext : DbContext
{
    public MiniLmsContext()
    {
    }

    public MiniLmsContext(DbContextOptions<MiniLmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiHoc> BaiHocs { get; set; }

    public virtual DbSet<BaiNop> BaiNops { get; set; }

    public virtual DbSet<BaiTap> BaiTaps { get; set; }

    public virtual DbSet<BaiVietDienDan> BaiVietDienDans { get; set; }

    public virtual DbSet<BinhLuanDienDan> BinhLuanDienDans { get; set; }

    public virtual DbSet<CauHoi> CauHois { get; set; }

    public virtual DbSet<ChiTietTraLoi> ChiTietTraLois { get; set; }

    public virtual DbSet<ChuongMuc> ChuongMucs { get; set; }

    public virtual DbSet<DangKyKhoaHoc> DangKyKhoaHocs { get; set; }

    public virtual DbSet<KhoaHoc> KhoaHocs { get; set; }

    public virtual DbSet<LuaChonDapAn> LuaChonDapAns { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiHoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bai_hoc__3213E83FFFCF4D9E");

            entity.ToTable("bai_hoc");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DuongDanTaiLieu)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("duong_dan_tai_lieu");
            entity.Property(e => e.IdChuongMuc).HasColumnName("id_chuong_muc");
            entity.Property(e => e.LoaiNoiDung)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("loai_noi_dung");
            entity.Property(e => e.ThuTu).HasColumnName("thu_tu");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(200)
                .HasColumnName("tieu_de");

            entity.HasOne(d => d.IdChuongMucNavigation).WithMany(p => p.BaiHocs)
                .HasForeignKey(d => d.IdChuongMuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bai_hoc__id_chuo__5EBF139D");
        });

        modelBuilder.Entity<BaiNop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bai_nop__3213E83F06616BD4");

            entity.ToTable("bai_nop");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DuongDanFile)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("duong_dan_file");
            entity.Property(e => e.IdBaiTap).HasColumnName("id_bai_tap");
            entity.Property(e => e.IdSinhVien).HasColumnName("id_sinh_vien");
            entity.Property(e => e.NhanXetGiangVien).HasColumnName("nhan_xet_giang_vien");
            entity.Property(e => e.ThoiGianNop)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_nop");
            entity.Property(e => e.TongDiem).HasColumnName("tong_diem");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("DA_NOP")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.IdBaiTapNavigation).WithMany(p => p.BaiNops)
                .HasForeignKey(d => d.IdBaiTap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bai_nop__id_bai___70DDC3D8");

            entity.HasOne(d => d.IdSinhVienNavigation).WithMany(p => p.BaiNops)
                .HasForeignKey(d => d.IdSinhVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bai_nop__id_sinh__71D1E811");
        });

        modelBuilder.Entity<BaiTap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bai_tap__3213E83FA43A0874");

            entity.ToTable("bai_tap");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiemToiDa)
                .HasDefaultValue(100.0)
                .HasColumnName("diem_toi_da");
            entity.Property(e => e.HanNop)
                .HasColumnType("datetime")
                .HasColumnName("han_nop");
            entity.Property(e => e.IdKhoaHoc).HasColumnName("id_khoa_hoc");
            entity.Property(e => e.LoaiBaiTap)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("loai_bai_tap");
            entity.Property(e => e.MoTa).HasColumnName("mo_ta");
            entity.Property(e => e.ThoiGianLamPhut)
                .HasDefaultValue(0)
                .HasColumnName("thoi_gian_lam_phut");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(200)
                .HasColumnName("tieu_de");

            entity.HasOne(d => d.IdKhoaHocNavigation).WithMany(p => p.BaiTaps)
                .HasForeignKey(d => d.IdKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bai_tap__id_khoa__628FA481");
        });

        modelBuilder.Entity<BaiVietDienDan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bai_viet__3213E83FA3B20F3B");

            entity.ToTable("bai_viet_dien_dan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GhimBai)
                .HasDefaultValue(false)
                .HasColumnName("ghim_bai");
            entity.Property(e => e.IdKhoaHoc).HasColumnName("id_khoa_hoc");
            entity.Property(e => e.IdTacGia).HasColumnName("id_tac_gia");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.NoiDung).HasColumnName("noi_dung");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(255)
                .HasColumnName("tieu_de");

            entity.HasOne(d => d.IdKhoaHocNavigation).WithMany(p => p.BaiVietDienDans)
                .HasForeignKey(d => d.IdKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bai_viet___id_kh__7B5B524B");

            entity.HasOne(d => d.IdTacGiaNavigation).WithMany(p => p.BaiVietDienDans)
                .HasForeignKey(d => d.IdTacGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bai_viet___id_ta__7C4F7684");
        });

        modelBuilder.Entity<BinhLuanDienDan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__binh_lua__3213E83F9E9BF476");

            entity.ToTable("binh_luan_dien_dan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBaiViet).HasColumnName("id_bai_viet");
            entity.Property(e => e.IdTacGia).HasColumnName("id_tac_gia");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.NoiDung).HasColumnName("noi_dung");

            entity.HasOne(d => d.IdBaiVietNavigation).WithMany(p => p.BinhLuanDienDans)
                .HasForeignKey(d => d.IdBaiViet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__binh_luan__id_ba__01142BA1");

            entity.HasOne(d => d.IdTacGiaNavigation).WithMany(p => p.BinhLuanDienDans)
                .HasForeignKey(d => d.IdTacGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__binh_luan__id_ta__02084FDA");
        });

        modelBuilder.Entity<CauHoi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cau_hoi__3213E83F908E2142");

            entity.ToTable("cau_hoi");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiemCauHoi)
                .HasDefaultValue(10.0)
                .HasColumnName("diem_cau_hoi");
            entity.Property(e => e.IdBaiTap).HasColumnName("id_bai_tap");
            entity.Property(e => e.LoaiCauHoi)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("MOT_DAP_AN")
                .HasColumnName("loai_cau_hoi");
            entity.Property(e => e.NoiDungCauHoi).HasColumnName("noi_dung_cau_hoi");
            entity.Property(e => e.ThuTu).HasColumnName("thu_tu");

            entity.HasOne(d => d.IdBaiTapNavigation).WithMany(p => p.CauHois)
                .HasForeignKey(d => d.IdBaiTap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cau_hoi__id_bai___68487DD7");
        });

        modelBuilder.Entity<ChiTietTraLoi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__chi_tiet__3213E83F0E408524");

            entity.ToTable("chi_tiet_tra_loi");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBaiNop).HasColumnName("id_bai_nop");
            entity.Property(e => e.IdCauHoi).HasColumnName("id_cau_hoi");
            entity.Property(e => e.IdLuaChon).HasColumnName("id_lua_chon");
            entity.Property(e => e.KetQuaDungSai).HasColumnName("ket_qua_dung_sai");

            entity.HasOne(d => d.IdBaiNopNavigation).WithMany(p => p.ChiTietTraLois)
                .HasForeignKey(d => d.IdBaiNop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__chi_tiet___id_ba__76969D2E");

            entity.HasOne(d => d.IdCauHoiNavigation).WithMany(p => p.ChiTietTraLois)
                .HasForeignKey(d => d.IdCauHoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__chi_tiet___id_ca__778AC167");

            entity.HasOne(d => d.IdLuaChonNavigation).WithMany(p => p.ChiTietTraLois)
                .HasForeignKey(d => d.IdLuaChon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__chi_tiet___id_lu__787EE5A0");
        });

        modelBuilder.Entity<ChuongMuc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__chuong_m__3213E83F5385E837");

            entity.ToTable("chuong_muc");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdKhoaHoc).HasColumnName("id_khoa_hoc");
            entity.Property(e => e.TenChuong)
                .HasMaxLength(200)
                .HasColumnName("ten_chuong");
            entity.Property(e => e.ThuTu).HasColumnName("thu_tu");

            entity.HasOne(d => d.IdKhoaHocNavigation).WithMany(p => p.ChuongMucs)
                .HasForeignKey(d => d.IdKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__chuong_mu__id_kh__5BE2A6F2");
        });

        modelBuilder.Entity<DangKyKhoaHoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dang_ky___3213E83FD02ED0A3");

            entity.ToTable("dang_ky_khoa_hoc");

            entity.HasIndex(e => new { e.IdKhoaHoc, e.IdSinhVien }, "UQ_DangKy").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdKhoaHoc).HasColumnName("id_khoa_hoc");
            entity.Property(e => e.IdSinhVien).HasColumnName("id_sinh_vien");
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_dang_ky");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("DANG_HOC")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.IdKhoaHocNavigation).WithMany(p => p.DangKyKhoaHocs)
                .HasForeignKey(d => d.IdKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__dang_ky_k__id_kh__5629CD9C");

            entity.HasOne(d => d.IdSinhVienNavigation).WithMany(p => p.DangKyKhoaHocs)
                .HasForeignKey(d => d.IdSinhVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__dang_ky_k__id_si__571DF1D5");
        });

        modelBuilder.Entity<KhoaHoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__khoa_hoc__3213E83FCEE74FB8");

            entity.ToTable("khoa_hoc");

            entity.HasIndex(e => e.MaKhoaHoc, "UQ__khoa_hoc__50BFE164913AA2E2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HocKy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hoc_ky");
            entity.Property(e => e.IdGiangVien).HasColumnName("id_giang_vien");
            entity.Property(e => e.MaKhoaHoc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ma_khoa_hoc");
            entity.Property(e => e.MoTa).HasColumnName("mo_ta");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.TenKhoaHoc)
                .HasMaxLength(200)
                .HasColumnName("ten_khoa_hoc");

            entity.HasOne(d => d.IdGiangVienNavigation).WithMany(p => p.KhoaHocs)
                .HasForeignKey(d => d.IdGiangVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__khoa_hoc__id_gia__5165187F");
        });

        modelBuilder.Entity<LuaChonDapAn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__lua_chon__3213E83F90D92461");

            entity.ToTable("lua_chon_dap_an");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCauHoi).HasColumnName("id_cau_hoi");
            entity.Property(e => e.LaDapAnDung)
                .HasDefaultValue(false)
                .HasColumnName("la_dap_an_dung");
            entity.Property(e => e.NoiDungDapAn).HasColumnName("noi_dung_dap_an");

            entity.HasOne(d => d.IdCauHoiNavigation).WithMany(p => p.LuaChonDapAns)
                .HasForeignKey(d => d.IdCauHoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__lua_chon___id_ca__6D0D32F4");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nguoi_du__3213E83FCCFE2EE5");

            entity.ToTable("nguoi_dung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__nguoi_du__363698B3046EF0A7").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__nguoi_du__AB6E6164FEBE07FA").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Anh)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("anh");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.HoVaTen)
                .HasMaxLength(100)
                .HasColumnName("ho_va_ten");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mat_khau");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ten_dang_nhap");
            entity.Property(e => e.VaiTro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("vai_tro");
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__thong_ba__3213E83F651E0BA4");

            entity.ToTable("thong_bao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DaDoc)
                .HasDefaultValue(false)
                .HasColumnName("da_doc");
            entity.Property(e => e.IdNguoiNhan).HasColumnName("id_nguoi_nhan");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.NoiDung).HasColumnName("noi_dung");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(200)
                .HasColumnName("tieu_de");

            entity.HasOne(d => d.IdNguoiNhanNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.IdNguoiNhan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__thong_bao__id_ng__05D8E0BE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
