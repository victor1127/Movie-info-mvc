using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieProDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieProDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
