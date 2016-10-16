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
        public StockMarketDbContext() : base("StockMarketDB3")
        {
            System.Data.Entity.Database.SetInitializer<StockMarketDbContext>(new DropCreateDatabaseAlways<StockMarketDbContext>());
        }
        
        public DbSet<CompanyDetail> CompanyDetails { get; set; }
        public DbSet<CompanyOfficer> CompanyOfficers { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<HistoricalData> HistoricalDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyDetail>().HasKey(cd => cd.Symbol);

            //one-to-many 
            modelBuilder.Entity<CompanyOfficer>()
                        .HasRequired<CompanyDetail>(cd => cd.CompanyDetail) // CompanyOfficer entity requires CompanyDetail 
                        .WithMany(co => co.CompanyOfficers)
                        .HasForeignKey(co => co.Symbol); // CompanyDetail entity includes many CompanyOfficers entities

            //primary key
            modelBuilder.Entity<CompanyProfile>().HasKey(cp => cp.Symbol);

            //one-to-one
            modelBuilder.Entity<CompanyDetail>()
                .HasOptional(cd => cd.CompanyProfile) //CompanyDetail entity has optional CompanyProfile
                .WithRequired(cp => cp.CompanyDetail); // companyProfile entity required CompanyDetail entity

            //one-to-many 
            modelBuilder.Entity<HistoricalData>()
                .HasRequired<CompanyDetail>(cd => cd.CompanyDetail)
                .WithMany(hd => hd.HistoricalDatas)
                .HasForeignKey(hd => hd.Symbol);
        }
    }
}
