using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }

    }
}
