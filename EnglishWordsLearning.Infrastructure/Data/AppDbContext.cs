using EnglishWordsLearning.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordsLearning.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<HistoryLogs> HistoryLogs { get; set; }
}