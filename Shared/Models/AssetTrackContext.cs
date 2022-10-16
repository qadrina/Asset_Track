using Microsoft.Extensions.Configuration;
using Asset_Track.Shared.StoredProcedures;
using Microsoft.EntityFrameworkCore;


namespace Asset_Track.Shared.Models
{
    public partial class AssetTrackContext : DbContext
    {
        public AssetTrackContext()
        {
        }
        public IConfiguration Configuration { get; } = null!;

        public AssetTrackContext(DbContextOptions<AssetTrackContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<Asset> Asset { get; set; } = null!;
        public virtual DbSet<Spot> Spot { get; set; } = null!;
        public virtual DbSet<Spot_Ext> Spot_Ext { get; set; } = null!;
        public virtual DbSet<Track> Track { get; set; } = null!;

        // SP
        public virtual DbSet<spAsset> spAsset { get; set; } = null!;
        public virtual DbSet<spSpot> spSpot { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=NQHW\\DUBAI; Database=AssetTrack; Trusted_Connection=True; MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.Asset_Id);

                entity.ToTable("Asset");

                entity.Property(e => e.Asset_Id).HasMaxLength(10);

                entity.Property(e => e.Asset_Title).HasMaxLength(50);

                entity.Property(e => e.Spot_Ex).HasMaxLength(10);

                entity.Property(e => e.Spot_Id).HasMaxLength(10);

                entity.Property(e => e.Time_Stamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Spot>(entity =>
            {
                entity.HasKey(e => e.Spot_Id);

                entity.ToTable("Spot");

                entity.Property(e => e.Spot_Id).HasMaxLength(10);

                entity.Property(e => e.Spot_Title).HasMaxLength(50);

                entity.Property(e => e.Spot_Up).HasMaxLength(10);
            });

            modelBuilder.Entity<Spot_Ext>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Spot_Ext");

                entity.Property(e => e.Spot_Id).HasMaxLength(10);

                entity.Property(e => e.Spot_Up).HasMaxLength(10);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Track");

                entity.Property(e => e.Asset_Id).HasMaxLength(10);

                entity.Property(e => e.Spot_Id).HasMaxLength(10);

                entity.Property(e => e.Time_Stamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<spAsset>().HasNoKey();
            modelBuilder.Entity<spSpot>().HasNoKey();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
