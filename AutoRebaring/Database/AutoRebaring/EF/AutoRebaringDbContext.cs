namespace AutoRebaring.Database.AutoRebaring.EF
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

        public virtual DbSet<ARAnchorParameter> ARAnchorParameters { get; set; }
        public virtual DbSet<ARCoverParameter> ARCoverParameters { get; set; }
        public virtual DbSet<ARDesignGeneral> ARDesignGenerals { get; set; }
        public virtual DbSet<ARDesignLevel> ARDesignLevels { get; set; }
        public virtual DbSet<ARDevelopmentParameter> ARDevelopmentParameters { get; set; }
        public virtual DbSet<ARDimensionParameterType> ARDimensionParameterTypes { get; set; }
        public virtual DbSet<ARDimensionParameterValue> ARDimensionParameterValues { get; set; }
        public virtual DbSet<AREDParameterType> AREDParameterTypes { get; set; }
        public virtual DbSet<AREDParameterValue> AREDParameterValues { get; set; }
        public virtual DbSet<ARElementType> ARElementTypes { get; set; }
        public virtual DbSet<ARElementTypeProject> ARElementTypeProjects { get; set; }
        public virtual DbSet<ARLevel> ARLevels { get; set; }
        public virtual DbSet<ARLockheadParameter> ARLockheadParameters { get; set; }
        public virtual DbSet<ARMacAddress> ARMacAddresses { get; set; }
        public virtual DbSet<ARMark> ARMarks { get; set; }
        public virtual DbSet<AROtherParameter> AROtherParameters { get; set; }
        public virtual DbSet<ARProject> ARProjects { get; set; }
        public virtual DbSet<ARRebarBarType> ARRebarBarTypes { get; set; }
        public virtual DbSet<ARRebarDesignType> ARRebarDesignTypes { get; set; }
        public virtual DbSet<ARRebarType> ARRebarTypes { get; set; }
        public virtual DbSet<ARRebarVerticalParameter> ARRebarVerticalParameters { get; set; }
        public virtual DbSet<ARStandardChosen> ARStandardChosens { get; set; }
        public virtual DbSet<ARStandardDesign> ARStandardDesigns { get; set; }
        public virtual DbSet<ARStandardDesignParameterType> ARStandardDesignParameterTypes { get; set; }
        public virtual DbSet<ARStandardDesignParameterValue> ARStandardDesignParameterValues { get; set; }
        public virtual DbSet<ARStandardEndType> ARStandardEndTypes { get; set; }
        public virtual DbSet<ARStandardFitLength> ARStandardFitLengths { get; set; }
        public virtual DbSet<ARStandardFitType> ARStandardFitTypes { get; set; }
        public virtual DbSet<ARStandardStartZ> ARStandardStartZs { get; set; }
        public virtual DbSet<ARStandardStartZType> ARStandardStartZTypes { get; set; }
        public virtual DbSet<ARStirrupDesign> ARStirrupDesigns { get; set; }
        public virtual DbSet<ARStirrupDesignParameterType> ARStirrupDesignParameterTypes { get; set; }
        public virtual DbSet<ARStirrupDesignParameterValue> ARStirrupDesignParameterValues { get; set; }
        public virtual DbSet<ARStirrupFamilyName> ARStirrupFamilyNames { get; set; }
        public virtual DbSet<ARStirrupFamilyType> ARStirrupFamilyTypes { get; set; }
        public virtual DbSet<ARUser> ARUsers { get; set; }
        public virtual DbSet<ARUserProject> ARUserProjects { get; set; }
        public virtual DbSet<ARUserType> ARUserTypes { get; set; }
        public virtual DbSet<ARView3d> ARView3d { get; set; }
        public virtual DbSet<ARStirrupParameter> ARStirrupParameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ARDesignGeneral>()
                .HasMany(e => e.ARStandardStartZs)
                .WithRequired(e => e.ARDesignGeneral)
                .HasForeignKey(e => e.IDStandardDesignGeneral)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARDesignLevel>()
                .HasMany(e => e.ARStandardDesigns)
                .WithRequired(e => e.ARDesignLevel)
                .HasForeignKey(e => e.IDDesignLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARDesignLevel>()
                .HasMany(e => e.ARStandardDesignParameterValues)
                .WithRequired(e => e.ARDesignLevel)
                .HasForeignKey(e => e.IDDesignLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARDesignLevel>()
                .HasMany(e => e.ARStirrupDesigns)
                .WithRequired(e => e.ARDesignLevel)
                .HasForeignKey(e => e.IDDesignLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARDesignLevel>()
                .HasMany(e => e.ARStirrupDesignParameterValues)
                .WithRequired(e => e.ARDesignLevel)
                .HasForeignKey(e => e.IDDesignLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARDimensionParameterType>()
                .Property(e => e.B1_Param)
                .IsUnicode(false);

            modelBuilder.Entity<ARDimensionParameterType>()
                .Property(e => e.B2_Param)
                .IsUnicode(false);

            modelBuilder.Entity<ARDimensionParameterType>()
                .HasMany(e => e.ARDimensionParameterValues)
                .WithRequired(e => e.ARDimensionParameterType)
                .HasForeignKey(e => e.IDDimensionParameterType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AREDParameterType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<AREDParameterType>()
                .HasMany(e => e.AREDParameterValues)
                .WithRequired(e => e.AREDParameterType)
                .HasForeignKey(e => e.IDParameterType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARElementType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARElementType>()
                .HasMany(e => e.ARDimensionParameterTypes)
                .WithRequired(e => e.ARElementType)
                .HasForeignKey(e => e.IDElementType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARElementType>()
                .HasMany(e => e.AREDParameterTypes)
                .WithRequired(e => e.ARElementType)
                .HasForeignKey(e => e.IDElementType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARElementType>()
                .HasMany(e => e.ARElementTypeProjects)
                .WithRequired(e => e.ARElementType)
                .HasForeignKey(e => e.IDElementType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARElementType>()
                .HasMany(e => e.ARRebarDesignTypes)
                .WithRequired(e => e.ARElementType)
                .HasForeignKey(e => e.IDElementType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARLevel>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ARLevel>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<ARLevel>()
                .HasMany(e => e.ARDesignGenerals)
                .WithRequired(e => e.ARLevel)
                .HasForeignKey(e => e.IDStartLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARLevel>()
                .HasMany(e => e.ARDesignGenerals1)
                .WithRequired(e => e.ARLevel1)
                .HasForeignKey(e => e.IDEndLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARLevel>()
                .HasMany(e => e.ARDesignLevels)
                .WithRequired(e => e.ARLevel)
                .HasForeignKey(e => e.IDDesignLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARMacAddress>()
                .Property(e => e.MacAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ARMacAddress>()
                .HasMany(e => e.ARUserProjects)
                .WithRequired(e => e.ARMacAddress)
                .HasForeignKey(e => e.IDMacAddress)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARMark>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<ARMark>()
                .HasMany(e => e.ARDesignGenerals)
                .WithRequired(e => e.ARMark)
                .HasForeignKey(e => e.IDMark)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARMark>()
                .HasMany(e => e.ARDesignLevels)
                .WithRequired(e => e.ARMark)
                .HasForeignKey(e => e.IDMark)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARMark>()
                .HasMany(e => e.AREDParameterValues)
                .WithOptional(e => e.ARMark)
                .HasForeignKey(e => e.IDMark);

            modelBuilder.Entity<ARMark>()
                .HasMany(e => e.ARElementTypeProjects)
                .WithRequired(e => e.ARMark)
                .HasForeignKey(e => e.IDMark)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARMark>()
                .HasMany(e => e.AROtherParameters)
                .WithRequired(e => e.ARMark)
                .HasForeignKey(e => e.IDMark)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARAnchorParameters)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARCoverParameters)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARDevelopmentParameters)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARDimensionParameterValues)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARLevels)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARLockheadParameters)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARMarks)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARRebarVerticalParameters)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARStandardChosens)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARStandardFitLengths)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARStirrupFamilyTypes)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARUserProjects)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARProject>()
                .HasMany(e => e.ARView3d)
                .WithRequired(e => e.ARProject)
                .HasForeignKey(e => e.IDProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARRebarBarType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARRebarBarType>()
                .HasMany(e => e.ARStandardDesigns)
                .WithRequired(e => e.ARRebarBarType)
                .HasForeignKey(e => e.IDRebarBarType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARRebarBarType>()
                .HasMany(e => e.ARStirrupDesigns)
                .WithRequired(e => e.ARRebarBarType)
                .HasForeignKey(e => e.IDRebarBarType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARRebarDesignType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARRebarDesignType>()
                .HasMany(e => e.ARStandardDesigns)
                .WithRequired(e => e.ARRebarDesignType)
                .HasForeignKey(e => e.IDRebarDesignType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARRebarDesignType>()
                .HasMany(e => e.ARStirrupFamilyTypes)
                .WithRequired(e => e.ARRebarDesignType)
                .HasForeignKey(e => e.IDRebarDesignType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARRebarType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARRebarType>()
                .HasMany(e => e.ARRebarDesignTypes)
                .WithRequired(e => e.ARRebarType)
                .HasForeignKey(e => e.IDRebarType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARRebarType>()
                .HasMany(e => e.ARRebarVerticalParameters)
                .WithRequired(e => e.ARRebarType)
                .HasForeignKey(e => e.IDRebarType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARStandardDesignParameterType>()
                .Property(e => e.Parameter)
                .IsUnicode(false);

            modelBuilder.Entity<ARStandardDesignParameterType>()
                .HasMany(e => e.ARStandardDesignParameterValues)
                .WithRequired(e => e.ARStandardDesignParameterType)
                .HasForeignKey(e => e.IDStandardDesignParameterType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARStandardEndType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARStandardEndType>()
                .HasMany(e => e.ARDesignGenerals)
                .WithRequired(e => e.ARStandardEndType)
                .HasForeignKey(e => e.IDStandardEndType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARStandardFitType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARStandardFitType>()
                .HasMany(e => e.ARStandardFitLengths)
                .WithRequired(e => e.ARStandardFitType)
                .HasForeignKey(e => e.IDStandardFitType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARStandardStartZType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARStandardStartZType>()
                .HasMany(e => e.ARStandardStartZs)
                .WithRequired(e => e.ARStandardStartZType)
                .HasForeignKey(e => e.IDStandardStartZType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARStirrupDesignParameterType>()
                .Property(e => e.Parameter)
                .IsUnicode(false);

            modelBuilder.Entity<ARStirrupDesignParameterType>()
                .HasMany(e => e.ARStirrupDesignParameterValues)
                .WithRequired(e => e.ARStirrupDesignParameterType)
                .HasForeignKey(e => e.IDStirrupDesignParameterType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARStirrupFamilyName>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ARStirrupFamilyName>()
                .HasMany(e => e.ARStirrupFamilyTypes)
                .WithRequired(e => e.ARStirrupFamilyName)
                .HasForeignKey(e => e.IDStirrupFamilyName)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARStirrupFamilyType>()
                .HasMany(e => e.ARStirrupDesigns)
                .WithRequired(e => e.ARStirrupFamilyType)
                .HasForeignKey(e => e.IDStirrupFamilyType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARUser>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<ARUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<ARUser>()
                .HasMany(e => e.ARUserProjects)
                .WithRequired(e => e.ARUser)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARUserType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARUserType>()
                .HasMany(e => e.ARUserProjects)
                .WithRequired(e => e.ARUserType)
                .HasForeignKey(e => e.IDUserType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARView3d>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ARView3d>()
                .HasMany(e => e.AROtherParameters)
                .WithRequired(e => e.ARView3d)
                .HasForeignKey(e => e.IDView3d)
                .WillCascadeOnDelete(false);
        }
    }
}
