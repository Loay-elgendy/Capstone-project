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
    }
}

