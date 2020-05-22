using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW11.Models
{
    public class CodeFirstContext : DbContext
    {
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<Medicament> Medicament { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        public CodeFirstContext(DbContextOptions<CodeFirstContext> options) : base(options)
        {

        } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                    entity.HasKey(e => e.IdPatient).HasName("Patient_PK");
                  //  entity.Property(e => e.IdPatient).ValueGeneratedNever();//
                    entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                    entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                    entity.Property(e => e.BirthDate).IsRequired();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.IdDoctor).HasName("Doctor_PK");
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();

            });

            modelBuilder.Entity<Medicament>(entity=>
            {
                entity.HasKey(e => e.IdMedicament).HasName("Medicament_PK");
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Type).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.IdPrescription).HasName("Prescription_PK");
              //  entity.Property(e => e.IdPrescription).ValueGeneratedNever();
                entity.Property(e => e.Date).HasDefaultValueSql("GETDATE()").IsRequired();
                entity.Property(e => e.DueDate).IsRequired();

                entity.HasOne(d => d.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Prescription_Patient");

                entity.HasOne(d => d.Doctor)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Prescription_Doctor");
            });

            modelBuilder.Entity<PrescriptionMedicament>(entity =>
            {
                entity.ToTable("Prescription_Medicament");
                entity.HasKey(d => new { d.IdMedicament, d.IdPrescription }).HasName("PrescriptionMedicament_PK");
                entity.Property(e => e.Dose);
                entity.Property(e => e.Details).HasMaxLength(100).IsRequired();

                entity.HasOne(d => d.Prescription)
                .WithMany(p=> p.PrescriptionsMedicament)
                .HasForeignKey(d=> d.IdPrescription)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PrescriptionMedicament_Prescription");

                entity.HasOne(d => d.Medicament)
                .WithMany(p => p.PrescriptionsMedicament)
                .HasForeignKey(d => d.IdMedicament)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PrescriptionMedicament_Medicament");
            });
        }
    }
}
