using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using Personal.WebAPI.Models;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using static Personal.WebAPI.Models.DB_model;

namespace Personal.WebAPI.Context
{
    public partial class Personal_Context : DbContext
    {
        public Personal_Context(DbContextOptions<Personal_Context> options) : base(options)
        {
        }
        public virtual DbSet<tagent> tagent { get; set; }
        public virtual DbSet<tcall> tcall { get; set; }
        public virtual DbSet<tticket> tticket { get; set; }
        public virtual DbSet<tcustomer> tcustomer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tagent>(ent =>
            {
                ent.ToTable("tagent");

            });
            modelBuilder.Entity<tcall>(ent =>
            {
                ent.ToTable("tcall");

            });
            modelBuilder.Entity<tticket>(ent =>
            {
                ent.ToTable("tticket");

            });
            modelBuilder.Entity<tcustomer>(ent =>
            {
                ent.ToTable("tcustomer");

                ent.Property(e => e.LastContactDate)
   .HasConversion(
       v => v.HasValue ? v.Value.ToString("yyyy-MM-dd HH:mm:ss") : null, // Handle nullable input
       v => string.IsNullOrEmpty(v) ? (DateTime?)null : DateTime.Parse(v) // Handle null/empty strings
   );
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
