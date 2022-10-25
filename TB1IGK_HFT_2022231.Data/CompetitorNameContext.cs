using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Models;

namespace TB1IGK_HFT_2022231.Data
{
    public class CompetitorNameContext : DbContext
    {
        public virtual DbSet<Competitor> competitors { get; set; }
        public virtual DbSet<Category> categories { get; set; }
        public virtual DbSet<Competition> competitions { get; set; }

        public CompetitorNameContext() => this.Database.EnsureCreated();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;
                //    AttachDbFilename=|DataDirectory|\CompetitorName.mdf;Integrated Security=True";

                optionsBuilder
                    //.UseSqlServer(conn)
                    .UseInMemoryDatabase("competitors")
                    .UseLazyLoadingProxies();
                    
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var competition1 = new Competition(1, 1, 2, 5, "Szeged", 1000);
            var competition2 = new Competition(1, 3, 4, 2, "Duisburg", 200);
            var competition3 = new Competition(1, 5, 6, 15, "Racice", 500);

            var category1 = new Category(1, "U23", "Canoe");
            var category2 = new Category(2, "Adult", "Kayak");
            var category3 = new Category(3, "Junior", "Canoe");

            var competitor1 = new Competitor(1, "Tom", 22, 1, 1, "GER");
            var competitor2 = new Competitor(2, "Rask", 21, 1, 1, "DEN");
            var competitor3 = new Competitor(3, "Liam", 31, 2, 2, "GB");
            var competitor4 = new Competitor(4, "Sándor", 28, 2, 2, "HUN");
            var competitor5 = new Competitor(5, "Vold", 18, 3, 3, "NOR");
            var competitor6 = new Competitor(6, "Martin", 17, 3, 3, "CZE");

            modelBuilder.Entity<Competitor>().HasData(competitor1, competitor2, competitor3, competitor4, competitor5, competitor6);
            modelBuilder.Entity<Competition>().HasData(competition1, competition2, competition3);
            modelBuilder.Entity<Category>().HasData(category1, category2, category3);
        }

        
    }
}

