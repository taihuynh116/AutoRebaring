namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARElementType")]
    public partial class ARElementType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARElementType()
        {
            ARDimensionParameterTypes = new HashSet<ARDimensionParameterType>();
            AREDParameterTypes = new HashSet<AREDParameterType>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(15)]
        public string Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDimensionParameterType> ARDimensionParameterTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AREDParameterType> AREDParameterTypes { get; set; }
    }
}
