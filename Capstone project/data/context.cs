using Capstone_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Capstone_project.data
{
    public class context : DbContext
    {
        public context(DbContextOptions<context> options) : base(options) { }

        public DbSet<Login> Logins { get; set; }
        public DbSet<SignUp> SignUps { get; set; }
        public DbSet<home> Homes { get; set; }
        public DbSet<AddClinic> AddClinics { get; set; }
        public DbSet<statusmodel> Status { get; set; }
        public DbSet<Dash> Dashs { get; set; }
        public DbSet<Select> Selects { get; set; }
        public DbSet<PrescriptionForm> PrescriptionForms { get; set; }

    }
}

