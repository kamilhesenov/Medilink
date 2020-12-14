using Medilink_Final_Project.Models;
using Medilink_Final_Project.Models.About;
using Medilink_Final_Project.Models.Appointment;
using Medilink_Final_Project.Models.Contact;
using Medilink_Final_Project.Models.Home;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Data
{
    public class AplicationDbContext : IdentityDbContext<AppUser>
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        public DbSet<HomeBrand> HomeBrands { get; set; }
        public DbSet<HomeCounter> HomeCounters { get; set; }
        public DbSet<HomeHealth> HomeHealths { get; set; }
        public DbSet<HomeIntro> HomeIntros { get; set; }
        public DbSet<HomeNew> HomeNews { get; set; }
        public DbSet<HomeService> HomeServices { get; set; }
        public DbSet<HomeSpecialityDoctor> HomeSpecialityDoctors { get; set; }
        public DbSet<HomeSpecialityModern> HomeSpecialityModerns { get; set; }
        public DbSet<HomeSpecialitySucces> homeSpecialitySucces { get; set; }
        public DbSet<HomeTestimonial> HomeTestimonials { get; set; }
        public DbSet<WelcomeHospital> WelcomeHospitals { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Labaratory> Labaratories { get; set; }
        public DbSet<AboutChooseUs> AboutChooseUs { get; set; }
        public DbSet<AboutChooseUsItem> AboutChooseUsItems { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentPhoto> AppointmentPhotos { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<ShopCkeckout> ShopCkeckouts { get; set; }



    }
}
