namespace AutoRebaring.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AutoRebaringDbContext : DbContext
    {
        public AutoRebaringDbContext()
            : base("data source=118.69.224.199,1444;initial catalog=TAIHT;persist security info=True;user id=taiht;password=Skarner116!;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public virtual DbSet<AddinConduction> AddinConductions { get; set; }
        public virtual DbSet<AluformSchedule> AluformSchedules { get; set; }
        public virtual DbSet<ColumnParameter> ColumnParameters { get; set; }
        public virtual DbSet<DevelopmentRebar> DevelopmentRebars { get; set; }
        public virtual DbSet<GeneralParameterInput> GeneralParameterInputs { get; set; }
        public virtual DbSet<LevelTitle> LevelTitles { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<OtherParameter> OtherParameters { get; set; }
        public virtual DbSet<ProductSource> ProductSources { get; set; }
        public virtual DbSet<RebarChosen> RebarChosens { get; set; }
        public virtual DbSet<RebarChosenGeneral> RebarChosenGenerals { get; set; }
        public virtual DbSet<RebarDesign> RebarDesigns { get; set; }
        public virtual DbSet<RebarDesignGeneral> RebarDesignGenerals { get; set; }
        public virtual DbSet<UserAddinConduction> UserAddinConductions { get; set; }
        public virtual DbSet<UserManagement> UserManagements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddinConduction>()
                .Property(e => e.AddinName)
                .IsUnicode(false);

            modelBuilder.Entity<ColumnParameter>()
                .Property(e => e.B1_Param)
                .IsUnicode(false);

            modelBuilder.Entity<ColumnParameter>()
                .Property(e => e.B2_Param)
                .IsUnicode(false);

            modelBuilder.Entity<LevelTitle>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Mark>()
                .Property(e => e.Mark1)
                .IsUnicode(false);

            modelBuilder.Entity<OtherParameter>()
                .Property(e => e.View3d)
                .IsUnicode(false);

            modelBuilder.Entity<OtherParameter>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<RebarChosen>()
                .Property(e => e.LType)
                .IsUnicode(false);

            modelBuilder.Entity<RebarChosenGeneral>()
                .Property(e => e.FamilyStirrup1)
                .IsUnicode(false);

            modelBuilder.Entity<RebarChosenGeneral>()
                .Property(e => e.FamilyStirrup2)
                .IsUnicode(false);

            modelBuilder.Entity<RebarDesign>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<RebarDesign>()
                .Property(e => e.RebarType)
                .IsUnicode(false);

            modelBuilder.Entity<RebarDesign>()
                .Property(e => e.StirrupType1)
                .IsUnicode(false);

            modelBuilder.Entity<RebarDesign>()
                .Property(e => e.StirrupType2)
                .IsUnicode(false);

            modelBuilder.Entity<RebarDesignGeneral>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddinConduction>()
                .Property(e => e.AddinName)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddinConduction>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddinConduction>()
                .Property(e => e.MacAddress)
                .IsUnicode(false);

            modelBuilder.Entity<UserManagement>()
                .Property(e => e.ProjectName)
                .IsUnicode(false);

            modelBuilder.Entity<UserManagement>()
                .Property(e => e.MacAddress)
                .IsUnicode(false);

            modelBuilder.Entity<UserManagement>()
                .Property(e => e.WebUsername)
                .IsUnicode(false);

            modelBuilder.Entity<UserManagement>()
                .Property(e => e.WebPassword)
                .IsUnicode(false);

            modelBuilder.Entity<UserManagement>()
                .Property(e => e.LoginType)
                .IsUnicode(false);

            modelBuilder.Entity<UserManagement>()
                .Property(e => e.ChangeMacAddress)
                .IsUnicode(false);
        }
    }
}
