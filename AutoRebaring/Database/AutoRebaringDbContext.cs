namespace AutoRebaring.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AutoRebaringDbContext : DbContext
    {
        public AutoRebaringDbContext()
            : base("name=AutoRebaringDbContext")
        {
        }

        public virtual DbSet<AddinConduction> AddinConductions { get; set; }
        public virtual DbSet<AluformSchedule> AluformSchedules { get; set; }
        public virtual DbSet<GeneralParameterInput> GeneralParameterInputs { get; set; }
        public virtual DbSet<LevelTitle> LevelTitles { get; set; }
        public virtual DbSet<ProductSource> ProductSources { get; set; }
        public virtual DbSet<RebarChosen> RebarChosens { get; set; }
        public virtual DbSet<RebarDesign> RebarDesigns { get; set; }
        public virtual DbSet<RebarDesignGeneral> RebarDesignGenerals { get; set; }
        public virtual DbSet<StirrupFamily> StirrupFamilies { get; set; }
        public virtual DbSet<UserAddinConduction> UserAddinConductions { get; set; }
        public virtual DbSet<UserManagement> UserManagements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddinConduction>()
                .Property(e => e.AddinName)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralParameterInput>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralParameterInput>()
                .Property(e => e.B1_Param)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralParameterInput>()
                .Property(e => e.B2_Param)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralParameterInput>()
                .Property(e => e.ElementType)
                .IsUnicode(false);

            modelBuilder.Entity<LevelTitle>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<RebarChosen>()
                .Property(e => e.LType)
                .IsUnicode(false);

            modelBuilder.Entity<RebarDesign>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<RebarDesign>()
                .Property(e => e.StandardType)
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

            modelBuilder.Entity<RebarDesignGeneral>()
                .Property(e => e.EndRebarType)
                .IsUnicode(false);

            modelBuilder.Entity<StirrupFamily>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<StirrupFamily>()
                .Property(e => e.FamiyStirrup)
                .IsUnicode(false);

            modelBuilder.Entity<StirrupFamily>()
                .Property(e => e.StirrupType)
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
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<UserManagement>()
                .Property(e => e.Password)
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
