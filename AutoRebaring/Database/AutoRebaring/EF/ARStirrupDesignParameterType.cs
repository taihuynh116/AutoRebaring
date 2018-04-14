namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStirrupDesignParameterType")]
    public partial class ARStirrupDesignParameterType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARStirrupDesignParameterType()
        {
            ARStirrupDesignParameterValues = new HashSet<ARStirrupDesignParameterValue>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDElementType { get; set; }

        [Required]
        [StringLength(10)]
        public string Parameter { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStirrupDesignParameterValue> ARStirrupDesignParameterValues { get; set; }
    }
}
