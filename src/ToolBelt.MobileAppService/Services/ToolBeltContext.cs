using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolBelt.Data;

namespace ToolBelt.MobileAppService.Services
{
    public class ToolBeltContext : DbContext
    {
        public DbSet<Trade> Trades { get; set; }

        public DbSet<TradeSpecialty> TradeSpecialties { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ContractorAccount> ContractorAccounts { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase(
                "ToolBelt",
                new Action<Microsoft.EntityFrameworkCore.Infrastructure.InMemoryDbContextOptionsBuilder>(_ => { }));

            // TODO: Remove
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ProjectTradeSpecialty>()
                .HasKey(t => new { t.ProjectId, t.TradeSpecialtyId });

            modelBuilder
                .Entity<ProjectTradeSpecialty>()
                .HasOne(pt => pt.TradeSpecialty)
                .WithMany(p => p.ProjectTradeSpecialties)
                .HasForeignKey(pt => pt.TradeSpecialtyId);

            modelBuilder
                .Entity<ProjectTradeSpecialty>()
                .HasOne(pt => pt.Project)
                .WithMany(t => t.ProjectTradeSpecialties)
                .HasForeignKey(pt => pt.ProjectId);


            modelBuilder
                .Entity<AccountTrade>()
                .HasKey(t => new { t.AccountId, t.TradeId});

            modelBuilder
                .Entity<AccountTrade>()
                .HasOne(pt => pt.Account)
                .WithMany(p => p.AccountTrades)
                .HasForeignKey(pt => pt.AccountId);

            modelBuilder
                .Entity<AccountTrade>()
                .HasOne(pt => pt.Trade)
                .WithMany(t => t.AccountTrades)
                .HasForeignKey(pt => pt.TradeId);
        }
    }
}
