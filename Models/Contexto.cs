using Microsoft.EntityFrameworkCore;

namespace movie.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }
        public DbSet<Movie> movies { get; set; }
    }
}