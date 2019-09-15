using AzureDevopsWork.Services.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AzureDevopsWork.Services.Context
{
    public class AzWorkItemContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AzureDevopsWork;Trusted_Connection=true;");
        }

        public DbSet<WorkItem> WorkItems { get; set; }

      
    }
}
