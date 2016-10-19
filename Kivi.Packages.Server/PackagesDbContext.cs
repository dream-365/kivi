using Kivi.Packages.Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kivi.Packages.Server
{
    public class PackagesDbContext : DbContext
    {
        public DbSet<PackageModel> Packages { get; set; }

        public PackagesDbContext () : base("DefaultConnectionString")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PackageModelEFConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}