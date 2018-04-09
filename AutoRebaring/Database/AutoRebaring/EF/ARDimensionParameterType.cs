namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARDimensionParameterType")]
    public partial class ARDimensionParameterType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARDimensionParameterType()
        {
            ARDimensionParameterValues = new HashSet<ARDimensionParameterValue>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDElementType { get; set; }

        [Required]
        [StringLength(15)]
        public string B1_Param { get; set; }

        [Required]
        [StringLength(15)]
        public string B2_Param { get; set; }

        public virtual ARElementType ARElementType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARDimensionParameterValue> ARDimensionParameterValues { get; set; }
    }
}
