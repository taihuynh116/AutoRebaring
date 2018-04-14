namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARLevel")]
    public partial class ARLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARLevel()
        {
            ARDesignGenerals = new HashSet<ARDesignGeneral>();
            ARDesignGenerals1 = new HashSet<ARDesignGeneral>();
            ARDesignLevels = new HashSet<ARDesignLevel>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        public double Elevation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignGeneral> ARDesignGenerals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignGeneral> ARDesignGenerals1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignLevel> ARDesignLevels { get; set; }

        public virtual ARProject ARProject { get; set; }
    }
}
