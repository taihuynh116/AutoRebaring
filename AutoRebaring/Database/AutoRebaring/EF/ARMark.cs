namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARMark")]
    public partial class ARMark
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARMark()
        {
            ARDesignGenerals = new HashSet<ARDesignGeneral>();
            ARDesignLevels = new HashSet<ARDesignLevel>();
            ARDesignLevelLimits = new HashSet<ARDesignLevelLimit>();
            AREDParameterValues = new HashSet<AREDParameterValue>();
            ARElementTypeProjects = new HashSet<ARElementTypeProject>();
            AROtherParameters = new HashSet<AROtherParameter>();
            ARWallParameters = new HashSet<ARWallParameter>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        [Required]
        [StringLength(15)]
        public string Mark { get; set; }

        public DateTime LastUpdate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignGeneral> ARDesignGenerals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignLevel> ARDesignLevels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignLevelLimit> ARDesignLevelLimits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AREDParameterValue> AREDParameterValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARElementTypeProject> ARElementTypeProjects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AROtherParameter> AROtherParameters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARWallParameter> ARWallParameters { get; set; }
    }
}
