namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARDesignLevel")]
    public partial class ARDesignLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARDesignLevel()
        {
            ARStandardDesigns = new HashSet<ARStandardDesign>();
            ARStandardDesignParameterValues = new HashSet<ARStandardDesignParameterValue>();
            ARStirrupDesigns = new HashSet<ARStirrupDesign>();
            ARStirrupDesignParameterValues = new HashSet<ARStirrupDesignParameterValue>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDMark { get; set; }

        public long IDDesignLevel { get; set; }

        public int Numeric { get; set; }

        public DateTime LastUpdate { get; set; }

        public virtual ARLevel ARLevel { get; set; }

        public virtual ARMark ARMark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStandardDesign> ARStandardDesigns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStandardDesignParameterValue> ARStandardDesignParameterValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStirrupDesign> ARStirrupDesigns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStirrupDesignParameterValue> ARStirrupDesignParameterValues { get; set; }
    }
}
