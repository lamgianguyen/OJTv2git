using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OJTv2.Models
{
    public partial class SWPOJTContext : DbContext
    {
        public SWPOJTContext()
        {
        }

        public SWPOJTContext(DbContextOptions<SWPOJTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Apply> Applies { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<BusinessinSemester> BusinessinSemesters { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Industry> Industries { get; set; }
        public virtual DbSet<JobDepartment> JobDepartments { get; set; }
        public virtual DbSet<JobPosition> JobPositions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentinSemester> StudentinSemesters { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-RIT3G3D\\SQLEXPRESS;Database=SWPOJT;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Apply>(entity =>
            {
                entity.ToTable("Apply");

                entity.Property(e => e.ApplyId).HasColumnName("ApplyID");

                entity.Property(e => e.ApplyDate).HasColumnType("datetime");

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("businessID");

                entity.Property(e => e.Cv)
                    .IsRequired()
                    .HasColumnName("CV");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("StudentID");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Applies)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Apply_BusinessinSemester");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Applies)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Apply_StudentinSemester");
            });

            modelBuilder.Entity<Business>(entity =>
            {
                entity.ToTable("Business");

                entity.Property(e => e.BusinessId)
                    .HasMaxLength(20)
                    .HasColumnName("BusinessID");

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ContactEmail).HasMaxLength(100);

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Image).HasMaxLength(1000);

                entity.Property(e => e.IndustryId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("IndustryID");

                entity.Property(e => e.SemesterId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.Website).HasMaxLength(100);

                entity.HasOne(d => d.BusinessNavigation)
                    .WithOne(p => p.Business)
                    .HasForeignKey<Business>(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Business_User");

                entity.HasOne(d => d.Industry)
                    .WithMany(p => p.Businesses)
                    .HasForeignKey(d => d.IndustryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Business_Industry");
            });

            modelBuilder.Entity<BusinessinSemester>(entity =>
            {
                entity.HasKey(e => e.BusinessId);

                entity.ToTable("BusinessinSemester");

                entity.Property(e => e.BusinessId)
                    .HasMaxLength(20)
                    .HasColumnName("BusinessID");

                entity.Property(e => e.JobPositionId).HasColumnName("JobPositionID");

                entity.Property(e => e.SemesterId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SemesterID");

                entity.HasOne(d => d.Business)
                    .WithOne(p => p.BusinessinSemester)
                    .HasForeignKey<BusinessinSemester>(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessinSemester_Business1");

                entity.HasOne(d => d.JobPosition)
                    .WithMany(p => p.BusinessinSemesters)
                    .HasForeignKey(d => d.JobPositionId)
                    .HasConstraintName("FK_BusinessinSemester_JobPosition");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.BusinessinSemesters)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessinSemester_Semester1");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.DepartmentId)
                    .HasMaxLength(10)
                    .HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Industry>(entity =>
            {
                entity.ToTable("Industry");

                entity.Property(e => e.IndustryId)
                    .HasMaxLength(20)
                    .HasColumnName("IndustryID");

                entity.Property(e => e.IndustryName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<JobDepartment>(entity =>
            {
                entity.ToTable("JobDepartment");

                entity.Property(e => e.JobDepartmentId).HasColumnName("JobDepartmentID");

                entity.Property(e => e.DepartmentId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("DepartmentID");

                entity.Property(e => e.JobPostionId).HasColumnName("JobPostionID");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.JobDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobDepartment_Department1");

                entity.HasOne(d => d.JobPostion)
                    .WithMany(p => p.JobDepartments)
                    .HasForeignKey(d => d.JobPostionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobDepartment_JobPosition1");
            });

            modelBuilder.Entity<JobPosition>(entity =>
            {
                entity.ToTable("JobPosition");

                entity.Property(e => e.JobPositionId).HasColumnName("JobPositionID");

                entity.Property(e => e.Benefit).HasMaxLength(200);

                entity.Property(e => e.DetailBusiness).HasMaxLength(2000);

                entity.Property(e => e.DetailWork).HasMaxLength(1000);

                entity.Property(e => e.JobName).HasMaxLength(200);

                entity.Property(e => e.Request).HasMaxLength(1000);

                entity.Property(e => e.WorkLocation).HasMaxLength(200);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(10)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.SemesterName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(20)
                    .HasColumnName("StudentID");

                entity.Property(e => e.Avatar).HasMaxLength(500);

                entity.Property(e => e.DeparmentId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("DeparmentID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.SemesterId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Deparment)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.DeparmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_Department");

                entity.HasOne(d => d.StudentNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_User");
            });

            modelBuilder.Entity<StudentinSemester>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.ToTable("StudentinSemester");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(20)
                    .HasColumnName("StudentID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(10)
                    .HasColumnName("SemesterID");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.StudentinSemesters)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_StudentinSemester_Semester1");

                entity.HasOne(d => d.Student)
                    .WithOne(p => p.StudentinSemester)
                    .HasForeignKey<StudentinSemester>(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentinSemester_Student1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .HasMaxLength(20)
                    .HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
