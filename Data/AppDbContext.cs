using EnglishWordsLearning.Controllers;
using EnglishWordsLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordsLearning.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<HistoryLogs> HistoryLogs { get; set; }
    
}