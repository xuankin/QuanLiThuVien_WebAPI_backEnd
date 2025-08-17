using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.Models;
using LibraryManegermentAPI.Models;

namespace LibraryManagementApi.Data
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChiTietMuon> ChiTietMuons { get; set; }
        public DbSet<DocGia> DocGias { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<PhieuMuon> PhieuMuons { get; set; }
        public DbSet<PhieuPhat> PhieuPhats { get; set; }
        public DbSet<PhieuTra> PhieuTras { get; set; }
        public DbSet<Sach> Saches { get; set; }
        public DbSet<Sach_TacGia> SachTacGias { get; set; }
        public DbSet<Sach_TheLoai> SachTheLoais { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<TheLoai> TheLoais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ 1-nhiều giữa PhieuMuon và DocGia/NhanVien
            modelBuilder.Entity<PhieuMuon>()
                .HasOne(p => p.DocGia)
                .WithMany()
                .HasForeignKey(p => p.MaDocGia)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(p => p.NhanVien)
                .WithMany()
                .HasForeignKey(p => p.MaNhanVien)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ 1-nhiều giữa ChiTietMuon và PhieuMuon/Sach
            modelBuilder.Entity<ChiTietMuon>()
                .HasOne(c => c.PhieuMuon)
                .WithMany()
                .HasForeignKey(c => c.MaPhieuMuon)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietMuon>()
                .HasOne(c => c.Sach)
                .WithMany()
                .HasForeignKey(c => c.MaSach)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ 1-nhiều giữa PhieuPhat và ChiTietMuon/NhanVien
            modelBuilder.Entity<PhieuPhat>()
                .HasOne(p => p.ChiTietMuon)
                .WithMany()
                .HasForeignKey(p => p.MaChiTiet)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhieuPhat>()
                .HasOne(p => p.NhanVien)
                .WithMany()
                .HasForeignKey(p => p.MaNhanVien)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ 1-nhiều giữa PhieuTra và PhieuMuon/NhanVien
            modelBuilder.Entity<PhieuTra>()
                .HasOne(p => p.PhieuMuon)
                .WithMany()
                .HasForeignKey(p => p.MaPhieuMuon)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhieuTra>()
                .HasOne(p => p.NhanVien)
                .WithMany()
                .HasForeignKey(p => p.MaNhanVien)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ nhiều-nhiều giữa Sach và TacGia qua Sach_TacGia
            modelBuilder.Entity<Sach_TacGia>()
                .HasKey(st => new { st.MaSach, st.MaTacGia });

            modelBuilder.Entity<Sach_TacGia>()
                .HasOne(st => st.Sach)
                .WithMany(s => s.SachTacGias)
                .HasForeignKey(st => st.MaSach)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false); // Không bắt buộc

            modelBuilder.Entity<Sach_TacGia>()
                .HasOne(st => st.TacGia)
                .WithMany(t => t.SachTacGias)
                .HasForeignKey(st => st.MaTacGia)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false); // Không bắt buộc

            // Cấu hình quan hệ nhiều-nhiều giữa Sach và TheLoai qua Sach_TheLoai
            modelBuilder.Entity<Sach_TheLoai>()
                .HasKey(st => new { st.MaSach, st.MaTheLoai });

            modelBuilder.Entity<Sach_TheLoai>()
                .HasOne(st => st.Sach)
                .WithMany(s => s.SachTheLoais)
                .HasForeignKey(st => st.MaSach)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false); // Không bắt buộc

            modelBuilder.Entity<Sach_TheLoai>()
                .HasOne(st => st.TheLoai)
                .WithMany(t => t.SachTheLoais)
                .HasForeignKey(st => st.MaTheLoai)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false); // Không bắt buộc
        }
    }
}