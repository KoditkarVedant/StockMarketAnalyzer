using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockMarketAnalyzer.DAL.DataModels;

namespace StockMarketAnalyzer.DAL
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext()
            : base("name=StockMarketDB")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<StockMarketDbContext>());
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyOfficer> CompanyOfficers { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<HistoricalData> HistoricalDatas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserPortfolio> UserPortfolios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasKey(cd => cd.Symbol);

            //one-to-many 
            modelBuilder.Entity<CompanyOfficer>()
                        .HasRequired<Company>(cd => cd.Company) // CompanyOfficer entity requires CompanyDetail 
                        .WithMany(co => co.CompanyOfficers)
                        .HasForeignKey(co => co.Symbol); // CompanyDetail entity includes many CompanyOfficers entities

            //primary key
            modelBuilder.Entity<CompanyProfile>().HasKey(cp => cp.Symbol);

            //one-to-one
            modelBuilder.Entity<Company>()
                .HasOptional(cd => cd.CompanyProfile) //CompanyDetail entity has optional CompanyProfile
                .WithRequired(cp => cp.Company); // companyProfile entity required CompanyDetail entity

            //one-to-many 
            modelBuilder.Entity<HistoricalData>()
                .HasRequired<Company>(cd => cd.Company)
                .WithMany(hd => hd.HistoricalDatas)
                .HasForeignKey(hd => hd.Symbol);

            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasRequired(u => u.UserProfile)
                .WithRequiredDependent(up => up.User);

            modelBuilder.Entity<UserProfile>().HasKey(up => up.UserId);

            modelBuilder.Entity<UserPortfolio>().HasKey(up => up.UserPortfolioId);

            modelBuilder.Entity<UserPortfolio>()
                .HasRequired<User>(up => up.User)
                .WithMany(u => u.UserPortfolios)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPortfolio>()
                .HasRequired<Company>(up => up.Company)
                .WithMany(c => c.UserPortfolios)
                .HasForeignKey(up => up.Symbol);
        }
    }
}
