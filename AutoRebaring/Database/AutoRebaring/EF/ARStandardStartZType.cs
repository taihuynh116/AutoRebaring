namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStandardStartZType")]
    public partial class ARStandardStartZType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARStandardStartZType()
        {
            ARStandardStartZs = new HashSet<ARStandardStartZ>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDElementType { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; }

        public virtual ARElementType ARElementType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStandardStartZ> ARStandardStartZs { get; set; }
    }
}
