using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Data
{
    public class HelpDeskDbContext : DbContext
    {
        public HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}