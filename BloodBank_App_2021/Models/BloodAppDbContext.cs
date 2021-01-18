using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BloodBank_App_2021.Models
{
    public class BloodAppDbContext:DbContext
    {
        public BloodAppDbContext(DbContextOptions<BloodAppDbContext> options):base(options)
        {
            
        }

        public DbSet<BloodGroup> BloodGroup { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Religion> Religion { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<Thana> Thana { get; set; }
        public DbSet<BloodDonorRegistration> BloodDonorRegistration { get; set; }
        public DbSet<DonorPicture> DonorPicture { get; set; }
        public DbSet<BloodDonationRecord> BloodDonationRecord { get; set; }
        public DbSet<BloodRequestUser> BloodRequestUser { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<BloodDonorRegistration>()
                .HasOne(b => b.DonorPicture)
                .WithOne(i => i.BloodDonorRegistration)
                .HasForeignKey<DonorPicture>(b => b.DonorContactNumber);

            modelBuilder.Entity<BloodDonationRecord>()
                .HasOne(dr => dr.BloodDonorRegistration)
                .WithMany(dn => dn.BloodDonationRecord)
                .HasForeignKey(s => s.DonorContactNumber)
                .IsRequired();
     
        }

    }

}
