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
            AREDParameterValues = new HashSet<AREDParameterValue>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        [Required]
        [StringLength(15)]
        public string Mark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignGeneral> ARDesignGenerals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDesignLevel> ARDesignLevels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AREDParameterValue> AREDParameterValues { get; set; }

        public virtual ARProject ARProject { get; set; }
    }
}
